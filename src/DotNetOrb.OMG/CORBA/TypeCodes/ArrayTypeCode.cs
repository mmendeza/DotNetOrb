// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class ArrayTypeCode : TypeCode
    {

        protected TypeCode elementType;
        public override TypeCode ContentType
        {
            get
            {
                return elementType;
            }            
        }

        protected int length;
        public override int Length
        {
            get
            {
                return length;
            }            
        }

        public ArrayTypeCode(TypeCode elementType, int length)
        {
            this.elementType = elementType;
            this.length = length;
            this.kind = TCKind.TkArray;
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                // New typecode with same kind, id and a blank name.
                var result = new ArrayTypeCode(elementType.CompactTypeCode, length);
                return result;
            }            
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
                if (length != tc.Length)
                {
                    return false;
                }
                return (elementType.Equal(tc.ContentType));
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
                if (length != tc.Length)
                {
                    return false;
                }
                return elementType.Equivalent(tc.ContentType);
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
                for (int i = 0; i < length; i++)
                {
                    (elementType as TypeCode).RemarshalValue(input, output);
                }
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
            inputStream.OpenEncapsulation();
            var elementType = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
            int length = inputStream.ReadLong();
            var result = orb.CreateArrayTc(length, elementType);            
            repeatedTCMap.Add(startPosition, result);
            inputStream.CloseEncapsulation();
            return result;
        }

        public override void WriteTypeCode(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            outputStream.WriteLong((int)kind);
            outputStream.BeginEncapsulation();
            outputStream.WriteTypeCode(elementType, recursiveTCMap, repeatedTCMap);
            outputStream.WriteLong(length);
            outputStream.EndEncapsulation();            
        }        

    }
}
