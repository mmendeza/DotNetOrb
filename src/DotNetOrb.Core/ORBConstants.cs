// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DotNetOrb.Core
{
    public static class ORBConstants
    {
        /**
         * <code>VMCID</code> is the Vendor Minor Codeset ID. According to
         * ftp://ftp.omg.org/pub/docs/ptc/01-08-35.txt the VMCID is equivalent
         * to the VSCID
         */
        public static int VMCID = 0x6A430000;
        public static int DOTNETORB_ORB_ID = 0x6A414300;
        public static int SERVICE_PADDING_CONTEXT = 0x6A414301;
        public static int SERVICE_PROXY_CONTEXT = 0x6A414302;

        /**
         * <code>JAC_SSL_PROFILE_ID/JAC_NOSSL_PROFILE_ID</code> are used to
         * distinguish between IIOP profiles that do and do not support SSL.
         */
        public static int DOTNETORB_SSL_PROFILE_ID = VMCID;
        public static int DOTNETORB_NOSSL_PROFILE_ID = VMCID | 0x01;

        // Added to support iterop with another non-jacorb
        public static int TAO_ORB_ID = 0x54414F00;
    }
}
