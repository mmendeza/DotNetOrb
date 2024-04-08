// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = true)]
    public sealed class StubAttribute : Attribute
    {

        public Type StubType { get; set; }


        public StubAttribute(Type exceptionType)
        {
            StubType = exceptionType;
        }
        
        public override bool Equals(object obj)
        {            
            if (obj is StubAttribute other)
            {
                return (StubType != null ? StubType == other.StubType : other.StubType == null);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return (StubType != null ? StubType.GetHashCode() : 0);
        }        

    }
}
