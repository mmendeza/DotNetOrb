// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class SequenceTypeCode : ArrayTypeCode
    {

        public SequenceTypeCode(TypeCode elementType, int length): base (elementType, length)
        {
            this.kind = TCKind.TkSequence;
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                // New typecode with same kind, id and a blank name.
                var result = new SequenceTypeCode(elementType.CompactTypeCode, length);
                return result;
            }            
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {            
            try
            {
                int len = input.ReadLong();                
                output.WriteLong(len);
                for (int i = 0; i < len; i++)
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

        public static new TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap, int startPosition)
        {
            inputStream.OpenEncapsulation();
            var elementType = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
            int length = inputStream.ReadLong();
            var result = orb.CreateSequenceTc(length, elementType);
            repeatedTCMap.Add(startPosition, result);
            inputStream.CloseEncapsulation();
            return result;
        }

    }
}
