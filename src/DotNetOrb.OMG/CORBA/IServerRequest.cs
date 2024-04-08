// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace CORBA
{
    public interface IServerRequest
    {
        string Operation { get; }

        NVList Arguments { get; }

        void GetArguments(NVList arguments);

        Any Result { get; set; }

        Any Exception { get; set; }

    }
}
