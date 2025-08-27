// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core;
using PortableServer;
using Test;

namespace DotNetOrb.Test
{
    [TestClass]
    public class BasicTypesTest
    {
        static ORB orb;
        static IBasicTypesIfz testIfz;

        [ClassInitialize]
        // This method will execute before all the tests in the class
        public static void BeforeClass(TestContext context)
        {
            System.Environment.SetEnvironmentVariable("OAAddress", "iiop://localhost:3001");
            orb = (ORB)ORB.Init();
            var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            poa.ThePOAManager.Activate();

            CORBA.IObject o = poa.ServantToReference(new BasicTypesIfzImpl());
            var ior = orb.ObjectToString(o);
            testIfz = BasicTypesIfzHelper.Narrow(orb.StringToObject(ior));

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
        public void TestBoolean()
        {
            testIfz.ABoolean = true;
            var tmp = testIfz.ABoolean;
            Assert.AreEqual(true, tmp);
        }

        [TestMethod]
        public void TestBooleanSeq()
        {
            var input = new bool[] { true, false, true, false };
            testIfz.ABooleanSeq = input;
            var output = testIfz.ABooleanSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestOctet()
        {
            byte input = 84;
            testIfz.AOctet = input;
            var output = testIfz.AOctet;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestOctetSeq()
        {
            var input = new byte[] { 1, 2, 3, 4};
            testIfz.AOctetSeq = input;
            var output = testIfz.AOctetSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestSByte()
        {
            sbyte input = -84;
            testIfz.ASbyte = input;
            var output = testIfz.ASbyte;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestSByteSeq()
        {
            var input = new sbyte[] { 10, -12, 23, -34 };
            testIfz.ASbyteSeq = input;
            var output = testIfz.ASbyteSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestByte()
        {
            byte input = 84;
            testIfz.AByte = input;
            var output = testIfz.AByte;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestByteSeq()
        {
            var input = new byte[] { 10, 12, 23, 34 };
            testIfz.AByteSeq = input;
            var output = testIfz.AByteSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestShort()
        {
            short input = 8433;
            testIfz.AShort = input;
            var output = testIfz.AShort;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestShortSeq()
        {
            var input = new short[] { -102, 132, -1223, 334 };
            testIfz.AShortSeq = input;
            var output = testIfz.AShortSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestUShort()
        {
            ushort input = 8433;
            testIfz.AUshort = input;
            var output = testIfz.AUshort;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestUShortSeq()
        {
            var input = new ushort[] { 102, 132, 1223, 334 };
            testIfz.AUshortSeq = input;
            var output = testIfz.AUshortSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestLong()
        {
            int input = 8433000;
            testIfz.ALong = input;
            var output = testIfz.ALong;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestLongSeq()
        {
            var input = new int[] { -102343, 13332, -125323, 33224 };
            testIfz.ALongSeq = input;
            var output = testIfz.ALongSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestULong()
        {
            uint input = 567087;
            testIfz.AUlong = input;
            var output = testIfz.AUlong;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestULongSeq()
        {
            var input = new uint[] { 103332, 132222, 12345523, 33434434 };
            testIfz.AUlongSeq = input;
            var output = testIfz.AUlongSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestLongLong()
        {
            long input = 3423133000;
            testIfz.ALongLong = input;
            var output = testIfz.ALongLong;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestLongLongSeq()
        {
            var input = new long[] { -1023423343, 44513234332, -125333213423, 3322434344 };
            testIfz.ALongLongSeq = input;
            var output = testIfz.ALongLongSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestULongLong()
        {
            ulong input = 567043444387;
            testIfz.AUlongLong = input;
            var output = testIfz.AUlongLong;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestULongLongSeq()
        {
            var input = new ulong[] { 123434503332, 132324543222, 12264867345523, 33435675674434 };
            testIfz.AUlongLongSeq = input;
            var output = testIfz.AUlongLongSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestFloat()
        {
            float input = 2.3f;
            testIfz.AFloat = input;
            var output = testIfz.AFloat;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestFloatSeq()
        {
            var input = new float[] { -1.4f, 132.532f, -225.5623f, 33224f };
            testIfz.AFloatSeq= input;
            var output = testIfz.AFloatSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestDouble()
        {
            double input = 232.4563d;
            testIfz.ADouble= input;
            var output = testIfz.ADouble;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestDoubleSeq()
        {
            var input = new double[] { -345.444d, 532.133d, -123.462d, 2.3224d };
            testIfz.ADoubleSeq= input;
            var output = testIfz.ADoubleSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestFixed()
        {
            decimal input = 232.45m;
            testIfz.AFixed = input;
            var output = testIfz.AFixed;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestFixedException()
        {
            decimal input = 2322.45m;            
            var output = testIfz.AFixed;
            Assert.ThrowsException<CORBA.Marshal>(() => testIfz.AFixed = input);
        }

        [TestMethod]
        public void TestFixedSeq()
        {
            var input = new decimal[] { -223.444m, 53.133m, -12.462m, 2.32222m };
            testIfz.AFixedSeq = input;
            var output = testIfz.AFixedSeq;
            CollectionAssert.AreEqual(new decimal[] { -223.44m, 53.13m, -12.46m, 2.32m }, output);
        }

        [TestMethod]
        public void TestChar()
        {
            char input = 'x';
            testIfz.AChar = input;
            var output = testIfz.AChar;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestCharSeq()
        {
            var input = new char[] { 'g', 'i', 'o', 'p' };
            testIfz.ACharSeq= input;
            var output = testIfz.ACharSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestWChar()
        {
            char input = 'ä';
            testIfz.AWchar = input;
            var output = testIfz.AWchar;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestWCharSeq()
        {
            var input = new char[] { 'ä', 'ë', 'ï', 'ö', 'ü' };
            testIfz.AWcharSeq = input;
            var output = testIfz.AWcharSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestString()
        {
            string input = "CORBA";
            testIfz.AString = input;
            var output = testIfz.AString;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestStringSeq()
        {
            var input = new string[] { "ONE", "TWO", "THREE", "FOUR", "FIVE" };
            testIfz.AStringSeq = input;
            var output = testIfz.AStringSeq;
            CollectionAssert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestWString()
        {
            string input = "CÖRBÄ";
            testIfz.AWstring = input;
            var output = testIfz.AWstring;
            Assert.AreEqual(input, output);
        }

        [TestMethod]
        public void TestWStringSeq()
        {
            var input = new string[] { "ÖNE", "TWÖ", "THRËË", "FÖÜR", "FÏVE" };
            testIfz.AWstringSeq = input;
            var output = testIfz.AWstringSeq;
            CollectionAssert.AreEqual(input, output);
        }

    }
}