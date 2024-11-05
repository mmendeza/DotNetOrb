// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetOrb.IdlCompiler.Symbols
{

    public class Scope
    {
        //public static readonly string[] IDLKeywords = { "abstract", "any", "alias", "attribute", "bitfield", "bitmask", "bitset", "boolean", "case", "char", "component", "connector", "const", "consumes", "context", "custom", "default", "double", "exception", "emits", "enum", "eventtype", "factory", "false", "finder", "fixed", "float", "getraises", "home", "import", "in", "inout", "interface", "local", "long", "manages", "map", "mirrorport", "module", "multiple", "native", "object", "octet", "oneway", "out", "primarykey", "private", "port", "porttype", "provides", "public", "publishes", "raises", "readonly", "setraises", "sequence", "short", "string", "struct", "supports", "switch", "true", "truncatable", "typedef", "typeid", "typename", "typeprefix", "unsigned", "union", "uses", "valuebase", "valuetype", "void", "wchar", "wstring", "int8", "uint8", "int16", "int32", "int64", "uint16", "uint32", "uint64" };                
        public static readonly string[] IDLKeywords = { "abstract", "any", "alias", "attribute", "bitfield", "bitmask", "bitset", "boolean", "case", "char", "component", "connector", "const", "consumes", "context", "custom", "default", "double", "exception", "emits", "enum", "factory", "false", "finder", "fixed", "float", "getraises", "home", "import", "in", "inout", "interface", "local", "long", "manages", "map", "mirrorport", "module", "multiple", "native", "object", "octet", "oneway", "out", "primarykey", "private", "porttype", "provides", "public", "publishes", "raises", "readonly", "setraises", "sequence", "short", "string", "struct", "supports", "switch", "true", "truncatable", "typedef", "typeid", "typename", "typeprefix", "unsigned", "union", "uses", "valuebase", "valuetype", "void", "wchar", "wstring", "int8", "uint8", "int16", "int32", "int64", "uint16", "uint32", "uint64" };

        public IScopeSymbol Symbol { get; set; }
        public string Name { get; set; }
        public Scope ParentScope { get; set; }
        private Dictionary<string, Scope> childScopes = new Dictionary<string, Scope>();
        /// <summary>
        /// Dictionary of child scopes. Key is lowercase name of scope
        /// </summary>
        public Dictionary<string, Scope> ChildScopes { get => childScopes; }
        private Dictionary<string, Scope> inheritedScopes = new Dictionary<string, Scope>();
        /// <summary>
        /// Dictionary of inherited scopes. Key is lowercase name of scope
        /// </summary>
        public Dictionary<string, Scope> InheritedScopes { get => inheritedScopes; }
        public String Prefix { get; set; }

        /// <summary>
        /// Symbols defined in this scope
        /// </summary>
        public OrderedDictionary Symbols { get; set; }
        /// <summary>
        /// Symbols introduced in this scope by referencing them
        /// </summary>
        public OrderedDictionary IntroducedSymbols { get; set; }

        public Dictionary<string, string> RepositoryIds = new Dictionary<string, string>();
        public Dictionary<string, string> Versions = new Dictionary<string, string>();
        public Dictionary<string, string> Prefixes = new Dictionary<string, string>();

        /// <summary>
        ///  Constructor for global scope
        /// </summary>
        public Scope()
        {
            Name = "";
            Symbols = new OrderedDictionary();
            IntroducedSymbols = new OrderedDictionary();
            AddPredefinedSymbols();
        }

        public void AddPredefinedSymbols()
        {
            var typeCode = new TypeCodeType("TypeCode");
            var userException = new UserExceptionType();
            //var ob = new ObjectType("Object", true);
            //AddSymbolDefinition(typeCode);
            //AddSymbolDefinition(ob);
            var corba = new Module("CORBA");
            AddSymbolDefinition(corba);
            corba.NamingScope.AddSymbolDefinition(typeCode);
            corba.NamingScope.AddSymbolDefinition(userException);
        }

        public Scope(IScopeSymbol symbol)
        {
            Name = symbol.Name;
            Symbol = symbol;
            symbol.NamingScope = this;
            Symbols = new OrderedDictionary();
            IntroducedSymbols = new OrderedDictionary();
            AddInheritedScopes();
        }

        private Scope GlobalScope
        {
            get
            {
                var parent = ParentScope;
                while (parent.ParentScope != null)
                {
                    parent = parent.ParentScope;
                }
                return parent;
            }
        }
        public String Namespace
        {
            get
            {
                var ns = new List<string>();
                var parent = this;
                while (parent != null && !String.IsNullOrEmpty(parent.Name))
                {
                    ns.Add(parent.Name);
                    parent = parent.ParentScope;
                }
                ns.Reverse();
                return String.Join("::", ns);
            }
        }

        public bool IsGlobalScope
        {
            get
            {
                return ParentScope == null;
            }
        }

        public bool ContainsChildScope(String name)
        {
            var key = name.ToLower();
            if (ChildScopes.ContainsKey(key))
            {
                var scope = ChildScopes[key];
                if (!scope.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + scope.Name);
                }
                return true;
            }
            return false;
        }

        public bool ContainsSymbol(String name)
        {
            var key = name.ToLower();
            if (Symbols.Contains(key))
            {
                var symbol = Symbols[key] as IDLSymbol;
                if (!symbol.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + symbol.Name);
                }
                return true;
            }
            return false;
        }

        /// <summary>Returns scope for symbol. Searchs in inherited and child scopes, then continue in enclosing scopes
        /// <param name="name">Name of scope</param>
        /// <exception cref="IdlCompilerException">When scope not found or case mismatch</exception>
        /// </summary>
        public Scope GetScopeForSymbol(String name)
        {
            var key = name.ToLower();
            if (InheritedScopes.ContainsKey(key))
            {
                var scope = InheritedScopes[key];
                if (!scope.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + scope.Name);
                }
                return scope;
            }
            if (ChildScopes.ContainsKey(key))
            {
                var scope = ChildScopes[key];
                if (!scope.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + scope.Name);
                }
                return scope;
            }
            if (ParentScope != null)
            {
                return ParentScope.GetScopeForSymbol(name);
            }
            throw new IdlCompilerException("Scope  not found: " + name);
        }

        /// <summary>Add symbol to scope</summary>
        /// <param name="symbol">Symbol to add</param>
        /// <exception cref="IdlCompilerException">On symbol redefinition</exception>        
        /// <returns>Same symbol or previous symbol if forward declaration exists</returns>
        public IIDLSymbol AddSymbolDefinition(IIDLSymbol symbol)
        {
            //Console.WriteLine(Namespace + " -> " + symbol.Name);
            var key = symbol.Name.ToLower();
            if (IDLKeywords.Contains(symbol.Name))
            {
                throw new IdlCompilerException("Identifier " + symbol.Name + " collides with keyword");
            }
            //if (key.Equals(Name.ToLower()))
            if (key.Equals(Name))
            {
                throw new IdlCompilerException("Redefinition of scope name " + Name);
            }
            if (IntroducedSymbols.Contains(key))
            {
                var s = IntroducedSymbols[key] as IDLSymbol;
                throw new IdlCompilerException("Redefinition of introduced symbol " + s.Name);
            }
            if (InheritedScopes.ContainsKey(key))
            {
                var s = InheritedScopes[key];
                throw new IdlCompilerException("Redefinition of inherited scope name " + s.Name);
            }
            if (Symbols.Contains(key))
            {                
                if (symbol is IFwdDeclSymbol symbolDef && Symbols[key] is IFwdDeclSymbol declaredSymbol && declaredSymbol.GetType().Equals(symbol.GetType()))
                {
                    if (symbolDef.IsForwardDeclaration)
                    {
                        symbolDef.ParentScope = this;
                        symbolDef.NamingScope = (declaredSymbol as IFwdDeclSymbol).NamingScope;
                        return declaredSymbol;
                    }
                    else if (declaredSymbol.IsForwardDeclaration)
                    {
                        //Definition of forward declared symbol
                        declaredSymbol.Define(symbolDef);
                        declaredSymbol.NamingScope.AddInheritedScopes();
                        symbolDef.ParentScope = this;
                        symbolDef.NamingScope = declaredSymbol.NamingScope;
                        return declaredSymbol;
                    }
                    else
                    {
                        throw new IdlCompilerException("Redefinition of symbol " + declaredSymbol.Name);
                    }
                }
                else
                {
                    throw new IdlCompilerException("Redefinition of symbol " + symbol.Name);
                }
            }
            symbol.ParentScope = this;
            Symbols[symbol.Name.ToLower()] = symbol;
            if (symbol is IScopeSymbol scopeSymbol)
            {
                if (ChildScopes.ContainsKey(key))
                {
                    var scope = ChildScopes[key];
                    throw new IdlCompilerException("Scope " + scope.Name + " already exists");
                }
                else
                {
                    var scope = new Scope(scopeSymbol);
                    scope.ParentScope = this;
                    ChildScopes.Add(key, scope);
                    scopeSymbol.NamingScope = scope;
                }
            }
            return symbol;
        }


        /// <summary>Add inherited scopes</summary>        
        public void AddInheritedScopes()
        {
            if (Symbol is Interface ifz)
            {
                foreach (var ancestor in ifz.Inherits)
                {
                    InheritedScopes.Add(ancestor.Name, ancestor.NamingScope);
                }
            } 
            else if (Symbol is Struct @struct)
            {
                if (@struct.Base != null)
                {
                    InheritedScopes.Add(@struct.Base.Name, @struct.Base.NamingScope);
                }
            }
            else if (Symbol is ValueType vt)
            {
                foreach (var ancestor in vt.Inherits)
                {
                    InheritedScopes.Add(ancestor.Name, ancestor.NamingScope);
                }
                foreach (var ancestor in vt.Supports)
                {
                    InheritedScopes.Add(ancestor.Name, ancestor.NamingScope);
                }
            }
        }


        /// <summary>Add new child scope</summary>
        /// <param name="name">Child scope name</param>
        /// <param name="symbol">Symbol that originates child scope</param>
        /// <returns>Created scope</returns>
        /// <exception cref="IdlCompilerException">When scope already exists</exception>        
        //private Scope AddChildScope(ScopeSymbol symbol)
        //{
        //    var key = symbol.Name.ToLower();
        //    if (ChildScopes.ContainsKey(key))
        //    {
        //        var scope = ChildScopes[key];
        //        throw new IdlCompilerException("Scope " + scope.Name + " already exists");
        //    }
        //    else
        //    {
        //        var scope = new Scope(name, symbol);
        //        scope.ParentScope = this;
        //        symbol.ParentScope = this;
        //        ChildScopes.Add(key, scope);
        //        return scope;
        //    }
        //}

        /// <summary>Returns child or inherited scope</summary>
        /// <param name="name">Name of scope</param>
        /// <exception cref="IdlCompilerException">When scope not found or case mismatch</exception>        
        public Scope GetChildScope(String name)
        {
            var key = name.ToLower();
            if (ChildScopes.ContainsKey(key))
            {
                var scope = ChildScopes[key];
                if (!scope.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + scope.Name);
                }
                return scope;
            }
            else if (InheritedScopes.ContainsKey(key))
            {
                var scope = InheritedScopes[key];
                if (!scope.Name.Equals(name))
                {
                    throw new IdlCompilerException("Case mismatch searching " + name + " found " + scope.Name);
                }
                return scope;
            }
            else
            {
                throw new IdlCompilerException("Scope  not found: " + name);
            }
        }

        /// <summary>Search symbol in current or nested scopes</summary>
        /// <param name="qname">Qualified name of symbol</param>
        /// <returns>Symbol or null if not found</returns>
        /// <exception cref="IdlCompilerException">When ambiguous symbol found, case mismatch or global scope unexpected</exception>        
        public IIDLSymbol GetSymbolDefinition(String qname)
        {            
            var parts = new List<String>(Regex.Split(qname, "::"));
            //Global scope
            if (parts.Count > 1 && String.IsNullOrEmpty(parts[0]))
            {
                //if i'm not global scope return error
                if (ParentScope != null)
                {
                    throw new IdlCompilerException("Global scope not expected " + qname);
                }
                parts.RemoveAt(0);
            }
            if (parts.Count > 1)
            {
                var scope = GetChildScope(parts[0]);
                parts.RemoveAt(0);
                return scope.GetSymbolDefinition(String.Join("::", parts));
            }
            if (parts.Count > 0)
            {
                var symbolName = parts[0].ToLower();
                IIDLSymbol ret = null;
                if (Symbols.Contains(symbolName))
                {
                    ret = Symbols[symbolName] as IIDLSymbol;

                    if (!parts[0].Equals(ret.Name))
                    {
                        throw new IdlCompilerException("Case mismatch searching " + parts[0] + " found " + ret.Name);
                    }
                }
                else
                {
                    foreach (var scope in InheritedScopes.Values)
                    {
                        var symbol = scope.GetSymbolDefinition(parts[0]);
                        if (symbol != null)
                        {
                            ret = symbol;
                        }
                    }
                }                
                return ret;
            }
            return null;
        }

        /// <summary>Search type in enclosing scopes</summary>
        /// <param name="qname">Qualified name of symbol</param>
        /// <exception cref="IdlCompilerException">When symbol not found</exception>
        public ITypeSymbol ResolveType(String qname)
        {
            var symbol = ResolveSymbol(qname);
            if (symbol == null)
            {
                throw new IdlCompilerException("Symbol not found: " + qname);
            }
            if (symbol is TypeDefinition typedef)
            {
                return typedef.ResolveDataType();
            }
            if (symbol is ITypeSymbol)
            {
                return symbol as ITypeSymbol;
            }
            else
            {
                throw new IdlCompilerException("Symbol " +  qname + " is not a type");
            }
        }

        public IIDLSymbol ResolveSymbol(String qname)
        {
            var parts = new List<String>(Regex.Split(qname,"::"));
            //Qualified name
            if (parts.Count > 1)
            {
                //Global scope
                if (String.IsNullOrEmpty(parts[0]))
                {
                    parts.RemoveAt(0);
                    return GlobalScope.GetSymbolDefinition(String.Join("::", parts));
                }
                else
                {
                    //first search in inherited scopes
                    var symbolName = parts[0].ToLower();
                    if (InheritedScopes.ContainsKey(symbolName))
                    {
                        var scope = InheritedScopes[symbolName];
                        parts.RemoveAt(0);
                        return scope.GetSymbolDefinition(String.Join("::", parts));
                    }
                    //search for type in parent scope
                    if (ParentScope != null)
                    {
                        var scope = ParentScope.GetScopeForSymbol(parts[0]);
                        if (scope.Symbol != null)
                        {
                            if (IntroducedSymbols.Contains(scope.Symbol.Name.ToLower()))
                            {
                                if (scope.Symbol != IntroducedSymbols[scope.Symbol.Name.ToLower()])
                                {
                                    throw new IdlCompilerException("Already introduced different symbol with the same name");
                                }
                            }
                            else
                            {
                                IntroducedSymbols.Add(scope.Symbol.Name.ToLower(), scope.Symbol);
                            }                            
                        }
                        parts.RemoveAt(0);
                        return scope.GetSymbolDefinition(String.Join("::", parts));
                    }
                }
            }
            else
            {
                //search in current and inherited scopes
                var symbol = GetSymbolDefinition(qname);
                if (symbol != null)
                {
                    return symbol;
                }
                //seach for type in parent scopes
                if (ParentScope != null)
                {
                    symbol = ParentScope.ResolveSymbol(qname);
                    if (symbol != null)
                    {
                        if (Symbol is not Module)
                        {
                            if (IntroducedSymbols.Contains(symbol.Name.ToLower()))
                            {
                                if (symbol != IntroducedSymbols[symbol.Name.ToLower()])
                                {
                                    throw new IdlCompilerException("Already introduced different symbol with the same name");
                                }
                            }
                            else
                            {
                                IntroducedSymbols.Add(symbol.Name.ToLower(), symbol);
                            }
                        }
                        return symbol;
                    }                    
                }
            }
            return null;
        }        

        public bool IsRecursive(ITypeSymbol typeSymbol)
        {
            var resolvedType = typeSymbol;
            //if (typeSymbol is TypeDefinition typedef)
            //{
            //    resolvedType = typedef.ResolveDataType();
            //}            
            if (resolvedType is Sequence seq)
            {
                resolvedType = seq.DataType;
            }
            if (Symbol != null && Symbol.FullName.Equals(resolvedType.FullName))
            {
                return true;
            }
            if (ParentScope != null)
            {
                return ParentScope.IsRecursive(resolvedType);
            }
            return false;
        }

        public void UpdateTypeInfo()
        {
            foreach(var ns in RepositoryIds.Keys)
            {
                try
                {
                    var symbol = ResolveSymbol(ns);
                    if (symbol != null)
                    {
                        symbol.TypeId = RepositoryIds[ns];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ns + " not found: " + ex.Message);
                }               
            }
            foreach (var ns in Versions.Keys)
            {
                try
                {
                    var symbol = ResolveSymbol(ns);
                    if (symbol != null)
                    {
                        symbol.TypeVersion = Versions[ns];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ns + " not found: " + ex.Message);
                }
            }
            foreach (var ns in Prefixes.Keys)
            {
                try
                {
                    var symbol = ResolveSymbol(ns);
                    if (symbol != null)
                    {
                        symbol.TypePrefix = Prefixes[ns];
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ns + " not found: " + ex.Message);
                }
            }
            foreach (var scope in ChildScopes.Values)
            {
                scope.UpdateTypeInfo();
            }
        }

        //private void AddPredefinedSymbol(IDLSymbol symbol)
        //{            
        //    symbol.ParentScope = this;
        //    Symbols.Add(symbol.Name.ToLower(), symbol);
        //}

        //private void AddPredefinedSymbols()
        //{
        //    AddPredefinedSymbol(new BasicType(BasicType.INT8));
        //    AddPredefinedSymbol(new BasicType(BasicType.UINT8));
        //    AddPredefinedSymbol(new BasicType(BasicType.SHORT));
        //    AddPredefinedSymbol(new BasicType(BasicType.INT16));
        //    AddPredefinedSymbol(new BasicType(BasicType.UNSIGNED_SHORT));
        //    AddPredefinedSymbol(new BasicType(BasicType.UINT16));
        //    AddPredefinedSymbol(new BasicType(BasicType.LONG));
        //    AddPredefinedSymbol(new BasicType(BasicType.INT32));
        //    AddPredefinedSymbol(new BasicType(BasicType.UNSIGNED_LONG));
        //    AddPredefinedSymbol(new BasicType(BasicType.UINT32));
        //    AddPredefinedSymbol(new BasicType(BasicType.LONG_LONG));
        //    AddPredefinedSymbol(new BasicType(BasicType.INT64));
        //    AddPredefinedSymbol(new BasicType(BasicType.UNSIGNED_LONG_LONG));
        //    AddPredefinedSymbol(new BasicType(BasicType.UINT64));
        //    AddPredefinedSymbol(new BasicType(BasicType.FLOAT));
        //    AddPredefinedSymbol(new BasicType(BasicType.DOUBLE));
        //    AddPredefinedSymbol(new BasicType(BasicType.LONG_DOUBLE));
        //    AddPredefinedSymbol(new BasicType(BasicType.CHAR));
        //    AddPredefinedSymbol(new BasicType(BasicType.WCHAR));
        //    AddPredefinedSymbol(new BasicType(BasicType.BOOLEAN));
        //    AddPredefinedSymbol(new BasicType(BasicType.OCTET));
        //    AddPredefinedSymbol(new BasicType(BasicType.OBJECT));
        //    AddPredefinedSymbol(new BasicType(BasicType.VALUE_BASE));
        //    AddPredefinedSymbol(new BasicType(BasicType.ANY));
        //}

    }
}
