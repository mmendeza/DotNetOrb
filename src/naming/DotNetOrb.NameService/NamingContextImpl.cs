// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using CosNaming;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.Util;
using Microsoft.Extensions.Logging;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CosNaming.NamingContext;

namespace DotNetOrb.NameService
{
    public class NamingContextImpl : NamingContextExtPOA, IConfigurable
    {
        // all name bindings in this contexts
        private Dictionary<Name, IObject> names = new Dictionary<Name, IObject>();

        // all subordinate naming contexts
        private Dictionary<Name, IObject> contexts = new Dictionary<Name, IObject>();
        
        private Core.Config.IConfiguration configuration = null;

        // no tests of bound objects for existence
        private bool ping = false;

        // purge?
        private bool doPurge = false;
        
        private Core.ILogger logger = null;

        // the POAs used
        private IPOA poa;
        public IPOA DefaultPOA => poa;

        private static IPOA rootPoa;
        private static ORB orb;

        private int childCount;
        private bool destroyed = false;

        public int Count
        {
            get
            {
                if (destroyed)
                    return 0;
                return names.Count + contexts.Count;
            }            
        }

        public void Configure(Core.Config.IConfiguration config)
        {
            this.configuration = config;
            logger = configuration.GetLogger(GetType());
            doPurge = configuration.GetAsBoolean("DotNetOrb.Naming.Purge", false);
            ping = configuration.GetAsBoolean("DotNetOrb.Naming.Ping", false);
        }

        /// <summary>
        ///  This method needs to be called once to initialize the static fields orb and rootPoa.
        /// </summary>
        public static void Init(ORB orb, IPOA rootPoa)
        {
            NamingContextImpl.orb = orb;
            NamingContextImpl.rootPoa = rootPoa;
        }
        
        public NamingContextImpl(IPOA poa)
        {
            this.poa = poa;
        }


        /// <summary>
        /// Bind a name (an array of name components) to an object
        /// </summary>        
        public override void Bind(NameComponent[] nc, IObject obj)
        {
            if (destroyed)
                throw new CannotProceed();

            if (nc == null || nc.Length == 0)
                throw new NamingContext.InvalidName();

            if (obj == null)
                throw new CORBA.BadParam();

            Name n = new Name(nc);
            Name ctx = n.CtxName;
            NameComponent nb = n.BaseNameComponent;
            if (ctx == null)
            {
                if (names.ContainsKey(n))
                {
                    // if the name is still in use, try to ping the object
                    IObject s = names[n];
                    if (IsDead(s))
                    {
                        Rebind(n.Components, obj);
                        return;
                    }
                    throw new AlreadyBound();
                }
                else if (contexts.ContainsKey(n))
                {
                    // if the name is still in use, try to ping the object
                    IObject s = contexts[n];
                    if (IsDead(s))
                    {
                        Unbind(n.Components);
                    }
                    throw new AlreadyBound();
                }

                if (!names.TryAdd(n, obj))
                    throw new CannotProceed(_This(), n.Components);

                if (logger.IsInfoEnabled)
                {
                    logger.Info("Bound name: " + n.ToString());
                }
            }
            else
            {
                NameComponent[] ncx = new NameComponent[1];
                ncx[0] = nb;
                NamingContextExtHelper.Narrow(Resolve(ctx.Components)).Bind(ncx, obj);
            }
        }

        public override void BindContext(NameComponent[] nc, INamingContext obj)
        {
            if (destroyed)
                throw new CannotProceed();

            Name n = new Name(nc);
            Name ctx = n.CtxName;
            NameComponent nb = n.BaseNameComponent;

            if (ctx == null)
            {
                if (names.ContainsKey(n))
                {
                    // if the name is still in use, try to ping the object
                    var s = names[n];
                    if (IsDead(s))
                    {
                        Unbind(n.Components);
                    }
                    else
                        throw new AlreadyBound();
                }
                else if (contexts.ContainsKey(n))
                {
                    // if the name is still in use, try to ping the object
                    var s = contexts[n];
                    if (IsDead(s))
                    {
                        RebindContext(n.Components, obj);
                        return;
                    }
                    throw new AlreadyBound();
                }

                if (!contexts.TryAdd(n, obj))
                    throw new CannotProceed(_This(), n.Components);

                if (logger.IsInfoEnabled)
                {
                    logger.Info("Bound context: " + n.ToString());
                }
            }
            else
            {
                NameComponent[] ncx = new NameComponent[1];
                ncx[0] = nb;
                NamingContextExtHelper.Narrow(Resolve(ctx.Components)).BindContext(ncx, obj);
            }
        }

        public override INamingContext BindNewContext(NameComponent[] nc)
        {
            if (destroyed)
                throw new CannotProceed();

            if (nc == null || nc.Length == 0)
                throw new NamingContext.InvalidName();

            var ns = NamingContextExtHelper.Narrow(NewContext());
            BindContext(nc, ns);

            if (ns == null)
            {
                throw new CannotProceed();
            }
            return ns;
        }

        private void Cleanup()
        {
            // Check if object purging enabled
            if (!doPurge)
            {
                return;
            }
            var toDelete = new List<Name>();
            foreach (var kvp in names)
            {
                if (IsDead(kvp.Value))
                {
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Removing name " + kvp.Key.BaseNameComponent.Id);
                    }
                    toDelete.Add(kvp.Key);
                }
            }            
            if (toDelete.Count > 0)
            {
                foreach (var name in toDelete)
                {
                    names.Remove(name);
                }                
                toDelete.Clear();
            }
            // ping contexts
            foreach (var kvp in contexts)
            {
                if (IsDead(kvp.Value))
                {
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Removing context " + kvp.Key.BaseNameComponent.Id);
                    }
                    toDelete.Add(kvp.Key);
                }
            }
            if (toDelete.Count > 0)
            {
                foreach (var name in toDelete)
                {
                    contexts.Remove(name);
                }
                toDelete.Clear();
            }
        }

        public override void Destroy()
        {
            if (destroyed)
                return;

            if (names.Count > 0 || contexts.Count > 0)
                throw new NotEmpty();
            else
            {
                names = null;
                contexts = null;
                destroyed = true;
            }
        }

        public override void List(uint howMany, out Binding[] bl, out IBindingIterator bi)
        {
            bl = null;
            bi = null;

            if (destroyed)
            {
                return;
            }                

            var result = new List<Binding>();

            Cleanup();

            int size = Count;

            var n = names.Keys.GetEnumerator();
            n.MoveNext();
            var c = contexts.Keys.GetEnumerator();
            c.MoveNext();

            
            if (howMany < size)
            {
                var counter = howMany;

                for (; n.MoveNext() && counter > 0; counter--)
                {
                    result.Add(new Binding(n.Current.Components, BindingType.Nobject));
                }

                for (; c.MoveNext() && counter > 0; counter--)
                {
                    result.Add(new Binding(c.Current.Components, BindingType.Ncontext));
                }

                // create a new BindingIterator for the remaining arrays
                size -= Count;
                Binding[] rest = new Binding[size];

                for (; n.MoveNext() && size > 0; size--)
                {
                    rest[size - 1] = new Binding(n.Current.Components, BindingType.Nobject);
                }

                for (; c.MoveNext() && size > 0; size--)
                {
                    rest[size - 1] = new Binding(c.Current.Components, BindingType.Ncontext);
                }

                CORBA.IObject o = null;
                try
                {
                    // Iterators are activated with the RootPOA (transient)
                    byte[] oid = rootPoa.ActivateObject(new BindingIterator(rest));
                    o = rootPoa.IdToReference(oid);
                }
                catch (Exception e)
                {
                    logger.Error("unexpected exception", e);
                    throw new CORBA.Internal(e.ToString());
                }
                bi = BindingIteratorHelper.Narrow(o);
            }
            else
            {                
                for (; n.MoveNext() && size > 0; size--)
                {
                    result.Add(new Binding(n.Current.Components, BindingType.Nobject));
                }

                for (; c.MoveNext() && size > 0; size--)
                {
                    result.Add(new Binding(c.Current.Components, BindingType.Ncontext));
                }
            }
            bl = result.ToArray();
        }

        public override INamingContext NewContext()
        {
            if (destroyed)
                return null;

            CORBA.IObject ctx = null;
            try
            {
                var oidStr = Encoding.Default.GetString(poa.ServantToId(this)) + "_ctx" + (++childCount);
                byte[] oid = Encoding.Default.GetBytes(oidStr);

                ctx = poa.CreateReferenceWithId(oid, "IDL:omg.org/CosNaming/NamingContextExt:1.0");
                if (logger.IsInfoEnabled)
                {
                    logger.Info("New context.");
                }
            }
            catch (Exception ue)
            {
                if (logger.IsErrorEnabled)
                {
                    logger.Error("failed to create new context", ue);
                }
                throw new RuntimeException("failed to create new context: " + ue.ToString());
            }
            return NamingContextExtHelper.Narrow(ctx);
        }

        public override void Rebind(NameComponent[] nc, IObject obj)
        {
            if (destroyed)
                throw new CannotProceed();

            if (nc == null || nc.Length == 0)
                throw new NamingContext.InvalidName();

            if (obj == null)
                throw new CORBA.BadParam();

            Name n = new Name(nc);
            Name ctx = n.CtxName;
            NameComponent nb = n.BaseNameComponent;

            // the name is bound, but it is bound to a context,
            // the client should have been using rebind_context!
            if (contexts.ContainsKey(n))
                throw new NotFound(NotFoundReason.NotObject, new NameComponent[] { nb });

            // try remove an existing binding
            if (names.Remove(n, out IObject _o))
            {
                _o._Release();
            }
                           

            if (ctx == null)
            {
                // do the rebinding in this context
                names[n] = obj;
                if (logger.IsInfoEnabled)
                {
                    logger.Info("re-Bound name: " + n.ToString());
                }
            }
            else
            {
                // rebind in the correct context
                NameComponent[] ncx = new NameComponent[1];
                ncx[0] = nb;
                INamingContextExt nce = NamingContextExtHelper.Narrow(Resolve(ctx.Components));
                if (nce == null)
                    throw new CannotProceed();
                nce.Rebind(ncx, obj);
            }
        }

        public override void RebindContext(NameComponent[] nc, INamingContext obj)
        {
            if (destroyed)
                throw new CannotProceed();

            if (nc == null || nc.Length == 0)
                throw new NamingContext.InvalidName();

            if (obj == null)
                throw new CORBA.BadParam();

            Name n = new Name(nc);
            Name ctx = n.CtxName;
            NameComponent nb = n.BaseNameComponent;

            // the name is bound, but it is bound to an object,
            // the client should have been using rebind() !
            if (names.ContainsKey(n))
                throw new NotFound(NotFoundReason.NotContext, new NameComponent[] { nb });

            // try to remove an existing context binding
            if (contexts.Remove(n, out IObject _o))
            {
                _o._Release();
            }           

            if (ctx == null)
            {
                contexts[n] = obj;
                if (logger.IsInfoEnabled)
                {
                    logger.Info("Re-Bound context: " + n.BaseNameComponent.Id);
                }
            }
        }

        public override IObject Resolve(NameComponent[] n)
        {
            throw new NotImplementedException();
        }

        public override IObject ResolveStr([WideChar(false)] string n)
        {
            return Resolve(ToName(n));
        }

        public override NameComponent[] ToName([WideChar(false)] string sn)
        {
            return Name.ToName(sn);
        }

        [return: WideChar(false)]
        public override string ToString(NameComponent[] n)
        {
            return Name.ToString(n);
        }

        [return: WideChar(false)]
        public override string ToUrl([WideChar(false)] string addr, [WideChar(false)] string sn)
        {
            CorbaLoc corbaLoc;
            try
            {
                corbaLoc = new CorbaLoc((Core.ORB)orb, addr);
                return corbaLoc.ToCorbaName(sn);
            }
            catch (ArgumentException)
            {
                throw new NamingContextExt.InvalidAddress();
            }
        }

        public override void Unbind(NameComponent[] nc)
        {
            if (destroyed)
                throw new CannotProceed();

            if (nc == null || nc.Length == 0)
                throw new NamingContext.InvalidName();

            Name n = new Name(nc);
            Name ctx = n.CtxName;
            NameComponent nb = n.BaseNameComponent;

            if (ctx == null)
            {
                if (names.ContainsKey(n))
                {
                    if (names.Remove(n, out IObject o))
                    {
                        o._Release();
                    }                    
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Unbound: " + n.ToString());
                    }
                }
                else if (contexts.ContainsKey(n))
                {
                    if (contexts.Remove(n, out IObject o))
                    {
                        o._Release();
                    }                    
                    if (logger.IsInfoEnabled)
                    {
                        logger.Info("Unbound: " + n.ToString());
                    }
                }
                else
                {
                    if (logger.IsWarnEnabled)
                    {
                        logger.Warn("Unbind failed for " + n.ToString());
                    }
                    throw new NotFound(NotFoundReason.NotContext, n.Components);
                }
            }
            else
            {
                NameComponent[] ncx = new NameComponent[1];
                ncx[0] = nb;
                NamingContextExtHelper.Narrow(Resolve(ctx.Components)).Unbind(ncx);
            }
        }

        /// <summary>
        /// Determine if object is non existent
        /// </summary>
        private bool IsDead(IObject o)
        {
            bool nonExist = true;
            try
            {
                nonExist = o._NonExistent();                
                if (!nonExist)
                {
                    o._Release();
                }
            }
            catch (CORBA.NoImplement)
            {
                // not a failure, the peer is alive, it just doesn't implement _nonExistent()
                nonExist = false;
            }
            catch (CORBA.SystemException)
            {
                nonExist = true;
            }
            return nonExist;
        }

        public void Read(StreamReader sr)
        {
            var count = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < count; i++)
            {
                var name = new Name(sr.ReadLine());
                var obj = orb.StringToObject(sr.ReadLine());
                names.Add(name, obj);
            }
            count = Convert.ToInt32(sr.ReadLine());
            for (int i = 0; i < count; i++)
            {
                var name = new Name(sr.ReadLine());
                var obj = orb.StringToObject(sr.ReadLine());
                contexts.Add(name, obj);
            }
        }

        public void Write(StreamWriter sw)
        {            
            sw.WriteLine(names.Count);
            foreach (var kvp in names)
            {
                sw.WriteLine(kvp.Key.ToString());
                sw.WriteLine(orb.ObjectToString(kvp.Value));
            }
            sw.WriteLine(contexts.Count);
            foreach (var kvp in contexts)
            {
                sw.WriteLine(kvp.Key.ToString());
                sw.WriteLine(orb.ObjectToString(kvp.Value));
            }
        }
    }
}
