// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.InterfaceRepository
{
    public abstract class IRObject : IIRObjectOperations
    {
        protected ORB orb;
        protected IObject myRef;
        public IObject Reference
        {
            get
            {
                if (myRef == null)
                {
                    throw new IntfRepos("Reference undefined!");
                }
                return myRef;
            }
            set
            {
                myRef = value;
                orb = myRef._GetOrb();
            }
        }        
        protected DefinitionKind defKind;
        public DefinitionKind DefKind { get { return defKind; } }

        protected string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public abstract void Destroy();

        /// <summary>
        /// Second phase of loading IRObjects, define any unresolved links
        /// </summary>
        protected abstract void Define();


    }
}
