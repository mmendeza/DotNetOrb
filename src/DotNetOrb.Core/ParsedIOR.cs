// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CONV_FRAME;
using CORBA;
using CosNaming;
using ETF;
using GIOP;
using IOP;
using SSLIOP;
using System.Text;
using System.Reflection;
using DotNetOrb.Core.Util;
using DotNetOrb.Core.IIOP;
using DotNetOrb.Core.CDR;
using DotNetOrb.Core.ETF;
using System.Collections.Generic;
using System;
using System.IO;

namespace DotNetOrb.Core
{
    public class ParsedIOR
    {
        private IProfile effectiveProfile = null;
        private IProfile lastUsedProfile = null;
        private List<IProfile> profiles = new List<IProfile>();
        public List<IProfile> Profiles { get { return profiles; } }
        private TaggedComponentList components = new TaggedComponentList();
        private IProfileSelector profileSelector = null;
        public bool IsProfileSelectorSet
        {
            get
            {
                return profileSelector != null;
            }

        }

        protected bool endianness = false;
        private string iorStr = null;
        private IOR ior = null;
        public IOR IOR { get { return ior; } }
        private ORB orb;
        private CodeSetComponentInfo csInfo = null;
        public CodeSetComponentInfo CodeSetComponentInfo { get => csInfo; }
        private int? orbTypeId = null;
        public int? ORBTypeId { get => orbTypeId; }
        private ILogger logger;
        private string iorTypeIdName = null;

        /// <summary>
        /// This is used to store the original corbaname reference. If this is not null it 
        /// indicates the client was using the Naming Service (as oppose to the IMR) to reach the server,
        /// so the retry logic pull out the saved NS IOR for retrying.
        /// </summary>
        private string corbaNameOriginalObjRef = null;

        private ParsedIOR(ORB orb)
        {
            this.orb = orb;
            logger = this.orb.Configuration.GetLogger(GetType().Name);
        }

        public ParsedIOR(ORB orb, string objRef) : this(orb)
        {
            Parse(objRef);
        }

        public ParsedIOR(ORB orb, IOR ior) : this(orb)
        {
            Decode(ior);
        }

        public static IOR CreateObjectIOR(IProfile profile)
        {
            return CreateObjectIOR((ORB)CORBA.ORB.Init(), profile);
        }

        public static IOR CreateObjectIOR(ORB orb, IProfile profile)
        {
            string repId = "IDL:omg.org/CORBA/Object:1.0";
            TaggedComponentList components = new TaggedComponentList();
            CDROutputStream outputStream = new CDROutputStream(orb);
            try
            {
                outputStream.BeginEncapsulatedArray();
                outputStream.WriteLong(ORBConstants.DOTNETORB_ORB_ID);
                components.AddComponent(new TaggedComponent(TAG_ORB_TYPE.Value, outputStream.GetBufferCopy()));
            }
            finally
            {
                outputStream.Close();
            }
            List<TaggedProfile> taggedProfileList = new List<TaggedProfile>();
            var tp = new TaggedProfile();
            TaggedComponent[] tcs = components.AsArray();
            profile.Marshal(ref tp, ref tcs);
            taggedProfileList.Add(tp);
            // copy the profiles into the IOR            
            var tps = taggedProfileList.ToArray();
            return new IOR(repId, tps);
        }


        public static IOR CreateObjectIOR(IProfile[] profiles)
        {
            return CreateObjectIOR((ORB)CORBA.ORB.Init(), profiles);
        }

        public static IOR CreateObjectIOR(ORB orb, IProfile[] profiles)
        {
            string repId = "IDL:omg.org/CORBA/Object:1.0";
            var taggedProfileList = new List<TaggedProfile>();
            for (int count = 0; count < profiles.Length; count++)
            {
                if (profiles[count] == null)
                {
                    continue;
                }
                TaggedComponentList components = new TaggedComponentList();

                CDROutputStream outputStream = new CDROutputStream(orb);
                try
                {
                    outputStream.BeginEncapsulatedArray();
                    outputStream.WriteLong(ORBConstants.DOTNETORB_ORB_ID);
                    components.AddComponent(new TaggedComponent(TAG_ORB_TYPE.Value, outputStream.GetBufferCopy()));
                }
                finally
                {
                    outputStream.Close();
                }
                var tp = new TaggedProfile();
                var tcs = components.AsArray();
                profiles[count].Marshal(ref tp, ref tcs);
                taggedProfileList.Add(tp);
            }
            // copy the profiles into the IOR
            var tps = taggedProfileList.ToArray();
            return new IOR(repId, tps);
        }

        /// <summary>
        /// It will extract an object key from any given GIOP::TargetAddress assuming an appropriate
        /// ETF::Factories implementation is available for the profile in use.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="orb"></param>
        /// <returns></returns>
        /// <exception cref="BadParam"></exception>
        public static byte[] ExtractObjectKey(TargetAddress addr, ORB orb)
        {
            TaggedProfile tp = null;
            switch (addr._Discriminator)
            {
                case KeyAddr.Value:
                    {
                        return addr.ObjectKey;
                    }
                case ProfileAddr.Value:
                    {
                        tp = new TaggedProfile(addr.Profile.Tag, addr.Profile.ProfileData);
                        break;
                    }
                case ReferenceAddr.Value:
                    {
                        IORAddressingInfo info = addr.Ior;
                        tp = new TaggedProfile(info.Ior.Profiles[(int)info.SelectedProfileIndex].Tag,
                                               info.Ior.Profiles[(int)info.SelectedProfileIndex].ProfileData);
                        break;
                    }
                default:
                    {
                        throw new BadParam("Invalid value for TargetAddress discriminator");
                    }
            }

            var profileFactory = orb.TransportManager.GetFactory(tp.Tag);
            if (profileFactory != null)
            {
                return profileFactory.DemarshalProfile(ref tp, out TaggedComponent[] components).GetObjectKey();
            }
            return null;
        }

        /// <summary>
        /// Returns the value of the TAG_JAVA_CODEBASE component from this IOR, 
        /// or null if no such component exists.The component is first searched
        /// in the effective profile, if that is an IIOPProfile, and failing that,
        /// in the MULTIPLE_COMPONENTS list.
        /// </summary>
        /// <returns></returns>
        public string GetCodebaseComponent()
        {
            return GetStringComponent(TAG_JAVA_CODEBASE.Value);
        }

        public override bool Equals(object? other)
        {
            if (other == null)
            {
                return false;
            }

            return

                other is ParsedIOR &&
                ((ParsedIOR)other).GetIORString().Equals(GetIORString()) &&
                effectiveProfile != null &&
                effectiveProfile.IsMatch(((ParsedIOR)other).GetEffectiveProfile())
            ;
        }

        public override int GetHashCode()
        {
            return GetIORString().GetHashCode();
        }

        /// <summary>
        /// When multiple internet IOP tags are present, they will probably
        /// have different versions, we will use the highest version between 0 and 1.
        /// </summary>
        /// <param name="ior"></param>
        private void Decode(IOR ior)
        {
            for (int i = 0; i < ior.Profiles.Length; i++)
            {
                uint tag = ior.Profiles[i].Tag;

                switch (tag)
                {
                    case TAG_MULTIPLE_COMPONENTS.Value:
                        {
                            components = new TaggedComponentList(ior.Profiles[i].ProfileData);
                            break;
                        }
                    default:
                        {
                            var factory = orb.TransportManager.GetFactory(tag);
                            if (factory != null)
                            {
                                var tp = ior.Profiles[i];
                                profiles.Add(factory.DemarshalProfile(ref tp, out TaggedComponent[] components));
                            }
                            else
                            {
                                if (logger.IsDebugEnabled)
                                {
                                    logger.Debug("No transport available for profile tag " + tag);
                                }
                            }
                            break;
                        }
                }
            }
            this.ior = ior;
            SetIorTypeIdName();
            SetEffectiveProfile();
        }

        public string GetIORString()
        {
            if (iorStr == null)
            {
                try
                {
                    CDROutputStream outputStream = new CDROutputStream(orb);
                    try
                    {
                        outputStream.BeginEncapsulatedArray();
                        IORHelper.Write(outputStream, ior);

                        byte[] bytes = outputStream.GetBufferCopy();

                        StringBuilder sb = new StringBuilder("IOR:");

                        for (int j = 0; j < bytes.Length; j++)
                        {
                            ObjectUtil.AppendHex(sb, bytes[j] >> 4 & 0xF);
                            ObjectUtil.AppendHex(sb, bytes[j] & 0xF);
                        }

                        iorStr = sb.ToString();
                    }
                    finally
                    {
                        outputStream.Close();
                    }
                }
                catch (Exception e)
                {
                    logger.Error("Error in building IIOP-IOR", e);
                    throw new Unknown("Error in building IIOP-IOR");
                }
            }

            return iorStr;
        }

        public byte[] GetObjectKey()
        {
            return effectiveProfile.GetObjectKey();
        }

        public IProfile GetEffectiveProfile()
        {
            return effectiveProfile;
        }

        private void SetEffectiveProfile()
        {
            effectiveProfile = GetProfileSelector().SelectProfile(profiles, orb.ConnectionManager);
            lastUsedProfile = effectiveProfile;

            if (effectiveProfile != null)
            {
                csInfo = (CodeSetComponentInfo)GetComponent(TAG_CODE_SETS.Value, typeof(CodeSetComponentInfoHelper));
                orbTypeId = GetLongComponent(TAG_ORB_TYPE.Value);
            }
        }

        public IProfile GetNextEffectiveProfile()
        {
            effectiveProfile = GetProfileSelector().SelectNextProfile(profiles, effectiveProfile);

            if (effectiveProfile != null && effectiveProfile != GetLastUsedProfile())
            {
                csInfo = (CodeSetComponentInfo)GetComponent(TAG_CODE_SETS.Value, typeof(CodeSetComponentInfoHelper));
                orbTypeId = GetLongComponent(TAG_ORB_TYPE.Value);
            }
            return effectiveProfile;
        }

        public IProfile GetLastUsedProfile()
        {
            if (lastUsedProfile == null)
            {
                lastUsedProfile = effectiveProfile;
            }
            return lastUsedProfile;
        }

        public void MarkLastUsedProfile()
        {
            lastUsedProfile = effectiveProfile;
        }

        public void PatchSSL()
        {
            var ssl = (SSLIOP.SSL)components.GetComponent(SSLIOP.TAG_SSL_SEC_TRANS.Value, typeof(SSLHelper));
            if (ssl == null)
            {
                return;
            }
            foreach (var p in profiles)
            {
                if (p is IIOPProfile iiopProfile)
                {
                    if (iiopProfile.Version.Minor == 0)
                    {
                        logger.Debug("patching GIOP 1.0 profile to contain SSL information from the multiple components profile");
                        iiopProfile.AddComponent(SSLIOP.TAG_SSL_SEC_TRANS.Value, ssl, typeof(SSLHelper));
                    }
                }
            }
        }

        private void SetIorTypeIdName()
        {
            string iorTypeId = GetTypeId();
            int colon = iorTypeId.LastIndexOf(":");
            if (colon > 0)
            {
                // chop off the version id at the end
                iorTypeIdName = iorTypeId.Substring(0, colon);
            }
        }

        public string GetTypeIdName()
        {
            return iorTypeIdName;
        }

        public bool UseCorbaName()
        {
            return corbaNameOriginalObjRef != null;
        }

        public string? GetCorbaNameOriginalObjRef()
        {
            return corbaNameOriginalObjRef;
        }

        public void ReparseCorbaName()
        {
            if (corbaNameOriginalObjRef != null)
            {
                ParseCorbaName(corbaNameOriginalObjRef);
            }
        }

        public string GetTypeId()
        {
            return ior.TypeId;
        }

        public string GetIDString()
        {
            StringBuilder sb = new StringBuilder(GetTypeId());
            sb.Append(":");
            var bytes = GetObjectKey();

            for (int j = 0; j < bytes.Length; j++)
            {
                ObjectUtil.AppendHex(sb, bytes[j] >> 4 & 0xF);
                ObjectUtil.AppendHex(sb, bytes[j] & 0xF);
            }

            return sb.ToString();
        }

        public TaggedComponentList GetMultipleComponents()
        {
            return components;
        }

        public bool IsNull()
        {
            return IsNull(ior);
        }

        public static bool IsNull(IOR ior)
        {
            return "".Equals(ior.TypeId) && ior.Profiles.Length == 0;
        }

        private void Parse(string objRef)
        {
            if (objRef == null)
            {
                throw new ArgumentException("Null object reference");
            }

            if (objRef.StartsWith("IOR:"))
            {
                ParseStringifiedIOR(objRef);
            }
            else if (objRef.StartsWith("corbaloc:"))
            {
                ParseCorbaloc(objRef);
            }
            else if (objRef.StartsWith("corbaname:"))
            {
                ParseCorbaName(objRef);
            }
            else if (objRef.StartsWith("resource:"))
            {
                ParseResource(objRef.Substring(9));
            }
            else if (objRef.StartsWith("jndi:"))
            {
                throw new NoImplement("jndi not supported");
            }
            else
            {
                if (logger.IsDebugEnabled)
                {
                    logger.Debug("Trying to resolve URL/IOR from: " + objRef);
                }

                string content = null;
                try
                {
                    content = ObjectUtil.ReadURL(objRef);
                }
                catch (Exception e)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Error reading IOR/URL: ", e);
                    }
                    // ignore;
                }
                if (content == null)
                {
                    throw new BadParam("Invalid or unreadable URL/IOR: " + objRef);
                }
                Parse(content);
            }
            iorStr = GetIORString();
        }

        private void ParseStringifiedIOR(string objRef)
        {
            int length = objRef.Length;
            int cnt = (length - 4) / 2;

            if (length % 2 != 0)
            {
                throw new BadParam("Odd number of characters within object reference");
            }

            using (var memStream = new MemoryStream())
            {
                for (int j = 0; j < cnt; j++)
                {
                    char c1 = objRef[j * 2 + 4];
                    char c2 = objRef[j * 2 + 5];
                    int i1 =
                        c1 >= 'a'
                            ? 10 + c1 - 'a'
                            : c1 >= 'A' ? 10 + c1 - 'A' : c1 - '0';
                    int i2 =
                        c2 >= 'a'
                            ? 10 + c2 - 'a'
                            : c2 >= 'A' ? 10 + c2 - 'A' : c2 - '0';
                    memStream.WriteByte((byte)(i1 * 16 + i2));
                }
                CDRInputStream inputStream;

                if (orb == null)
                {
                    inputStream = new CDRInputStream((ORB)CORBA.ORB.Init(), memStream.GetBuffer());
                }
                else
                {
                    inputStream = new CDRInputStream(orb, memStream.GetBuffer());
                }

                endianness = inputStream.ReadBoolean();
                if (endianness)
                {
                    inputStream.IsLittleEndian = true;
                }

                try
                {
                    IOR _ior = IORHelper.Read(inputStream);
                    Decode(_ior);
                }
                catch (CORBA.Marshal e)
                {
                    if (logger.IsDebugEnabled)
                    {
                        logger.Debug("Invalid IOR", e);
                    }
                    throw new BadParam("Invalid IOR " + e, 10, CompletionStatus.No);
                }
            }
        }

        private void ParseCorbaloc(string objRef)
        {
            CorbaLoc corbaLoc = new CorbaLoc(orb, objRef);
            IOR ior = null;
            if (corbaLoc.IsRir)
            {
                try
                {
                    CORBA.Object obj = (CORBA.Object)orb.ResolveInitialReferences(corbaLoc.KeyString);

                    if (obj == null)
                    {
                        throw new ArgumentException("Unable to resolve reference for " + corbaLoc.KeyString);
                    }

                    ior = ((Delegate)obj._Delegate).GetIOR();
                }
                catch (Exception e)
                {
                    logger.Error("Invalid corbaloc URL " + corbaLoc.KeyString, e);
                    throw new ArgumentException("Invalid corbaloc: URL");
                }
            }
            else
            {
                if (corbaLoc.profileList.Length == 0)
                {
                    // no profiles found; no point continuing
                    return;
                }
                for (int count = 0; count < corbaLoc.profileList.Length; count++)
                {
                    corbaLoc.profileList[count].SetObjectKey(corbaLoc.Key);
                }
                ior = CreateObjectIOR(orb, corbaLoc.profileList);
            }
            Decode(ior);
        }

        private void ParseCorbaName(string objectReference)
        {
            string corbaloc = "corbaloc:";
            string name = "";
            int colon = objectReference.IndexOf(':');
            int pound = objectReference.IndexOf('#');

            //save objectReference string for falling back by try_rebind()
            if (corbaNameOriginalObjRef == null)
            {
                corbaNameOriginalObjRef = objectReference;
            }

            if (pound == -1)
            {
                corbaloc += objectReference.Substring(colon + 1);
            }
            else
            {
                corbaloc += objectReference.Substring(colon + 1, pound - (colon + 1));
                name = objectReference.Substring(pound + 1);
            }

            /* empty key string in corbaname becomes NameService */
            if (corbaloc.IndexOf('/') == -1)
            {
                corbaloc += "/NameService";
            }

            try
            {
                INamingContextExt n = NamingContextExtHelper.Narrow(orb.StringToObject(corbaloc));
                IOR ior = null;
                // If the name hasn't been set - which is possible if we're just
                // resolving the root context down try to use name.
                if (name.Length > 0)
                {
                    CORBA.Object target = (CORBA.Object)n.ResolveStr(name);
                    ior = ((Delegate)target._Delegate).GetIOR();
                }
                else
                {
                    ior = ((Delegate)((CORBA.Object)n)._Delegate).GetIOR();
                }
                Decode(ior);
            }
            catch (Exception e)
            {
                logger.Error("Invalid object reference", e);

                throw new ArgumentException("Invalid object reference: " + objectReference);
            }
        }

        private void ParseResource(string resourceName)
        {
            if (logger.IsDebugEnabled)
            {
                logger.Debug("Trying to resolve URL/IOR from resource: " + resourceName);
            }

            Assembly assembly = Assembly.GetExecutingAssembly();
            TextReader inputStream = new StreamReader(assembly.GetManifestResourceStream(resourceName));
            string content = inputStream.ReadToEnd();

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Failed to get resource: " + resourceName);
            }

            try
            {
                Parse(content);
            }
            catch (IOException e)
            {
                throw new ArgumentException("Failed to read resource: " + resourceName);
            }
        }

        /// <summary>
        /// Returns the component with the given tag, searching the effective profile's components first 
        /// and then the MULTIPLE_COMPONENTS profile, if one exists. If no component with the given tag exists, 
        /// this method returns null.
        /// </summary>    
        private object GetComponent(uint tag, Type helper)
        {
            object result = null;
            if (effectiveProfile is ProfileBase pb && pb.GetComponents() != null)
            {
                result = pb.GetComponent(tag, helper);
            }

            if (result != null)
            {
                return result;
            }
            return components.GetComponent(tag, helper);
        }

        /// <summary>
        /// Wrapper class so component access for ETF profiles may use the same interface.
        /// </summary>
        internal static class LongHelper
        {
            public static int Read(IInputStream inputStream)
            {
                return inputStream.ReadLong();
            }
        }

        /// <summary>
        /// Works like GetComponent(), but for component values of CORBA type long.
        /// </summary>        
        private int? GetLongComponent(uint tag)
        {
            var type = typeof(LongHelper);
            return (int?)GetComponent(tag, type);
        }

        /// <summary>
        /// Wrapper class so component access for ETF profiles may use the same interface.
        /// </summary>
        internal static class StringHelper
        {
            public static string Read(IInputStream inputStream)
            {
                return inputStream.ReadString();
            }
        }

        /// <summary>
        /// Works like getComponent(), but for component values of type string.
        /// </summary>        
        private string GetStringComponent(uint tag)
        {
            return (string)GetComponent(tag, typeof(StringHelper));
        }

        /// <summary>
        /// Returns true if ParsedIOR can handle the protocol within the string.
        /// </summary>    
        public static bool IsParsableProtocol(string check)
        {
            if (check.StartsWith("IOR:") ||
                check.StartsWith("corbaloc:") ||
                check.StartsWith("corbaname:") ||
                check.StartsWith("resource:") ||
                check.StartsWith("file:") ||
                check.StartsWith("http:")
               )
            {
                return true;
            }
            return false;
        }

        public void SetProfileSelector(IProfileSelector selector)
        {
            profileSelector = selector;
            SetEffectiveProfile();
        }

        private IProfileSelector GetProfileSelector()
        {
            if (profileSelector == null)
            {
                return orb.TransportManager.ProfileSelector;
            }
            return profileSelector;
        }
    }
}
