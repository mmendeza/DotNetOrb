// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class Bounds : UserException
    {
        public static readonly string _Id = "IDL:omg.org/CORBA/Bounds:1.0";

        public Bounds() : base(_Id) { }

        public Bounds(string reason) : base(_Id + " " + reason) { }
    }
}
