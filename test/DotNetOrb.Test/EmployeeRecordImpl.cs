// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Test.VT;

namespace DotNetOrb.Test
{
    public class EmployeeRecordImpl : EmployeeRecord
    {
        public EmployeeRecordImpl()
        {

        }

        public EmployeeRecordImpl(string name, string email, string ssn)
        {
            this.Name = name;
            this.Email = email;
            this.SSN = ssn;
        }
    }
}
