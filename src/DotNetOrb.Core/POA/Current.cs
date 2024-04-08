// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;
using System.Collections.Generic;
using System.Threading;

namespace DotNetOrb.Core.POA
{
    public class Current : PortableServer._CurrentLocalBase
    {
        private Dictionary<Thread, LinkedList<IInvocationContext>> threadTable = new Dictionary<Thread, LinkedList<IInvocationContext>>();
        private object syncObject = new object();

        public void AddContext(Thread thread, IInvocationContext c)
        {
            lock (syncObject)
            {
                if (!threadTable.ContainsKey(thread))
                {
                    var list = new LinkedList<IInvocationContext>();
                    threadTable[thread] = list;
                }
                threadTable[thread].AddLast(c);
            }
        }

        public void RemoveContext(Thread thread)
        {
            lock (syncObject)
            {
                if (threadTable.ContainsKey(thread))
                {
                    var list = threadTable[thread];

                    list.RemoveLast();

                    if (list.Count == 0)
                    {
                        threadTable.Remove(thread);
                    }
                }
            }
        }

        private IInvocationContext GetInvocationContext()
        {
            lock (syncObject)
            {
                Thread thread = Thread.CurrentThread;

                if (threadTable.ContainsKey(thread))
                {
                    var list = threadTable[thread];

                    var context = list.Last;

                    if (context != null)
                    {
                        return context.Value;
                    }
                }
                throw new PortableServer.Current.NoContext();
            }
        }
        public ORB GetORB()
        {
            return GetInvocationContext().ORB;
        }

        public override byte[] GetObjectId()
        {
            return GetInvocationContext().ObjectId;
        }

        public override IPOA GetPoa()
        {
            return GetInvocationContext().POA;
        }

        public override IObject GetReference()
        {
            return GetServant()._ThisObject(GetORB());
        }

        public override Servant GetServant()
        {
            return GetInvocationContext().Servant;
        }
    }
}
