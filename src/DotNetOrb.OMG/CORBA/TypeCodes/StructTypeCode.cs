// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class StructTypeCode : ComplexTypeCode
    {
        protected StructMember[] members;
        public override int MemberCount
        {
            get { return members.Length; }            
        }
        public override string MemberName(int index)
        {
            if (index < members.Length)
            {
                return members[index].Name;
            }
            else
            {
                throw new Bounds();
            }
        }
        public override TypeCode MemberType(int index)
        {
            if (index < members.Length)
            {
                return members[index].Type;
            }
            else
            {
                throw new Bounds();
            }
        }

        public StructTypeCode(string id, string name, StructMember[] members) : base(TCKind.TkStruct, id, name)
        {
            this.members = members;
            ResolveRecursion(this);
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
                var result = new StructTypeCode(id, "", membersClone);
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
                if (members.Length != tc.MemberCount)
                {
                    return false;
                }
                for (int i = 0; i < members.Length; i++)
                {
                    if (!members[i].Name.Equals(tc.MemberName(i)))
                    {
                        return false;
                    }
                    if (!members[i].Type.Equal(tc.MemberType(i)))
                    {
                        return false;
                    }
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
                if (members.Length != tc.MemberCount)
                {
                    return false;
                }
                for (int i = 0; i < members.Length; i++)
                {
                    if (!members[i].Name.Equals(tc.MemberName(i)))
                    {
                        return false;
                    }
                    if (!members[i].Type.Equivalent(tc.MemberType(i)))
                    {
                        return false;
                    }
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
                for (int i = 0; i < members.Length; i++)
                {
                    (MemberType(i) as TypeCode).RemarshalValue(input, output);
                }
            }
            catch (BadKind ex)
            {
                throw new Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
            catch (Bounds ex)
            {
                throw new Marshal("When processing TypeCode with kind: " + kind + " caught " + ex);
            }
        }

        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int, TypeCode> repeatedTCMap, int startPosition)
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
                //string[] memberNames = new string[memberCount];
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
                result = orb.CreateStructTc(repositoryID, name, structMembers, false);
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
            int memberCount = members.Length;   
            outputStream.WriteLong(memberCount);
            for (int i = 0; i < memberCount; i++)
            {
                outputStream.WriteString(members[i].Name);
                outputStream.WriteTypeCode(members[i].Type, recursiveTCMap, repeatedTCMap);
            }
        }
    }
}
