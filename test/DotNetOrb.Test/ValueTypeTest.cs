// Copyright (c) DotNetOrb Team (dotnetorb@gmail.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DotNetOrb.Core;
using PortableServer;
using Test.VT;

namespace DotNetOrb.Test
{
    [TestClass]
    public class ValueTypeTest
    {
        static ORB orb;
        static IValueTypesIfz testIfz;

        [ClassInitialize]
        // This method will execute before all the tests in the class
        public static void BeforeClass(TestContext context)
        {
            System.Environment.SetEnvironmentVariable("OAAddress", "iiop://localhost:3001");
            orb = (ORB)ORB.Init();
            var poa = POAHelper.Narrow(orb.ResolveInitialReferences("RootPOA"));
            poa.ThePOAManager.Activate();

            CORBA.IObject o = poa.ServantToReference(new ValueTypesIfzImpl());
            var ior = orb.ObjectToString(o);
            testIfz = ValueTypesIfzHelper.Narrow(orb.StringToObject(ior));

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
        public void TestManager()
        {
            var manager = new ManagerRecordImpl("Director","director@company.com","12342322");
            var reports = new List<EmployeeRecord>();
            var e1 = new EmployeeRecordImpl("Employee1", "e1@company.com", "111111");
            e1.Manager = manager;
            reports.Add(e1);
            var e2 = new EmployeeRecordImpl("Employee2", "e2@company.com", "222222");
            e2.Manager = manager;
            reports.Add(e2);
            var e3 = new EmployeeRecordImpl("Employee3", "e3@company.com", "333333");
            e3.Manager = manager;
            reports.Add(e3);
            manager.DirectReports = reports.ToArray();
            var result = testIfz.GetRecord(manager);
            Assert.IsTrue(result is ManagerRecord);
            Assert.IsTrue(result.DirectReports.Length == 3);
        }        

    }
}