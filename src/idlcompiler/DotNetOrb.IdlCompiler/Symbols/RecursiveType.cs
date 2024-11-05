// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public class RecursiveType: IDLSymbol, ITypeSymbol
    {
        public ITypeSymbol ActualType { get; set; }

        public int TCKind => ActualType.TCKind;

        public string IDLType => ActualType.IDLType;

        public string MappedType => ActualType.MappedType;

        public string HelperName => ActualType.HelperName;

        public string FullHelperName => ActualType.FullHelperName;

        public string TypeCodeExp
        {
            get 
            {
                return $"CORBA.ORB.Init().CreateRecursiveTc(\"{ActualType.RepositoryId}\")";
            }
        }

        public RecursiveType(ITypeSymbol actualType, string name, Scope parentScope, List<Annotation> annotations = null) : base(name, annotations)
        {
            ActualType = actualType;
        }                

        public void PrintRead(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            ActualType.PrintRead(indent, sw, streamName, varName, iteration);
        }

        public void PrintWrite(string indent, TextWriter sw, string streamName, string varName, int iteration)
        {
            ActualType.PrintWrite(indent, sw, streamName, varName, iteration);
        }

        public void PrintInsert(string indent, TextWriter sw, string anyName, string varName)
        {
            ActualType.PrintInsert(indent, sw, anyName, varName);
        }

        public void PrintExtract(string indent, TextWriter sw, string anyName, string varName, string type = null)
        {
            ActualType.PrintExtract(indent, sw, anyName, varName, type);
        }

        public override void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null)
        {
            if (!IsIncluded)
            {
                IsIncluded = true;
                ActualType.Include(currentDir, indent, stream);
            }
        }
    }
}
