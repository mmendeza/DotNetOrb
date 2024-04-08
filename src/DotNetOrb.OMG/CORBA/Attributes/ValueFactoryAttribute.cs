// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ValueFactoryAttribute : Attribute
    {

        public Type ValueType { get; set; }


        public ValueFactoryAttribute(Type exceptionType)
        {
            ValueType = exceptionType;
        }
        
        public override bool Equals(object obj)
        {            
            if (obj is ValueFactoryAttribute other)
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
