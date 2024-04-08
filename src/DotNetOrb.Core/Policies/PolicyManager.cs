// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DotNetOrb.Core.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DotNetOrb.Core.Policies
{
    /// <summary>
    ///  Implementation of the ORB-level policy management interface as per CORBA 2.6, p. 4-43 to 4-45: 
    ///  This PolicyManager has operations through which a set of Policies can be applied and the current 
    ///  overriding Policy settings can be obtained.Policies applied at the ORB level override any system defaults.
    ///  The ORB's PolicyManager is obtained through an invocation of ORB::resolve_initial_references, 
    ///  specifying an identifier of "ORBPolicyManager."
    /// </summary>
    public class PolicyManager : _PolicyManagerLocalBase
    {
        private static IPolicy[] EmptyResult = new IPolicy[0];
        private Dictionary<uint, IPolicy> policyOverrides = new Dictionary<uint, IPolicy>();
        private ILogger logger;

        public PolicyManager(IConfiguration config) : base()
        {
            logger = config.GetLogger(GetType());
        }

        /// <summary>
        ///  Returns a PolicyList containing the overridden Polices for the requested PolicyTypes.
        ///  If the specified sequence is empty, all Policy overrides at this scope will be returned.
        ///  If none of the requested PolicyTypes are overridden at the target PolicyManager, an empty sequence is returned.
        ///  This accessor returns only those Policy overrides that have been set at the specific scope corresponding
        ///  to the target PolicyManager (no evaluation is done with respect to overrides at other scopes).
        /// </summary>        
        public override IPolicy[] GetPolicyOverrides(uint[] ts)
        {
            if (ts == null)
            {
                throw new ArgumentException("Argument may not be null");
            }

            if (ts.Length == 0)
            {
                return policyOverrides.Values.ToArray();
            }

            List<IPolicy> policyList;
            lock (policyOverrides)
            {
                if (policyOverrides.Count == 0)
                {
                    return EmptyResult;
                }

                if (ts.Length == 0)
                {
                    return policyOverrides.Values.ToArray();
                }

                policyList = new List<IPolicy>();

                for (int i = 0; i < ts.Length; i++)
                {
                    var policy = policyOverrides[ts[i]];
                    if (policy != null)
                    {
                        policyList.Add(policy);
                    }
                }
            }
            var result = policyList.ToArray();
            if (logger.IsDebugEnabled && result.Length > 0)
            {
                logger.Debug("get_policy_overrides returns " + result.Length + " policies");
            }
            return result;
        }

        /// <summary>
        /// Modifies the current set of overrides with the requested list of Policy overrides.
        /// The first parameter policies is a sequence of references to Policy objects.
        /// The second parameter setAdd of type SetOverrideType indicates whether these policies should 
        /// be added onto any other overrides that already exist (ADD_OVERRIDE) in the PolicyManager, 
        /// or they should be added to a clean PolicyManager free of any other overrides (SET_OVERRIDE).
        /// 
        /// Invoking set_policy_overrides with an empty sequence of policies and a mode of SET_OVERRIDE removes all overrides from 
        /// a PolicyManager.Only certain policies that pertain to the invocation of an operation at the client end can be overridden
        /// using this operation.Attempts to override any other policy will result in the raising of the CORBA::NO_PERMISSION exception. 
        /// If the request would put the set of overriding policies for the target PolicyManager in an inconsistent state, 
        /// no policies are changed or added, and the exception InvalidPolicies is raised.There is no evaluation of compatibility 
        /// with policies set within other PolicyManagers.
        /// </summary>
        /// <param name="policies">a sequence of Policy objects that are to be associated with the PolicyManager object.</param>
        /// <param name="setAdd">whether the association is in addition to (ADD_OVERRIDE) or as a replacement of (SET_OVERRIDE) any 
        /// existing overrides already associated with the PolicyManager object. If the value of this parameter is SET_OVERRIDE, 
        /// the supplied policies completely replace all existing overrides associated with the PolicyManager object. 
        /// If the value of this parameter is ADD_OVERRIDE, the supplied policies are added to the existing overrides associated 
        /// with the PolicyManager object, except that if a supplied Policy object has the same PolicyType value as an existing override,
        /// the supplied Policy object replaces the existing override.</param>
        /// <exception cref="InvalidPolicies">A list of indices identifying the position in the input policies 
        /// list that are occupied by invalid policies</exception>
        /// <exception cref="BadParam">If the sequence contains two or more Policy objects with the same PolicyType value, 
        /// the operation raises the standard sytem exception BAD_PARAM with standard minor code 30.</exception>
        public override void SetPolicyOverrides(IPolicy[] policies, SetOverrideType setAdd)
        {
            if (policies == null)
            {
                throw new ArgumentException("Argument may not be null");
            }

            var newPolicies = new Dictionary<uint, IPolicy>();
            var sb = new StringBuilder();

            // check that the policies argument does not contain multiple
            // policies of the same type while (copying the list of policies)
            // and does not override policies that cannot be overriden
            for (int i = 0; i < policies.Length; i++)
            {
                if (!PolicyUtil.IsInvocationPolicy(policies[i].PolicyType))
                {
                    throw new NoPermission("Not an invocation policy, type " + policies[i].PolicyType);
                }
                // else:
                var key = policies[i].PolicyType;
                if (newPolicies.ContainsKey(key))
                {
                    throw new BadParam("Multiple policies of type " + policies[i].PolicyType, 30, CompletionStatus.No);
                }
                newPolicies.Add(key, policies[i]);
                sb.Append(" " + policies[i].PolicyType);
            }


            lock (policyOverrides)
            {
                if (setAdd == SetOverrideType.SET_OVERRIDE)
                {
                    PolicyUtil.CheckValidity(newPolicies);
                    policyOverrides.Clear();
                    foreach (var kvp in newPolicies)
                    {
                        policyOverrides.Add(kvp.Key, kvp.Value);
                    }
                }
                else if (setAdd == SetOverrideType.ADD_OVERRIDE)
                {
                    // adds policies (and replaces any existing policies)
                    var test = new Dictionary<uint, IPolicy>(policyOverrides);
                    foreach (var kvp in newPolicies)
                    {
                        test.Add(kvp.Key, kvp.Value);
                    }
                    PolicyUtil.CheckValidity(test);
                    policyOverrides.Clear();
                    foreach (var kvp in newPolicies)
                    {
                        policyOverrides.Add(kvp.Key, kvp.Value);
                    }
                }
            }

            if (logger.IsDebugEnabled)
            {
                string prefix = setAdd == SetOverrideType.ADD_OVERRIDE ? "ADD_OVERRIDE" : "SET_OVERRIDE";
                logger.Debug(prefix + ", types: " + sb);
            }
        }
    }
}
