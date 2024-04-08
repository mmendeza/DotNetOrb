// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public enum InterfaceType
    {
        ConcreteInterface,
        AbstractInterface,
        LocalInterface,
        AbstractValueType,
    }

    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    public sealed class InterfaceTypeAttribute : Attribute
    {        
        private InterfaceType ifzType;
       
        public InterfaceTypeAttribute(InterfaceType idlType)
        {
            ifzType = idlType;            
        }

        public InterfaceType IfzType
        {
            get
            {
                return ifzType;
            }
        }

        public override bool Equals(object ?obj)
        {
            if (obj is InterfaceTypeAttribute att)
            {
                return (ifzType == att.ifzType);
            }            
            return false;            
        }
        
        public override int GetHashCode()
        {
            return ifzType.GetHashCode();
        }
    }
}
