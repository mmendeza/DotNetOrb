// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public class RemarshalException : Exception
    {
        public RemarshalException() : base() { }

        public RemarshalException(string message) : base(message) { }

        public RemarshalException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
