// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = true, Inherited = true)]
    public sealed class RepositoryIDAttribute : Attribute
    {
        
        private string id;

        public RepositoryIDAttribute(string id)
        {
            this.id = id;
        }        

        public string Id
        {
            get
            {
                return id;
            }
        }

        public override bool Equals(object? obj)
        {
            if (obj is RepositoryIDAttribute att)
            {
                return (id == att.id);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }

}
