// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class UserExceptionType: ExceptionSymbol
    {        
        public UserExceptionType(bool dotNetNaming, List<Annotation> annotations = null) : base("UserException", dotNetNaming, annotations)
        {

        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {            
            IsIncluded = true;            
        }       

    }
}
