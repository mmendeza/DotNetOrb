// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core.POA
{
    public class POAConstants
    {
        public enum ShutdownState
        {
            NotCalled,
            ShutdownInProgress,
            DestructionApparent,
            DestructionComplete
        }

        public enum POAState
        {
            Active,
            Holding,
            Discarding,
            Inactive,
            Destroyed
        }

        /* separator char for qualified poa names */
        public const char ObjectKeySeparator = '/';
        public const byte ObjectKeySeparatorByte = (byte)ObjectKeySeparator;

        public const byte MaskByte = (byte)'&';
        public const byte MaskMaskByte = (byte)'&';
        public const byte SepaMaskByte = (byte)'%';

        /* root POA name */
        public const string RootPOAName = "RootPOA";

    }
}
