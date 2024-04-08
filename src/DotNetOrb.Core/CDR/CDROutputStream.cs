// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using IOP;
using Marshal = CORBA.Marshal;
using TypeCode = CORBA.TypeCode;
using DotNetty.Buffers;
using DotNetOrb.Core.BufferManager;
using System;
using System.Collections.Generic;

namespace DotNetOrb.Core.CDR
{
    public class CDROutputStream : IOutputStream, CodeSet.OutputBuffer, IValueOutputStream, IDataOutputStream, IDisposable
    {
        private static IOR nullIOR = new IOR("", new TaggedProfile[0]);

        // needed for alignment purposes
        private int index;
        //private readonly IBufferManager bufferManager;
        protected IByteBuffer buffer;

        public IByteBuffer Buffer => buffer;

        /// <summary>Current position in the buffer</summary>
        public int Pos
        {
            get
            {
                if (buffer != null)
                {
                    return buffer.WriterIndex;
                }
                else
                {
                    throw new BadInvOrder("CDRStream not initialized");
                }
            }
            set
            {
                if (buffer != null)
                {
                    buffer.SetWriterIndex(value);
                }
                else
                {
                    throw new BadInvOrder("CDRStream not initialized");
                }
            }
        }

        private bool closed;

        // character encoding code sets for char and wchar, default ISO8859_1 
        private CodeSet codeSet;
        private CodeSet codeSetW;

        private int encapsStart = -1;

        /// <summary>
        /// Used to store encapsulations.
        /// </summary>
        private Stack<EncapsulationInfo> encapsStack = new Stack<EncapsulationInfo>();
        /// <summary>
        /// used to map all value objects that have already been written to this stream to their position within the buffer.
        /// The position is stored as int
        /// </summary>
        private Dictionary<object, int> valueMap = new Dictionary<object, int>();
        /// <summary>
        /// used to map all repository ids that have already been written to this stream to their position within the buffer.
        /// The position is stored as int
        /// </summary>
        private Dictionary<string, int> repIdMap = new Dictionary<string, int>();
        /// <summary>
        /// Used to map all codebase strings that have already been written to this stream to their position within the buffer.
        /// The position is stored as int.
        /// </summary>
        private Dictionary<string, int> codebaseMap = new Dictionary<string, int>();
        private Dictionary<TypeCode, int> repeatedTCMap = new Dictionary<TypeCode, int>();
        private Dictionary<string, int> recursiveTCMap = new Dictionary<string, int>();


        // Remembers the starting position of the current chunk.
        private int chunkSizeTagPos = -1;   // -1 means we're not within a chunk
        private int chunkSizeTagIndex;
        private int chunkOctetsPos;

        // Nesting level of chunked valuetypes
        private int valueNestingLevel = 0;

        // Nesting level of calls write_value_internal
        private int writeValueNestingLevel = 0;

        // True if write_value_internal called writeReplace
        private bool writeReplaceCalled = false;

        private readonly ORB orb;
        public CORBA.ORB ORB => orb;

        private int giopMinor = 2;
        public int GIOPMinor { get { return giopMinor; } set { giopMinor = value; } }

        // The chunking flag is either 0 (no chunking) or 0x00000008 (chunking), to be bitwise or'ed into value tags.
        private int chunkingFlag = 0;

        //private bool codesetEnabled;

        /** configurable properties */
        private bool useBOM = false;
        //private bool chunkCustomRmiValuetypes = false;
        private bool isIndirectionEnabled = true;
        public bool IsIndirectionEnabled { get { return isIndirectionEnabled; } set { isIndirectionEnabled = value; } }
        private bool nullStringEncoding;

        // by default stream version is 1 for GIOP v1.2 messages
        //private byte maxStreamFormatVersion = ValueHandler.STREAM_FORMAT_VERSION_1;

        //private final TypeCodeCompactor typeCodeCompactor;

        //private final static DelegatingTypeCodeWriter typeCodeWriter = new DelegatingTypeCodeWriter();                


        private IExpansionPolicy expansionPolicy;

        /// <summary>
        /// <param name="orb">a DotNetORB</param>
        /// </summary>
        public CDROutputStream(ORB orb, int bufferSize = -1)
        {
            this.orb = orb;
            //bufferManager = orb.BufferManager;
            var configuration = orb.Configuration;

            //codesetEnabled = configuration.GetAsBoolean("DotNetOrb.Codeset", true);

            useBOM = configuration.GetAsBoolean("DotNetOrb.UseBom", false);

            //chunkCustomRmiValuetypes = configuration.getAttributeAsBoolean("DotNetOrb.interop.chunk_custom_rmi_valuetypes", false);

            isIndirectionEnabled = configuration.GetAsBoolean("DotNetOrb.Interop.IndirectionEncoding", true);

            nullStringEncoding = configuration.GetAsBoolean("DotNetOrb.Interop.NullStringEncoding", false);

            expansionPolicy = configuration.GetAsObject("DotNetOrb.BufferManager.ExpansionPolicy", typeof(DefaultExpansionPolicy).AssemblyQualifiedName, configuration) as IExpansionPolicy;

            //maxStreamFormatVersion = (byte)configuration.getAttributeAsInteger("DotNetOrb.interop.maximum_stream_format_version", 1);

            codeSet = orb.DefaultCodeSetChar;
            codeSetW = orb.DefaultCodeSetWChar;

            if (bufferSize == -1)
            {
                buffer = PooledByteBufferAllocator.Default.Buffer();
            }
            else
            {
                buffer = PooledByteBufferAllocator.Default.Buffer(bufferSize);
            }
            buffer.Retain();
        }

        public CDROutputStream(ORB orb, IByteBuffer buffer, int giopMinor) : this(orb)
        {
            this.buffer = buffer;
            this.giopMinor = giopMinor;
        }

        public CDROutputStream(ORB orb, byte[] buffer, int giopMinor) : this(orb)
        {
            this.buffer = Unpooled.WrappedBuffer(buffer);
            this.giopMinor = giopMinor;
        }

        /// <summary>
        /// OutputStreams created using  the empty constructor are used for 
        /// in  memory marshaling. A stream created with this ctor is not explicitly
        /// configured, i.e.it will use default configuration only
        /// </summary>
        public CDROutputStream() : this(CORBA.ORB.Init() as ORB)
        {

        }

        /// <summary>
        /// Start a CDR encapsulation. All subsequent writes will place data in the encapsulation until
        /// endEncapsulation is called.This will write the size of the encapsulation.
        /// </summary>
        public void BeginEncapsulation()
        {
            // align to the next four byte boundary
            // as a preparation for writing the size
            // integer (which we don't know before the
            // encapsulation is closed)
            Check(8, 4);
            // leave 4 bytes for the encaps. size that
            // is to be written later
            Pos += 4;
            index += 4;
            //Because encapsulations can be nested, we need to remember the beginnning of the enclosing
            //encapsulation (or -1 if we are in the outermost encapsulation)
            //Also, remember the current index and the indirection maps because
            //we need to restore these when closing the encapsulation
            encapsStack.Push(new EncapsulationInfo(index, encapsStart, valueMap, repIdMap, codebaseMap));

            // set up new indirection maps for this encapsulation
            valueMap = new Dictionary<object, int>();
            repIdMap = new Dictionary<string, int>();
            codebaseMap = new Dictionary<string, int>();

            // the start of this encapsulation
            encapsStart = Pos;
            BeginEncapsulatedArray();
        }

        /// <summary>
        /// Can be used locally for data type conversions without preceeding call to beginEncapsulation, 
        /// i.e. without a leading long that indicates the size.
        /// </summary>
        public void BeginEncapsulatedArray()
        {
            /* set the index for alignment to 0, i.e. align relative to the
               beginning of the encapsulation */
            index = 0;

            // byte_order flag set to FALSE

            buffer.SetByte(Pos++, 0);
            index++;
        }

        /// <summary>
        /// Terminate the encapsulation by writing its length to its beginning.
        /// </summary>
        public void EndEncapsulation()
        {
            if (encapsStart == -1)
            {
                throw new Marshal("Too many end-of-encapsulations");
            }

            if (encapsStack == null)
            {
                throw new Marshal("Internal Error - closeEncapsulation failed");
            }

            // determine the size of this encapsulation

            int encapsSize = Pos - encapsStart;

            // insert the size integer into the appropriate place
            buffer.SetInt(encapsStart - 4, encapsSize);

            // restore index and encapsStart information and indirection maps

            EncapsulationInfo ei = encapsStack.Pop();
            encapsStart = ei.Start;
            index = ei.Index + encapsSize;
            valueMap = ei.ValueMap;
            repIdMap = ei.RepIdMap;
            codebaseMap = ei.CodebaseMap;
        }

        /// <summary>
        /// This version of check does both array length checking and data type alignment.        
        /// </summary>
        public void Check(int i, int align)
        {
            int remainder = align - index % align;

            Check(i + remainder);

            if (remainder != align)
            {
                // Clear padding. Allowing for possible buffer end.
                int blp = buffer.WritableBytes;
                int topad = blp <= 8 ? blp : 8;
                for (int j = Pos; j < Pos + topad; j++)
                {
                    buffer.SetByte(j, 0);
                }
                index += remainder;
                Pos += remainder;
            }
        }

        /// <summary>
        /// check whether the current buffer is big enough to receive i more bytes. If it isn't, get a bigger buffer.        
        /// </summary>
        private void Check(int i)
        {
            int requiredSize = i + 2;
            if (buffer == null)
            {
                buffer = PooledByteBufferAllocator.Default.Buffer();
            }
            else if (requiredSize > buffer.WritableBytes)
            {
                var newSize = buffer.Capacity + expansionPolicy.GetExpandedSize(requiredSize);
                buffer.AdjustCapacity(newSize);
            }
        }

        /// <summary>
        /// write the contents of this CDR stream to the output stream called by, e.g.GIOPConnection 
        /// to write directly to the wire.
        /// </summary>
        //public void Write(Stream outputStream, int start, int length)
        //{
        //    if (start < start + length)
        //    {
        //        outputStream.Write(buffer, start, length);
        //    }
        //}

        public void Close()
        {
            // Don't need to call super.close as super is noop.
            if (closed)
            {
                return;
            }
            if (buffer != null)
            {
                buffer.Release();
            }
            //bufferManager.ReturnBuffer(buffer, true);

            buffer = null;
            closed = true;
        }

        public byte[] GetBufferCopy()
        {
            int length = buffer.ReadableBytes;
            byte[] array = new byte[length];
            buffer.GetBytes(buffer.ReaderIndex, array);
            return array;
        }

        public void Skip(int step)
        {
            Pos += step;
            index += step;
        }

        public void ReduceSize(int amount)
        {
            Pos -= amount;
        }

        public void IncreaseSize(int amount)
        {
            Check(amount);

            Pos += amount;
        }

        /// <summary>
        /// Replace existing buffer by a new one and reset indices.
        /// </summary>
        public void SetBuffer(IByteBuffer b)
        {
            //bufferManager.ReturnBuffer(buffer, true);
            if (buffer != null)
            {
                buffer.Release();
            }
            buffer = b;
            Pos = 0;
            index = 0;
        }

        /// <summary>
        /// Replace existing buffer by a new one and reset indices.
        /// </summary>
        public void SetBuffer(byte[] b)
        {
            //bufferManager.ReturnBuffer(buffer, true);
            if (buffer != null)
            {
                buffer.Release();
            }
            buffer = Unpooled.WrappedBuffer(b);
            Pos = 0;
            index = 0;
        }

        /// <summary>
        /// Give up the buffer to the caller and mark the stream closed. It is
        /// up to the caller to eventually return the buffer to the manager.
        /// </summary>
        public IByteBuffer ReleaseBuffer()
        {
            var retn = buffer;
            buffer = null;
            Close();
            return retn;
        }

        public IInputStream CreateInputStream()
        {
            //byte[] result = new byte[index];
            //Array.Copy(buffer, result, result.Length);
            return new CDRInputStream(orb, buffer);
        }

        public void WriteAny(CORBA.Any value)
        {
            if (value == null)
            {
                throw new Marshal("Cannot marshall null value.");
            }
            WriteTypeCode(value.Type);
            value.WriteValue(this);
        }

        public void WriteBoolean(bool value)
        {
            Check(1);
            if (value)
            {
                buffer.SetByte(Pos++, 1);
            }
            else
            {
                buffer.SetByte(Pos++, 0);
            }
            index++;
        }

        public void WriteBooleanArray(bool[] value, int offset, int length)
        {
            if (value != null)
            {
                //no alignment necessary
                Check(length);
                for (int i = offset; i < offset + length; i++)
                {
                    if (value[i])
                    {
                        buffer.SetByte(Pos++, 1);
                    }
                    else
                    {
                        buffer.SetByte(Pos++, 0);
                    }
                }
                index += length;
            }
        }

        /// <summary>
        /// Writes a character to the output stream. If codeset translation is active then it will use String and
        /// an encoding to get the bytes.It can then do a test for whether to throw DATA_CONVERSION.     
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="DataConversion"></exception>
        public void WriteChar(char value)
        {
            // According to 15.3.1.6 of CORBA 3.0 'a single instance of the char type
            // may only hold one octet of any multi-byte character encoding.'
            // Therefore we ensure that we are in the single byte range i.e.
            // less than 0xFF or \377.
            if (value > 0xFF)
            {
                throw new DataConversion("Char " + value + " out of range");
            }
            Check(1);
            buffer.SetByte(Pos++, (byte)value);
            index++;
        }

        public void WriteCharArray(char[] value, int offset, int length)
        {
            if (value == null)
            {
                throw new Marshal("Cannot marshall null array.");
            }
            else if (offset + length > value.Length || length < 0 || offset < 0)
            {
                throw new Marshal("Cannot marshall as indices for array are out bounds.");
            }

            Check(length);

            for (int i = offset; i < length + offset; i++)
            {
                if (value[i] > 0xFF)
                {
                    throw new DataConversion("Char " + value[i] + " out of range");
                }
                buffer.SetByte(Pos++, (byte)value[i]);
            }
            index += length;
        }

        /// <summary>
        /// Writes a string to the output stream. It is optimised for whether 
        /// it is writing a blank string or for whether codeset translation is active.
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="org.omg.CORBA.Marshal"></exception>
        public void WriteString(string value)
        {
            if (value == null)
            {
                if (nullStringEncoding)
                {
                    WriteLong(0);
                    return;
                }
                else
                {
                    throw new Marshal("Cannot marshall null string.");
                }
            }
            int valueLength = value.Length;
            // size leaves room for ulong, plus the string itself (one or more
            // bytes per char in the string, depending on the codeset), plus the
            // terminating NUL char
            int size;
            // sizePosition is the position in the buffer for the size to be
            // written.
            int sizePosition;

            if (valueLength == 0)
            {
                // Blank string; just write size and terminator. Don't need to
                // align here as these methods will do it for us.
                WriteLong(1);
                WriteOctet(0);
            }
            else
            {
                // in the worst case (UTF-8) a string char might take up to 4 bytes
                size = 4 + 4 * valueLength + 1;
                Check(size, 4);
                sizePosition = Pos;

                Pos += 4;
                index += 4;

                codeSet.WriteString(this, value, false, false, giopMinor);
                buffer.SetByte(Pos++, 0); //terminating NUL char
                index++;

                // Now write the size back in.
                size = Pos - (sizePosition + 4); // compute translated size                
                buffer.SetInt(sizePosition, size);
            }
        }

        public void WriteWChar(char value)
        {
            // maximum is UTF-16 handling non-BPM character with BOM
            // alignment/check must happen prior to calling.
            Check(6);
            codeSetW.WriteChar(this, value, codeSetW.WriteBom(useBOM), true, giopMinor);
        }

        public void WriteByte(byte b)
        {
            buffer.SetByte(Pos++, b);
            index++;
        }

        public void WriteWCharArray(char[] value, int offset, int length)
        {
            if (value == null)
            {
                throw new Marshal("Null References");
            }

            Check(length * 3);

            for (int i = offset; i < offset + length; i++)
            {
                WriteWChar(value[i]);
            }
        }

        public void WriteWString(string value)
        {
            if (value == null)
            {
                throw new Marshal("Null References");
            }

            // size ulong + no of bytes per char (max 3 if UTF-8) + terminating NUL
            Check(4 + value.Length * 3 + 3, 4);

            int startPos = Pos;         // store position for length indicator
            Pos += 4;
            index += 4;                 // reserve for length indicator

            // the byte order marker
            if (GIOPMinor == 2 && codeSetW.WriteBom(useBOM) && value.Length > 0)
            {
                // big endian encoding
                buffer.SetByte(Pos++, 0xFE);
                buffer.SetByte(Pos++, 0xFF);
                index += 2;
            }

            // write characters in current wide encoding, add null terminator
            for (int i = 0; i < value.Length; i++)
            {
                codeSetW.WriteChar(this, value[i], false, false, giopMinor);
            }

            int strSize;
            if (giopMinor >= 2)
            {
                // size in bytes (without the size ulong)
                strSize = Pos - startPos - 4;
            }
            else
            {
                // terminating NUL char
                codeSetW.WriteChar(this, (char)0, false, false, giopMinor);
                strSize = codeSetW.GetWStringSize(value, startPos, Pos);
            }

            // write length indicator
            buffer.SetInt(startPos, strSize);
        }


        public void WriteContext(IContext value)
        {
            throw new NotImplementedException();
        }

        public void WriteDouble(double value)
        {
            Check(15, 8);
            buffer.SetDouble(Pos, value);
            index += 8;
            Pos += 8;
        }

        public void WriteDoubleArray(double[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 8 byte boundary
            Check(7 + length * 8, 8);

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetDouble(Pos, value[i]);
                    Pos += 8;
                }
                index += 8 * length;
            }
        }

        public void WriteFixed(decimal value, ushort digits, ushort scale)
        {
            var truncated = Math.Round(value, scale);

            int[] parts = decimal.GetBits(truncated);

            bool sign = (parts[3] & 0x80000000) != 0;

            //byte decimalScale = (byte)((parts[3] >> 16) & 0x7F);                        

            decimal unscaledValue = new decimal(parts[0], parts[1], parts[2], false, 0);

            string format = new string('0', digits);
            string v = unscaledValue.ToString(format);
            if (v.Length > digits)
            {
                throw new Marshal($"Unable to marshal fixed<{digits},{scale}> with {v.Length} digits");
            }
            byte[] representation;
            int b, c;

            if (v.Length % 2 == 0)
            {
                representation = new byte[v.Length / 2 + 1];
                representation[0] = 0x00;

                for (int i = 0; i < v.Length; i++)
                {
                    c = (int)char.GetNumericValue(v[i]);
                    b = representation[(1 + i) / 2] << 4;
                    b |= c;
                    representation[(1 + i) / 2] = (byte)b;
                }
            }
            else
            {
                representation = new byte[(v.Length + 1) / 2];
                for (int i = 0; i < v.Length; i++)
                {
                    c = (int)char.GetNumericValue(v[i]);
                    b = representation[i / 2] << 4;
                    b |= c;
                    representation[i / 2] = (byte)b;
                }
            }
            b = representation[representation.Length - 1] << 4;

            representation[representation.Length - 1] = (byte)(sign ? b | 0xD : b | 0xC);

            Check(representation.Length);
            //Array.Copy(representation, 0, buffer, Pos, representation.Length);
            buffer.SetBytes(Pos, representation, 0, representation.Length);
            index += representation.Length;
            Pos += representation.Length;
        }

        public void WriteFloat(float value)
        {
            Check(7, 4);
            buffer.SetFloat(Pos, value);
            Pos += 4;
            index += 4;
        }

        public void WriteFloatArray(float[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 4 byte boundary
            Check(3 + length * 4, 4);

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetFloat(Pos, value[i]);
                    Pos += 4;
                }
                index += 4 * length;
            }
        }

        public void WriteLong(int value)
        {
            Check(7, 4);
            buffer.SetInt(Pos, value);
            Pos += 4;
            index += 4;
        }

        public void WriteLongArray(int[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 4 byte boundary
            Check(3 + length * 4, 4);

            int remainder = 4 - index % 4;
            if (remainder != 4)
            {
                index += remainder;
                Pos += remainder;
            }

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetInt(Pos, value[i]);
                    Pos += 4;
                }
                index += 4 * length;
            }
        }

        public void WriteLongLong(long value)
        {
            Check(15, 8);
            buffer.SetLong(Pos, value);
            index += 8;
            Pos += 8;
        }

        public void WriteLongLongArray(long[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            Check(7 + length * 8, 8);

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetLong(Pos, value[i]);
                    Pos += 8;
                }
                index += 8 * length;
            }
        }

        public void WriteObject(IObject value)
        {
            if (value == null)
            {
                IORHelper.Write(this, nullIOR);
            }
            else
            {
                if (value is LocalObject)
                {
                    throw new Marshal("Attempt to serialize a locality-constrained object.");
                }
                CORBA.Object obj = (CORBA.Object)value;

                IOR intermediary = ((Delegate)obj._Delegate).GetIOR();

                IORHelper.Write(this, intermediary);
            }
        }

        public void WriteIOR(IOR ior)
        {
            if (ior == null)
            {
                IORHelper.Write(this, nullIOR);
            }
            else
            {
                IORHelper.Write(this, ior);
            }
        }

        public void WriteOctet(byte value)
        {
            Check(1);
            index++;
            buffer.SetByte(Pos++, value);
        }

        public void WriteOctetArray(byte[] value, int offset, int length)
        {
            if (value != null)
            {
                Check(length);
                //Array.Copy(value, offset, buffer, Pos, length);
                buffer.SetBytes(Pos, value, offset, length);
                index += length;
                Pos += length;
            }
        }

        public void WriteShort(short value)
        {
            Check(3, 2);

            buffer.SetByte(Pos++, (byte)(value >> 8 & 0xFF));
            buffer.SetByte(Pos++, (byte)(value & 0xFF));
            index += 2;
        }

        public void WriteShortArray(short[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 2-byte boundary
            Check(2 * length + 3);

            int remainder = 2 - index % 2;
            if (remainder != 2)
            {
                index += remainder;
                Pos += remainder;
            }

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetByte(Pos++, (byte)(value[i] >> 8 & 0xFF));
                    buffer.SetByte(Pos++, (byte)(value[i] & 0xFF));
                }
                index += 2 * length;
            }
        }

        public void WriteTypeCode(TypeCode typeCode)
        {
            if (typeCode == null)
            {
                throw new BadParam("TypeCode is null");
            }

            if (orb.CompactTypeCodes)
            {
                if (orb.CompactedTypeCodeCache.ContainsKey(typeCode.Id))
                {
                    typeCode = orb.CompactedTypeCodeCache[typeCode.Id];
                }
                else
                {
                    typeCode = typeCode.CompactTypeCode;
                    orb.CompactedTypeCodeCache[typeCode.Id] = typeCode;
                }
            }

            if (repeatedTCMap == null)
            {
                repeatedTCMap = new Dictionary<TypeCode, int>();
            }

            if (recursiveTCMap == null)
            {
                recursiveTCMap = new Dictionary<string, int>();
            }

            try
            {
                WriteTypeCode(typeCode, recursiveTCMap, repeatedTCMap);
            }
            finally
            {
                repeatedTCMap.Clear();
                recursiveTCMap.Clear();
            }
        }

        public void WriteTypeCode(TypeCode typeCode, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            typeCode.WriteTypeCode(this, recursiveTCMap, repeatedTCMap);
        }

        public void WriteULong(uint value)
        {
            WriteLong((int)value);
        }

        public void WriteULongArray(uint[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 4 byte boundary
            Check(3 + length * 4, 4);

            int remainder = 4 - index % 4;
            if (remainder != 4)
            {
                index += remainder;
                Pos += remainder;
            }

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetInt(Pos, (int)value[i]);
                    Pos += 4;
                }
                index += 4 * length;
            }
        }

        public void WriteULongLong(ulong value)
        {
            WriteLongLong((long)value);
        }

        public void WriteULongLongArray(ulong[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            Check(7 + length * 8, 8);

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetLong(Pos, (long)value[i]);
                    Pos += 8;
                }
                index += 8 * length;
            }
        }

        public void WriteUShort(ushort value)
        {
            WriteShort((short)value);
        }

        public void WriteUShortArray(ushort[] value, int offset, int length)
        {
            // if nothing has to be written, return, and especially DON'T ALIGN
            if (length == 0)
            {
                return;
            }

            // align to 2-byte boundary
            Check(2 * length + 3);

            int remainder = 2 - index % 2;
            if (remainder != 2)
            {
                index += remainder;
                Pos += remainder;
            }

            if (value != null)
            {
                for (int i = offset; i < offset + length; i++)
                {
                    buffer.SetByte(Pos++, (byte)(value[i] >> 8 & 0xFF));
                    buffer.SetByte(Pos++, (byte)(value[i] & 0xFF));
                }
                index += 2 * length;
            }
        }

        public void WriteValue(object value)
        {
            if (!WriteValueIndirection(value))
            {
                if (value is string)
                {
                    WriteValueInternal(value, new string[] { WStringValueHelper.Id });
                }
                else if (value is IValueBase valueBase)
                {
                    WriteValueInternal(value, valueBase._TruncatableIds);
                }
                else
                {
                    throw new BadParam("Object is not a IValueBase");
                }
            }
        }

        public void WriteValue(object value, string repositoryId)
        {
            if (!WriteValueIndirection(value))
            {
                if (string.IsNullOrEmpty(repositoryId))
                {
                    WriteValueInternal(value, null);
                }
                else
                {
                    WriteValueInternal(value, new string[] { repositoryId });
                }
            }
        }

        public void WriteValue(object value, string[] repositoryIds)
        {
            if (!WriteValueIndirection(value))
            {
                WriteValueInternal(value, repositoryIds);
            }
        }

        private bool WriteValueIndirection(object value)
        {
            if (value == null)
            {
                // null tag
                WriteLong(0x00000000);
                return true;
            }

            if (valueMap.ContainsKey(value))
            {
                var index = valueMap[value];
                // value has already been written -- make an indirection
                WriteULong(0xffffffff);
                WriteLong(index - Pos);
                return true;
            }
            return false;
        }

        private void WriteRepositoryId(string repositoryId)
        {
            if (repIdMap.ContainsKey(repositoryId))
            {
                // a previously written repository id -- make an indirection
                var _index = repIdMap[repositoryId];
                WriteULong(0xffffffff);
                WriteLong(_index - Pos);
            }
            else
            {
                // a new repository id -- write it
                // first make sure the pos we're about to remember is
                // a correctly aligned one, i.e., the actual writing position
                int remainder = 4 - index % 4;
                if (remainder != 4)
                {
                    index += remainder;
                    Pos += remainder;
                }
                repIdMap[repositoryId] = Pos;
                WriteString(repositoryId);
            }
        }

        private void WriteCodebase(string codebase)
        {
            if (codebaseMap.ContainsKey(codebase))
            {
                var _index = codebaseMap[codebase];
                // a previously written codebase -- make an indirection
                WriteULong(0xffffffff);
                WriteLong(_index - Pos);
            }
            else
            {
                // a new codebase -- write it#

                // first make sure the pos we're about to remember is
                // a correctly aligned one
                int remainder = 4 - index % 4;
                if (remainder != 4)
                {
                    index += remainder;
                    Pos += remainder;
                }
                codebaseMap[codebase] = Pos;
                WriteString(codebase);
            }
        }

        /// <summary>
        /// Writes to this stream a value header with the specified `repository_id' and no codebase string.
        /// </summary>
        /// <param name="repositoryIds"></param>        
        private void WriteValueHeader(string[] repositoryIds)
        {
            if (repositoryIds != null)
            {
                if (repositoryIds.Length > 1)
                {
                    // truncatable value type, must use chunking!

                    chunkingFlag = 0x00000008;
                    WriteLong(0x7fffff06 | chunkingFlag);
                    WriteLong(repositoryIds.Length);
                    for (int i = 0; i < repositoryIds.Length; i++)
                    {
                        WriteRepositoryId(repositoryIds[i]);
                    }
                }
                else
                {
                    WriteLong(0x7fffff02 | chunkingFlag);
                    WriteRepositoryId(repositoryIds[0]);
                }
            }
            else
            {
                WriteLong(0x7fffff00 | chunkingFlag);
            }
        }

        /// <summary>
        /// Writes to this stream a value header with the specified `repository_id' and codebase string.
        /// </summary>
        /// <param name="repositoryIds"></param>
        /// <param name="codebase"></param>
        private void WriteValueHeader(string[] repositoryIds, string codebase)
        {
            if (codebase != null)
            {
                if (repositoryIds != null)
                {
                    if (repositoryIds.Length > 1)
                    {
                        // truncatable value type, must use chunking!

                        chunkingFlag = 0x00000008;
                        WriteLong(0x7fffff07 | chunkingFlag);
                        WriteCodebase(codebase);
                        WriteLong(repositoryIds.Length);

                        for (int i = 0; i < repositoryIds.Length; i++)
                        {
                            WriteRepositoryId(repositoryIds[i]);
                        }
                    }
                    else
                    {
                        WriteLong(0x7fffff03 | chunkingFlag);
                        WriteCodebase(codebase);
                        WriteRepositoryId(repositoryIds[0]);
                    }
                }
                else
                {
                    WriteLong(0x7fffff01 | chunkingFlag);
                    WriteCodebase(codebase);
                }
            }
            else
            {
                WriteValueHeader(repositoryIds);
            }
        }

        public void WriteValue(object value, IBoxedValueHelper factory)
        {
            if (!WriteValueIndirection(value))
            {
                WritePreviousChunkSize();
                Check(7, 4);
                valueMap.Add(value, Pos);

                if (value is IIDLEntity || IsSimpleString(value, factory))
                {
                    WriteLong(0x7fffff00 | chunkingFlag);
                }
                else
                {
                    // repository id is required for RMI: types
                    WriteLong(0x7fffff02 | chunkingFlag);

                    string repId = factory.GetId();
                    WriteRepositoryId(repId);
                }
                StartChunk();
                factory.WriteValue(this, value);
                EndChunk();
            }
        }

        /// <summary>
        /// Check if the specified value may be marshalled as a simple string (length, value) or it should be encoded as a valuetype.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        private bool IsSimpleString(object value, IBoxedValueHelper factory)
        {
            if (!(value is string))
            {
                return false;
            }

            if (factory.GetId().Equals(StringValueHelper.Id))
            {
                return false;
            }

            if (factory.GetId().Equals(WStringValueHelper.Id))
            {
                return false;
            }
            return true;
        }


        /// <summary>
        /// Start a new chunk, end any previously started chunk (no nesting!)
        /// </summary>
        private void StartChunk()
        {
            if (chunkingFlag > 0)
            {
                WritePreviousChunkSize();
                valueNestingLevel++;
                SkipChunkSizeTag();
            }
        }

        private void EndChunk()
        {
            if (chunkingFlag > 0)
            {
                WritePreviousChunkSize();
                WriteLong(-valueNestingLevel);
                if (--valueNestingLevel == 0)
                {
                    // ending chunk for outermost value
                    chunkingFlag = 0;
                }
                else
                {
                    // start continuation chunk for outer value
                    SkipChunkSizeTag();
                }
            }
        }

        /// <summary>
        /// Writes the chunk size to the header of the previous chunk
        /// </summary>
        private void WritePreviousChunkSize()
        {
            if (chunkSizeTagPos != -1)
            {
                if (Pos == chunkOctetsPos)
                {
                    // empty chunk: erase chunk size tag
                    Pos = chunkSizeTagPos;      // the tag will be overwritten
                    index = chunkSizeTagIndex;  //            by subsequent data
                }
                else
                {
                    // go to the beginning of the chunk and write the size tag
                    RewriteLong(chunkSizeTagPos, chunkSizeTagIndex, Pos - chunkOctetsPos);
                }
                chunkSizeTagPos = -1; // no chunk is currently open
            }
        }


        private void SkipChunkSizeTag()
        {
            // remember where we are right now,
            chunkSizeTagPos = Pos;
            chunkSizeTagIndex = index;

            // insert four bytes here as a place-holder
            WriteLong(0); // need to go back later and write the actual size

            // remember starting position of chunk data
            chunkOctetsPos = Pos;
        }

        /// <summary>
        /// Writes a CORBA long value to (writePos, writeIndex) without clearing the buffer padding.
        /// In the case of a non-sequential write, clearing buffer positions after the data just written 
        /// is likely to erase data previously written.
        /// </summary>
        /// <param name="writePos"></param>
        /// <param name="writeIndex"></param>
        /// <param name="value"></param>
        private void RewriteLong(int writePos, int writeIndex, int value)
        {
            int align = 4;
            int remainder = align - writeIndex % align;
            if (remainder != align)
            {
                writePos += remainder;
            }
            buffer.SetInt(writePos, value);
        }


        private void WriteValueInternal(object value, string[] repositoryIds)
        {
            WritePreviousChunkSize();
            Check(7, 4);
            valueMap[value] = Pos;

            if (value is string str)
            {
                // special handling for strings required according to spec
                WriteValueHeader(repositoryIds);
                StartChunk();
                WriteWString(str);
                EndChunk();
            }
            else if (value is IStreamableValue streamable)
            {
                WriteValueHeader(streamable._TruncatableIds);
                StartChunk();
                streamable._Write(this);
                EndChunk();
            }
            else if (value is ICustomValue customValue)
            {
                IDataOutputStream outputStream = new DataOutputStream(this);
                WriteValueHeader(customValue._TruncatableIds);
                customValue._Marshal(outputStream);
            }
            else if (value is IIDLEntity)
            {
                WriteValueHeader(repositoryIds);
                StartChunk();
                if (value is Any any)
                {
                    WriteAny(any);
                }
                else
                {
                    var helperType = RepositoryId.GetHelperType(repositoryIds[0]);
                    try
                    {
                        var instance = Activator.CreateInstance(helperType);
                        var methodInfo = helperType.GetMethod("Write");
                        if (methodInfo != null)
                        {
                            object[] parametersArray = new object[] { this, value };
                            var result = methodInfo.Invoke(instance, parametersArray);
                        }
                        else
                        {
                            throw new Marshal("Method Write not found in helper class");
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Marshal("Error loading class " + helperType.FullName + ": " + ex);
                    }
                    //var helperClassName = value.GetType().FullName + "Helper";
                    //try
                    //{
                    //    var helperType = Type.GetType(helperClassName);
                    //    if (helperType != null)
                    //    {
                    //        var instance = Activator.CreateInstance(helperType);
                    //        var methodInfo = helperType.GetMethod("Write");
                    //        if (methodInfo != null)
                    //        {
                    //            object[] parametersArray = new object[] { this, value };
                    //            var result = methodInfo.Invoke(instance, parametersArray);
                    //        }
                    //        else
                    //        {
                    //            throw new Marshal("Method Write not found in helper class");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        throw new Marshal("Error loading class " + helperClassName);
                    //    }

                    //}
                    //catch (Exception e)
                    //{
                    //    throw new Marshal("Error loading class " + helperClassName + ": " + e);
                    //}
                }
                EndChunk();
            }
            else
            {
                throw new Marshal("Value is not an IDLEntity");
            }
        }

        /// <summary>
        ///  Writes an abstract interface to this stream. The abstract interface is written as a union 
        ///  with a boolean discriminator, which is true if the union contains a CORBA object reference,
        ///  or false if the union contains a value.
        /// </summary>
        /// <param name="obj"></param>
        public void WriteAbstractInterface(object obj)
        {
            if (obj is CORBA.Object)
            {
                WriteBoolean(true);
                WriteObject((CORBA.Object)obj);
            }
            else
            {
                WriteBoolean(false);
                WriteValue(obj);
            }
        }

        /// <summary>        
        /// It should end any currently open chunk, write a valuetype header for a nested custom valuetype 
        /// (with a null codebase and the specified repository ID), and increment the valuetype nesting depth.
        /// </summary>
        /// <param name="rep_id"></param>
        public void StartValue(string repId)
        {
            WriteValueHeader(new string[] { repId });
            StartChunk();
        }

        /// <summary>
        /// It should end any currently open chunk, write the end tag for the nested custom valuetype, 
        /// and decrement the valuetype nesting depth.
        /// </summary>
        public void EndValue()
        {
            EndChunk();
        }

        public void Dispose()
        {
            Close();
        }

        public void WriteLongDouble(decimal value)
        {
            throw new NoImplement("long double not supported");
        }

        public void WriteLongDoubleArray(decimal[] value, int offset, int length)
        {
            throw new NoImplement("long double not supported");
        }
    }

}
