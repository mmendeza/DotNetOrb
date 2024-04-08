// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
    public sealed class IdlNameAttribute : Attribute
    {
        private readonly int order;
        public int Order { get { return order; } }

        private readonly string name;
        public string Name { get { return name; } }

        public IdlNameAttribute(string name, [CallerLineNumber] int order = 0)
        {
            this.name = name;
            this.order = order;
        }
        
    }
}