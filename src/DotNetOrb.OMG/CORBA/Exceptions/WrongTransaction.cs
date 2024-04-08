// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class WrongTransaction : UserException
    {
        public static readonly string _Id = "IDL:omg.org/CORBA/WrongTransaction:1.0";

        public WrongTransaction() : base(_Id) { }

        public WrongTransaction(string reason) : base(_Id + " " + reason) { }
    }
}
