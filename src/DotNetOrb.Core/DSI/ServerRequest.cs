// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.POA.Util;
using GIOP;
using IOP;
using System;
using System.Collections.Generic;

namespace DotNetOrb.Core.DSI
{
    public class ServerRequest : IServerRequest, IResponseHandler
    {
        private ORB orb;
        private ILogger logger;
        private RequestInputStream inputStream;
        public RequestInputStream InputStream
        {
            get
            {
                IsStreamBased = true;
                return inputStream;
            }
        }

        private ReplyOutputStream outputStream;
        private ReplyOutputStream OutputStream
        {
            get
            {
                if (outputStream == null)
                {
                    outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
                }
                return outputStream;
            }
        }

        private GIOPChannelHandler channelHandler;
        public GIOPChannelHandler ChannelHandler => channelHandler;

        /// <summary>
        /// Caches the scoped poa names
        /// </summary>
        private List<string> scopes;
        public List<string> Scopes
        {
            get
            {
                if (scopes == null || !cachePoaNames)
                {
                    scopes = POAUtil.ExtractScopedPOANames(POAUtil.ExtractPOAName(objectKey));
                }
                return scopes;
            }
        }

        private bool cachePoaNames;

        private byte[] oid;
        public byte[] ObjectId
        {
            get { return oid; }
        }
        private object syncObject = new object();

        private ByteArrayKey byteArrayKey;
        public ByteArrayKey ObjectIdAsByteArrayKey
        {
            get
            {
                lock (syncObject)
                {
                    if (byteArrayKey == null)
                    {
                        byteArrayKey = new ByteArrayKey(oid);
                    }

                    return byteArrayKey;
                }
            }
        }

        private byte[] objectKey;

        public byte[] ObjectKey
        {
            get
            {
                return objectKey;
            }
            set
            {
                objectKey = value;
                oid = POAUtil.ExtractOID(objectKey);
            }
        }

        private ReplyStatusType12 replyStatus = ReplyStatusType12.NO_EXCEPTION;
        public ReplyStatusType12 ReplyStatus
        {
            get => replyStatus;
        }

        /// <summary>
        /// Target poa's name in relation to parent.
        /// </summary>
        private string[] restOfName = null;

        /// <summary>
        /// If this request could not be delivered directly to the correct POA because the POA's adapter activator 
        /// could not be called when the parent POA was in holding state, the parent will queue the request and later 
        /// return it to the adapter layer.In order to be able to find the right POA when trying to deliver again, 
        /// we have to remember the target POA's name
        /// </summary>
        public string[] RemainingPOAName
        {
            get => restOfName;
            set
            {
                restOfName = value;
            }
        }

        /// <summary>
        /// Is this request stream or DSI-based ?
        /// </summary>
        public bool IsStreamBased { get; private set; }

        private CORBA.SystemException sysEx;

        public CORBA.SystemException SystemException
        {
            get
            {
                return sysEx;
            }
            set
            {
                replyStatus = ReplyStatusType12.SYSTEM_EXCEPTION;
                /* we need to create a new output stream here because a system exception may
                    have occurred *after* a no_exception request header was written onto the
                    original output stream*/


                outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
                string msg = value.Message;
                if (msg != null)
                {
                    outputStream.ServiceContextList.Add(CreateExceptionDetailMessage(msg));
                }
                sysEx = value;
            }
        }
        private PortableServer.ForwardRequest locationForward;

        public PortableServer.ForwardRequest LocationForward
        {
            get { return locationForward; }
            set
            {
                replyStatus = ReplyStatusType12.LOCATION_FORWARD;

                outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
                locationForward = value;
            }
        }

        public CORBA.Object ForwardReference
        {
            get
            {
                if (locationForward != null)
                {
                    return (CORBA.Object)locationForward.ForwardReference;
                }
                return null;
            }
        }

        private CORBA.Any exception;
        public CORBA.Any Exception
        {
            get
            {
                if (IsStreamBased)
                {
                    throw new BadInvOrder("This ServerRequest is stream-based!");
                }
                return exception;
            }
            set
            {
                if (IsStreamBased)
                {
                    throw new BadInvOrder("This ServerRequest is stream-based!");
                }
                exception = value;
                replyStatus = ReplyStatusType12.USER_EXCEPTION;
            }
        }
        private NVList argList;

        //private ServerRequestInfo info = null;

        //MIOP
        //private ByteArrayKey byteArrayKey;
        //private TagGroupTaggedComponent tagGroup = null;

        public string Operation
        {
            get
            {
                return inputStream.Operation;
            }
        }

        public bool ResponseExpected
        {
            get
            {
                return inputStream.ResponseExpected;
            }
        }

        public SyncScope SyncScope
        {
            get
            {
                return inputStream.SyncScope;
            }
        }

        public int RequestId { get; set; }

        private CORBA.Any result;
        public CORBA.Any Result
        {
            get
            {
                if (IsStreamBased)
                {
                    CORBA.Any any = orb.CreateAny();
                    // create the output stream for the result
                    CDROutputStream _out = (CDROutputStream)any.CreateOutputStream();

                    // get a copy of the content of this reply
                    //byte[] resultBuf = orb.BufferManager.GetBuffer(outputStream.Size() - outputStream.GetBodyBegin());
                    //Array.Copy(outputStream.getBufferCopy(), outputStream.getBodyBegin(), resultBuf, 0, resultBuf.Length);
                    // ... and insert it
                    //_out.SetBuffer(resultBuf);
                    _out.SetBuffer(outputStream.Buffer);
                    // important: set the _out buffer's position to the end of the contents!
                    //_out.Skip(resultBuf.Length);
                    return any;
                }
                return result;
            }
            set
            {
                if (IsStreamBased)
                {
                    throw new BadInvOrder("This ServerRequest is stream based!");
                }
                result = value;
            }
        }

        public InvocationPolicies InvocationPolicies { get; set; }

        public ServerRequest(ORB orb, RequestInputStream inputStream, GIOPChannelHandler channelHandler)
        {
            this.orb = orb;
            var config = orb.Configuration;
            logger = config.GetLogger(GetType().FullName);
            cachePoaNames = config.GetAsBoolean("DotNetOrb.CachePoaNames", false);

            this.inputStream = inputStream;
            this.channelHandler = channelHandler;
            InvocationPolicies = new InvocationPolicies(inputStream.ServiceContextList);

            objectKey = orb.MapObjectKey(ParsedIOR.ExtractObjectKey(inputStream.Target, orb));
            oid = POAUtil.ExtractOID(objectKey);
        }

        public NVList Arguments
        {
            get
            {
                if (IsStreamBased)
                {
                    throw new BadInvOrder("This ServerRequest is stream based!");
                }
                return argList;
            }
        }

        public void GetArguments(NVList argList)
        {
            if (argList != null)
            {
                inputStream.Mark(0);
                foreach (var namedValue in argList)
                {
                    if (namedValue.Flags != ARG_OUT.Value)
                    {
                        // out parameters are not received
                        try
                        {
                            namedValue.Value.ReadValue(inputStream, namedValue.Value.Type);
                        }
                        catch (Exception e1)
                        {
                            throw new Marshal("Couldn't unmarshal object of type " + namedValue.Value.Type + " in ServerRequest.");
                        }
                    }
                }
                try
                {
                    inputStream.Reset();
                }
                catch (Exception e1)
                {
                    throw new Unknown("Could not reset input stream");
                }

                /*if (info != null)
                {
                    //invoke interceptors
                    Dynamic.Parameter[] parameters = new Dynamic.Parameter[argList.Count];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        try
                        {
                            CORBA.NamedValue value = argList.Item(i);

                            CORBA.ParameterMode mode = CORBA.ParameterMode.PARAM_IN;
                            if (value.Flags == CORBA.ARG_IN.Value)
                            {
                                mode = CORBA.ParameterMode.PARAM_IN;
                            }
                            else if (value.Flags == CORBA.ARG_OUT.Value)
                            {
                                mode = CORBA.ParameterMode.PARAM_OUT;
                            }
                            else if (value.Flags == CORBA.ARG_IN_OUT.Value)
                            {
                                mode = CORBA.ParameterMode.PARAM_INOUT;
                            }

                        parameters[i] = new Dynamic.Parameter(value.Value, mode);
                        }
                        catch (Exception e)
                        {
                            logger.Info("DSI arguments: Caught exception ", e);
                        }
                    }
                }*/
            }
        }

        /// <summary>
        /// This operation sets the request be stream-based, 
        /// ie.all attempts to extract data using DII-based operations will throw exceptions
        /// </summary>
        /// <returns>The input stream</returns>
        public CDRInputStream GetInputStream()
        {
            IsStreamBased = true;
            return inputStream;
        }

        public void Reply()
        {
            if (ResponseExpected)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("ServerRequest: reply to " + Operation);
                }

                try
                {
                    if (outputStream == null)
                    {
                        outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
                    }

                    /*
                     * DSI-based servers set results and user exceptions
                     * using anys, so we have to treat this differently
                     */
                    if (!IsStreamBased)
                    {
                        if (replyStatus == ReplyStatusType12.USER_EXCEPTION)
                        {
                            exception.WriteValue(outputStream);
                        }
                        else if (replyStatus == ReplyStatusType12.NO_EXCEPTION)
                        {
                            if (result != null)
                            {
                                result.WriteValue(outputStream);
                            }

                            if (argList != null)
                            {
                                foreach (var namedValue in argList)
                                {
                                    if (namedValue.Flags != ARG_IN.Value)
                                    {
                                        // in parameters are not returned
                                        try
                                        {
                                            namedValue.Value.WriteValue(outputStream);
                                        }
                                        catch (Exception e1)
                                        {
                                            throw new Marshal("Couldn't return (in)out arg of type " + namedValue.Value.Type + " in ServerRequest.");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    /*
                     * these two exceptions are set in the same way for
                     * both stream-based and DSI-based servers
                     */
                    if (replyStatus == ReplyStatusType12.LOCATION_FORWARD)
                    {
                        outputStream.WriteObject((CORBA.Object)locationForward.ForwardReference);
                    }
                    else if (replyStatus == ReplyStatusType12.SYSTEM_EXCEPTION)
                    {
                        SystemExceptionHelper.Write(outputStream, sysEx);
                    }

                    /*
                     * everything is written to out by now, be it results
                     * or exceptions.
                     */

                    var task = channelHandler.SendReplyAsync(outputStream);

                    task.Wait();
                }
                catch (AggregateException ae)
                {
                    logger.Info("Error replying to request!", ae.InnerException);
                }
                catch (Exception ioe)
                {
                    logger.Info("Error replying to request!", ioe);
                }
                finally
                {
                    outputStream.Close();
                    // nullify output stream to force its recreation on next call
                    outputStream = null;
                }
            }
        }

        public IOutputStream CreateReply()
        {
            IsStreamBased = true;
            if (outputStream != null)
            {
                // The reply was already created.  This happens in oneway
                // operations using SyncScope SYNC_WITH_SERVER, and does not do any harm.
                return outputStream;
            }
            replyStatus = ReplyStatusType12.NO_EXCEPTION;
            outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
            //outputStream.WriteShort(31);            
            return outputStream;
        }

        public IOutputStream CreateExceptionReply()
        {
            IsStreamBased = true;
            replyStatus = ReplyStatusType12.USER_EXCEPTION;
            outputStream = new ReplyOutputStream(orb, inputStream.RequestId, replyStatus, inputStream.ServiceContextList, inputStream.GiopMinor);
            return outputStream;
        }

        /// <summary>
        /// Creates a ServiceContext for transmitting an exception detail message
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ServiceContext CreateExceptionDetailMessage(string message)
        {
            CDROutputStream outputStream = new CDROutputStream();

            try
            {
                outputStream.BeginEncapsulatedArray();
                outputStream.WriteWString(message);
                return new ServiceContext(ExceptionDetailMessage.Value, outputStream.GetBufferCopy());
            }
            finally
            {
                outputStream.Close();
            }
        }
    }
}
