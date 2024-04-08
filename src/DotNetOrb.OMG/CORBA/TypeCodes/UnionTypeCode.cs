// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class UnionTypeCode : ComplexTypeCode
    {
        protected TypeCode discriminatorType = null;
        protected int defaultIndex = -1;                
        protected UnionMember[] members;
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
        public override Any MemberLabel(int index)
        {
            if (index < members.Length)
            {
                return members[index].Label;
            }
            else
            {
                throw new Bounds();
            }
        }

        public override TypeCode DiscriminatorType
        {
            get
            {
                return discriminatorType;
            }            
        }

        public override int DefaultIndex
        {
            get
            {
                return defaultIndex;
            }            
        }

        public UnionTypeCode(string id, string name, TypeCode discriminatorType, UnionMember[] members) : base(TCKind.TkUnion, id, name)
        {
            this.discriminatorType = discriminatorType;
            this.members = members;
            for (int i = 0; i < members.Length; i++)
            {
                var member = members[i];
                //TODO default?
                if (member.Label.Type.Kind == TCKind.TkOctet && member.Label.ExtractOctet() == 0)
                {
                    defaultIndex = i;
                }
            }
            ResolveRecursion(this);
        }
        public override TypeCode CompactTypeCode
        {
            get
            {
                var membersClone = new UnionMember[members.Length];
                for (int i = 0; i < members.Length; i++)
                {
                    membersClone[i] = new UnionMember("", members[i].Label, members[i].Type.CompactTypeCode, members[i].TypeDef);
                }
                var result = new UnionTypeCode(id, "", discriminatorType, membersClone);
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
                if (!discriminatorType.Equal(tc.DiscriminatorType))
                {
                    return false;
                }
                if (defaultIndex!= tc.DefaultIndex)
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
                    if (!members[i].Label.Equals(tc.MemberLabel(i)))
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
                if (!discriminatorType.Equivalent(tc.DiscriminatorType))
                {
                    return false;
                }
                if (defaultIndex != tc.DefaultIndex)
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
                    if (!members[i].Label.Equals(tc.MemberLabel(i)))
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
                var disc = discriminatorType.OriginalTypeCode;
                int member_idx = -1;
                switch (disc.Kind)
                {
                    case TCKind.TkShort:  // 2
                        {
                            short s = input.ReadShort();
                            output.WriteShort(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractShort())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkLong:   // 3
                        {
                            int s = input.ReadLong();
                            output.WriteLong(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractLong())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkUshort: // 4
                        {
                            ushort s = input.ReadUShort();
                            output.WriteUShort(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractUShort())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkUlong:  // 5
                        {
                            uint s = input.ReadULong();
                            output.WriteULong(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractULong())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkFloat:      // 6                                            
                    case TCKind.TkDouble:     // 7
                        {
                            throw new CORBA.Marshal("Invalid union discriminator type: " + disc);
                        }
                    case TCKind.TkBoolean:    // 8
                        {
                            bool b = input.ReadBoolean();
                            output.WriteBoolean(b);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (b == members[i].Label.ExtractBoolean())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkChar:   // 9
                        {
                            char s = input.ReadChar();
                            output.WriteChar(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractChar())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkOctet:      // 10
                    case TCKind.TkAny:        // 11
                    case TCKind.TkTypeCode:   // 12
                    case TCKind.TkPrincipal:  // 13
                    case TCKind.TkObjref:     // 14
                    case TCKind.TkStruct:     // 15
                    case TCKind.TkUnion:      // 16
                        {
                            throw new CORBA.Marshal("Invalid union discriminator type: " + disc);
                        }
                    case TCKind.TkEnum:       // 17
                        {
                            int s = input.ReadLong();
                            output.WriteLong(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    int label = members[i].Label.ExtractLong();
                                    if (s == label)
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkString:     // 18
                    case TCKind.TkSequence:   // 19
                    case TCKind.TkArray:      // 20
                    case TCKind.TkAlias:      // 21
                    case TCKind.TkExcept:     // 22
                        {
                            throw new CORBA.Marshal("Invalid union discriminator type: " + disc);
                        }
                    case TCKind.TkLonglong:  // 23
                        {
                            long s = input.ReadLongLong();
                            output.WriteLongLong(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractLong())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    case TCKind.TkUlonglong:  // 24
                        {
                            ulong s = input.ReadULongLong();
                            output.WriteULongLong(s);
                            for (int i = 0; i < members.Length; i++)
                            {
                                if (i != defaultIndex)
                                {
                                    if (s == members[i].Label.ExtractULongLong())
                                    {
                                        member_idx = i;
                                        break;
                                    }
                                }
                            }
                            break;
                        }
                    default:
                        {
                            throw new CORBA.Marshal("Invalid union discriminator type: " + disc);
                        }
                } // switch

                if (member_idx != -1)
                {
                    (MemberType(member_idx) as TypeCode).RemarshalValue(input, output);
                }
                else if (defaultIndex != -1)
                {
                    (MemberType(defaultIndex) as TypeCode).RemarshalValue(input, output);
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
                var discriminatorType = inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap);
                // Use the dealiased discriminator type for the label types.
                // This works because the JacORB IDL compiler ignores any aliasing
                // of label types and only the discriminator type is passed on the
                // wire.
                var origDiscType = discriminatorType.OriginalTypeCode;
                int defaultIndex = inputStream.ReadLong();
                int memberCount = inputStream.ReadLong();                
                UnionMember[] unionMembers = new UnionMember[memberCount];

                for (int i = 0; i < memberCount; i++)
                {
                    var label = orb.CreateAny();

                    if (i == defaultIndex)
                    {
                        // Default discriminator
                        label.InsertOctet(inputStream.ReadOctet());
                    }
                    else
                    {
                        // use the dealiased discriminator type to construct labels
                        label.ReadValue(inputStream, origDiscType);
                    }

                    unionMembers[i] = new UnionMember
                    (
                            inputStream.ReadString(),
                            label,
                            inputStream.ReadTypeCode(recursiveTCMap, repeatedTCMap),
                            null
                    );
                }
                result = orb.CreateUnionTc(repositoryID, name, discriminatorType, unionMembers, false);
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
            outputStream.WriteTypeCode(discriminatorType, recursiveTCMap, repeatedTCMap);
            outputStream.WriteLong(defaultIndex);
            int memberCount = members.Length;
            outputStream.WriteLong(memberCount);
            for (int i = 0; i < memberCount; i++)
            {
                if (i == defaultIndex)
                {
                    outputStream.WriteOctet((byte)0);
                }
                else
                {
                    members[i].Label.WriteValue(outputStream);
                }
                outputStream.WriteString(members[i].Name);
                outputStream.WriteTypeCode(members[i].Type, recursiveTCMap, repeatedTCMap);
            }
        }

    }
}
