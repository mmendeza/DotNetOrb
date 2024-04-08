// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core;
using PortableServer;
using Test;
using static Test.Recursive;
using Foo = Test.Recursive.Foo;

namespace DotNetOrb.Test
{
    [TestClass]
    public class RecursiveTest
    {
        static ORB orb;
        static IRecursive testIfz;

        [ClassInitialize]
        // This method will execute before all the tests in the class
        public static void BeforeClass(TestContext context)
        {
            System.Environment.SetEnvironmentVariable("OAAddress", "iiop://localhost:3001");
            orb = (ORB)ORB.Init();
            var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            poa.ThePOAManager.Activate();

            CORBA.IObject o = poa.ServantToReference(new RecursiveTestImpl());
            var ior = orb.ObjectToString(o);
            testIfz = RecursiveHelper.Narrow(orb.StringToObject(ior));

            //System.Environment.SetEnvironmentVariable("OAAddress", "iiop://localhost:3001");
            //orb = (ORB)ORB.Init();            
            //string serverIOR = File.ReadAllText("E:\\testior.txt");
            //testIfz = BasicTypesIfzHelper.Narrow(orb.StringToObject(serverIOR));
        }

        [ClassCleanup]
        // This method will execute after all the tests in the class
        public static void AfterClass()
        {
            orb.Shutdown(false);
        }

        [TestMethod]
        public void TestFoo()
        {
            var foo = new Foo(1, new Foo[0]);
            var foo2 = new Foo(2, new Foo[1] { foo });
            var foo3 = new Foo(3, new Foo[2] { foo, foo2 });
            var result = testIfz.SendFoo(foo3);
            Assert.IsTrue(foo3.Equals(result));
        }

        [TestMethod]
        public void TestBar()
        {
            var bar1 = new Bar();
            bar1.LMem = 1;            
            var bar2 = new Bar();
            bar2.SMem = new Bar.Foo()
            {
                DMem = 2,
                Nested = new Bar[] { bar1 }
            };
            var result = testIfz.SendBar(bar2);                       
            Assert.IsTrue(bar2.Equals(result));
        }

        [TestMethod]
        public void TestAny()
        {
            var a = orb.CreateAny();
            var foo1 = new Foo(1, new Foo[0]);
            var foo2 = new Foo(2, new Foo[1] { foo1 });
            var foo3 = new Foo(3, new Foo[2] { foo1, foo2 });
            var foo4 = new Foo(4, new Foo[1] { new Foo(5, new Foo[0]) });
            var fooSeq = new Foo[4] { foo1, foo2, foo3, foo4 };
            FooSeqHelper.Insert(a, fooSeq);
            var b = FooSeqHelper.Extract(a);
            Assert.IsTrue(fooSeq.SequenceEqual(b));            
            var result = testIfz.SendAny(a);
            var resultSeq = FooSeqHelper.Extract(result);
            Assert.IsTrue(fooSeq.SequenceEqual(resultSeq));
            Assert.IsTrue(a.Equal(result));
        }

    }
}