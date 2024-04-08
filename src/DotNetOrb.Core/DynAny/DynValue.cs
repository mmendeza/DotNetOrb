// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using DynamicAny;

namespace DotNetOrb.Core.DynAny
{
    public class DynValue : DynAny, IDynValue
    {
        private DynValue() : base(null, null, null)
        {

        }

        public TCKind CurrentMemberKind()
        {
            throw new NoImplement();
        }

        [return: WideChar(false)]
        public string CurrentMemberName()
        {
            throw new NoImplement();
        }

        public NameValuePair[] GetMembers()
        {
            throw new NoImplement();
        }

        public NameDynAnyPair[] GetMembersAsDynAny()
        {
            throw new NoImplement();
        }

        public bool IsNull()
        {
            throw new NoImplement();
        }

        public void SetMembers(NameValuePair[] value)
        {
            throw new NoImplement();
        }

        public void SetMembersAsDynAny(NameDynAnyPair[] value)
        {
            throw new NoImplement();
        }

        public void SetToNull()
        {
            throw new NoImplement();
        }

        public void SetToValue()
        {
            throw new NoImplement();
        }
    }
}
