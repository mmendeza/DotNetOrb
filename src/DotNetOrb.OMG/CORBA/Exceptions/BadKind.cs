// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class BadKind : UserException
    {
        public static readonly string _Id = "IDL:omg.org/CORBA/BadKind:1.0";

        public BadKind() : base(_Id) { }

        public BadKind(string reason) : base(_Id + " " + reason) { }
    }
}
