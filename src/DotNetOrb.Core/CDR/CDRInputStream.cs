// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using DotNetty.Buffers;
using GIOP;
using IOP;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Marshal = CORBA.Marshal;
using TypeCode = CORBA.TypeCode;

namespace DotNetOrb.Core.CDR
{
    public class CDRInputStream : CodeSet.InputBuffer, IInputStream, IDataInputStream, IDisposable
    {
        public class MarkedPosition
        {
            public int Pos { get; set; }
            public int Index { get; set; }
            public int ChunkEndPos { get; set; }
            public int ValueNestingLevel { get; set; }

            public MarkedPosition(int pos, int index, int chunkEndPos, int valueNestingLevel)
            {
                Pos = pos;
                Index = index;
                ChunkEndPos = chunkEndPos;
                ValueNestingLevel = valueNestingLevel;
            }
        }
        private MarkedPosition markedPosition;
        private Stack<EncapsulationInfo> encapsulations = new Stack<EncapsulationInfo>();
        private SortedDictionary<int, TypeCode> repeatedTCMap = new SortedDictionary<int, TypeCode>();
        private Dictionary<int, string> recursiveTCMap = new Dictionary<int, string>();
        private bool isClosed;
        private bool cometInteropFix;
        private bool laxBooleanEncoding;
        private bool nullStringEncoding;
        public CodeSet CodeSetChar { get; set; }
        public CodeSet CodeSetWChar { get; set; }

        // needed to determine size in chars
        protected int giopMinor = 2;
        public int GiopMinor
        {
            get
            {
                return giopMinor;
            }
            set
            {
                giopMinor = value;
            }
        }
        /// <summary>
        /// indices within the buffer to the values that appear at these indices
        /// </summary>
        private Dictionary<int, object> valueMap = new Dictionary<int, object>();
        /// <summary>
        /// indices within the buffer to repository ids that appear at these indices
        /// </summary>
        private Dictionary<int, string> repIdMap = new Dictionary<int, string>();
        /// <summary>
        /// indices within the buffer to codebase strings that appear at these indices.
        /// </summary>
        private Dictionary<int, string> codeBaseMap = new Dictionary<int, string>();

        private ConcurrentDictionary<string, List<(TypeCode typeCode, int position)>> typeCodeCache;

        private int typeCodeNestingLevel = -1;

        /// <summary>
        /// Index of the current IDL value that is being unmarshalled.
        /// This is kept here so that when the value object has been
        /// created, the value factory can immediately store it into this
        /// stream's valueMap by calling `RegisterValue()'.
        /// </summary>
        private int currentValueIndex;
        private bool isLittleEndian;
        public bool IsLittleEndian { get { return isLittleEndian; } set { isLittleEndian = value; } }
        private ORB orb;
        public CORBA.ORB Orb => orb;

        /// <summary>this is the lowest possible value_tag indicating the begin of a valuetype (15.3.4)</summary>
        private static int MaxBlockSize = 0x7fffff00;
        /// <summary>
        ///  fixes RMI/IIOP related interoperability issues with the
        ///  sun the orb that occured
        ///  while receiving serialized collections.
        ///  <see href="http://lists.spline.inf.fu-berlin.de/mailman/htdig/jacorb-developer/2006-May/008251.html"/>
        ///  mailing list for details.
        ///  </summary>
        private bool sunInteropFix;

        internal IByteBuffer Buffer { get; set; }

        public int Pos
        {
            get
            {
                if (Buffer != null)
                {
                    return Buffer.ReaderIndex;
                }
                else
                {
                    throw new BadInvOrder("CDRStream not initialized");
                }
            }
            set
            {
                if (Buffer != null)
                {
                    if (value > Buffer.WriterIndex)
                    {
                        Buffer.SetReaderIndex(Buffer.WriterIndex);
                    }
                    else
                    {
                        Buffer.SetReaderIndex(value);
                    }
                }
                else
                {
                    throw new BadInvOrder("CDRStream not initialized");
                }
            }
        }

        protected int Index { get; set; }

        // Last value tag read had the chunking bit on
        private bool chunkedValue = false;

        // Nesting level of chunked valuetypes
        private int valueNestingLevel = 0;

        // Ending position of the current chunk
        private int chunkEndPos = -1;   // -1 means we're not within a chunk

        private CDRInputStream(ORB orb)
        {
            if (orb == null)
            {
                this.orb = CORBA.ORB.Init() as ORB;
            }
            else
            {
                this.orb = orb;
            }
            typeCodeCache = this.orb.TypeCodeCache;
            try
            {
                var configuration = this.orb.Configuration;
                cometInteropFix = configuration.GetAsBoolean("DotNetOrb.Interop.Comet", false);
                laxBooleanEncoding = configuration.GetAsBoolean("DotNetOrb.Interop.LaxBooleanEncoding", false);
                sunInteropFix = configuration.GetAsBoolean("DotNetOrb.Interop.Sun", false);
                nullStringEncoding = configuration.GetAsBoolean("DotNetOrb.Interop.NullStringEncoding", false);

                CodeSetChar = this.orb.DefaultCodeSetChar;
                CodeSetWChar = this.orb.DefaultCodeSetWChar;
            }
            catch (ConfigException e)
            {
                throw new CORBA.Internal("ConfigurationException: " + e);
            }
        }

        public CDRInputStream(ORB orb, IByteBuffer buffer) : this(orb)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer cannot be null");
            }
            Buffer = buffer;
        }

        public CDRInputStream(ORB orb, byte[] buffer) : this(orb)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer cannot be null");
            }
            Buffer = Unpooled.WrappedBuffer(buffer);
        }

        public CDRInputStream(byte[] buffer) : this(null, buffer)
        {

        }

        public CDRInputStream(ORB orb, byte[] buf, int giopMinor, bool littleEndian) : this(orb, buf)
        {
            isLittleEndian = littleEndian;
            this.giopMinor = giopMinor;
            Buffer = Unpooled.WrappedBuffer(buf);
        }

        public CDRInputStream(ORB orb, IByteBuffer buf, int giopMinor, bool littleEndian) : this(orb, buf)
        {
            isLittleEndian = littleEndian;
            this.giopMinor = giopMinor;
            Buffer = buf;
            Index = buf.ReaderIndex;
        }

        public void Close()
        {
            if (isClosed)
            {
                return;
            }
            Buffer.Release();
            Buffer = null;
            encapsulations = null;
            isClosed = true;

            if (recursiveTCMap != null)
            {
                recursiveTCMap.Clear();
            }
        }

        private long GetLong(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetLongLE(pos);
            }
            return Buffer.GetLong(pos);
        }

        private int GetInt(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetIntLE(pos);
            }
            return Buffer.GetInt(pos);
        }

        private uint GetUInt(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetUnsignedIntLE(pos);
            }
            return Buffer.GetUnsignedInt(pos);
        }

        private short GetShort(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetShortLE(pos);
            }
            return Buffer.GetShort(pos);
        }

        private ushort GetUShort(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetUnsignedShortLE(pos);
            }
            return Buffer.GetUnsignedShort(pos);
        }

        private float GetFloat(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetFloatLE(pos);
            }
            return Buffer.GetFloat(pos);
        }

        private double GetDouble(int pos)
        {
            if (isLittleEndian)
            {
                return Buffer.GetDoubleLE(pos);
            }
            return Buffer.GetDouble(pos);
        }

        private int ReadLongInternal()
        {
            int result;
            result = GetInt(Pos);
            Index += 4;
            Pos += 4;
            return result;
        }

        private uint ReadULongInternal()
        {
            uint result;
            result = GetUInt(Pos);
            Index += 4;
            Pos += 4;
            return result;
        }

        private long ReadLongLongInternal()
        {
            long result;
            result = GetLong(Pos);
            Index += 8;
            Pos += 8;
            return result;
        }

        private float ReadFloatInternal()
        {
            float result;
            result = GetFloat(Pos);
            Index += 4;
            Pos += 4;
            return result;
        }

        private double ReadDoubleInternal()
        {
            double result;
            result = GetDouble(Pos);
            Index += 8;
            Pos += 8;
            return result;
        }

        private void HandleChunking()
        {
            int remainder = 4 - Index % 4;
            int alignedPos = remainder != 4 ? Pos + remainder : Pos;

            if (chunkEndPos >= Pos && chunkEndPos <= alignedPos)
            {
                AdjustPositions();
            }
        }

        private void AdjustPositions()
        {
            chunkEndPos = -1;
            int savedPos = Pos;
            int savedIndex = Index;
            int tag = ReadLong();

            if (tag < 0)
            {
                // tag is an end tag
                if (-tag > valueNestingLevel)
                {
                    throw new CORBA.Internal("received end tag " + tag + " with value nesting level " + valueNestingLevel);
                }
                valueNestingLevel = -tag;
                valueNestingLevel--;

                if (valueNestingLevel > 0)
                {
                    chunkEndPos = Pos;
                    HandleChunking();
                }
            }
            else if (tag > 0 && tag < 0x7fffff00)
            {
                // tag is the chunk size tag of another chunk

                chunkEndPos = Pos + tag;
            }
            else // (tag == 0 || tag >= 0x7fffff00)
            {
                // tag is the null value tag or the value tag of a nested value

                Pos = savedPos;      // "unread" the tag
                Index = savedIndex;
            }
        }

        public void Skip(int distance)
        {
            Pos += distance;
            Index += distance;
        }


        /// <summary>
        /// close a CDR encapsulation and restore index and byte order information
        /// </summary>
        public void CloseEncapsulation()
        {
            if (encapsulations == null)
            {
                throw new Marshal("Internal Error - closeEncapsulation failed");
            }

            EncapsulationInfo ei = encapsulations.Pop();
            isLittleEndian = ei.IsLittleEndian;
            int size = ei.Size;
            int start = ei.Start;

            if (Pos < start + size)
            {
                Pos = start + size;
            }

            Index = ei.Index + size;
        }

        /// <summary>
        /// open a CDR encapsulation and restore index and byte order information
        /// </summary>
        public int OpenEncapsulation()
        {
            bool oldEndian = isLittleEndian;
            int size = ReadLong();

            // Check if size looks sane. If not try changing byte order.
            // This is a specific fix for interoperability with the Iona
            // Comet COM/CORBA bridge that has problems with size marshalling.
            if (cometInteropFix && (size < 0 || size > Buffer.ReadableBytes))
            {
                int temp =

                    (int)((size >> 24 & 0x000000FF) +
                    (size >> 8 & 0x0000FF00) +
                    (size << 8 & 0x00FF0000) +
                    (size << 24 & 0xFF000000))
                ;

                //log.Debug("Size of CDR encapsulation larger than buffer, swapping byte order. " +
                //                 "Size of CDR encapsulation was " + size + ", is now " + temp);
                size = temp;
            }

            // save current index plus size of the encapsulation on the stack.
            // When the encapsulation is closed, this value will be restored as
            // index            
            encapsulations.Push(new EncapsulationInfo(oldEndian, Index, Pos, size));

            OpenEncapsulatedArray();

            return size;
        }

        public void OpenEncapsulatedArray()
        {
            // reset index  to zero, i.e. align relative  to the beginning of the encaps.
            ResetIndex();
            isLittleEndian = ReadBoolean();
        }

        /// <summary>
        /// Return a copy of the current buffer.
        /// </summary>        
        public IByteBuffer GetBufferCopy()
        {
            return Buffer.Copy();
        }

        /// <summary>
        /// Reads the next byte of data from the input stream. The value byte is
        /// returned as an int in the range 0 to 255. If no byte is available
        /// because the end of the stream has been reached, the value -1 is returned.
        /// </summary>
        public int Read()
        {
            if (isClosed)
            {
                throw new IOException("Stream already closed!");
            }

            if (Available < 1)
            {
                return -1;
            }
            ++Index;
            return Buffer.GetByte(Pos++);
        }

        /// <summary>
        /// the number of bytes that can be read (or skipped over) from this input stream.
        /// This is not necessarily the number of 'valid' bytes.
        /// </summary>
        public int Available
        {
            get
            {
                return Buffer.ReadableBytes;
            }
        }

        public int Read(byte[] b)
        {
            return Read(b, 0, b.Length);
        }

        public int Read(byte[] b, int off, int len)
        {
            if (b == null)
            {
                throw new IOException("buffer may not be null");
            }

            if (off < 0 || len < 0 || off + len > b.Length)
            {
                throw new IOException("buffer index out of bounds");
            }

            if (len == 0)
            {
                return 0;
            }

            if (Available < 1)
            {
                return -1;
            }

            if (isClosed)
            {
                throw new IOException("Stream already closed!");
            }

            int min = Math.Min(len, Available);
            Buffer.GetBytes(Pos, b, off, min);
            //Array.Copy(Buffer, Index, b, off, min);
            Pos += min;
            Index += min;
            return min;
        }

        public CORBA.Any ReadAny()
        {
            TypeCode _tc = ReadTypeCode();
            CORBA.Any any = orb.CreateAny();
            any.ReadValue(this, _tc);
            return any;
        }

        public bool ReadBoolean()
        {
            if (chunkEndPos == Pos)
            {
                AdjustPositions();
            }

            Index++;
            byte value = Buffer.GetByte(Pos++);

            return ParseBoolean(value);
        }

        public void ReadBooleanArray(ref bool[] value, int offset, int length)
        {
            HandleChunking();

            if (length == 0)
            {
                return;
            }

            int until = offset + length;

            int j = offset;
            do
            {
                byte bb = Buffer.GetByte(Pos++);
                value[j] = ParseBoolean(bb);
                ++j;
            }
            while (j < until);

            Index += length;
        }

        private bool ParseBoolean(byte value)
        {
            if (value == 0)
            {
                return false;
            }
            else
            {
                if (value == 1)
                {
                    return true;
                }
                else if (laxBooleanEncoding)
                {
                    // Technically only valid values are 0 (false) and 1 (true)
                    // however some ORBs send values other than 1 for true.
                    return true;
                }
                else
                {
                    throw new Marshal("Unexpected boolean value: " + value + " pos: " + Pos);
                }
            }
        }

        public char ReadChar()
        {
            if (chunkEndPos == Pos)
            {
                AdjustPositions();
            }

            Index++;
            return (char)(Buffer.GetByte(Pos++) & 0xFF);
        }

        public void ReadCharArray(ref char[] value, int offset, int length)
        {
            if (value == null)
            {
                throw new Marshal("Cannot marshal result into null array.");
            }
            else if (offset + length > value.Length || length < 0 || offset < 0)
            {
                throw new Marshal("Cannot marshal as indices for array are out bounds.");
            }

            HandleChunking();

            for (int j = offset; j < offset + length; j++)
            {
                Index++;
                value[j] = (char)(0xff & Buffer.GetByte(Pos++));
            }
        }

        public double ReadDouble()
        {
            HandleChunking();

            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }

            return ReadDoubleInternal();
        }

        public void ReadDoubleArray(ref double[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }

            HandleChunking();

            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }

            for (int j = offset; j < offset + length; j++)
            {
                value[j] = ReadDoubleInternal();
            }
        }

        public decimal ReadFixed(ushort digits, ushort scale)
        {
            if (digits < 1)
            {
                throw new BadParam("digits must be a positive value: " + digits + ".");
            }

            if (scale < 0)
            {
                throw new BadParam("scale must be a non-negative value: " + scale + ".");
            }

            if (scale > digits)
            {
                throw new BadParam("scale factor " + scale + " must be less than or equal to the total number of digits " + digits + ".");
            }

            HandleChunking();

            var sb = new StringBuilder();

            int b = Buffer.GetByte(Pos++);
            int c = b & 0x0F; // second half byte
            Index++;

            while (true)
            {
                c = (b & 0xF0) >>> 4;

                if (sb.Length > 0 || c != 0)
                {
                    sb.Append(c);
                }

                c = b & 0x0F;
                if (c == 0xC || c == 0xD)
                {
                    break;
                }
                sb.Append(c);

                b = Buffer.GetByte(Pos++);
                Index++;
            }

            if ((digits == 1 || digits == -1) && sb.Length == 0)
            {
                sb.Append('0');
            }
            if (digits != -1 && sb.Length > digits)
            {
                throw new Marshal("unexpected number of digits: expected " + digits + " got " + sb.Length + " " + sb);
            }

            var value = decimal.Parse(sb.ToString());

            int[] parts = decimal.GetBits(value);

            var result = new decimal(parts[0], parts[1], parts[2], false, (byte)scale);

            if (c == 0xD)
            {
                return decimal.Negate(result);
            }

            return result;
        }

        public float ReadFloat()
        {
            HandleChunking();

            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }

            return ReadFloatInternal();
        }

        public void ReadFloatArray(ref float[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = ReadFloatInternal();
            }
        }

        public int ReadLong()
        {
            HandleChunking();
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }
            return ReadLongInternal();
        }

        public void ReadLongArray(ref int[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = GetInt(Pos);
                Pos += 4;
            }
            Index += 4 * length;
        }

        public long ReadLongLong()
        {
            HandleChunking();

            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }

            return ReadLongLongInternal();
        }

        public void ReadLongLongArray(ref long[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }

            HandleChunking();

            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }

            for (int j = offset; j < offset + length; j++)
            {
                value[j] = ReadLongLongInternal();
            }
        }


        public IObject ReadObject()
        {
            HandleChunking();

            IOR ior = IORHelper.Read(this);

            if (ParsedIOR.IsNull(ior))
            {
                return null;
            }
            else
            {
                return orb.GetObject(ior);
            }
        }


        public IObject ReadObject(Type type)
        {
            if (typeof(IObject).IsAssignableFrom(type))
            {
                CORBA.Object obj = (CORBA.Object)ReadObject();
                if (obj is IObject)
                {
                    CORBA.Object stub = null;
                    try
                    {
                        stub = (CORBA.Object)Activator.CreateInstance(type);
                    }
                    catch (Exception e)
                    {
                        throw new Marshal("Exception in stub instantiation: " + e);
                    }
                    stub._Delegate = obj._Delegate;
                    return stub;
                }
                return obj;
            }
            return ReadObject();
        }

        public byte ReadOctet()
        {
            if (chunkEndPos == Pos)
            {
                AdjustPositions();
            }

            Index++;
            return Buffer.GetByte(Pos++);
        }

        public void ReadOctetArray(ref byte[] value, int offset, int length)
        {
            HandleChunking();
            Buffer.GetBytes(Pos, value, offset, length);
            Index += length;
            Pos += length;
        }

        public short ReadShort()
        {
            HandleChunking();
            int remainder = 2 - Index % 2;
            if (remainder != 2)
            {
                Index += remainder;
                Pos += remainder;
            }

            short result = GetShort(Pos);
            Pos += 2;
            Index += 2;
            return result;
        }

        public void ReadShortArray(ref short[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();
            int remainder = 2 - Index % 2;
            if (remainder != 2)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = GetShort(Pos);
                Pos += 2;
            }
            Index += length * 2;
        }

        public string ReadString()
        {
            string result = null;
            HandleChunking();
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }

            // read size (#bytes)
            int size = GetInt(Pos);
            if (size < 1 && !nullStringEncoding)
            {
                throw new Marshal("invalid string size: " + size);
            }

            int start = Pos + 4;

            int stringTerminatorPosition = start + size - 1;

            if (nullStringEncoding && size == 0)
            {
                // Some ORBs wrongly encode empty string with a size 0
                return null;
            }
            else if (size > Buffer.ReadableBytes)
            {
                string message = "Size (" + size + ") invalid for string extraction from buffer length of " + Buffer.ReadableBytes + " from position " + start;
                throw new Marshal(message);
            }

            Index += size + 4;
            Pos += size + 4;

            if (Buffer.GetByte(stringTerminatorPosition) == 0)
            {
                size--;
            }
            else
            {
                throw new Marshal("unexpected string terminator value " + Buffer.GetByte(stringTerminatorPosition).ToString("X") + " at buffer index " + stringTerminatorPosition);
              }

            // Optimize for empty strings.
            if (size == 0)
            {
                return "";
            }

            try
            {
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding(CodeSetChar.Name);
                result = Buffer.GetString(start, size, enc);
            }
            catch (Exception ex)
            {
                //log.Error("Charset " + codeSet.getName() + " is unsupported");
                char[] buf = new char[size];

                for (int i = 0; i < size; i++)
                {
                    buf[i] = (char)(0xff & Buffer.GetByte(start + i));
                }
                result = new string(buf);
            }
            return result;
        }

        public TypeCode ReadTypeCode()
        {

            try
            {
                return ReadTypeCode(recursiveTCMap, repeatedTCMap);
            }
            finally
            {
                recursiveTCMap.Clear();
                repeatedTCMap.Clear();
            }
        }

        public TypeCode ReadTypeCode(IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap)
        {
            try
            {
                ++typeCodeNestingLevel;
                return TypeCode.ReadTypeCode(this, recursiveTCMap, repeatedTCMap);
            }
            finally
            {
                --typeCodeNestingLevel;
            }
        }

        /// <summary>
        /// Skip amount is skip(size - ((pos - start_pos) - 4 - 4));
        /// EncapsulationSize - (PositionAfterReadingId - start_pos - 4 [Size] - 4 [KindSize] ) = RemainingSizeToSkip
        /// </summary>
        public void SkipRemainingTypeCode(int startPos, int size)
        {
            Skip(size - (Pos - startPos - 4 - 4));
        }

        public uint ReadULong()
        {
            HandleChunking();
            uint result;
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }
            result = GetUInt(Pos);
            Index += 4;
            Pos += 4;
            return result;
        }

        public void ReadULongArray(ref uint[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();
            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = GetUInt(Pos);
                Pos += 4;
            }
            Index += 4 * length;
        }

        public ulong ReadULongLong()
        {
            HandleChunking();
            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }
            return (ulong)ReadLongLongInternal();
        }

        public void ReadULongLongArray(ref ulong[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();

            int remainder = 8 - Index % 8;
            if (remainder != 8)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = (ulong)ReadLongLongInternal();
            }
        }
        public ushort ReadUShort()
        {
            HandleChunking();

            int remainder = 2 - Index % 2;
            if (remainder != 2)
            {
                Index += remainder;
                Pos += remainder;
            }
            ushort result = GetUShort(Pos);
            Pos += 2;
            Index += 2;
            return result;
        }

        public void ReadUShortArray(ref ushort[] value, int offset, int length)
        {
            if (length == 0)
            {
                return;
            }
            HandleChunking();
            int remainder = 2 - Index % 2;
            if (remainder != 2)
            {
                Index += remainder;
                Pos += remainder;
            }
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = GetUShort(Pos);
                Pos += 2;
            }
            Index += length * 2;
        }

        public char ReadWChar()
        {
            if (GiopMinor == 0)
            {
                var msgType = Buffer.GetByte(7);
                int minor = msgType == (byte)MsgType11.Reply ? 6 : 5;
                throw new Marshal("GIOP 1.0 does not support the type wchar", minor, CompletionStatus.No);
            }
            HandleChunking();
            if (GiopMinor == 2)
            {
                //ignore size indicator
                Index++;
                Pos++;
                bool littleEndian = ReadBOM();
                return ReadWChar(littleEndian);
            }
            return ReadWChar(isLittleEndian);
        }

        public byte ReadByte()
        {
            Index++;
            return Buffer.GetByte(Pos++);
        }

        private char ReadWChar(bool littleEndian)
        {
            return CodeSetWChar.ReadWChar(this, GiopMinor, littleEndian);
        }

        /// <summary>
        /// Read the byte order marker indicating the endianess.
        /// </summary>
        /// <returns>
        /// true for little endianess, false otherwise (including no BOM present.
        /// In this case, big endianess is assumed per spec)
        /// </returns>
        public bool ReadBOM()
        {
            if (Buffer.GetByte(Pos) == 0xFE && Buffer.GetByte(Pos + 1) == 0xFF)
            {
                //encountering a byte order marker indicating big
                //endianess
                Pos += 2;
                Index += 2;
                return false;
            }
            else if (Buffer.GetByte(Pos) == 0xFF && Buffer.GetByte(Pos + 1) == 0xFE)
            {
                //encountering a byte order marker indicating
                //little endianess
                Pos += 2;
                Index += 2;
                return true;
            }
            else
            {
                //no BOM so big endian per spec.
                return false;
            }
        }

        public void ReadWCharArray(ref char[] value, int offset, int length)
        {
            HandleChunking();
            for (int j = offset; j < offset + length; j++)
            {
                value[j] = ReadWChar(); // inlining later...
            }
        }

        public string ReadWString()
        {
            if (GiopMinor == 0)
            {
                var msgType = Buffer.GetByte(7);
                int minor = msgType == (byte)MsgType11.Reply ? 6 : 5;
                throw new Marshal("GIOP 1.0 does not support the IDL type wstring", minor, CompletionStatus.No);
            }

            HandleChunking();

            int remainder = 4 - Index % 4;
            if (remainder != 4)
            {
                Index += remainder;
                Pos += remainder;
            }

            // read length indicator
            int size = GetInt(Pos);
            Index += 4;
            Pos += 4;

            if (size == 0) return "";

            return CodeSetWChar.ReadWString(this, size, GiopMinor, isLittleEndian);
        }

        public void Mark(int readLimit)
        {
            markedPosition = GetCurrentPosition();
        }

        /// <summary>
        /// Gets the current position of the input stream without setting the internal marked position.
        /// </summary>
        public MarkedPosition GetCurrentPosition()
        {
            return new MarkedPosition(Pos, Index, chunkEndPos, valueNestingLevel);
        }

        public void Reset()
        {
            if (Pos < 0)
            {
                throw new Marshal("Mark has not been set!");
            }
            SetCurrentPosition(markedPosition);
        }

        /// <summary>
        /// Reset the input stream to a specific marked position.
        /// </summary>
        public void SetCurrentPosition(MarkedPosition mark)
        {
            if (markedPosition == null)
            {
                throw new Marshal("Mark has not been set!");
            }
            Pos = mark.Pos;
            Buffer.SetReaderIndex(Pos);
            Index = mark.Index;
            chunkEndPos = mark.ChunkEndPos;
            valueNestingLevel = mark.ValueNestingLevel;
        }

        private void ResetIndex()
        {
            Index = 0;
        }

        /// <summary>
        /// Reads an instance of the type described by type code tc from this CDRInputStream
        /// and remarshals it to the given OutputStream, outputStream.  Called from Any.
        /// </summary>
        //void ReadValue(TypeCode typeCode, IOutputStream outputStream)
        //{
        //    if (typeCode is not TypeCode tc)
        //    {
        //        throw new BadParam("TypeCode is null");
        //    }
        //    tc.RemarshalValue(this, outputStream);
        //}

        public object ReadValue(object value)
        {
            if (value is IStreamable streamable)
            {
                RegisterValue(value);
                streamable._Read(this);
            }
            else if (value is ICustomValue custom)
            {
                RegisterValue(value);
                custom._Unmarshal(new DataInputStream(this));
            }
            else
            {
                throw new BadParam("ReadValue is only implemented for Streamables");
            }
            return value;
        }

        public object ReadValue(Type type)
        {
            uint tag = ReadULong();
            int startOffset = Pos - 4;

            if (tag == 0xffffffff)
            {
                // indirection
                return ReadIndirectValue();
            }
            else if (tag == 0x00000000)
            {
                // null tag
                return null;
            }

            string codebase = (tag & 1) != 0 ? ReadCodebase() : null;
            chunkedValue = (tag & 8) != 0;

            uint theTag = tag;
            tag = tag & 0xfffffff6;

            if (tag == 0x7fffff00)
            {
                var idAtt = type.GetCustomAttributes(typeof(RepositoryIDAttribute)).FirstOrDefault() as RepositoryIDAttribute;
                if (idAtt != null)
                {
                    return ReadUntypedValue(new string[] { idAtt.Id }, startOffset, codebase);
                }
                throw new Marshal($"Unknow repository id for type {type.FullName}");
            }
            else if (tag == 0x7fffff02)
            {
                return ReadTypedValue(startOffset, codebase);
            }
            else if (tag == 0x7fffff06)
            {
                return ReadMultiTypedValue(startOffset, codebase);
            }
            else
            {
                throw new Marshal("Unknown value tag: 0x" + theTag.ToString("X") + " (offset=0x" + startOffset.ToString("X") + ")");
            }
        }

        public object ReadValue()
        {
            uint tag = ReadULong();
            int startOffset = Pos - 4;

            if (tag == 0xffffffff)
            {
                // indirection
                return ReadIndirectValue();
            }
            else if (tag == 0x00000000)
            {
                // null tag
                return null;
            }

            string codebase = (tag & 1) != 0 ? ReadCodebase() : null;
            chunkedValue = (tag & 8) != 0;

            uint theTag = tag;
            tag = tag & 0xfffffff6;

            if (tag == 0x7fffff00)
            {
                throw new Marshal("missing value type information");
            }
            else if (tag == 0x7fffff02)
            {
                return ReadTypedValue(startOffset, codebase);
            }
            else if (tag == 0x7fffff06)
            {
                return ReadMultiTypedValue(startOffset, codebase);
            }
            else
            {
                throw new Marshal("unknown value tag: 0x" + theTag.ToString("X") + " (offset=0x" + startOffset.ToString("X") + ")");
            }
        }

        public object ReadValue(string repositoryId)
        {
            uint tag = ReadULong();
            int startOffset = Pos - 4;

            if (tag == 0xffffffff)
            {
                // indirection
                return ReadIndirectValue();
            }
            else if (tag == 0x00000000)
            {
                // null tag
                return null;
            }

            string codebase = (tag & 1) != 0 ? ReadCodebase() : null;
            chunkedValue = (tag & 8) != 0;

            uint theTag = tag;
            tag = tag & 0xfffffff6;

            if (tag == 0x7fffff00)
            {
                return ReadUntypedValue(new string[] { repositoryId }, startOffset, codebase);
            }
            else if (tag == 0x7fffff02)
            {
                return ReadTypedValue(startOffset, codebase);
            }
            else if (tag == 0x7fffff06)
            {
                return ReadMultiTypedValue(startOffset, codebase);
            }
            else
            {
                throw new Marshal("unknown value tag: 0x" + theTag.ToString("X") + " (offset=0x" + startOffset.ToString("X") + ")");
            }
        }

        /// <summary>
        /// Unmarshals a valuetype instance from this stream.  The value returned
        /// is the same value passed in, with all the data unmarshaled
        /// (IDL-to-Java Mapping 1.2, August 2002, 1.13.1, p. 1-39).  The specified
        /// value is an uninitialized value that is added to the ORB's indirection
        /// table before unmarshaling(1.21.4.1, p. 1-117).
        /// This method is intended to be called from custom valuetype factories.
        /// Unlike the other read_value() methods in this class, this method does
        /// not expect a GIOP value tag nor a repository id in the stream.
        /// Overrides read_value(value) in IInputStream
        /// </summary>
        public object ReadValue(IValueBase value)
        {
            if (value is IStreamable streamable)
            {
                RegisterValue(value);
                streamable._Read(this);
            }
            else if (value is ICustomValue custom)
            {
                RegisterValue(value);
                custom._Unmarshal(this);
            }
            else
            {
                throw new BadParam("ReadValue is only implemented for Streamables");
            }
            return value;
        }

        public object ReadValue(IBoxedValueHelper factory)
        {
            uint tag = ReadULong();
            int startOffset = Pos - 4;

            if (tag == 0xffffffff)
            {
                // indirection
                return ReadIndirectValue();
            }
            else if (tag == 0x00000000)
            {
                // null tag, explicit representation of null value
                return null;
            }

            string codebase = (tag & 1) != 0 ? ReadCodebase() : null;
            chunkedValue = (tag & 8) != 0;

            uint theTag = tag;
            tag = tag & 0xfffffff6;

            if (tag == 0x7fffff00)
            {
                var result = factory.ReadValue(this);

                if (result != null)
                {
                    valueMap.Add(startOffset, result);
                }

                return result;
            }
            else if (tag == 0x7fffff02)
            {
                var result = ReadTypedValue(startOffset, codebase, factory);

                if (result != null)
                {
                    valueMap.Add(startOffset, result);
                }

                return result;
            }
            else
            {
                throw new Marshal("unknown value tag: 0x" + theTag.ToString("X") + " (offset=0x" + startOffset.ToString("X") + ")");
            }
        }

        /// <summary>
        /// Immediately reads a value from this stream; i.e. without any repository id preceding 
        /// </summary>
        /// <param name="repositoryIds">the expected type of the value</param>
        /// <param name="index">the index at which the value started</param>
        private object ReadUntypedValue(string[] repositoryIds, int index, string codebase)
        {
            object result = null;

            if (chunkedValue || valueNestingLevel > 0 && !sunInteropFix)
            {
                valueNestingLevel++;
                ReadChunkSizeTag();
            }

            for (int i = 0; i < repositoryIds.Length; i++)
            {
                if (repositoryIds[i].Equals(WStringValueHelper.Id))
                {
                    // special handling of strings, according to spec
                    result = ReadWString();
                    break;
                }
                else if (repositoryIds[i].Equals(StringValueHelper.Id))
                {
                    // special handling of strings, according to spec
                    result = ReadString();
                    break;
                }
                else
                {
                    IValueFactory factory = orb.LookupValueFactory(repositoryIds[i]);
                    if (factory != null)
                    {
                        currentValueIndex = index;
                        result = factory.ReadValue(this);
                        break;
                    }

                    if (i < repositoryIds.Length - 1)
                    {
                        continue;
                    }
                    throw new Marshal("No factory found for: " + repositoryIds[0]);
                }
            }
            return result;
        }

        /// <summary>
        /// try to read in the chunk size. Special handling if there's no chunk size in the stream.
        /// </summary>
        private void ReadChunkSizeTag()
        {
            int savedPos = Pos;
            int savedIndex = Index;
            int chunkSizeTag = ReadLong();

            if (!sunInteropFix || chunkSizeTag > 0 && chunkSizeTag < MaxBlockSize)
            {
                // valid chunk size: set the ending position of the chunk
                chunkEndPos = Pos + chunkSizeTag;
            }
            else
            {
                // reset buffer and remember that we're not within a chunk
                Pos = savedPos;
                Index = savedIndex;

                AdjustPositions();
            }
        }

        /// <summary>
        /// Reads a value with type information, i.e. one that is preceded 
        /// by a single RepositoryID.It is assumed that the tag and the codebase
        /// of the value have already been read.
        /// </summary>
        private object ReadTypedValue(int index, string codebase)
        {
            return ReadUntypedValue(new string[] { ReadRepositoryId() }, index, codebase);
        }

        /// <summary>
        /// Reads a value using the specified factory. The preceeding single RepositoryID is ignored
        /// since the type information is most likely redundant. It is assumed that the tag and the codebase
        /// of the value have already been read.
        /// </summary>
        private object ReadTypedValue(int index, string codebase, IBoxedValueHelper factory)
        {
            string repId = ReadRepositoryId();

            if (!factory.GetId().Equals(repId))
            {
                // just to be sure.
                throw new Marshal("unexpected RepositoryID. expected: " + factory.GetId() + " got: " + repId);
            }

            return factory.ReadValue(this);
        }

        /// <summary>
        /// Reads a value with type information, i.e. one that is preceded 
        /// by an array of RepositoryIDs.It is assumed that the tag and the codebase
        /// of the value have already been read.
        /// </summary>
        private object ReadMultiTypedValue(int index, string codebase)
        {
            int id_count = ReadLong();
            string[] ids = new string[id_count];

            for (int i = 0; i < id_count; i++)
            {
                ids[i] = ReadRepositoryId();
            }

            return ReadUntypedValue(ids, index, codebase);
        }

        /// <summary>
        /// Reads a RepositoryID from the buffer, either directly or via indirection.
        /// </summary>
        private string ReadRepositoryId()
        {
            uint tag = ReadULong();
            if (tag == 0xffffffff)
            {
                // indirection
                int index = ReadLong();
                index = index + Pos - 4;

                //String repId = (String)getRepIdMap().get(Integer.valueOf(index));
                string existingRepId = repIdMap[index];
                if (existingRepId == null)
                {
                    throw new Marshal("stale RepositoryID indirection");
                }
                return existingRepId;
            }

            // a new id
            Pos -= 4;
            Index -= 4;
            int startOffset = Pos;
            string repId = ReadString();
            repIdMap.Add(startOffset, repId);
            return repId;
        }

        /// <summary>
        /// Reads a codebase from the buffer, either directly or via indirection.
        /// </summary>
        private string ReadCodebase()
        {
            uint tag = ReadULong();

            if (tag == 0xffffffff)
            {
                // indirection
                int index = ReadLong();
                index = index + Pos - 4;
                string existingCodebase = codeBaseMap[index];
                if (existingCodebase == null)
                {
                    throw new Marshal("stale codebase indirection");
                }

                return existingCodebase;
            }
            // a new codebase string
            Pos -= 4;
            Index -= 4;
            int startOffset = Pos;
            string codebase = ReadString();
            codeBaseMap.Add(startOffset, codebase);
            return codebase;
        }

        /// <summary>
        /// Reads an indirect value from this stream. It is assumed that the value tag (0xffffffff) has already been read.
        /// </summary>
        private object ReadIndirectValue()
        {
            // indirection
            int index = ReadLong();
            index = index + Pos - 4;
            object value = valueMap[index];

            if (value == null)
            {
                // Java to IDL Language Mapping, v1.1, page 1-44:
                //
                // "The ValueHandler object may receive an IndirectionException
                // from the ORB stream. The ORB input stream throws this exception
                // when it is called to unmarshal a value encoded as an indirection
                // that is in the process of being unmarshaled. This can occur when
                // the ORB stream calls the ValueHandler object to unmarshal an RMI
                // value whose state contains a recursive reference to itself.
                // Because the top-level ValueHandler.readValue call has not yet
                // returned a value, the ORB stream's indirection table contains no
                // entry for an object with the stream offset specified by the
                // indirection tag. This stream offset is returned in the
                // exception's offset field."

                throw new IndirectionException(index);
            }

            return value;
        }


        /// <summary>
        ///  Reads an abstract interface from this stream.The abstract interface
        ///  appears as a union with a boolean discriminator, which is true if the 
        ///  union contains a CORBA object reference, or false if the union contains a value.
        /// </summary>
        public object ReadAbstractInterface()
        {
            return ReadBoolean() ? ReadObject() : ReadValue();
        }

        /// <summary>
        /// Reads an abstract interface from this stream. The abstract interface
        /// appears as a union with a boolean discriminator, which is true if the
        /// union contains a CORBA object reference, or false if the union contains a value.
        /// </summary>
        public object ReadAbstractInterface(Type type)
        {
            return ReadBoolean() ? ReadObject(type) : ReadValue();
        }

        /// <summary>
        /// Stores `value' into this stream's valueMap.  This is provided 
        /// as a callback for value factories, so that a value factory can 
        /// store an object into the map before actually reading its state. 
        /// This is essential for unmarshalling recursive values.
        /// </summary>
        public void RegisterValue(object value)
        {
            valueMap.Add(currentValueIndex, value);
        }


        /// <summary>
        /// update the TypeCodeCache with the contents of the current repeatedTCMap using the 
        /// entries between startPosition and(startPosition + size).
        /// the entries are stored using repositoryID as a key.
        /// </summary>
        public void UpdateTypeCodeCache(string repositoryID, int startPosition, int size)
        {
            if (!orb.CacheTypeCodes)
            {
                return;
            }
            var from = startPosition; //Integer.valueOf(startPosition.intValue());
            var to = startPosition + size;
            var sortedMap = repeatedTCMap.Where(d => d.Key >= from && d.Key <= to).ToArray();
            var toBeCached = new List<(TypeCode typeCode, int position)>();

            foreach (var kvp in sortedMap)
            {
                // only remember the offset between the start of the nested TypeCode
                // and the start of the parent TypeCode here.
                var offset = kvp.Key - startPosition;
                toBeCached.Add((kvp.Value, offset));
            }
            typeCodeCache.TryAdd(repositoryID, toBeCached);
        }

        /// <summary>
        /// Try to locate a TypeCode identified by its repository id in the TypeCodeCache
        /// as cached complex TypeCodes may contain nested TypeCodes.
        /// The repeatedTCMap will be updated with entries for these TypeCodes.
        /// This is necessary so that later indirections pointing to these nested TypeCodes can be resolved.
        /// </summary>
        /// <param name="repositoryID">repository id of the TypeCode that should be looked up in the cache</param>
        /// <param name="startPosition">start position in the buffer of the TypeCode that should be looked up in the cache</param>
        /// <returns>the cached TypeCode or null</returns>
        public TypeCode? ReadTypeCodeCache(string repositoryID, int startPosition)
        {
            List<(TypeCode typeCode, int position)>? result;

            typeCodeCache.TryGetValue(repositoryID, out result);

            if (result == null)
            {
                return null;
            }

            if (result.Count == 0)
            {
                return null;
            }

            for (int i = 0; i < result.Count; i++)
            {
                // calculate the position of the nested TypeCode by adding its offset
                // to the startPosition of the parent TypeCode
                var position = startPosition + result[i].position;
                if (!repeatedTCMap.ContainsKey(position))
                {
                    repeatedTCMap.Add(position, result[i].typeCode);
                }
            }
            return result[0].typeCode;
        }

        /// <summary>
        /// implements ValueInputStream JavaToIDL mapping 02-01-12.
        /// It should read a valuetype header for a nested custom valuetype and increment
        /// the valuetype nesting depth.
        /// </summary>
        public void StartValue()
        {
            int valueTag = ReadLong();

            if (valueTag == 0x00000000)
            {
                // null tag, just do nothing
                return;
            }

            // check that codebase is null
            string codebase = (valueTag & 1) != 0 ? ReadCodebase() : null;
            if (codebase != null)
            {
                throw new Marshal("Custom marshaled value should have null codebase");
            }

            chunkedValue = (valueTag & 0x00000008) != 0;

            // single repository ID
            ReadRepositoryId();

            if (chunkedValue || valueNestingLevel > 0 && !sunInteropFix)
            {
                valueNestingLevel++;
                ReadChunkSizeTag();

                HandleChunking();
            }
        }

        /// <summary>
        /// implements ValueInputStream JavaToIDL mapping 02-01-12. 
        /// It should read the end tag for the nested custom valuetype(after skipping 
        /// any data that precedes the end tag) and decrements the valuetype nesting depth.
        /// </summary>
        public void EndValue()
        {
            // skip rest of chunk to its end
            if (chunkEndPos != -1 && Pos > chunkEndPos)
            {
                Skip(Pos - chunkEndPos);
            }

            HandleChunking();
        }

        public void Dispose()
        {
            Close();
        }

        public decimal ReadLongDouble()
        {
            throw new NoImplement("long double not supported");
        }

        public void ReadLongDoubleArray(ref decimal[] value, int offset, int length)
        {
            throw new NoImplement("long double not supported");
        }
    }
}
