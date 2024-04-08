// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public class RecursiveTypeCode : TypeCode
    {
        private TypeCode actualTypecode;

        public TypeCode ActualTypeCode {
            get
            {
                if (actualTypecode == null)
                {
                    throw new BadInvOrder("ActualTypeCode of RecursiveTypeCode not set");
                }
                return actualTypecode;
            }
            set { actualTypecode = value; }
        }
        private bool isSecondIteration = false;

        //public override bool IsSimple { get => false; }

        public override bool IsPrimitive 
        {
            get
            {
                return false;
            }
        }
        
        public RecursiveTypeCode(string id)
        {
            this.id = id;
        }

        public override TypeCode CompactTypeCode
        {
            get
            {
                return ActualTypeCode.CompactTypeCode;
            }            
        }

        private string id;
        public override string Id
        {
            get
            {
                return id;
            }            
        }

        public override string Name
        {
            get
            {
                return ActualTypeCode.Name;
            }            
        }

        public override bool Equal(TypeCode tc)
        {
            if (tc is RecursiveTypeCode rtc)
            {
                if (isSecondIteration)
                {
                    return true;
                }
                isSecondIteration = true;
                bool result = ActualTypeCode.Equal(rtc.ActualTypeCode);
                isSecondIteration = false;
                return result;
            }
            return tc.Equal(ActualTypeCode);
        }

        public override bool Equivalent(TypeCode tc)
        {
            if (tc is AliasTypeCode tcAlias)
            {
                return Equivalent(tcAlias.ContentType);
            }
            if (tc is RecursiveTypeCode rtc)
            {
                if (isSecondIteration)
                {
                    return true;
                }
                isSecondIteration = true;
                bool result = ActualTypeCode.Equivalent(rtc.ActualTypeCode);
                isSecondIteration = false;
                return result;
            }
            return tc.Equivalent(ActualTypeCode);
        }

        public override TCKind Kind
        {
            get
            {
                return ActualTypeCode.Kind;
            }            
        }
        
        public static TypeCode ReadTypeCode(IInputStream inputStream, IDictionary<int, string> recursiveTCMap, IDictionary<int,TypeCode> repeatedTCMap, int startPos)
        {
            //recursive or repeated tc
            int negativeOffset = inputStream.ReadLong();

            var origTCStartPos = inputStream.Pos - 4 + negativeOffset;


            // check repeatedTCMap first
            // this map contains TypeCode's that are already completely read in
            if (repeatedTCMap.ContainsKey(startPos))
            {
                return repeatedTCMap[startPos];
            }

            // check recursiveTCMap next
            // this map contains not yet completely read in TypeCode's
            if (recursiveTCMap.ContainsKey(origTCStartPos))
            {
                var recursiveId = recursiveTCMap[origTCStartPos];
                return orb.CreateRecursiveTc(recursiveId);
            }

            //if we end up here, we didn't find an entry in either
            //repeatedTCMap and recursiveTCMap
            throw new CORBA.Marshal("Found indirection marker, but no corresponding " + "original typecode (pos: " + origTCStartPos + ")");
        }

        public override void WriteTypeCode(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {            
            try
            {
                if (recursiveTCMap.ContainsKey(Id))
                {
                    // Write indirection marker
                    outputStream.WriteLong(-1); // recursion marker
                    int negativeOffset = recursiveTCMap[Id] - outputStream.Pos - 4;
                    outputStream.WriteLong(negativeOffset);
                }
                else
                {
                    ActualTypeCode.WriteTypeCode(outputStream, recursiveTCMap, repeatedTCMap);
                }
            }
            catch (BadKind e)
            {                
                throw new RuntimeException(e.Message, e);
            }
        }

        public override void RemarshalValue(IInputStream input, IOutputStream output)
        {
            ActualTypeCode.RemarshalValue(input, output);
        }

        public override int DefaultIndex
        {
            get
            {
                return ActualTypeCode.DefaultIndex;
            }
            
        }

        public override TypeCode DiscriminatorType
        {
            get
            {
                return ActualTypeCode.DiscriminatorType;
            }            
        }

        public override int MemberCount
        {
            get
            {
                return ActualTypeCode.MemberCount;
            }            
        }

        public override Any MemberLabel(int index)
        {
            return ActualTypeCode.MemberLabel(index);
        }

        public override string MemberName(int index)
        {
            return ActualTypeCode.MemberName(index);
        }

        public override TypeCode MemberType(int index)
        {
            return ActualTypeCode.MemberType(index);
        }

    }
}
