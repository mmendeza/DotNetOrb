// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class NamedValue
    {
        public string Name { get; set; }
        public Any Value { get; set; }
        public uint Flags { get; set; }

        public NamedValue(string name, Any value, uint flags)
        {
            this.Name = name;
            this.Value = value;
            this.Flags = flags;
        }
    }
}
