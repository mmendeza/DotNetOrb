// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.GIOP
{
    public abstract class GIOPFragmentedMessage : GIOPMessage
    {
        public uint RequestId { get; set; }

        public GIOPFragmentedMessage(ORB orb) : base(orb)
        {
        }

        public bool AddFragment(GIOPFragmentMessage fragment)
        {
            content.AddComponent(fragment.Content);
            content.SetWriterIndex(content.WriterIndex + fragment.Content.ReadableBytes);
            //if (!fragment.Header.HasMoreFragments)
            //{
            //    content.Consolidate();
            //    return true;
            //}
            return false;
        }

    }
}
