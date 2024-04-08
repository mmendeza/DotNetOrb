// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using CORBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetOrb.Core.CDR
{
    public class CodeSet
    {
        public interface InputBuffer
        {

            /// <summary>
            /// The current position in the buffer
            /// </summary>
            int Pos { get; }

            /// <summary>
            /// Reads the next byte from an in-memory buffer
            /// </summary>
            byte ReadByte();

            /// <summary>
            /// Looks ahead in the buffer to see if a byte-order marker is present. If so, reads it from the buffer
            /// and returns the result.
            /// </summary>
            /// <returns>
            /// true if a marker indicating little-endian was read.
            /// </returns>
            bool ReadBOM();
        }

        /// <summary>
        /// Represents a buffer to which character data may be written
        /// </summary>
        public interface OutputBuffer
        {
            /// <summary>
            /// Writes the specified byte to the buffer
            /// </summary>
            /// <param name="b">The byte to write</param>
            void WriteByte(byte b);


            /// <summary>
            /// Forces short (2-byte) alignment and writes the specified value to the buffer.
            /// </summary>
            /// <param name="value">The byte to write</param>
            void WriteShort(short value);

            /// <summary>
            /// Write an array of bytes to the buffer
            /// </summary>
            /// <param name="b"></param>
            /// <param name="offset"></param>
            /// <param name="length"></param>
            void WriteOctetArray(byte[] b, int offset, int length);
        }

        static string CodeSetPrefix = "0x00000000";

        /// <summary>represents the base 7-bits of ISO8859_1</summary>        
        public static CodeSet ASCII_CODESET = new AsciiCodeSet();

        /// <summary>
        /// represents the default 8-bit codeset. 
        /// It is ISO 8859-1:1987; Latin Alphabet No. 1
        /// </summary>
        public static CodeSet ISO8859_1_CODESET = new Iso8859_1CodeSet();


        /// <summary>represents Latin Alphabet No. 9</summary>         
        public static CodeSet ISO8859_15_CODESET = new Iso8859_15CodeSet();

        /// <summary>
        /// represents UTF8 1-6 bytes for every character
        /// X/Open UTF-8; UCS Transformation Format 8 (UTF-8)
        /// </summary>         
        public static CodeSet UTF8_CODESET = new Utf8CodeSet();

        /// <summary>
        /// represents extended UCS2, 2 or 4 bytes for every char
        /// ISO/IEC 10646-1:1993; UTF-16, UCS Transformation Format 16-bit form
        /// </summary>         
        public static CodeSet UTF16_CODESET = new Utf16CodeSet();

        /// <summary>
        /// represents UCS2, 2bytes for every char
        /// ISO/IEC 10646-1:1993; UTF-16, UCS Transformation Format 16-bit form
        /// </summary>         
        public static CodeSet UCS2_CODESET = new Ucs2CodeSet();

        public static CodeSet MAC_ROMAN_CODESET = new MacRomanCodeSet();

        /// <summary>All of the encodings supported. These should be listed in order of preference.</summary>         
        public static CodeSet[] KnownEncodings = { ISO8859_1_CODESET, ISO8859_15_CODESET, UTF16_CODESET, UTF8_CODESET, UCS2_CODESET, MAC_ROMAN_CODESET, ASCII_CODESET };

        public static string DefaultPlatformEncoding = Encoding.Default.BodyName;

        private readonly uint id;
        /// <summary>
        /// The standard CORBA identifier associated with this code set; used during negotiation        
        /// </summary>
        public uint Id { get => id; }

        private readonly string name;
        /// <summary>
        /// The canonical name of this code set        
        /// </summary>
        public string Name { get => name; }

        private readonly bool isAlias;
        /// <summary>
        /// Identify this codeset as a local alias of some shared codeset and thus not to be added to the IOR        
        /// </summary>
        public bool IsAlias { get => isAlias; }

        public CodeSet(uint id, string name)
        {
            this.id = id;
            this.name = name;
            isAlias = false;
        }

        public CodeSet(uint id, string name, bool isAlias)
        {
            this.id = id;
            this.name = name;
            this.isAlias = isAlias;
        }

        /// <summary>
        /// Returns the code set which matches the specified name, which should either be the canonical name of
        /// a supported encoding or the hex representation of its ID.
        /// </summary>
        public static CodeSet GetCodeSet(string name)
        {
            string ucName = name.ToUpper();
            for (int i = 0; i < KnownEncodings.Length; i++)
            {
                CodeSet codeset = KnownEncodings[i];
                if (codeset.Name.Equals(ucName)) return codeset;
            }

            try
            {
                int id = int.Parse(name, System.Globalization.NumberStyles.HexNumber);
                for (int i = 0; i < KnownEncodings.Length; i++)
                {
                    CodeSet codeset = KnownEncodings[i];
                    if (id == codeset.Id) return codeset;
                }
                throw new CodeSetIncompatible(2, CompletionStatus.Maybe);
            }
            catch (FormatException ex)
            {
                throw new CodeSetIncompatible(2, CompletionStatus.Maybe);
            }
        }

        /// <summary>
        /// Returns the code set which matches the specified ID.        
        /// </summary>
        public static CodeSet GetCodeSet(uint id)
        {
            for (int i = 0; i < KnownEncodings.Length; i++)
            {
                CodeSet codeset = KnownEncodings[i];
                if (id == codeset.id) return codeset;
            }
            throw new CodeSetIncompatible(2, CompletionStatus.Maybe);
        }

        /// <summary>
        /// Convert the CORBA standard id to a String name.
        /// </summary>
        public static string CSName(int id)
        {
            for (int i = 0; i < KnownEncodings.Length; i++)
            {
                if (id == KnownEncodings[i].Id) return KnownEncodings[i].Name;
            }
            return "Unknown TCS: 0x" + id.ToString("X4");
        }

        public static CodeSet GetNegotiatedCodeSet(ORB orb, CodeSetComponentInfo serverCodeSetInfo, bool wide)
        {
            return GetMatchingCodeSet(GetSelectedComponent(orb.LocalCodeSetComponentInfo, wide),
                                       GetSelectedComponent(serverCodeSetInfo, wide),
                                       wide);
        }


        public static CodeSetComponent CreateCodeSetComponent(bool wide, CodeSet nativeCodeSet)
        {
            List<CodeSet> codeSets = new List<CodeSet>();
            codeSets.Add(nativeCodeSet);
            for (int i = 0; i < KnownEncodings.Length; i++)
            {
                if (KnownEncodings[i].SupportsCharacterData(wide) && !codeSets.Contains(KnownEncodings[i]))
                {
                    if (!KnownEncodings[i].IsAlias)
                        codeSets.Add(KnownEncodings[i]);
                }
            }
            codeSets.RemoveAt(0);
            return new CodeSetComponent(nativeCodeSet.Id, codeSets.Select(c => c.Id).ToArray());
        }


        public static CodeSet GetMatchingCodeSet(CodeSetComponent local, CodeSetComponent remote, bool wide)
        {
            CodeSet codeSet = GetCodeSetIfMatched(local.NativeCodeSet, remote);
            if (codeSet != null) return codeSet;

            for (int i = 0; i < local.ConversionCodeSets.Length; i++)
            {
                codeSet = GetCodeSetIfMatched(local.ConversionCodeSets[i], remote);
                if (codeSet != null) return codeSet;
            }

            return ReportNegotiationFailure(local, remote, wide);
        }


        public static CodeSet GetCodeSetIfMatched(uint localCodeSetId, CodeSetComponent remote)
        {
            if (localCodeSetId == remote.NativeCodeSet)
            {
                return GetCodeSet(localCodeSetId);
            }
            else
            {
                for (int i = 0; i < remote.ConversionCodeSets.Length; i++)
                {
                    if (localCodeSetId == remote.ConversionCodeSets[i])
                    {
                        return GetCodeSet(localCodeSetId);
                    }
                }
            }
            return null;
        }


        private static CodeSet ReportNegotiationFailure(CodeSetComponent local, CodeSetComponent remote, bool wide)
        {
            StringBuilder sb = new StringBuilder("No matching ");
            if (wide) sb.Append("wide ");
            sb.Append("code set found. Client knows {");
            AppendCodeSetList(sb, local);
            sb.Append("}. Server offered {");
            AppendCodeSetList(sb, remote);
            sb.Append('}');
            throw new CodeSetIncompatible(sb.ToString(), 1, CompletionStatus.Maybe);
        }


        private static void AppendCodeSetList(StringBuilder sb, CodeSetComponent remote)
        {
            uint codeSet = remote.NativeCodeSet;
            sb.Append(ToCodeSetString(codeSet));
            for (int i = 0; i < remote.ConversionCodeSets.Length; i++)
            {
                sb.Append(',').Append(ToCodeSetString(remote.ConversionCodeSets[i]));
            }
        }

        private static string ToCodeSetString(uint id)
        {
            string rawString = id.ToString("X4");
            return CodeSetPrefix.Substring(0, CodeSetPrefix.Length - rawString.Length) + rawString;
        }

        private static CodeSetComponent GetSelectedComponent(CodeSetComponentInfo info, bool wide)
        {
            return wide ? info.ForWcharData : info.ForCharData;
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Returns true if this codeset supports the specified character type.
        /// </summary>
        public virtual bool SupportsCharacterData(bool wide)
        {
            return false;
        }

        /// <summary>
        /// Returns true if this code set requires byte-order-markers to be written to the beginning of a stream of text.
        /// </summary>
        /// <param name="configuredForBom">
        /// true if the orb has been configured to write byte-order-markers.
        /// </param>
        public virtual bool WriteBom(bool configuredForBom)
        {
            return false;
        }

        /// <summary>
        /// Reads a wide character from the specified buffer
        /// </summary>
        /// <param name="buffer">the buffer containing the data</param>
        /// <param name="giopMinor">the low-order byte of the giop version (1.x is assumed)</param>
        /// <param name="littleEndian">true if the character is to be read low end first</param>
        /// <returns>the wide character</returns>
        public virtual char ReadWChar(InputBuffer buffer, int giopMinor, bool littleEndian)
        {
            throw new Marshal("Bad wide char codeSet: " + Name);
        }

        /// <summary>
        /// Reads a wide character from the specified buffer. The length indicator is presumed already to have been read
        /// </summary>
        /// <param name="buffer">the buffer containing the data</param>
        /// <param name="lengthIndicator">the length indicator already read></param>
        /// <param name="giopMinor">the low-order byte of the giop version (1.x is assumed)</param>
        /// <param name="littleEndian">true if the character is to be read low end first</param>
        /// <returns>a string possibly containing wide characters.</returns>
        public virtual string ReadWString(InputBuffer buffer, int lengthIndicator, int giopMinor, bool littleEndian)
        {
            throw new Marshal("Bad wide char codeSet: " + Name);
        }

        /// <summary>
        /// Writes a character to the buffer with the appropriate encoding.
        /// </summary>
        /// <param name="buffer">the buffer to which the character is written</param>
        /// <param name="c">the character to write</param>
        /// <param name="writeBom">true if a byte-order-marker (indicating big-endian) should be written</param>
        /// <param name="writeLength">true if the length of the character should be written</param>
        /// <param name="giopMinor">the low-order byte of the giop version (1.x is assumed)</param>
        public virtual void WriteChar(OutputBuffer buffer, char c, bool writeBom, bool writeLength, int giopMinor)
        {
            throw new CodeSetIncompatible("Bad codeset: " + Name);
        }

        /// <summary>
        /// Writes a string to the buffer with the appropriate encoding.
        /// </summary>
        /// <param name="buffer">the buffer to which the character is written</param>
        /// <param name="s">the string to write</param>
        /// <param name="writeBom">true if a byte-order-marker (indicating big-endian) should be written</param>
        /// <param name="writeLength">true if the length of the character should be written</param>
        /// <param name="giopMinor">the low-order byte of the giop version (1.x is assumed)</param>
        public virtual void WriteString(OutputBuffer buffer, string s, bool writeBom, bool writeLength, int giopMinor)
        {
            for (int i = 0; i < s.Length; i++)
            {
                WriteChar(buffer, s[i], writeBom, writeLength, giopMinor);
            }
        }

        /// <summary>
        /// Returns the length of the string just written to the buffer.
        /// </summary>
        /// <param name="s">the string written</param>
        /// <param name="startPos">the starting position at which the string was written</param>
        /// <param name="currentPos">the current buffer position</param>
        public virtual int GetWStringSize(string s, int startPos, int currentPos)
        {
            return 0;
        }

        /// <summary>
        /// Reads a wide string from the buffer according to GIOP 1.2.
        /// The length indicator is presumed already to have been read.
        /// </summary>
        /// <param name="buffer">the buffer from which to read the string</param>
        /// <param name="size">the length indicator already read</param>
        /// <param name="giopMinor">the low-order byte of the giop version (must be &gt;= 2)</param>
        /// <returns>a string possibly containing wide characters.</returns>
        string ReadGiop12WString(InputBuffer buffer, int size, int giopMinor)
        {
            var buf = new char[size];
            int endPos = buffer.Pos + size;

            bool wcharLitteEndian = buffer.ReadBOM();

            int i = 0;
            while (buffer.Pos < endPos)
            {
                buf[i++] = ReadWChar(buffer, giopMinor, wcharLitteEndian);
            }

            return new string(buf, 0, i);
        }

        private class Iso8859_1CodeSet : CodeSet
        {

            public Iso8859_1CodeSet() : base(0x00010001, "iso-8859-1")
            {
            }

            /// <summary>Only used for derived codesets</summary>
            public Iso8859_1CodeSet(uint i, string name, bool isAlias) : base(i, name, isAlias)
            {

            }


            /// <summary>Returns true if 'wide' is not specified.</summary>
            public override bool SupportsCharacterData(bool wide)
            {
                return !wide;
            }


            public override void WriteChar(OutputBuffer buffer, char c, bool writeBom, bool writeLengthIndicator, int giopMinor)
            {
                try
                {
                    buffer.WriteByte(Convert.ToByte(c));
                }
                catch (OverflowException)
                {
                    buffer.WriteByte(Convert.ToByte('?'));
                }
            }
        }

        private class AsciiCodeSet : Iso8859_1CodeSet
        {

            public AsciiCodeSet() : base(0x00010001, "us-ascii", true)
            {
            }

            /// <summary>Only used for derived codesets</summary>
            public AsciiCodeSet(uint i, string name) : base(i, name, true)
            {
            }
        }

        private class MacRomanCodeSet : Iso8859_1CodeSet
        {

            public MacRomanCodeSet() : base(0x00010001, "macintosh", true)
            {
            }

            /// <summary>Only used for derived codesets</summary>
            public MacRomanCodeSet(uint i, string name) : base(i, name, true)
            {
            }
        }

        private class Iso8859_15CodeSet : Iso8859_1CodeSet
        {

            public Iso8859_15CodeSet() : base(0x0001000F, "iso-8859-15", false)
            {

            }

            public override void WriteChar(OutputBuffer buffer, char c, bool writeBom, bool writeLengthIndicator, int giopMinor)
            {
                switch (c)
                {
                    case '\u20AC':
                        {
                            buffer.WriteByte(0xA4);
                            break;
                        }
                    case '\u0160':
                        {
                            buffer.WriteByte(0xA6);
                            break;
                        }
                    case '\u0161':
                        {
                            buffer.WriteByte(0xA8);
                            break;
                        }
                    case '\u017D':
                        {
                            buffer.WriteByte(0xB4);
                            break;
                        }
                    case '\u017E':
                        {
                            buffer.WriteByte(0xB8);
                            break;
                        }
                    case '\u0152':
                        {
                            buffer.WriteByte(0xBC);
                            break;
                        }
                    case '\u0153':
                        {
                            buffer.WriteByte(0xBD);
                            break;
                        }
                    case '\u0178':
                        {
                            buffer.WriteByte(0xBE);
                            break;
                        }
                    default:
                        {
                            base.WriteChar(buffer, c, writeBom, writeLengthIndicator, giopMinor);
                            break;
                        }
                }
            }
        }

        private class Utf8CodeSet : CodeSet
        {

            private Encoding encoding = new UTF8Encoding();

            public Utf8CodeSet() : base(0x05010001, "utf-8")
            {
            }

            /// <summary>Returns true for both wide and non-wide characters.</summary>
            public override bool SupportsCharacterData(bool wide)
            {
                return true;
            }

            public override char ReadWChar(InputBuffer buffer, int giopMinor, bool littleEndian)
            {
                if (giopMinor < 2)
                {
                    throw new Marshal("GIOP 1." + giopMinor + " only allows 2 Byte encodings for wchar, but the selected TCSW is UTF-8");
                }

                short value = (short)(0xff & buffer.ReadByte());

                if ((value & 0x80) == 0)
                {
                    return (char)value;
                }
                else if ((value & 0xe0) == 0xc0)
                {
                    return (char)((value & 0x1F) << 6 | buffer.ReadByte() & 0x3F);
                }
                else
                {
                    short b2 = (short)(0xff & buffer.ReadByte());
                    return (char)((value & 0x0F) << 12 | (b2 & 0x3F) << 6 | buffer.ReadByte() & 0x3F);
                }
            }

            public override string ReadWString(InputBuffer source, int lengthIndicator, int giopMinor, bool littleEndian)
            {
                if (giopMinor < 2) throw new Marshal("Bad wide char codeSet: " + Name);
                return ReadGiop12WString(source, lengthIndicator, giopMinor); ;
            }

            public override void WriteChar(OutputBuffer buffer, char c, bool writeBom, bool writeLengthIndicator, int giopMinor)
            {
                if (c <= 0x007F)
                {
                    if (giopMinor == 2 && writeLengthIndicator)
                    {
                        //the chars length in bytes
                        buffer.WriteByte(1);
                    }

                    buffer.WriteByte((byte)c);
                }
                else if (c > 0x07FF)
                {
                    if (giopMinor == 2 && writeLengthIndicator)
                    {
                        //the chars length in bytes
                        buffer.WriteByte(3);
                    }

                    buffer.WriteByte((byte)(0xE0 | c >> 12 & 0x0F));
                    buffer.WriteByte((byte)(0x80 | c >> 6 & 0x3F));
                    buffer.WriteByte((byte)(0x80 | c >> 0 & 0x3F));
                }
                else
                {
                    if (giopMinor == 2 && writeLengthIndicator)
                    {
                        buffer.WriteByte(2);   //the chars length in bytes
                    }

                    buffer.WriteByte((byte)(0xC0 | c >> 6 & 0x1F));
                    buffer.WriteByte((byte)(0x80 | c >> 0 & 0x3F));
                }
            }

            public override void WriteString(OutputBuffer buffer, string s, bool writeBom, bool writeLength, int giopMinor)
            {
                try
                {
                    byte[] bytes = encoding.GetBytes(s);
                    buffer.WriteOctetArray(bytes, 0, bytes.Length);
                }
                catch (Exception e)
                {
                    throw new CodeSetIncompatible("Bad codeset: " + Name);
                }
            }

            public override int GetWStringSize(string s, int startPos, int currentPos)
            {
                return currentPos - startPos - 4;
            }
        }

        abstract private class TwoByteCodeSet : CodeSet
        {
            public TwoByteCodeSet(uint id, string name) : base(id, name)
            {
            }

            /// <summary>Returns true if the character type specified is 'wide'.</summary>
            public override bool SupportsCharacterData(bool wide)
            {
                return wide;
            }

            public override char ReadWChar(InputBuffer buffer, int giopMinor, bool littleEndian)
            {
                if (littleEndian)
                {
                    return (char)(buffer.ReadByte() & 0xFF | buffer.ReadByte() << 8);
                }
                else
                {
                    return (char)(buffer.ReadByte() << 8 | buffer.ReadByte() & 0xFF);
                }
            }

            public override string ReadWString(InputBuffer source, int lengthIndicator, int giopMinor, bool littleEndian)
            {
                if (giopMinor == 2)
                {
                    return ReadGiop12WString(source, lengthIndicator, giopMinor);
                }
                else //GIOP 1.1 / 1.0 : length indicates number of 2-byte characters
                {
                    var buf = new char[lengthIndicator];
                    int endPos = source.Pos + 2 * lengthIndicator;

                    int i = 0;
                    while (source.Pos < endPos)
                    {
                        buf[i++] = ReadWChar(source, giopMinor, littleEndian);
                    }

                    if (i != 0 && buf[i - 1] == 0) //don't return terminating NUL
                    {
                        return new string(buf, 0, i - 1);
                    }
                    else   //doesn't have a terminating NUL. This is actually not allowed
                    {
                        return new string(buf, 0, i);
                    }
                }
            }

            public override void WriteChar(OutputBuffer buffer, char c, bool writeBom, bool writeLengthIndicator, int giopMinor)
            {
                if (giopMinor < 2)
                {
                    buffer.WriteShort((short)c);   //UTF-16 char is treated as an ushort (write aligned)
                }
                else
                {
                    if (writeLengthIndicator)  //the chars length in bytes
                    {
                        buffer.WriteByte(2);
                    }

                    if (writeBom) //big endian encoding
                    {
                        buffer.WriteByte(0xFE);
                        buffer.WriteByte(0xFF);
                    }

                    //write unaligned
                    buffer.WriteByte((byte)(c >> 8 & 0xFF));
                    buffer.WriteByte((byte)(c & 0xFF));
                }
            }

            public override int GetWStringSize(string s, int startPos, int currentPos)
            {
                return s.Length + 1;   // size in chars (+ NUL char)
            }

        }

        private class Utf16CodeSet : TwoByteCodeSet
        {
            public Utf16CodeSet() : base(0x00010109, "utf-16")
            {
            }


            /// <summary>Returns the configured value to use BOMs only when specifically configured to do so</summary>
            public override bool WriteBom(bool configuredForBom)
            {
                return false;
            }
        }

        /// <summary>
        ///  According to:
        ///  http://www.omg.org/issues/issue4008.txt
        ///  http://bugs.sun.com/bugdatabase/view_bug.do?bug_id=4506930
        ///  UCS-2 should not write a BOM.
        /// </summary>
        private class Ucs2CodeSet : TwoByteCodeSet
        {
            public Ucs2CodeSet() : base(0x00010100, "UCS2")
            {
            }
        }
    }

}
