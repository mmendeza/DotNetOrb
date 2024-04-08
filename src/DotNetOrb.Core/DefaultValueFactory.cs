// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System;

namespace DotNetOrb.Core
{
    internal class DefaultValueFactory : IValueFactory
    {
        private Type implementationType;

        internal DefaultValueFactory(Type implementationType)
        {
            this.implementationType = implementationType;
        }

        public CORBA.Object ReadValue(IInputStream stream)
        {
            object implObj = Activator.CreateInstance(implementationType);
            if (implObj is IStreamableValue streamable)
            {
                return (CORBA.Object)stream.ReadValue(streamable);
            }
            else if (implObj is ICustomValue custom)
            {
                custom._Unmarshal(new DataInputStream(stream));
                return (CORBA.Object)custom;
            }
            else
            {
                throw new Marshal("Unknow value type " + implObj);
            }
        }
    }
}
