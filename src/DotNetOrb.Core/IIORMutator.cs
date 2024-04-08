// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.GIOP;
using IOP;

namespace DotNetOrb.Core
{
    /// <summary>
    ///  low level plugin that allows the user to mutate incoming or outgoing objects at the CDRStream level.
    ///  If the plugin is enabled both mutators will be called by the respective CDRStreams.
    /// </summary>
    interface IIORMutator
    {
        GIOPConnection Connection { get; set; }

        /// <summary>
        /// called by CDRInputStream::readObject. This allows the user to alter the IOR according the their own wishes.
        /// </summary>
        IOR MutateIncoming(IOR ior);

        /// <summary>
        /// called by CDROutputStream::writeObject This allows the user to alter the IOR according the their own wishes.
        /// </summary>
        IOR MutateOutgoing(IOR ior);
    }
}
