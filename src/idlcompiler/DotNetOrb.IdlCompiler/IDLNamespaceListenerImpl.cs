// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;

namespace DotNetOrb.IdlCompiler
{
    class IDLNamespaceListenerImpl : IDLParserBaseListener
    {        
        public HashSet<string> Namespaces = new HashSet<string>();
        public Stack<string> Scope = new Stack<string>();

        public IDLNamespaceListenerImpl()
        {
        }


        private string Namespace
        {
            get
            {
                return String.Join("::", Scope);
            }
        }

        public override void EnterModule([NotNull] IDLParser.ModuleContext context)
        {
            var name = context.ID().GetText();
            Scope.Push(name);
            Namespaces.Add(Namespace);
        }

        public override void EnterStruct_type([NotNull] IDLParser.Struct_typeContext context)
        {
            base.EnterStruct_type(context);

        }
        public override void ExitModule([NotNull] IDLParser.ModuleContext context)
        {            
            Scope.Pop();
        }

        public override void EnterInterface_decl([NotNull] IDLParser.Interface_declContext context)
        {
            var header = context.interface_header();
            var name = header.ID().GetText();
            Scope.Push(name);
            Namespaces.Add(Namespace);
        }

        public override void ExitInterface_decl([NotNull] IDLParser.Interface_declContext context)
        {
            Scope.Pop();
        }

        public override void EnterValue_decl([NotNull] IDLParser.Value_declContext context)
        {
            var header = context.value_header();
            var name = header.ID().GetText();
            Scope.Push(name);
            Namespaces.Add(Namespace);
        }

        public override void ExitValue_decl([NotNull] IDLParser.Value_declContext context)
        {
            Scope.Pop();
        }

        public override void EnterValue_abs_decl([NotNull] IDLParser.Value_abs_declContext context)
        {
            var name = context.ID().GetText();
            Scope.Push(name);
            Namespaces.Add(Namespace);
        }

        public override void ExitValue_abs_decl([NotNull] IDLParser.Value_abs_declContext context)
        {
            Scope.Pop();
        }

        public override void EnterPragma_prefix([NotNull] IDLParser.Pragma_prefixContext context)
        {
            base.EnterPragma_prefix(context);
        }

    }
}