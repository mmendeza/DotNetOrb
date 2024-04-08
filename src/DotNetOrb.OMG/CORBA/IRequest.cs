// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IRequest
    {
        CORBA.IObject Target { get; }
        string Operation { get; }
        NVList Arguments { get; }
        NamedValue Result { get; }
        CORBA.Environment Env { get; }
        ExceptionList Exceptions { get; }
        ContextList Contexts { get; }
        Any ReturnValue { get; }
        IContext Ctx { get; set; }
        Any AddInArg();
        Any AddNamedInArg(string name);
        Any AddInOutArg();
        Any AddNamedInOutArg(string name);
        Any AddOutArg();
        Any AddNamedOutArg(string name);
        void SetReturnType(TypeCode tc);
        void Invoke();
        void SendOneWay();
        void SendDeferred();
        [ThrowsIdlException(typeof(WrongTransaction))]
        void GetResponse();
        bool PollResponse();        
    }
}
