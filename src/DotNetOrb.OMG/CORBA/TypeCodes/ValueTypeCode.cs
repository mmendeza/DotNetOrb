// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class ValueTypeCode : ComplexTypeCode
    {
        protected ValueModifier valueModifier = 0;
        public override ValueModifier TypeModifier
        {
            get
            {
                return valueModifier;
            }            
        }

        protected TypeCode concreteBase;
        public override TypeCode ConcreteBaseType
        {
            get
            {
                return concreteBase;
            }            
        }
        protected ValueMember[] members;
        public override int MemberCount
        {
            get
            {
                return members.Length;
            }            
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
        public override short MemberVisibility(int index)
        {
            if (index < members.Length)
            {
                return (short) members[index].Access;
            }
            else
            {
                throw new Bounds();
            }
        }


        public ValueTypeCode(string id, string name, ValueModifier valueModifier, TypeCode concreteBase, ValueMember[] members) : base(TCKind.TkValue, id, name)
        {
            this.valueModifier = valueModifier;
            this.concreteBase = concreteBase;
            this.members = members;
            ResolveRecursion(this);
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                var membersClone = new ValueMember[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    membersClone[i] = new ValueMember("", members[i].Id, members[i].DefinedIn, members[i].Version, members[i].Type.CompactTypeCode, members[i].TypeDef, members[i].Access);
                }
                var result = new ValueTypeCode(id, "", valueModifier, concreteBase.CompactTypeCode, membersClone);
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
                if (valueModifier != tc.TypeModifier)
                {
                    return false;
                }
                if (!concreteBase.Equal(tc.ConcreteBaseType))
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
                    if ((short) members[i].Access != tc.MemberVisibility(i))
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
                if (valueModifier != tc.TypeModifier)
                {
                    return false;
                }
                if (!concreteBase.Equivalent(tc.ConcreteBaseType))
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
                    if ((short)members[i].Access != tc.MemberVisibility(i))
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
                var val = input.ReadValue();
                output.WriteValue(val, id);
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
                string name = inputStream.ReadString();
                if (string.IsNullOrEmpty(name))
                {
                    name = null;
                }
                var typeModifier = inputStream.ReadShort();
                var concreteBaseType = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
                var memberCount = inputStream.ReadLong();
                var members = new ValueMember[memberCount];
                for (int i = 0; i < memberCount; i++)
                {
                    members[i] = new ValueMember()
                    {
                        Name = inputStream.ReadString(),
                        Type = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap),
                        Access = inputStream.ReadShort()
                    };
                }
                result = orb.CreateValueTc(repositoryID, name, (ValueModifier)typeModifier, concreteBaseType, members);
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
            outputStream.WriteShort((short) TypeModifier);            
            if (ConcreteBaseType == null)
            {
                outputStream.WriteLong((int) TCKind.TkNull);
            }
            else
            {
            outputStream.WriteTypeCode(ConcreteBaseType, recursiveTCMap, repeatedTCMap);
            }
            int memberCount = MemberCount;
            outputStream.WriteLong(memberCount);
            for (int i = 0; i < memberCount; i++)
            {
                outputStream.WriteString(MemberName(i));
                outputStream.WriteTypeCode(MemberType(i), recursiveTCMap, repeatedTCMap);
                outputStream.WriteShort(MemberVisibility(i));
            }
        }

    }
}
