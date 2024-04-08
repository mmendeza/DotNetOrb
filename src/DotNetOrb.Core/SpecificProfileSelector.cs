// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.GIOP;
using DotNetOrb.Core.IIOP;
using ETF;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    public class SpecificProfileSelector : IProfileSelector
    {
        private RTCORBA.Protocol[] protocols;
        private IProfile currentProfile = null;

        public SpecificProfileSelector(RTCORBA.Protocol[] protocols)
        {
            this.protocols = protocols;
        }

        /// <param name="profiles"></param>
        /// <param name="ccm"></param>
        /// <returns>The first profile in the list if present</returns>
        public IProfile SelectProfile(List<IProfile> profiles, ConnectionManager ccm)
        {
            if (profiles == null || profiles.Count == 0)
            {
                return null;
            }

            return SelectNextProfile(profiles, null);
        }

        private bool Validate(IProfile profile)
        {
            var profileTag = profile.Tag;

            for (int i = 0; i < protocols.Length; i++)
            {
                var tagToMatch = protocols[i].ProtocolType;

                if (profileTag == tagToMatch)
                {
                    return true;
                }

                if (profile is IIOPProfile)
                {
                    // Special case check for IIOP profile supporting SSL
                    IIOPProfile iiopProfile = (IIOPProfile)profile;
                    if (tagToMatch == ORBConstants.DOTNETORB_SSL_PROFILE_ID && iiopProfile.GetSSL() != null)
                    {
                        return true;
                    }

                    // Special case check for IIOP profile not supporting SSL
                    if (tagToMatch == ORBConstants.DOTNETORB_NOSSL_PROFILE_ID && (iiopProfile.GetSSL() == null ||
                         // SSL port contains a valid value but further check is required
                         // see if protection is enabled.
                         (iiopProfile.GetSSL().TargetRequires & Security.NoProtection.Value) != 0))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <param name="profiles"></param>
        /// <param name="lastProfile"></param>
        /// <returns>The next profile on the list that is next to lastProfile.
        /// If lastProfile is null, or points to the last profile in the list,
        /// the first profile will be returned.</returns>        
        public IProfile SelectNextProfile(List<IProfile> profileList, IProfile lastProfile)
        {
            //sanity check
            if (profileList == null || profileList.Count == 0)
            {
                return null;
            }
            // locate the last profile in the list            
            IEnumerator<IProfile> enumerator;
            for (enumerator = profileList.GetEnumerator(); enumerator.MoveNext();)
            {
                var p = enumerator.Current;
                if (lastProfile != null)
                {
                    if (lastProfile.Equals(p))
                    {
                        break;
                    }
                }
                else if (Validate(p))
                {
                    currentProfile = p;
                    return p;
                }
            }

            // if we exit the loop but lastProfile is null, that means no valid profiles were found.
            if (lastProfile == null)
            {
                currentProfile = null;
                return null;
            }

            // return the next profile, which is next to the last profile.
            while (true)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator.Reset();
                    enumerator.MoveNext();
                }
                var p = enumerator.Current;

                if (lastProfile.Equals(p))
                {
                    // came all the way back around
                    break;
                }

                if (Validate(p))
                {
                    currentProfile = p;
                    return p;
                }
            }
            return null;
        }

    }
}
