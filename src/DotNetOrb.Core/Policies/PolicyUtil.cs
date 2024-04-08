// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using System.Collections.Generic;

namespace DotNetOrb.Core.Policies
{
    public class PolicyUtil
    {
        /// <summary>
        /// Ddetermine if a policy applies to operation invocations or not, called e.g., from PolicyManager operations 
        /// to check if  arguments are okay.
        /// </summary>
        public static bool IsInvocationPolicy(uint policyType)
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Determine if a given set of policies is consistent called e.g., from PolicyManager operations to check if arguments are okay.
        /// </summary>
        /// <param name="policies"></param>
        /// <exception cref="InvalidPolicies">A list of indices identifying the position in the input policies 
        /// list that are occupied by invalid policies</exception>
        public static void CheckValidity(Dictionary<uint, IPolicy> policies)
        {
            // TODO
        }
    }
}
