// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class UnknownUserException : UserException
    {
        public CORBA.Any Except { get; set; }

        public static readonly string _Id = "IDL:omg.org/CORBA/UnknownUserException:1.0";

        public UnknownUserException() : base(_Id) { }

        public UnknownUserException(string reason) : base(_Id + " " + reason) { }

        public UnknownUserException(CORBA.Any exception) : base(_Id) 
        {
            Except = exception;
        }
    }
}
