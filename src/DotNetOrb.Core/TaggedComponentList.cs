// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using IOP;
using System.Collections;
using System.Configuration;
using System.Reflection;
using CONV_FRAME;
using DotNetOrb.Core.Config;
using DotNetOrb.Core.CDR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetOrb.Core
{
    public class TaggedComponentList : ICloneable
    {
        private IList<TaggedComponent> components = null;

        /// <summary>
        /// Constructs a TaggedComponentList object from a CDR representation of an array of tagged components.
        /// </summary>
        public TaggedComponentList(IInputStream inputStream)
        {
            components = TaggedComponentSeqHelper.Read(inputStream);
        }

        /// <summary>
        /// Constructs a TaggedComponentList from a CDR encapsulation of an array of tagged components.
        /// </summary>
        public TaggedComponentList(byte[] data)
        {
            CDRInputStream inputStream = new CDRInputStream(data.ToArray());
            inputStream.OpenEncapsulatedArray();
            components = TaggedComponentSeqHelper.Read(inputStream);
        }

        /// <summary>
        /// Constructs a new, empty TaggedComponentList.
        /// </summary>     
        public TaggedComponentList()
        {
            components = new TaggedComponent[0];
        }

        public int Size()
        {
            return components.Count;
        }

        public bool IsEmpty()
        {
            return components.Count == 0;
        }

        public TaggedComponent Get(int index)
        {
            return components[index];
        }

        public object Clone()
        {
            TaggedComponentList result = new TaggedComponentList();

            result.components = new TaggedComponent[components.Count];
            for (int i = 0; i < components.Count; i++)
            {
                result.components[i] = new TaggedComponent
                (
                    components[i].Tag,
                    new byte[components[i].ComponentData.Length]
                );
                var tmp = new byte[components[i].ComponentData.Length];
                Array.Copy(components[i].ComponentData.ToArray(), 0, tmp, 0, components[i].ComponentData.Length);
                result.components[i].ComponentData = tmp;
            }
            return result;
        }

        public TaggedComponent[] AsArray()
        {
            return components.ToArray();
        }

        /// <summary>
        /// Adds a tagged component to this list. The component's data is created by marshaling 
        /// the given data Object using the Write() method of the given helper class.
        /// </summary>
        public void AddComponent(uint tag, object data, Type helper)
        {
            try
            {
                var writeMethod = helper.GetMethod("Write");
                if (writeMethod == null)
                {
                    throw new RuntimeException("Helper " + helper.Name + " has no appropriate Write() method.");
                }

                var outputStream = new CDROutputStream();

                try
                {
                    outputStream.BeginEncapsulatedArray();
                    writeMethod.Invoke(null, new object[] { outputStream, data });
                    AddComponent(tag, outputStream.GetBufferCopy());
                }
                finally
                {
                    outputStream.Close();
                }
            }
            catch (AmbiguousMatchException ex)
            {
                throw new RuntimeException("Helper " + helper.Name + ": ambiguous math for Write() method.");
            }
            catch (Exception ex)
            {
                throw new RuntimeException("Exception while marshaling component data: " + ex.Message);
            }
        }

        /// <summary>Adds a tagged component to this list.</summary>
        public void AddComponent(uint tag, byte[] data)
        {
            AddComponent(new TaggedComponent(tag, data));
        }

        /// <summary>Adds a tagged component to this list.</summary>
        public void AddComponent(TaggedComponent component)
        {
            TaggedComponent[] newComponents = new TaggedComponent[components.Count + 1];
            Array.Copy(components.ToArray(), 0, newComponents, 0, components.Count);
            newComponents[components.Count] = component;
            components = newComponents;
        }

        /// <summary>Adds an entire TaggedComponentList to this list.</summary>
        public void AddAll(TaggedComponentList other)
        {
            TaggedComponent[] newComponents = new TaggedComponent[components.Count + other.components.Count];
            Array.Copy(components.ToArray(), 0, newComponents, 0, components.Count);
            Array.Copy(other.components.ToArray(), 0, newComponents, components.Count, other.components.Count);
            components = newComponents;
        }

        /// <summary> 
        /// Searches for a component with the given tag in this component list.
        /// If one is found, this method reads the corresponding data with the given
        /// helper class, and returns the resulting object, otherwise returns
        /// null.
        /// </summary>
        public object GetComponent(uint tag, Type helper)
        {
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Tag == tag)
                {
                    return GetComponentData(components[i].ComponentData, helper);
                }
            }
            return null;
        }


        /// <summary>
        /// Returns a List of all components with the given tag from this
        /// TaggedComponentList.  Each individual component is read with
        /// the given helper class.  If no components with the given tag
        /// can be found, an empty list is returned.
        ///
        /// The only caller of this currently is IIOPProfile using a helper
        /// of IIOPAddress. To prevent non-configured IIOPAddresses we configure
        /// them here.
        /// </summary>
        public ArrayList GetComponents(IConfiguration configuration, uint tag, Type helper)
        {
            ArrayList result = new ArrayList();
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Tag == tag)
                {
                    object element = GetComponentData(components[i].ComponentData, helper);
                    if (element is IConfigurable && configuration != null)
                    {
                        try
                        {
                            ((IConfigurable)element).Configure(configuration);
                        }
                        catch (ConfigurationException e)
                        {
                            throw new CORBA.Internal("ConfigurationException: " + e.ToString());
                        }
                    }
                    result.Add(element);
                }
            }
            return result;
        }

        /// <summary>
        /// Uses the given helper class to read a CDR-encapsulated component_data
        /// field from the given byte array, data.
        /// </summary>
        private object GetComponentData(byte[] data, Type helper)
        {
            object result = null;
            var inputStream = new CDRInputStream(data.ToArray());
            try
            {
                inputStream.OpenEncapsulatedArray();
                if (helper == typeof(CodeSetComponentInfoHelper))
                {
                    result = new CodeSetComponentInfo();
                    ((CodeSetComponentInfo)result).ForCharData = CodeSetComponentHelper.Read(inputStream);
                    ((CodeSetComponentInfo)result).ForWcharData = CodeSetComponentHelper.Read(inputStream);
                }
                else if (helper == typeof(ParsedIOR.LongHelper))
                {
                    result = inputStream.ReadLong();
                }
                else if (helper == typeof(ParsedIOR.StringHelper))
                {
                    result = inputStream.ReadString();
                }
                else
                {
                    try
                    {
                        var methodInfo = helper.GetMethod(
                            "Read",
                            BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static,
                            null,
                            new[] { typeof(CDRInputStream) },
                            null);

                        if (methodInfo != null)
                        {
                            object[] parametersArray = new object[] { inputStream };
                            object target = methodInfo.IsStatic ? null : Activator.CreateInstance(helper);
                            result = methodInfo.Invoke(target, parametersArray);
                        }
                        else
                        {
                            throw new RuntimeException($"Helper {helper.Name} has no appropriate Read() method.");
                        }
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        throw new RuntimeException($"Multiple 'Read' methods found in helper {helper.Name}.", ex);
                    }
                    catch (MissingMethodException ex)
                    {
                        throw new RuntimeException($"Cannot create instance of helper {helper.Name}. No default constructor?", ex);
                    }
                    catch (TargetInvocationException ex)
                    {
                        throw new RuntimeException($"Exception while invoking 'Read' on {helper.Name}: {ex.InnerException?.Message}", ex.InnerException ?? ex);
                    }
                    catch (Exception ex)
                    {
                        throw new RuntimeException($"Unexpected error while processing helper {helper.Name}: {ex.Message}", ex);
                    }
                }
            }
            finally
            {
                inputStream.Close();
            }
            return result;
        }

        ///<summary>
        /// This function will search and remove the components, whose tag matches the given tag,
        /// from the components list.Removing tags are needed in the case the the ImR is used.
        ///</summary>
        public void RemoveComponents(uint tag)
        {
            // first, count the number of components whose tag matches the given tag.
            int matchCount = 0;
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].Tag == tag)
                {
                    matchCount++;
                }
            }
            // nothing more to do if there was no match
            if (matchCount == 0)
            {
                return;
            }

            // create a new TaggedComponent array and move the components whose tag
            // does not match the given tag.
            TaggedComponent[] newComponents = new TaggedComponent[components.Count - matchCount];

            for (int i = 0, n = 0; i < components.Count; i++)
            {
                if (components[i].Tag != tag)
                {
                    newComponents[n++] = components[i];
                }
            }

            // replace the list
            components = newComponents;
        }
    }
}
