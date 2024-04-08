// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace CORBA
{
    public class NVList: IEnumerable<NamedValue>
    {
        private List<NamedValue> list;
        private CORBA.ORB orb;
        public int Count => list.Count;

        public NVList(CORBA.ORB orb)
        {
            this.orb = orb;
            list = new List<NamedValue>();
            list.GetEnumerator();
        }

        public NVList(CORBA.ORB orb, int count)
        {
            this.orb = orb;
            list = new List<NamedValue>(count);
        }

        public NamedValue Add(uint flags)
        {
            lock(list)
            {
                CORBA.NamedValue namedValue = orb.CreateNamedValue("", null, flags);
                list.Add(namedValue);
                return namedValue;
            }            
        }

        public NamedValue AddItem(string name, uint flags)
        {
            lock (list)
            {
                CORBA.NamedValue namedValue = orb.CreateNamedValue(name, null, flags);
                list.Add(namedValue);
                return namedValue;
            }
        }

        public NamedValue AddValue(string name, CORBA.Any value, uint flags)
        {
            lock (list)
            {
                CORBA.NamedValue namedValue = orb.CreateNamedValue(name, value, flags);
                list.Add(namedValue);
                return namedValue;
            }
        }

        public NamedValue Item(int index)
        {
            lock (list)
            {
                try
                {
                    return list[index];
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new CORBA.Bounds(e.ToString());
                }
            }
        }

        public void Remove(int index)
        {
            lock (list)
            {
                try
                {
                    list.RemoveAt(index);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    throw new CORBA.Bounds(e.ToString());
                }
            }
        }

        public IEnumerator<NamedValue> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}
