// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class LocalInterfaceTypeCode : ComplexTypeCode
    {        

        public LocalInterfaceTypeCode(string id, string name) : base(TCKind.TkLocalInterface, id, name)
        {            
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                // New typecode with same kind, id and a blank name.
                var result = new LocalInterfaceTypeCode(id, "");
                return result;
            }            
        }

        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int,string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap,int startPosition)
        {
            int size = inputStream.OpenEncapsulation();
            string repositoryID = inputStream.ReadString();
            if (string.IsNullOrEmpty(repositoryID))
            {
                repositoryID = "IDL:";
            }
            TypeCode? result = inputStream.ReadTypeCodeCache(repositoryID, startPosition);
            if (result == null)
            {
                recursiveTCMap.Add(startPosition, repositoryID);
                //result = doReadTypeCodeInternal(in, recursiveTCMap, repeatedTCMap, startPosition, kind, repositoryID);
                string name = inputStream.ReadString();
                if (string.IsNullOrEmpty(name))
                {
                    name = null;
                }
                result = orb.CreateLocalInterfaceTc(repositoryID, name);
                recursiveTCMap.Remove(startPosition);
                repeatedTCMap.Add(startPosition, result);
                inputStream.UpdateTypeCodeCache(repositoryID, startPosition, size);
            }
            else
            {
                //Skip remaining typecode
                inputStream.Skip(size - ((inputStream.Pos - startPosition) - 4 - 4));                
            }
            inputStream.CloseEncapsulation();
            return result;
        }

        protected override void WriteTypeCodeParameters(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {
            outputStream.WriteString(id);
            outputStream.WriteString(name);
        }

    }
}
