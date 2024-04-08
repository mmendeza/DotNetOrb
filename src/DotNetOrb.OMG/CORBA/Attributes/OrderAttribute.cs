// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class OrderAttribute : Attribute
    {
        private readonly int order;

        public OrderAttribute([CallerLineNumber] int order = 0)
        {
            this.order = order;
        }

        public int Order { get { return order; } }
    }

    public interface A
    {
        public int Order { get; }
    }

}


//var properties = from property in typeof(Test).GetProperties()
//                 where Attribute.IsDefined(property, typeof(OrderAttribute))
//                 orderby ((OrderAttribute)property
//                           .GetCustomAttributes(typeof(OrderAttribute), false)
//                           .Single()).Order
//                 select property;

//foreach (var property in properties)
//{
//    //
//}