// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace CORBA
{
    public class ContextList : List<string>
    {
        public string Item(int index)
        {           
            try
            {                
                return this[index];
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new CORBA.Bounds(e.ToString());
            }            
        }
    }
}
