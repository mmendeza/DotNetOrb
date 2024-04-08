// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class InvalidName : UserException
    {
        public static readonly string _Id = "IDL:omg.org/CORBA/InvalidName:1.0";

        public InvalidName() : base(_Id) { }        

        public InvalidName(string reason) : base(_Id + " " + reason) { }
        
    }
}
