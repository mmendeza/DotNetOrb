// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;

namespace DotNetOrb.IdlCompiler.Symbols
{
    public interface IIDLSymbol
    {
        public bool IsIncluded { get; set; }
        public String TypePrefix { get; set; }
        public String TypeId { get; set; }
        public String TypeVersion { get; set; }
        public string Namespace { get; }
        public string Name { get; }        
        public string FullName { get; }
        public string MappedName { get; }
        public Scope ParentScope { get; set; }
        public List<Annotation> Annotations { get; }
        public List<string> MappedAttributes { get; }
        public Annotation GetAnnotation(string name);
        public string GetMappedName(string suffix = "", string prefix = "");                
        
        public string RepositoryId { get; }

        void Include(DirectoryInfo currentDir, string indent = "", StreamWriter stream = null);

        void PrintComment(StreamWriter sw);

        void PrintUsings(StreamWriter sw);
    }
}
