// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using PortableServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DotNetOrb.Core.POA.POAConstants;
using static PortableServer.POAManager;

namespace DotNetOrb.Core.POA
{
    public class POAManager : _POAManagerLocalBase
    {
        private State state = State.HOLDING;

        private ORB orb;
        private List<POA> poas = new List<POA>();
        internal IPOAManagerMonitor Monitor { get; set; }
        internal bool poaCreationFailed;
        private object syncObject = new object();

        /// <summary>
        /// It returns true if the current thread is not in an invocation context 
        /// dispatched by some POA belonging to the same ORB as this POAManager.
        /// </summary>
        private bool IsInInvocationContext
        {
            get
            {
                try
                {
                    if (orb.GetPOACurrent().GetORB() == orb)
                    {
                        return true;
                    }
                }
                catch (PortableServer.Current.NoContext)
                {

                }
                return false;
            }
        }

        internal POAManager(ORB orb)
        {
            this.orb = orb;
            Monitor = new POAManagerMonitor();
            Monitor.Configure(orb.Configuration);
            Monitor.Init(this);
            Monitor.OpenMonitor();
            Monitor.PrintMessage("ready");
        }

        public override State GetState()
        {
            return state;
        }

        public override void Activate()
        {
            CheckCreation();

            switch (state)
            {
                case State.INACTIVE:
                    throw new AdapterInactive();
                case State.ACTIVE:
                    break;
                default:
                    state = State.ACTIVE;
                    Monitor.SetToActive();
                    POA[] poaArray;
                    lock (syncObject)
                    {
                        poaArray = poas.ToArray();
                    }
                    // notify all registered poas
                    Task.Run(() =>
                    {
                        foreach (var poa in poaArray)
                        {
                            try
                            {
                                poa.ChangeToActive();
                            }
                            catch { }

                        }
                    });
                    break;
            }
        }

        public override void Deactivate(bool etherealizeObjects, bool waitForCompletion)
        {
            CheckCreation();

            if (waitForCompletion && IsInInvocationContext)
            {
                throw new BadInvOrder();
            }

            switch (state)
            {
                case State.INACTIVE:
                    throw new AdapterInactive();
                default:
                    state = State.INACTIVE;
                    Monitor.SetToInactive(waitForCompletion, etherealizeObjects);
                    POA[] poaArray;
                    lock (syncObject)
                    {
                        poaArray = poas.ToArray();
                    }
                    // notify all registered poas
                    var task = Task.Run(() =>
                    {
                        foreach (var poa in poaArray)
                        {
                            try
                            {
                                poa.ChangeToInactive(etherealizeObjects);
                            }
                            catch { }

                        }
                    });
                    if (waitForCompletion)
                    {
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception e) { }
                    }
                    break;
            }
        }

        public override void DiscardRequests(bool waitForCompletion)
        {
            CheckCreation();

            if (waitForCompletion && IsInInvocationContext)
            {
                throw new BadInvOrder();
            }

            switch (state)
            {
                case State.INACTIVE:
                    throw new AdapterInactive();
                case State.DISCARDING:
                    break;
                default:
                    state = State.DISCARDING;
                    Monitor.SetToDiscarding(waitForCompletion);
                    POA[] poaArray;
                    lock (syncObject)
                    {
                        poaArray = poas.ToArray();
                    }
                    var task = Task.Run(() =>
                    {
                        foreach (var poa in poaArray)
                        {
                            try
                            {
                                poa.ChangeToDiscarding();
                            }
                            catch { }

                        }
                    });
                    if (waitForCompletion)
                    {
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception e) { }
                    }
                    break;
            }
        }

        internal POA GetRegisteredPOA(string name)
        {
            lock (syncObject)
            {
                foreach (var poa in poas)
                {
                    if (name.Equals(poa.QualifiedName))
                    {
                        return poa;
                    }
                }
                throw new CORBA.Internal("POA not registered: " + RootPOAName + ObjectKeySeparator + name);
            }
        }

        public override void HoldRequests(bool waitForCompletion)
        {
            CheckCreation();

            if (waitForCompletion && IsInInvocationContext)
            {
                throw new BadInvOrder();
            }
            switch (state)
            {
                case State.INACTIVE:
                    throw new AdapterInactive();
                case State.HOLDING:
                    break;
                default:
                    state = State.HOLDING;
                    Monitor.SetToHolding(waitForCompletion);
                    POA[] poaArray;
                    lock (syncObject)
                    {
                        poaArray = poas.ToArray();
                    }
                    var task = Task.Run(() =>
                    {
                        foreach (var poa in poaArray)
                        {
                            try
                            {
                                poa.ChangeToHolding();
                            }
                            catch { }

                        }
                    });
                    if (waitForCompletion)
                    {
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception e) { }
                    }
                    break;
            }
        }

        internal void RegisterPOA(POA poa)
        {
            lock (syncObject)
            {
                if (!poas.Contains(poa))
                {
                    poas.Add(poa);
                    Monitor.AddPOA(poa.QualifiedName);
                }
            }
        }

        internal void UnregisterPOA(POA poa)
        {
            lock (syncObject)
            {
                poas.Remove(poa);
                Monitor.RemovePOA(poa.QualifiedName);
            }
        }

        private void CheckCreation()
        {
            if (poaCreationFailed)
            {
                throw new CORBA.Internal("POA Creation failed; unable to deactive");
            }
        }

        [return: WideChar(false)]
        public override string GetId()
        {
            return "POAManager";
        }
    }
}
