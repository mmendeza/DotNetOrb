// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.GIOP;
using System;
using System.Threading;

namespace DotNetOrb.Core.DII
{
    public class Request : IRequest
    {
        public byte[] ObjectKey { get; private set; }
        public GIOPChannelHandler ChannelHandler { get; private set; }
        public IObject Target { get; private set; }
        public string Operation { get; private set; }
        public NVList Arguments { get; private set; }
        public NamedValue Result { get; private set; }
        public CORBA.Environment Env { get; private set; }
        public ExceptionList Exceptions { get; private set; }
        public ContextList Contexts { get; private set; }

        public CORBA.Any ReturnValue => Result.Value;

        public ORB orb;

        public IContext Ctx { get; set; }

        private Caller deferredCaller;

        public IInputStream reply;

        public bool isImmediate = false;
        public bool isDeferred = false;
        public bool isFinished = false;

        public ILogger logger;

        public object syncObject = new object();

        public Request(CORBA.Object target, ORB orb, GIOPChannelHandler channelHandler, byte[] objectKey, string operation) : this(target, orb, channelHandler, objectKey, operation, orb.CreateList(10), null, CreateVoidResultValue(orb))
        {
        }

        public Request(CORBA.Object target, ORB orb, GIOPChannelHandler channelHandler, byte[] objectKey, string operation, NVList args, IContext context, NamedValue result) : this(target, orb, channelHandler, objectKey, operation, args, context, result, new ExceptionList(), new ContextList())
        {
        }

        public Request(CORBA.Object target, ORB orb, GIOPChannelHandler channelHandler, byte[] objectKey, string operation, NVList args, IContext context, NamedValue result, ExceptionList exceptions, ContextList contexts)
        {
            Target = target;
            this.orb = orb;
            ChannelHandler = channelHandler;
            ObjectKey = objectKey;
            Operation = operation;
            Exceptions = exceptions;
            Contexts = contexts;
            Arguments = args;
            Ctx = context;
            Result = result;

            logger = orb.Configuration.GetLogger(GetType());
        }

        private static NamedValue CreateVoidResultValue(ORB orb)
        {
            var any = orb.CreateAny();
            any.Type = orb.GetPrimitiveTc(TCKind.TkVoid);
            NamedValue namedValue = new NamedValue(null, any, 1);
            return namedValue;
        }

        public CORBA.Any AddInArg()
        {
            NamedValue nv = Arguments.Add((uint)ARG_IN.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }
        public CORBA.Any AddNamedInArg(string name)
        {
            NamedValue nv = Arguments.AddItem(name, (uint)ARG_IN.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }

        public CORBA.Any AddInOutArg()
        {
            NamedValue nv = Arguments.Add((uint)ARG_IN_OUT.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }

        public CORBA.Any AddNamedInOutArg(string name)
        {
            NamedValue nv = Arguments.AddItem(name, (uint)ARG_IN_OUT.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }

        public CORBA.Any AddOutArg()
        {
            NamedValue nv = Arguments.Add((uint)ARG_OUT.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }

        public CORBA.Any AddNamedOutArg(string name)
        {
            NamedValue nv = Arguments.AddItem(name, (uint)ARG_OUT.Value);
            nv.Value = orb.CreateAny();
            return nv.Value;
        }
        public void SetReturnType(CORBA.TypeCode tc)
        {
            Result.Value.Type = tc;
        }

        public void GetResponse()
        {
            lock (syncObject)
            {
                if (!isImmediate && !isDeferred)
                {
                    throw new BadInvOrder(11, CompletionStatus.No);
                }
                if (isImmediate)
                {
                    throw new BadInvOrder(13, CompletionStatus.No);
                }

                if (deferredCaller != null)
                {
                    deferredCaller.JoinWithCaller();
                    deferredCaller = null;
                    orb.RemoveRequest(this);
                }
            }
        }

        public void Invoke()
        {
            Start();
            _Invoke(true);
            Finish();
        }

        public bool PollResponse()
        {
            lock (syncObject)
            {
                if (!isImmediate && !isDeferred)
                {
                    throw new BadInvOrder(11, CompletionStatus.No);
                }
                if (isImmediate)
                {
                    throw new BadInvOrder(13, CompletionStatus.No);
                }
                if (deferredCaller == null)
                {
                    throw new BadInvOrder(12, CompletionStatus.No);
                }
                return isFinished;
            }
        }

        public void SendDeferred()
        {
            lock (syncObject)
            {
                Defer();
                orb.AddRequest(this);
                deferredCaller = new Caller(this);
                deferredCaller.Start();
            }
        }

        public void SendOneWay()
        {
            Start();
            _Invoke(false);
            Finish();
        }

        private void Start()
        {
            lock (syncObject)
            {
                if (isImmediate || isDeferred)
                {
                    throw new BadInvOrder(10, CompletionStatus.No);
                }
                isImmediate = true;
            }
        }

        private void Defer()
        {
            lock (syncObject)
            {
                if (isImmediate || isDeferred)
                {
                    throw new BadInvOrder(10, CompletionStatus.No);
                }
                isDeferred = true;
            }
        }

        private void Finish()
        {
            lock (syncObject)
            {
                isFinished = true;
            }
        }

        private void ReadResult()
        {
            if (Result.Value.Type.Kind != TCKind.TkVoid)
            {
                Result.Value.ReadValue(reply, Result.Value.Type);
            }

            /** get out/inout parameters if any */
            foreach (var arg in Arguments)
            {
                if (arg.Flags != ARG_IN.Value)
                {
                    arg.Value.ReadValue(reply, arg.Value.Type);
                }
            }
        }

        private void _Invoke(bool responseExpected)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("DII::Request for " + Target);
            }

            while (true)
            {
                var @delegate = (Delegate)((CORBA.Object)Target)._Delegate;
                var output = @delegate.Request((CORBA.Object)Target, Operation, responseExpected);

                try
                {
                    foreach (var arg in Arguments)
                    {
                        if (arg.Flags != ARG_OUT.Value)
                        {
                            arg.Value.WriteValue(output);
                        }
                    }

                    try
                    {
                        reply = @delegate.Invoke((CORBA.Object)Target, output);

                        if (responseExpected)
                        {
                            ReadResult();
                        }
                    }
                    catch (RemarshalException e)
                    {
                        // Try again
                        continue;
                    }
                    catch (CORBA.ApplicationException e)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("DII Request caught ApplicationException", e);
                        }

                        CORBA.Any any;
                        CORBA.TypeCode typeCode;
                        string id = e.Id;
                        int count = Exceptions == null ? 0 : Exceptions.Count;

                        // since count > 0, this means that we were provided a
                        // list of exceptions that can be thrown by the invoked
                        // operation.  see if the ApplicationException we
                        // received matches any item in the exception list and
                        // set it if it does
                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                typeCode = Exceptions.Item(i);

                                if (id.Equals(typeCode.Id))
                                {
                                    if (logger.IsDebugEnabled)
                                    {
                                        logger.Debug("Found matching typecode of " + id + " to throw.");
                                    }
                                    any = orb.CreateAny();
                                    any.ReadValue(e.InputStream, typeCode);
                                    Env.Exception = new UnknownUserException(any);
                                    break;
                                }
                            }
                            catch (BadKind ex) // NOPMD
                            {
                                // Ignored
                            }
                            catch (Bounds ex)
                            {
                                break;
                            }
                        }
                        if (count == 0)
                        {
                            Env.Exception = new Unknown("Caught an unknown exception with typecode id of " + e.InputStream.ReadString(), 0, CompletionStatus.Yes);
                        }

                        break;
                    }
                    catch (Exception e)
                    {
                        if (logger.IsDebugEnabled)
                        {
                            logger.Debug("DII Request caught Exception", e);
                        }
                        Env.Exception = e;
                        break;
                    }
                    break;
                }
                finally
                {
                    output.Close();
                }
            }
        }


        private class Caller
        {
            private Request request;
            private bool isActive = true;
            private Thread thread;

            public Caller(Request client)
            {
                request = client;
                thread = new Thread(new ThreadStart(Run));
            }

            public void Run()
            {
                request._Invoke(true);
                request.Finish();

                lock (request)
                {
                    isActive = false;
                    Monitor.PulseAll(request);
                }
            }

            public void Start()
            {
                thread.Start();
            }

            public void JoinWithCaller()
            {
                lock (request)
                {
                    while (isActive)
                    {
                        try
                        {
                            Monitor.Wait(request);
                        }
                        catch (ThreadInterruptedException e) // NOPMD
                        {
                            // ignored
                        }
                    }
                }
            }
        }
    }
}
