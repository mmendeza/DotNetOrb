// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public sealed class HelperAttribute : Attribute
    {

        public Type HelperType { get; set; }


        public HelperAttribute(Type exceptionType)
        {
            HelperType = exceptionType;
        }
        
        public override bool Equals(object obj)
        {            
            if (obj is HelperAttribute other)
            {
                return (HelperType != null ? HelperType == other.HelperType : other.HelperType == null);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return (HelperType != null ? HelperType.GetHashCode() : 0);
        }        

    }
}
