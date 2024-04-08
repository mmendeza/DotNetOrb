// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class EnumTypeCode : ComplexTypeCode
    {
        protected string[] members;
        public override int MemberCount
        {
            get { return members.Length; }            
        }
        public override string MemberName(int index)
        {
            if (index < members.Length)
            {
                return members[index];
            }
            else
            {
                throw new Bounds();
            }            
        }

        public EnumTypeCode(string id, string name, string[] members) : base(TCKind.TkEnum, id, name)
        {
            this.members = members;
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                var membersClone = new string[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    membersClone[i] = "";
                }
                var result = new EnumTypeCode(id, "", membersClone);
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
                    if (!members[i].Equals(tc.MemberName(i)))
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
                    if (!members[i].Equals(tc.MemberName(i)))
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
                output.WriteLong(input.ReadLong());
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
                string[] memberNames = new string[memberCount];
                for (int i = 0; i < memberCount; i++)
                {
                    memberNames[i] = inputStream.ReadString();
                }                
                result = orb.CreateEnumTc(repositoryID, name, memberNames, false);
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
                outputStream.WriteString(members[i]);
            }
        }

    }
}
