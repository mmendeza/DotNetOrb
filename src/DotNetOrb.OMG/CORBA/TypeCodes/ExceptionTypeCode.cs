// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class ExceptionTypeCode : StructTypeCode
    {

        public ExceptionTypeCode(string id, string name, StructMember[] members) : base(id, name, members)
        {
            this.kind = TCKind.TkExcept;            
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                var membersClone = new StructMember[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    membersClone[i] = new StructMember("", members[i].Type.CompactTypeCode, members[i].TypeDef);
                }
                var result = new ExceptionTypeCode(id, "", membersClone);
                return result;
            }            
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {
            try
            {
                output.WriteString(input.ReadString());
                for (int i = 0; i < members.Length; i++)
                {
                    (MemberType(i) as TypeCode).RemarshalValue(input, output);
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
                int memberCount = inputStream.ReadLong();                
                StructMember[] structMembers = new StructMember[memberCount];

                for (int i = 0; i < memberCount; i++)
                {
                    structMembers[i] = new StructMember
                    (
                            inputStream.ReadString(),
                            inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap),
                            null
                    );
                }
                result = orb.CreateExceptionTc(repositoryID, name, structMembers, false);
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

    }
}
