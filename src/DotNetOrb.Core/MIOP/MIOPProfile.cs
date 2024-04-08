// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System.Net;
using ETF;
using IOP;
using Version = GIOP.Version;
using MIOP;
using PortableGroup;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.ETF;
using System;

namespace DotNetOrb.Core.MIOP
{
    public class MIOPProfile : ProfileBase
    {
        private IPAddress ipAddress = null;
        private byte[] data = null;
        private UipmcProfileBody uipmc = null;
        private TagGroupTaggedComponent tagGroup = null;
        private IIOPProfile groupIIOPProfile = null;

        public MIOPProfile(byte[] data)
        {
            this.data = data;
            CDRInputStream inputStream = new CDRInputStream(data);
            inputStream.OpenEncapsulatedArray();
            uipmc = UipmcProfileBodyHelper.Read(inputStream);
            inputStream.Close();
            version = uipmc.MiopVersion;
        }

        public MIOPProfile(string address, short port, string domainId, ulong groupId, Version groupVersion, uint groupRefVersion, IIOPProfile iiop)
        {

            groupIIOPProfile = iiop;
            tagGroup = new TagGroupTaggedComponent(groupVersion, domainId, groupId, groupRefVersion);

            TaggedComponentList list = new TaggedComponentList();
            list.AddComponent(TAG_GROUP.Value, tagGroup, typeof(TagGroupTaggedComponentHelper));

            if (iiop != null)
            {
                list.AddAll(iiop.GetComponents());
            }

            uipmc = new UipmcProfileBody(new Version(1, 0), address, port, list.AsArray());

            try
            {
                IPHostEntry hostEntry;

                hostEntry = Dns.GetHostEntry(uipmc.TheAddress);

                if (hostEntry.AddressList.Length > 0)
                {
                    ipAddress = hostEntry.AddressList[0];
                }
            }
            catch (Exception uke)
            {
                throw new RuntimeException("Unable to create profile to unknown group address: " + uipmc.TheAddress, uke);
            }
        }

        public override object Clone()
        {
            return new MIOPProfile(data);
        }

        /// <summary>
        /// Constructs an MIOPProfile from a corbaloc URL.
        /// </summary>
        public MIOPProfile(string corbaloc)
        {
            corbalocStr = corbaloc;
        }


        public new void Configure(IConfiguration config)
        {
            base.Configure(config);

            logger = configuration.GetLogger(GetType().FullName);

            ORB orb = config.ORB;

            if (corbalocStr != null)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("MIOPProfile parsing corbaloc: " + corbalocStr);
                }

                // I create the profile from the corbaloc
                if (!corbalocStr.StartsWith("miop:"))
                {
                    throw new ArgumentException("URL must start with \'miop:\'");
                }

                int sub = corbalocStr.IndexOf(";");
                if (sub == -1)
                {
                    ParseMIOPCorbaloc(corbalocStr.Substring(0));
                }
                else
                {
                    ParseMIOPCorbaloc(corbalocStr.Substring(0, sub));
                }

                components = new TaggedComponentList();
                CDROutputStream outputStream = new CDROutputStream();
                outputStream.BeginEncapsulatedArray();
                TagGroupTaggedComponentHelper.Write(outputStream, tagGroup);
                components.AddComponent(TAG_GROUP.Value, outputStream.GetBufferCopy());
                outputStream.Close();

                if (sub != -1)
                {
                    groupIIOPProfile = (IIOPProfile)new ParsedIOR(orb, "corbaloc:" + corbalocStr.Substring(sub + 1)).GetEffectiveProfile();
                    objectKey = groupIIOPProfile.GetObjectKey();
                    TaggedProfile taggedProfile = groupIIOPProfile.AsTaggedProfile();
                    components.AddComponent(taggedProfile.Tag, taggedProfile.ProfileData);
                }
                uipmc.Components = components.AsArray();
            }

            if (tagGroup == null)
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("MIOPProfile inspecting uipmc components.");
                }

                components = new TaggedComponentList();
                for (int i = 0; i < uipmc.Components.Length; i++)
                {
                    TaggedComponent component = uipmc.Components[i];
                    components.AddComponent(component);
                    switch (component.Tag)
                    {
                        case TAG_GROUP.Value:
                            {
                                if (logger.IsDebugEnabled)
                                {
                                    logger.Debug("MIOPProfile inspecting tagGroup.");
                                }

                                CDRInputStream in2 = new CDRInputStream(orb, component.ComponentData);
                                in2.OpenEncapsulatedArray();
                                tagGroup = TagGroupTaggedComponentHelper.Read(in2);
                                in2.Close();
                                break;
                            }
                        case TAG_INTERNET_IOP.Value:
                            {
                                if (logger.IsDebugEnabled)
                                {
                                    logger.Debug("MIOPProfile inspecting group iiop profile.");
                                }
                                IOR ior = new IOR("IDL:omg.org/CORBA/Object:1.0", new TaggedProfile[] { new TaggedProfile(component.Tag, component.ComponentData) });

                                groupIIOPProfile = (IIOPProfile)new ParsedIOR(orb, ior).GetEffectiveProfile();
                                break;
                            }
                    }
                }
            }

            try
            {
                IPHostEntry hostEntry;

                hostEntry = Dns.GetHostEntry(uipmc.TheAddress);

                if (hostEntry.AddressList.Length > 0)
                {
                    ipAddress = hostEntry.AddressList[0];
                }
            }
            catch (Exception uke)
            {
                throw new RuntimeException("Unable to create profile to unknow group address: " + uipmc.TheAddress, uke);
            }
        }

        public override uint Hash()
        {
            return (uint)GetHashCode();
        }

        public override string ToString()
        {
            return uipmc.TheAddress + ":" + uipmc.ThePort;
        }

        public override bool IsMatch(IProfile prof)
        {
            return Equals(prof);
        }

        public override void Marshal(ref TaggedProfile taggedProfile, ref TaggedComponent[] taggedComponentSeq)
        {
            if (data == null)
            {
                CDROutputStream outputStream = new CDROutputStream();
                outputStream.BeginEncapsulatedArray();
                UipmcProfileBodyHelper.Write(outputStream, uipmc);
                data = outputStream.GetBufferCopy();
                outputStream.Close();
            }
            taggedProfile = new TaggedProfile(TAG_UIPMC.Value, data);
        }

        /// <summary>
        /// Does nothing because it is a group profile. Objectkey equals serialized tagGroup
        /// </summary>
        public override void SetObjectKey(byte[] key)
        {

        }

        public override int Tag
        {
            get
            {
                return (int)TAG_UIPMC.Value;
            }
        }

        public override Version Version { get => uipmc.MiopVersion; }

        public IPAddress GetGroupInetAddress()
        {
            return ipAddress;
        }

        public UipmcProfileBody GetUIPMCProfile()
        {
            return uipmc;
        }
        public TagGroupTaggedComponent GetTagGroup()
        {
            return tagGroup;
        }
        public IIOPProfile GetGroupIIOPProfile()
        {
            return groupIIOPProfile;
        }

        private void ParseMIOPCorbaloc(string corbaloc)
        {
            // removes the "miop:" header
            corbaloc = corbaloc.Substring(corbaloc.IndexOf(':') + 1);

            // parse version (optional)
            int sep = corbaloc.IndexOf('@');
            Version version = ParseVersion(sep == -1 ? "" : corbaloc.Substring(0, sep));
            corbaloc = corbaloc.Substring(sep + 1);

            // parse tag group
            sep = corbaloc.IndexOf('/');
            ParseTagGroup(corbaloc.Substring(0, sep));
            corbaloc = corbaloc.Substring(sep + 1);

            // parse group transport address
            ParseGroupAddress(corbaloc);

            uipmc.MiopVersion = version;
        }

        private Version ParseVersion(string verStr)
        {
            int major = 1;
            int minor = 0;
            int sep = verStr.IndexOf('.');
            if (sep != -1)
            {
                try
                {
                    major = int.Parse(verStr.Substring(0, sep));
                    minor = int.Parse(verStr.Substring(sep + 1));
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Invalid version :" + verStr, e);
                }
            }
            return new Version((byte)major, (byte)minor);
        }

        /// <summary>
        /// Parses a Tag Group from a String. Example: "1.0-MyDomain-1"
        /// </summary>
        /// <param name="s"></param>
        /// <exception cref="IllegalArgumentException"></exception>
        public void ParseTagGroup(string s)
        {
            tagGroup = new TagGroupTaggedComponent();
            if (s.IndexOf('-') != -1)
            {
                try
                {
                    var st = s.Split('-');
                    tagGroup.GroupVersion = ParseVersion(st[0]);
                    tagGroup.GroupDomainId = st[1];
                    tagGroup.ObjectGroupId = uint.Parse(st[2]);
                    if (st.Length > 3)
                    {
                        tagGroup.ObjectGroupRefVersion = uint.Parse(st[3]);
                    }
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Illegal group information format: " + s, e);
                }
            }
            else
            {
                throw new ArgumentException("Illegal group information format: " + s);
            }
        }

        /// <summary>
        /// Parse the group address. Example: "225.6.7.8:12345"
        /// </summary>
        private void ParseGroupAddress(string s)
        {
            uipmc = new UipmcProfileBody();

            int sep = s.IndexOf(':');
            if (sep != -1)
            {
                try
                {
                    uipmc.ThePort = short.Parse(s.Substring(sep + 1));
                    uipmc.TheAddress = s.Substring(0, sep);
                }
                catch (Exception e)
                {
                    throw new ArgumentException("Illegal port number in MIOP object address format: " + s, e);
                }
            }
        }

        public override void ReadAddressProfile(CDRInputStream stream)
        {
            throw new NotImplementedException();
        }

        public override void WriteAddressProfile(CDROutputStream stream)
        {
            throw new NotImplementedException();
        }
    }

}
