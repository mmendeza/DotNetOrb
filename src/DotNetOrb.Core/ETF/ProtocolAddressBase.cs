// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.CDR;
using DotNetOrb.Core.Config;

namespace DotNetOrb.Core.ETF
{
    /// <summary>
    ///  Provides an abstraction of a protocol specific address.
    ///  This is necessary to allow the ORB and other components deal with
    ///  addresses generally rather than using protocol specific address elements
    ///  such as IIOP-centric host and port values.
    /// </summary>
    public abstract class ProtocolAddressBase : IConfigurable
    {
        protected IConfiguration configuration;

        public abstract void Configure(IConfiguration config);

        public abstract bool FromString(string s);

        public abstract void Write(CDROutputStream s);

        public byte[] ToCDR()
        {
            CDROutputStream outputStream = new CDROutputStream();

            try
            {
                outputStream.BeginEncapsulatedArray();
                Write(outputStream);
                return outputStream.GetBufferCopy();
            }
            finally
            {
                outputStream.Close();
            }
        }

        /// <summary>
        /// This function shall return an equivalent, copy of the profile.
        /// </summary>
        public abstract ProtocolAddressBase Copy();
    }
}
