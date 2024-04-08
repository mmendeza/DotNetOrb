// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.POA.Exceptions;
using PortableServer;
using System.Collections.Generic;
using System.Text;
using static PortableServer.POAManager;

namespace DotNetOrb.Core.POA.Util
{
    public class POAUtil
    {
        private static int bytesPerLine = 20;
        private static char[] lookup = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        private POAUtil() { }

        /// <summary>
        /// outputs a byte oid in a hex string dump formatted
        /// </summary>        
        public static string Convert(byte[] data)
        {
            var result = new StringBuilder();
            int k = 0;

            for (int j = 0; j < data.Length; j++)
            {
                result.Append(ToHex(data[j]));

                bool lastLine = j >= data.Length - 1;

                if (lastLine)
                {
                    for (int p = 0;
                         p < bytesPerLine - j % bytesPerLine - 1;
                         p++)
                    {
                        result.Append("   ");
                    }
                }

                if (j % bytesPerLine == bytesPerLine - 1 || lastLine)
                {
                    for (; k <= j; k++)
                    {
                        result.Append(data[k] < 32 ? '.' : (char)data[k]);
                    }
                }
            }
            return result.ToString();
        }

        /// <summary>
        /// Reads the policy value from the specified policy and converts it into a string
        /// </summary>        
        public static string Convert(IPolicy policy, uint policyType)
        {
            switch (policyType)
            {
                case THREAD_POLICY_ID.Value:
                    if (policy == null || ((IThreadPolicy)policy).Value == ThreadPolicyValue.ORB_CTRL_MODEL) return "ORB_CTRL_MODEL";
                    else if (((IThreadPolicy)policy).Value == ThreadPolicyValue.SINGLE_THREAD_MODEL) return "SINGLE_THREAD_MODEL";
                    break;

                case LIFESPAN_POLICY_ID.Value:
                    if (policy == null || ((ILifespanPolicy)policy).Value == LifespanPolicyValue.TRANSIENT) return "TRANSIENT";
                    else if (((ILifespanPolicy)policy).Value == LifespanPolicyValue.PERSISTENT) return "PERSISTENT";
                    break;

                case ID_UNIQUENESS_POLICY_ID.Value:
                    if (policy == null || ((IIdUniquenessPolicy)policy).Value == IdUniquenessPolicyValue.UNIQUE_ID) return "UNIQUE_ID";
                    else if (((IIdUniquenessPolicy)policy).Value == IdUniquenessPolicyValue.MULTIPLE_ID) return "MULTIPLE_ID";
                    break;

                case ID_ASSIGNMENT_POLICY_ID.Value:
                    if (policy == null || ((IIdAssignmentPolicy)policy).Value == IdAssignmentPolicyValue.SYSTEM_ID) return "SYSTEM_ID";
                    else if (((IIdAssignmentPolicy)policy).Value == IdAssignmentPolicyValue.USER_ID) return "USER_ID";
                    break;

                case SERVANT_RETENTION_POLICY_ID.Value:
                    if (policy == null || ((IServantRetentionPolicy)policy).Value == ServantRetentionPolicyValue.RETAIN) return "RETAIN";
                    else if (((IServantRetentionPolicy)policy).Value == ServantRetentionPolicyValue.NON_RETAIN) return "NON_RETAIN";
                    break;

                case REQUEST_PROCESSING_POLICY_ID.Value:
                    if (policy == null || ((IRequestProcessingPolicy)policy).Value == RequestProcessingPolicyValue.USE_ACTIVE_OBJECT_MAP_ONLY) return "USE_ACTIVE_OBJECT_MAP_ONLY";
                    else if (((IRequestProcessingPolicy)policy).Value == RequestProcessingPolicyValue.USE_SERVANT_MANAGER) return "USE_SERVANT_MANAGER";
                    else if (((IRequestProcessingPolicy)policy).Value == RequestProcessingPolicyValue.USE_DEFAULT_SERVANT) return "USE_DEFAULT_SERVANT";
                    break;

                case IMPLICIT_ACTIVATION_POLICY_ID.Value:
                    if (policy == null || ((IImplicitActivationPolicy)policy).Value == ImplicitActivationPolicyValue.NO_IMPLICIT_ACTIVATION) return "NO_IMPLICIT_ACTIVATION";
                    else if (((IImplicitActivationPolicy)policy).Value == ImplicitActivationPolicyValue.IMPLICIT_ACTIVATION) return "IMPLICIT_ACTIVATION";
                    break;
            }
            return "unknown";
        }

        /// <summary>
        /// Extracts the impl name from a specified object key
        /// </summary>        
        /// <exception cref="POAInternalException"></exception>
        public static string ExtractImplName(byte[] objectKey)
        {
            for (int i = 0; i < objectKey.Length; i++)
            {
                if (objectKey[i] == POAConstants.ObjectKeySeparatorByte
                    && (i == 0 || objectKey[i - 1] != POAConstants.MaskByte))
                {
                    return UnmaskStr(objectKey, 0, i);
                }
            }
            throw new POAInternalException("error extracting impl name from objectKey: " + Convert(objectKey));
        }


        /// <summary>
        /// Extracts the oid from a specified object key
        /// </summary>        
        /// <exception cref="POAInternalException"></exception>
        public static byte[] ExtractOID(byte[] objectKey)
        {
            for (int i = objectKey.Length - 1; i >= 0; i--)
            {
                if (objectKey[i] == POAConstants.ObjectKeySeparatorByte
                    && (i == 0 || objectKey[i - 1] != POAConstants.MaskByte))
                {
                    ++i;
                    return UnmaskId(objectKey, i, objectKey.Length - i);
                }
            }
            throw new POAInternalException("error extracting oid from objectKey: " + Convert(objectKey));
        }


        /// <summary>
        /// Extracts the oid from a specified object reference
        /// </summary>        
        public static byte[] ExtractOID(CORBA.Object reference)
        {
            return ((Delegate)reference._Delegate).GetObjectId();
        }

        /// <summary>
        /// Extracts the poa name from a specified object key
        /// </summary>
        /// <exception cref="POAInternalException"></exception>
        public static string ExtractPOAName(byte[] objectKey)
        {
            int begin = objectKey.Length;
            int end = 0;
            for (int i = 0; i < objectKey.Length; i++)
            {
                if (objectKey[i] == POAConstants.ObjectKeySeparatorByte
                    && (i == 0 || objectKey[i - 1] != POAConstants.MaskByte))
                {
                    begin = i;
                    break;
                }
            }
            for (int i = objectKey.Length - 1; i >= 0; i--)
            {
                if (objectKey[i] == POAConstants.ObjectKeySeparatorByte
                    && (i == 0 || objectKey[i - 1] != POAConstants.MaskByte))
                {
                    end = i;
                    break;
                }
            }
            if (begin > end)
            {
                throw new POAInternalException("error extracting poa name from objectKey: " + Convert(objectKey));
            }
            if (begin == end)
            {
                return "";
            }

            begin++;
            return Encoding.Default.GetString(objectKey, begin, end - begin);
        }

        /// <summary>
        /// Returns a list containing the poa names.
        /// </summary>
        /// <param name="poaName">string value which may contain poa names separated by <see cref="POAConstants.ObjectKeySeparator"/> </param>
        /// <returns>List of poa names</returns>
        public static List<string> ExtractScopedPOANames(string poaName)
        {
            var scopes = new List<string>();
            int length = poaName.Length;

            if (length > 0)
            {
                // Fill in the list with the poaNames.
                int previous = 0, current = 0;

                for (; current < length; ++current)
                {
                    // If we've found a separator skip over it and add to the vector
                    if (poaName[current] == POAConstants.ObjectKeySeparator)
                    {
                        scopes.Add(poaName.Substring(previous, current - previous));
                        ++current;
                        previous = current;
                    }
                }
                // Add the final POA name
                scopes.Add(poaName.Substring(previous, current - previous));
            }
            return scopes;
        }


        /// <summary>
        /// Returns the policy with the specified policyType from a policy list
        /// </summary>
        public static IPolicy GetPolicy(IPolicy[] policies, uint policyType)
        {
            if (policies != null)
            {
                for (int i = 0; i < policies.Length; i++)
                {
                    if (policies[i].PolicyType == policyType)
                    {
                        return policies[i];
                    }
                }
            }
            return null;
        }

        public static bool IsActive(State state)
        {
            return state == State.ACTIVE ? true : false;
        }

        public static bool IsDiscarding(State state)
        {
            return state == State.DISCARDING ? true : false;
        }

        public static bool IsHolding(State state)
        {
            return state == State.HOLDING ? true : false;
        }

        public static bool IsInactive(State state)
        {
            return state == State.INACTIVE ? true : false;
        }

        /// <summary>
        /// Masks the object key separator bytes
        /// </summary>        
        public static byte[] MaskId(byte[] id)
        {
            int altered = id.Length;
            for (int i = 0; i < id.Length; i++)
            {
                if (id[i] == POAConstants.ObjectKeySeparatorByte)
                {
                    altered++;
                }
                else if (id[i] == POAConstants.MaskByte)
                {
                    altered++;
                }
            }
            if (altered == id.Length) return id;

            byte[] result = new byte[altered];

            altered = 0;
            for (int i = 0; i < id.Length; i++)
            {
                if (id[i] == POAConstants.ObjectKeySeparatorByte)
                {
                    result[altered] = POAConstants.MaskByte;
                    result[altered + 1] = POAConstants.SepaMaskByte;
                    altered += 2;

                }
                else if (id[i] == POAConstants.MaskByte)
                {
                    result[altered] = POAConstants.MaskByte;
                    result[altered + 1] = POAConstants.MaskMaskByte;
                    altered += 2;

                }
                else
                {
                    result[altered] = id[i];
                    altered++;
                }
            }
            return result;
        }

        /// <summary>
        /// Masks the object key separator chars
        /// </summary>        
        public static string MaskStr(string str)
        {
            return Encoding.Default.GetString(MaskId(Encoding.Default.GetBytes(str)));
        }

        /// <summary>
        /// Unmasks the object key separator chars
        /// </summary>        
        public static string UnmaskStr(string str)
        {
            return Encoding.Default.GetString(UnmaskId(Encoding.Default.GetBytes(str)));
        }

        private static string UnmaskStr(byte[] data, int offset, int length)
        {
            return Encoding.Default.GetString(UnmaskId(data, offset, length));
        }

        /// <summary>
        /// Unmasks the object key separator bytes
        /// </summary>
        public static byte[] UnmaskId(byte[] id)
        {
            int altered = GetAltered(id, 0, id.Length);

            if (altered == id.Length)
            {
                return id;
            }
            else
            {
                return UnmaskId(id, 0, id.Length, altered);
            }
        }

        /// <summary>
        /// Unmasks the object key separator bytes
        /// </summary>        
        private static byte[] UnmaskId(byte[] id, int start, int length)
        {
            int altered = GetAltered(id, start, length);

            return UnmaskId(id, start, length, altered);
        }

        private static int GetAltered(byte[] id, int start, int length)
        {
            int altered = length;
            for (int i = start; i < start + length; i++)
            {
                if (id[i] == POAConstants.MaskByte)
                {
                    altered--;
                    i++;
                }
            }
            return altered;
        }

        private static byte[] UnmaskId(byte[] id, int start, int length, int altered)
        {
            byte[] result = new byte[altered];

            for (int i = start, resultIdx = 0; i < start + length; ++i, ++resultIdx)
            {
                if (id[i] == POAConstants.MaskByte)
                {
                    if (id[i + 1] == POAConstants.MaskMaskByte)
                    {
                        result[resultIdx] = POAConstants.MaskByte;
                    }
                    else if (id[i + 1] == POAConstants.SepaMaskByte)
                    {
                        result[resultIdx] = POAConstants.ObjectKeySeparatorByte;
                    }
                    else
                    {
                        throw new POAInternalException("error: forbidden byte sequence \"" + POAConstants.MaskByte + id[i + 1] + "\" (unmaskId)");
                    }
                    ++i;
                }
                else
                {
                    result[resultIdx] = id[i];
                }
            }
            return result;
        }

        public static string ToHex(byte b)
        {
            var sb = new StringBuilder();

            int upper = b >> 4 & 0x0F;
            sb.Append(lookup[upper]);

            int lower = b & 0x0F;
            sb.Append(lookup[lower]);

            sb.Append(' ');

            return sb.ToString();
        }
    }
}
