// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class FixedTypeCode : TypeCode
    {

        protected ushort scale;
        public override ushort FixedScale
        {
            get { return scale; }            
        }
        protected ushort digits;
        public override ushort FixedDigits
        {
            get { return digits; }            
        }        

        public FixedTypeCode(ushort digits, ushort scale)
        {            
            this.digits = digits;
            this.scale= scale;
            this.kind = TCKind.TkFixed;
        }

        public override bool Equal(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equal(rtc.ActualTypeCode);
                }
                if (!base.Equal(tc))
                {
                    return false;
                }                    
                if (digits != tc.FixedDigits)
                {
                    return false;
                }
                if (scale != tc.FixedScale)
                {
                    return false;
                }
                return true;
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            {
                return false;
            }
        }

        public override bool Equivalent(TypeCode tc)
        {
            try
            {
                if (tc is RecursiveTypeCode rtc)
                {
                    return Equivalent(rtc.ActualTypeCode);
                }
                if (tc is AliasTypeCode tcAlias)
                {
                    return Equivalent(tcAlias.ContentType);
                }
                if (!base.Equivalent(tc))
                {
                    return false;
                }
                if (digits != tc.FixedDigits)
                {
                    return false;
                }
                if (scale != tc.FixedScale)
                {
                    return false;
                }
                return true;
            }
            catch (Bounds)
            {
                return false;
            }
            catch (BadKind)
            {
                return false;
            }
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {
            try
            {
                output.WriteFixed(input.ReadFixed(digits, scale), digits, scale);
            }
            catch (BadKind ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
            catch (Bounds ex)
            {
                throw new CORBA.Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }            
        }

        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap, int startPosition)
        {
            return orb.CreateFixedTc(inputStream.ReadUShort(), inputStream.ReadUShort());
        }

        public override void WriteTypeCode(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {            
            outputStream.WriteLong((int)kind);
            outputStream.WriteUShort(digits);
            outputStream.WriteUShort(scale);                                    
        }

    }
}
