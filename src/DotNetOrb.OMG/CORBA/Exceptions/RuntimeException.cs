// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public class RuntimeException : Exception
    {
        public RuntimeException() : base() { }

        public RuntimeException(string message) : base(message) { }

        public RuntimeException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
