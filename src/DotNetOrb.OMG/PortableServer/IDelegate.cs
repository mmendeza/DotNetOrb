// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;

namespace PortableServer
{
    public interface IDelegate
    {
        ORB ORB(Servant self);

        CORBA.Object ThisObject(Servant self);

        IPOA POA(Servant self);

        byte[] ObjectId(Servant self);

        IPOA DefaultPOA(Servant self);

        bool IsA(Servant self, string repositoryId);

        bool NonExistent(Servant self);

        CORBA.Object GetComponent(Servant self);

        IInterfaceDef GetInterface(Servant self);        

        CORBA.Object GetInterfaceDef(Servant self);
        
        string RepositoryId(Servant self);        
    }
}
