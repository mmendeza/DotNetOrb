// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.POA.Exceptions
{
    public class CompletionRequestedException : Exception
    {
        public CompletionRequestedException() : base() { }

        public CompletionRequestedException(string message) : base(message) { }

        public CompletionRequestedException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
