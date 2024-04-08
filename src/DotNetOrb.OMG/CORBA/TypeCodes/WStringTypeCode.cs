// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class WStringTypeCode : StringTypeCode
    {

        public WStringTypeCode(int length): base(length)
        {            
            this.kind = TCKind.TkWstring;
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {
            try
            {
                output.WriteWString(input.ReadWString());
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
        public new static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap, int startPosition)
        {
            return orb.CreateWStringTc(inputStream.ReadLong());
        }

    }
}
