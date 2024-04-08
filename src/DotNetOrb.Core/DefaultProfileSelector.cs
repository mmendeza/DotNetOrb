// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.GIOP;
using ETF;
using System.Collections.Generic;

namespace DotNetOrb.Core
{
    public class DefaultProfileSelector : IProfileSelector
    {

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

        /// <param name="profiles"></param>
        /// <param name="lastProfile"></param>
        /// <returns>The next profile on the list that is next to lastProfile.
        /// If lastProfile is null, or points to the last profile in the list,
        /// the first profile will be returned.</returns>        
        public IProfile SelectNextProfile(List<IProfile> profileList, IProfile lastProfile)
        {
            IProfile currentProfile = null;
            //sanity check
            if (profileList == null || profileList.Count == 0)
            {
                return null;
            }
            // locate the last profile in the list            
            IEnumerator<IProfile> enumerator;
            for (enumerator = profileList.GetEnumerator(); enumerator.MoveNext();)
            {
                currentProfile = enumerator.Current;
                if (lastProfile == null)
                {
                    return currentProfile;
                }

                if (lastProfile.Equals(currentProfile))
                {
                    break;
                }
            }
            if (!enumerator.MoveNext())
            {
                enumerator.Reset();
                enumerator.MoveNext();
            }
            currentProfile = enumerator.Current;
            // ensure the next profile is not the same as lastProfile
            if (lastProfile.Equals(currentProfile))
            {
                currentProfile = null;
            }
            return currentProfile;
        }

    }
}
