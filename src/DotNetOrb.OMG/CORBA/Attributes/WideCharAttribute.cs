// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    [AttributeUsage(AttributeTargets.ReturnValue | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
    public sealed class WideCharAttribute : Attribute
    {
        public bool IsWideChar { get; set; }


        public WideCharAttribute(bool isWideChar)
        {
            IsWideChar = isWideChar;
        }
    }
}
