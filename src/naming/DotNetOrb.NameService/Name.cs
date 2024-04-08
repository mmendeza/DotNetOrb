// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CosNaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotNetOrb.NameService
{
    public class Name
    {
        private NameComponent[] fullName;
        private NameComponent baseName;

        // context part of this Name
        private NameComponent[] ctxName;

        public Name()
        {
            fullName = null;
            baseName = null;
            ctxName = null;
        }

        /// <summary>
        /// Create a name from an array of NameComponents
        /// </summary>
        /// <exception cref="InvalidName"></exception>
        public Name(NameComponent[] n)
        {
            if (n == null || n.Length == 0)
            {
                throw new NamingContext.InvalidName();
            }
            fullName = n;
            baseName = n[n.Length - 1];
            if (n.Length > 1)
            {
                ctxName = new NameComponent[n.Length - 1];
                for (int i = 0; i < n.Length - 1; i++)
                    ctxName[i] = n[i];
            }
            else
            {
                ctxName = null;
            }
        }

        /// <summary>
        /// create a name from a stringified name
        /// </summary>
        /// <param name="strName"></param>
        public Name(string strName) : this(ToName(strName))
        {

        }

        /// <summary>
        /// Create a name from a singleNameComponent
        /// </summary>        
        /// <exception cref="NamingContext.InvalidName"></exception>
        public Name(NameComponent n)
        {
            if (n == null)
            {
                throw new NamingContext.InvalidName();
            }

            baseName = n;
            fullName = new NameComponent[1];
            fullName[0] = n;
            ctxName = null;
        }


        /// <summary>
        /// Return a NameComponent object representing the unstructured base name of this structured name
        /// </summary>    
        public NameComponent BaseNameComponent
        {
            get { return baseName; }
        }


        public string Kind
        {
            get { return baseName.Kind; }
        }


        /// <summary>
        /// Return this name as an array of org.omg.CosNaming.NameComponent, 
        /// neccessary for a number of operations on naming context
        /// </summary>
        /// <returns></returns>
        public NameComponent[] Components
        {
            get { return fullName; }
        }

        /// <summary>
        /// Return a Name object representing the name of the enclosing context
        /// </summary>
        public Name CtxName
        {
            get
            {
                // null if no further context
                if (ctxName != null)
                {
                    try
                    {
                        return new Name(ctxName);
                    }
                    catch (NamingContext.InvalidName e)
                    {
                        throw new CORBA.Internal(e.ToString());
                    }
                }
                return null;
            }
        }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Name)) return false;
            return (ToString().Equals(obj.ToString()));
        }


        public Name FullName
        {
            get
            {
                return new Name(fullName);
            }
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }


        /// <summary>
        /// Return  the string representation of this name
        /// </summary>        
        public override string ToString()
        {
            try
            {
                return ToString(fullName);
            }
            catch (NamingContext.InvalidName)
            {
                return "<invalid>";
            }
        }

        /// <summary>
        /// Return a single NameComponent, parsed from sn
        /// </summary>
        /// <exception cref="NamingContext.InvalidName"></exception>
        private static NameComponent GetComponent(string sn)
        {
            char ch;
            int len = sn.Length;
            bool inKind = false;
            StringBuilder id = new StringBuilder();
            StringBuilder kind = new StringBuilder();

            for (int i = 0; i < len; i++)
            {
                ch = sn[i];

                if (ch == '\\')
                {
                    // Escaped character
                    i++;
                    if (i >= len)
                    {
                        throw new NamingContext.InvalidName();
                    }
                    ch = sn[i];
                }
                else if (ch == '.')
                {
                    // id/kind separator character
                    if (inKind)
                    {
                        throw new NamingContext.InvalidName();
                    }
                    inKind = true;
                    continue;
                }
                if (inKind)
                {
                    kind.Append(ch);
                }
                else
                {
                    id.Append(ch);
                }
            }
            return (new NameComponent(id.ToString(), kind.ToString()));
        }

        /// <summary>
        /// return an a array of NameComponents
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        /// <exception cref="NamingContext.InvalidName"></exception>
        public static NameComponent[] ToName(string sn)
        {
            if (sn == null || sn.Length == 0 || sn.StartsWith("/"))
            {
                throw new NamingContext.InvalidName();
            }
            var components = new List<NameComponent>();
            int start = 0;
            int i = 0;
            for (; i < sn.Length; i++)
            {
                if (sn[i] == '/' && sn[i - 1] != '\\')
                {
                    if (i - start == 0)
                    {
                        throw new NamingContext.InvalidName();
                    }
                    components.Add(GetComponent(sn.Substring(start, i - start + 1)));
                    start = i + 1;
                }
            }
            if (start < i)
            {
                components.Add(GetComponent(sn.Substring(start, i - start + 1)));
            }
            return components.ToArray();
        }

        /// <summary>
        /// return the string representation of this NameComponent array
        /// </summary>
        /// <param name="n"></param>        
        /// <exception cref="NamingContext.InvalidName"></exception>
        public static string ToString(NameComponent[] n)
        {
            if (n == null || n.Length == 0)
            {
                throw new NamingContext.InvalidName();
            }

            var b = new StringBuilder();
            for (int i = 0; i < n.Length; i++)
            {
                if (i > 0)
                    b.Append("/");

                if (n[i].Id.Length > 0)
                    b.Append(Escape(n[i].Id));

                if (n[i].Kind.Length > 0 ||
                n[i].Id.Length == 0)
                    b.Append(".");

                if (n[i].Kind.Length > 0)
                    b.Append(Escape(n[i].Kind));
            }
            return b.ToString();
        }


        /// <summary>
        /// Rscape any occurrence of "/", "." and "\"
        /// </summary>
        private static string Escape(string s)
        {
            var sb = new StringBuilder(s);
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '/' ||
                sb[i] == '\\' ||
                sb[i] == '.')
                {
                    sb.Insert(i, '\\');
                    i++;
                }
            }
            return sb.ToString();
        }
    }
}
