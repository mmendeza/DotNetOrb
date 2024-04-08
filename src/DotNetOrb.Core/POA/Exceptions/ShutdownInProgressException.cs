// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.POA.Exceptions
{
    public class ShutdownInProgressException : Exception
    {
        public ShutdownInProgressException() : base() { }

        public ShutdownInProgressException(string message) : base(message) { }

        public ShutdownInProgressException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
