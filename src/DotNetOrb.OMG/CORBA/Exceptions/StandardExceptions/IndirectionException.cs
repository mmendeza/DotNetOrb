// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public class IndirectionException : CORBA.SystemException
    {
        public int Offset { get; set; }

        public IndirectionException(int offset): base("", 0, CompletionStatus.Maybe)
        {            
            Offset = offset;
        }        
    }
}
