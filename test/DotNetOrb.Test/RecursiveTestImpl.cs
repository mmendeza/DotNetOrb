// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CORBA;
using Test;

namespace DotNetOrb.Test
{
    public class RecursiveTestImpl : RecursivePOA
    {
        public override Any SendAny(Any a)
        {
            return a;
        }

        public override Task<Any> SendAnyAsync(Any a)
        {
            return Task.FromResult(a);
        }

        public override Recursive.Bar SendBar(Recursive.Bar b)
        {
            return b;
        }

        public override Recursive.Foo SendFoo(Recursive.Foo f)
        {
            return f;
        }
    }
}
