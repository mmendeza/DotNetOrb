// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ThrowsIdlExceptionAttribute : Attribute
    {

        public Type ExceptionType { get; set; }


        public ThrowsIdlExceptionAttribute(Type exceptionType)
        {
            ExceptionType = exceptionType;
        }
        
        public override bool Equals(object obj)
        {            
            if (obj is ThrowsIdlExceptionAttribute other)
            {
                return (ExceptionType != null ? ExceptionType == other.ExceptionType : other.ExceptionType == null);
            }
            else
            {
                return false;
            }
        }
        
        public override int GetHashCode()
        {
            return (ExceptionType != null ? ExceptionType.GetHashCode() : 0);
        }        

    }
}
