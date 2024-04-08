// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CosNaming;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetOrb.NameService
{
    public class BindingIterator : BindingIteratorPOA
    {
        private Binding[] bindings;
        private int pos = 0;

        public BindingIterator(Binding[] b)
        {
            bindings = b;
            if (b.Length > 0)
            {
                pos = 0;
            }
        }

        public override void Destroy()
        {
            bindings = null;
            try
            {
                _POA().DeactivateObject(_POA().ServantToId(this));
            }
            catch (CORBA.UserException e)
            {
                throw new CORBA.Internal("Caught exception destroying Iterator" + e);
            }
        }

        public override bool NextN(uint howMany, out Binding[] bl)
        {
            int diff = bindings.Length - pos;
            if (diff > 0)
            {                
                if (howMany <= diff)
                {
                    bl = new Binding[howMany];
                    Array.Copy(bindings, pos, bl, 0, howMany);
                    pos = (int)(pos + howMany);
                }
                else
                {
                    bl = new Binding[diff];
                    Array.Copy(bindings, pos, bl, 0, diff);
                    pos = bindings.Length;
                }                
                return true;
            }
            else
            {
                bl = new Binding[0];
                return false;
            }
        }

        public override bool NextOne(out Binding b)
        {
            if (pos < bindings.Length)
            {
                b = bindings[pos++];
                return true;
            }
            else
            {
                b = new Binding(new NameComponent[0], BindingType.Nobject);
                return false;
            }
        }
    }
}
