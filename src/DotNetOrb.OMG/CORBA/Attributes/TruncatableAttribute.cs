// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TruncatableAttribute : Attribute
    {

        public Type ValueType { get; set; }


        public TruncatableAttribute(Type valueType)
        {
            ValueType = valueType;
        }
        
        public override bool Equals(object obj)
        {            
            if (obj is TruncatableAttribute other)
            {
                return (ValueType != null ? ValueType == other.ValueType : other.ValueType == null);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return (ValueType != null ? ValueType.GetHashCode() : 0);
        }        

    }
}
