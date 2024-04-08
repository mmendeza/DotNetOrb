// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using ETF;
using IOP;
using System.Configuration;
using Version = GIOP.Version;
using IIOP;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.CDR;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DotNetOrb.Core.ETF
{
    public abstract class ProfileBase : _ProfileLocalBase, IConfigurable, ICloneable
    {
        protected Version version = null;
        protected byte[] objectKey = null;
        protected TaggedComponentList components = null;

        protected IConfiguration configuration;
        protected string corbalocStr = null;
        protected ILogger logger;

        public void Configure(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ConfigurationException("ProfileBase: given configuration was null");
            }

            this.configuration = configuration;
        }

        public override void SetObjectKey(byte[] key)
        {
            objectKey = key;
        }

        public override byte[] GetObjectKey()
        {
            return objectKey;
        }

        public override Version Version
        {
            get { return version; }            
        }

        /// <summary>
        /// Profiles use this method for taking alternative address values
        /// for replacement, such as when an IOR proxy or IMR is in use.
        /// This is a concrete method here to not break existing profiles
        /// that may not be interested in this behavior.
        /// </summary>
        public virtual void PatchPrimaryAddress(ProtocolAddressBase replacement)
        {

        }


        /// <summary>
        /// ETF defined function to marshal the appropriate information for this
        /// transport into the tagged profile.  ORBs will typically need
        /// to call the IOR interception points before calling marshal().     
        /// This particular implementation *should* work for any IOP
        /// type protocol that encodes its profile_data as a CDR encapsulated
        /// octet array as long as you have correctly implemented
        /// the <see cref="Encapsulation"/>, <see cref="WriteAddressProfile(CDROutputStream)"/>, and
        /// <see cref="ReadAddressProfile(CDRInputStream)"/> methods. But, feel free to override
        /// it for the purpose of optimisation or whatever. It should however,
        /// remain consistent with your implementation
        /// of the above mentioned methods. 
        /// </summary>        
        public override void Marshal(ref TaggedProfile taggedProfile, ref TaggedComponent[] components)
        {

            if (Encapsulation() != 0)
            {
                // You're going to have to define your own marshal operation
                // for littleEndian profiles.
                // The CDROutputStream only does big endian currently.
                throw new BadParam("We can only marshal big endian style profiles !!");
            }

            // Start a CDR encapsulation for the profile_data
            CDROutputStream profileDataStream = new CDROutputStream();
            try
            {
                profileDataStream.BeginEncapsulatedArray();

                // Write the opaque AddressProfile bytes for this profile...
                WriteAddressProfile(profileDataStream);

                // ... then the object key
                profileDataStream.WriteLong(objectKey.Length);
                profileDataStream.WriteOctetArray(objectKey.ToArray(), 0, objectKey.Length);

                switch (version.Minor)
                {
                    case 0:
                        // For GIOP 1.0 there were no tagged components
                        break;
                    case 1:
                    // fallthrough
                    case 2:
                        {
                            // Assume minor != 0 means 1.1 onwards and encode the TaggedComponents
                            if (components == null)
                            {
                                components = new TaggedComponent[0];
                            }
                            // Write the length of the TaggedProfile sequence.
                            profileDataStream.WriteLong(this.components.Size() + components.Length);

                            // Write the TaggedProfiles (ours first, then the ORB's)
                            TaggedComponent[] ourTaggedProfiles = this.components.AsArray();
                            for (int i = 0; i < ourTaggedProfiles.Length; i++)
                            {
                                TaggedComponentHelper.Write(profileDataStream, ourTaggedProfiles[i]);
                            }
                            for (int i = 0; i < components.Length; i++)
                            {
                                TaggedComponentHelper.Write(profileDataStream, components[i]);
                            }
                            break;
                        }
                    default:
                        {
                            throw new CORBA.Internal("Unknown GIOP version tag " + version.Minor + " when marshalling for IIOPProfile");
                        }
                }

                // Populate the TaggedProfile for return.
                taggedProfile = new TaggedProfile((uint)Tag, profileDataStream.GetBufferCopy());
            }
            finally
            {
                profileDataStream.Close();
            }
        }

        ///<summary>
        /// Method to mirror the marshal method.
        ///</summary>        
        public void Demarshal(ref TaggedProfile taggedProfile, ref TaggedComponent[] components)
        {
            if (taggedProfile.Tag != Tag)
            {
                throw new BadParam("Wrong tag for Transport, tag: " + taggedProfile.Tag);
            }
            InitFromProfileData(taggedProfile.ProfileData);
            if (components != null)
            {
                components = GetComponents().AsArray();
            }
        }

        /// <summary>
        /// Indicates the encapsulation that will be used by this profile
        /// when encoding its AddressProfile bytes, and which should subsequently
        /// be used when marshalling all the rest of the TaggedProfile.profile_data.
        /// Using the default CDROutputStream for a transport profile encapsulation
        /// this should always be 0.
        /// </summary>
        public short Encapsulation()
        {
            return 0; // i.e. Big endian TAG_INTERNET_IOP style
        }

        /// <summary>
        /// Write the AddressProfile to the supplied stream.
        /// Implementors can assume an encapsulation is already open.
        /// </summary>
        public abstract void WriteAddressProfile(CDROutputStream stream);


        /// <summary>
        /// Read the ETF::AddressProfile from the supplied stream.
        /// </summary>
        public abstract void ReadAddressProfile(CDRInputStream stream);

        /// <summary>
        /// Accessor for the TaggedComponents of the Profile.
        /// </summary>
        public TaggedComponentList GetComponents()
        {
            return components;
        }

        public object GetComponent(uint tag, Type helper)
        {
            return components.GetComponent(tag, helper);
        }

        public void AddComponent(uint tag, object data, Type helper)
        {
            components.AddComponent(tag, data, helper);
        }

        public void AddComponent(uint tag, byte[] data)
        {
            components.AddComponent(tag, data);
        }

        public TaggedProfile AsTaggedProfile()
        {
            var result = new TaggedProfile();
            TaggedComponent[] components = new TaggedComponent[0];
            Marshal(ref result, ref components);
            return result;
        }

        ///<summary>
        ///This function shall return an equivalent, deep-copy of the profile on the free store.
        ///</summary>
        public override IProfile Copy()
        {
            try
            {
                return (IProfile)Clone();
            }
            catch (Exception e)
            {
                throw new RuntimeException("error cloning profile: " + e); // NOPMD
            }
        }

        ///<summary>
        ///Used from the byte[] constructor and the demarshal method. 
        ///Relies on subclasses having satisfactorily implemented the <see cref="ReadAddressProfile(CDRInputStream)"/> method
        ///</summary>
        protected void InitFromProfileData(byte[] data)
        {
            CDRInputStream inputStream = new CDRInputStream(data.ToArray());
            try
            {
                inputStream.OpenEncapsulatedArray();
                ReadAddressProfile(inputStream);

                uint length = inputStream.ReadULong();

                if (inputStream.Available < length)
                {
                    throw new Marshal("Unable to extract object key. Only " + inputStream.Available + " available and trying to assign " + length);
                }
                var tmp = new byte[length];
                inputStream.ReadOctetArray(ref tmp, 0, (int)length);
                objectKey = tmp;
                components = version != null && version.Minor > 0 ? new TaggedComponentList(inputStream) : new TaggedComponentList();
            }
            finally
            {
                inputStream.Close();
            }
        }

        ///<summary>
        ///Collection of ListenPoints that represent the endpoints contained in this IIOPProfile.
        ///</summary>
        public virtual IEnumerable<ListenPoint> AsListenPoints()
        {
            return Enumerable.Empty<ListenPoint>();
        }

        ///<summary>
        ///This function will search and remove the components, whose tag matches the given tag, 
        ///from the components list.Removing tags are needed in the case the the ImR is used.
        ///</summary>
        public void RemoveComponents(uint tag)
        {
            if (components != null)
            {
                components.RemoveComponents(tag);
            }
        }

        public abstract object Clone();

    }
}
