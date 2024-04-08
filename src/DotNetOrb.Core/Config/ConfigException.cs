// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DotNetOrb.Core.Config
{
    public class ConfigException : Exception
    {
        public ConfigException() : base() { }

        public ConfigException(string message) : base(message) { }

        public ConfigException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}
