// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace CORBA
{
    public class Environment
    {
        private object syncLock = new object();
        private Exception? exception;
        public Exception Exception {
            get
            {
                lock (syncLock)
                {
                    return exception;
                }
            }
            set
            {
                lock (syncLock)
                {
                    exception = value;
                }
            }                
        }        
        public void Clear()
        {
            lock(syncLock)
            {
                exception = null;
            }
        }
    }
}
