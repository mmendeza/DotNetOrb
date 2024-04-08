// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class NativeTypeCode : ComplexTypeCode
    {

        public NativeTypeCode(string id, string name): base(TCKind.TkNative, id, name)
        {                        
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                // New typecode with same kind, id and a blank name.
                var result = new NativeTypeCode(id, "");
                return result;
            }            
        }

        protected override void WriteTypeCodeParameters(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            
        }

    }
}
