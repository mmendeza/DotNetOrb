// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.POA.Exceptions
{
    public class InactiveException : Exception
    {
        public InactiveException() : base() { }

        public InactiveException(string message) : base(message) { }

        public InactiveException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
