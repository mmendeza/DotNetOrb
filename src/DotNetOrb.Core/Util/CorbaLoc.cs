// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.MIOP;
using ETF;
using MIOP;
using PortableGroup;
using System;
using System.Net;
using System.Text;
using Version = GIOP.Version;

namespace DotNetOrb.Core.Util
{
    public class CorbaLoc
    {
        private ORB orb;
        private string keyString;
        public string KeyString { get { return keyString; } }

        private byte[] key;
        public byte[] Key { get { return key; } }

        private string bodyString;

        private bool isRir;
        public bool IsRir { get { return isRir; } }

        public IProfile[] profileList;

        public CorbaLoc(ORB orb, string addr)
        {
            this.orb = orb;
            isRir = false;
            Parse(addr);
        }

        public override string ToString()
        {
            return "corbaloc:" + Body();
        }

        private string Body()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append(bodyString);

            if (keyString != null)
            {
                buffer.Append('/');
                buffer.Append(keyString);
            }

            return buffer.ToString();
        }

        private void DefaultKeyString(string defaultKey)
        {
            if (keyString == null)
            {
                keyString = defaultKey;
            }
            else
            {
                throw new InvalidOperationException("KeyString not empty, cannot default to " + defaultKey);
            }
        }

        public string ToCorbaName(string strName)
        {
            if (keyString == null)
            {
                DefaultKeyString("NameService");
            }

            if (strName != null && strName.Length > 0)
            {
                try
                {
                    return "corbaname:" + Body() + "#" + strName;
                }
                catch (Exception e)
                {
                    return null;
                }
            }

            return "corbaname:" + Body();
        }

        private void Parse(string addr)
        {
            if (addr == null || !addr.StartsWith("corbaloc:"))
            {
                throw new ArgumentException("URL must start with \'corbaloc:\'");
            }

            string sb;
            bool isMIOP = addr.IndexOf("miop") != -1;
            if (isMIOP && addr.IndexOf(",iiop") != -1)
            {
                throw new ArgumentException("MIOP Profile does not support Gateway Profiles.");
            }

            if (!isMIOP && addr.IndexOf('/') == -1)
            {
                sb = addr.Substring(addr.IndexOf(':') + 1);
                if (addr.StartsWith("corbaloc:rir:"))
                {
                    isRir = true;
                    // default key string for rir protocol
                    keyString = "NameService";
                }
                else
                {
                    keyString = null;
                }
                key = new byte[0];
            }
            else
            {
                string iiopKey;
                int startIndex;
                int endIndex;
                // MIOP plus another e.g. IIOP profile. Scan IIOP profile for key.
                if (isMIOP && addr.IndexOf(';') != -1)
                {
                    iiopKey = addr.Substring(addr.IndexOf(';'));
                    startIndex = 1;
                    endIndex = iiopKey.IndexOf('/');
                }
                else
                {
                    iiopKey = addr;
                    startIndex = iiopKey.IndexOf(':') + 1;
                    endIndex = isMIOP ? iiopKey.Length : iiopKey.IndexOf('/');
                }

                sb = iiopKey.Substring(startIndex, endIndex - startIndex);
                keyString = iiopKey.Substring(iiopKey.IndexOf('/') + 1);
                key = ParseKey(keyString);
            }

            // ! MIOP as we don't currently support gateway profiles.
            if (!isMIOP && sb.IndexOf(',') > 0)
            {
                string[] tokens = sb.Split(',');
                profileList = new IProfile[tokens.Length];
                int pIndex = 0;
                for (int i = 0; i < profileList.Length; i++)
                {
                    IProfile p = ParseAddress(tokens[i]);
                    if (p == null)
                    {
                        continue;
                    }
                    profileList[pIndex] = p;
                    pIndex++;
                }
                while (pIndex < profileList.Length)
                {
                    profileList[pIndex] = null;
                    pIndex++;
                }

            }
            else
            {
                profileList = new IProfile[] { ParseAddress(sb) };
            }

            bodyString = sb;
        }

        private IProfile ParseAddress(string addr)
        {
            int colon = addr.IndexOf(':');
            if (colon == -1)
            {
                throw new ArgumentException("Illegal object address format: " + addr);
            }

            if ("rir:".Equals(addr))
            {
                isRir = true;
                /* resolve initials references protocol */
                return null;
            }

            IProfile result = null;
            if (orb == null && (colon == 0 || addr.StartsWith("iiop:") || addr.StartsWith("ssliop:")))
            {
                result = new IIOPProfile(addr);
            }
            else if (orb != null)
            {
                var factories = orb.TransportManager.GetFactoriesList();
                foreach (var f in factories)
                {
                    result = f.DecodeCorbaloc(addr);
                    if (result != null) break;
                }
            }
            if (result == null)
            {
                throw new ArgumentException("Unknown protocol in object address format: " + addr);
            }
            return result;
        }

        private static bool IsLegalChar(char c)
        {
            if (c >= '0' && c <= '9' || c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z')
            {
                return true;
            }
            return c == ';' || c == '/' || c == ':' || c == '?' || c == '@' || c == '&' || c == '=' || c == '+'
                    || c == '$' || c == ',' || c == '_' || c == '.' || c == '!' || c == '~' || c == '*' || c == '\''
                    || c == '-' || c == '(' || c == ')';
        }

        private static byte HexValue(char c)
        {
            return (byte)(c >= 'a' ? 10 + c - 'a' : c >= 'A' ? 10 + c - 'A' : c - '0');
        }

        private static char HexDigit(byte b)
        {
            if ((b & 0xf0) != 0)
            {
                throw new ArgumentException("Hex digit out of range " + b);
            }

            return (char)(b < 10 ? '0' + (char)b : 'A' + (char)b - 10);
        }

        private static bool IsHex(char c)
        {
            return c >= '0' && c <= '9' || c >= 'a' && c <= 'f' || c >= 'A' && c <= 'F';
        }

        public static byte[] ParseKey(string s)
        {
            char[] tmp = s.ToCharArray();
            int count = tmp.Length;

            for (int i = 0; i < tmp.Length; i++)
            {
                if (!IsLegalChar(tmp[i]))
                {
                    if (tmp[i] == '%')
                    {
                        if (IsHex(tmp[i + 1]) && IsHex(tmp[i + 2]))
                        {
                            count -= 2;
                            i += 2;
                        }
                        else
                        {
                            throw new ArgumentException("Illegal escape in URL character");
                        }
                    }
                    else
                    {
                        throw new ArgumentException("URL character out of range: " + tmp[i]);
                    }
                }
            }

            byte[] result = new byte[count];
            int idx = 0;

            for (int i = 0; i < count; i++)
            {
                if (IsLegalChar(tmp[idx]))
                {
                    result[i] = (byte)tmp[idx++];
                }
                else
                {
                    result[i] = (byte)(HexValue(tmp[idx + 1]) << 4 | HexValue(tmp[idx + 2]));
                    idx += 3;
                }
            }
            return result;
        }

        public static string ParseKey(byte[] key)
        {
            var buffer = new StringBuilder();

            for (int i = 0; i < key.Length; i++)
            {
                if (!IsLegalChar((char)key[i]))
                {
                    buffer.Append('%');
                    // Mask the bytes before shift to ensure 10001001 doesn't get
                    // shifted to 11111000 but 00001000 (linden java faq).
                    buffer.Append(HexDigit((byte)((key[i] & 0xff) >> 4)));
                    buffer.Append(HexDigit((byte)(key[i] & 0x0f)));
                }
                else
                {
                    buffer.Append((char)key[i]);
                }
            }
            return buffer.ToString();
        }

        public static string GenerateCorbaloc(CORBA.ORB orb, CORBA.Object objRef)
        {
            ParsedIOR pior = new ParsedIOR((ORB)orb, orb.ObjectToString(objRef));

            IProfile profile = pior.GetEffectiveProfile();

            if (profile is IIOPProfile)
            {
                return CreateCorbalocForIIOPProfile((IIOPProfile)profile);
            }
            else if (profile is MIOPProfile)
            {
                return CreateCorbalocForMIOPProfile((MIOPProfile)profile);
            }
            else
            {
                throw new ArgumentException("Profile type not suported: tag number=" + profile.Tag);
            }
        }

        private static string CreateCorbalocForMIOPProfile(MIOPProfile profile)
        {
            var sb = new StringBuilder("miop:");

            sb.Append(CreateString(profile.Version));
            sb.Append("@");
            sb.Append(CreateString(profile.GetTagGroup()));
            sb.Append("/");
            sb.Append(CreateString(profile.GetUIPMCProfile()));

            // group's IIOP component
            sb.Append(";");
            sb.Append(CreateCorbalocForIIOPProfile(profile.GetGroupIIOPProfile()));

            return sb.ToString();
        }

        private static string CreateString(TagGroupTaggedComponent groupInfo)
        {
            var sb = new StringBuilder();
            sb.Append(CreateString(groupInfo.GroupVersion));
            sb.Append("-");
            sb.Append(groupInfo.GroupDomainId);
            sb.Append("-");
            sb.Append(groupInfo.ObjectGroupId);
            if (groupInfo.ObjectGroupRefVersion != 0)
            {
                sb.Append("-");
                sb.Append(groupInfo.ObjectGroupRefVersion);
            }
            return sb.ToString();
        }

        private static string CreateString(UipmcProfileBody uipmc)
        {
            var sb = new StringBuilder();
            sb.Append(uipmc.TheAddress);
            sb.Append(":");
            sb.Append(uipmc.ThePort);
            return sb.ToString();
        }

        private static string CreateString(Version version)
        {
            var sb = new StringBuilder();
            sb.Append(version.Major);
            sb.Append(".");
            sb.Append(version.Minor);
            return sb.ToString();
        }

        public static string GenerateCorbalocForMultiIIOPProfiles(CORBA.ORB orb, CORBA.IObject objRef)
        {
            return GenerateCorbalocForMultiIIOPProfiles(orb, orb.ObjectToString(objRef));
        }

        public static string GenerateCorbalocForMultiIIOPProfiles(CORBA.ORB orb, string objRef)
        {
            ParsedIOR pior = new ParsedIOR((ORB)orb, objRef);

            string result = null;
            string objectKey = null;
            foreach (var profile in pior.Profiles)
            {
                if (profile is IIOPProfile)
                {
                    string s = CreateCorbalocForIIOPProfile((IIOPProfile)profile, true);
                    if (result != null)
                    {
                        result += "," + s;
                    }
                    else
                    {
                        result = "corbaloc:" + s;
                        objectKey = ParseKey(profile.GetObjectKey());
                    }

                }
            }
            if (result != null)
            {
                result += "/" + objectKey;
            }
            return result;
        }

        public static string CreateCorbalocForIIOPProfile(IIOPProfile profile)
        {
            return string.Concat(CreateCorbalocForIIOPProfile(profile, false), '/' + ParseKey(profile.GetObjectKey()));
        }

        /// <summary>
        /// Returns a iiop list ; note this function does not add the object_key.
        /// </summary>
        private static string CreateCorbalocForIIOPProfile(IIOPProfile profile, bool addAlternates)
        {
            var sb = new StringBuilder("iiop:");
            sb.Append(CreateString(profile.Version));
            sb.Append("@");
            sb.Append(WrapIPv6(((IIOPAddress)profile.GetAddress()).Address));
            sb.Append(":");
            sb.Append(((IIOPAddress)profile.GetAddress()).Port);
            if (addAlternates)
            {
                foreach (var address in profile.GetAlternateAddresses())
                {
                    sb.Append(",iiop:");
                    sb.Append(CreateString(profile.Version));
                    sb.Append("@");
                    sb.Append(WrapIPv6(address.Address));
                    sb.Append(":");
                    sb.Append(address.Port);
                }
            }
            return sb.ToString();
        }

        private static string WrapIPv6(IPAddress address)
        {
            string result = address.ToString();

            try
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                {
                    result = '[' + result + ']';
                }
            }
            catch (Exception e)
            {
            }
            return result;
        }
    }
}
