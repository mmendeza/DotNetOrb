// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace CORBA
{
    public abstract class ComplexTypeCode: TypeCode
    {

        //public override bool IsSimple { get => false; }

        public override bool IsPrimitive
        {
            get
            {
                return false;
            }
        }

        protected string id;
        public override string Id
        {
            get
            {
                return id;
            }            
        }

        protected string name;
        public override string Name
        {
            get
            {
                return name;
            }            
        }

        public ComplexTypeCode(TCKind kind, string id, string name): base(kind)
        {            
            this.id = id;
            this.name = name;
            this.kind = kind;
        }

        public override bool Equal(TypeCode tc)
        {
            if (tc is RecursiveTypeCode rtc)
            {
                return Equal(rtc.ActualTypeCode);
            }
            if (!base.Equal(tc))
                return false;
            return (id.Equals(tc.Id) && name.Equals(tc.Name));            
        }

        public override bool Equivalent(TypeCode tc)
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
            if (Id.Length > 0 && tc.Id.Length > 0)
            {
                if (Id.Equals(tc.Id))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public override void WriteTypeCode(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap)
        {            
            if (outputStream.IsIndirectionEnabled && repeatedTCMap.ContainsKey(this))
            {
                // Write indirection marker
                outputStream.WriteLong(-1); // recursion marker
                int negativeOffset = repeatedTCMap[this] - outputStream.Pos - 4;
                outputStream.WriteLong(negativeOffset);
            }                     
            else
            {
                try
                {                    
                    outputStream.WriteLong((int) kind);
                    // remember tc start pos before we start writing it out                    
                    var startPosition = outputStream.Pos;
                    recursiveTCMap.Add(Id, startPosition);
                    outputStream.BeginEncapsulation();

                    WriteTypeCodeParameters(outputStream, recursiveTCMap, repeatedTCMap);

                    outputStream.EndEncapsulation();

                    recursiveTCMap.Remove(id);

                    // add typecode to cache not until here to account for recursive TCs
                    repeatedTCMap.Add(this, startPosition);
                }
                catch (BadKind e)
                {                    
                    throw new Internal("Unable to write typecode caught " + e);
                }
                catch (Bounds e)
                {                    
                    throw new Internal("Unable to write typecode caught " + e);
                }
            }
        }

        protected abstract void WriteTypeCodeParameters(IOutputStream outputStream, IDictionary<string, int> recursiveTCMap, IDictionary<TypeCode, int> repeatedTCMap);

    }
}
