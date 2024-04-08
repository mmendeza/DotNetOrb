// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public class ApplicationException : Exception
    {
        public readonly string Id;

        public readonly IInputStream InputStream;

        public ApplicationException(string id, IInputStream inputStream) : base() 
        { 
            Id = id;
            InputStream = inputStream;
        }

        
    }
}
