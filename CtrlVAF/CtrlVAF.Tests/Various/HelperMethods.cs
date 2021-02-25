using CtrlVAF.Core;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;

namespace CtrlVAF.Tests.Various
{
    [TestClass]
    public class HelperMethods
    {
        [TestMethod]
        public void Test_AreSubConfigProperties_Test1()
        {
            var results = Dispatcher_Helpers.AreConfigSubProperties(typeof(Configuration), typeof(Configuration), typeof(Child_Configuration), typeof(GrandChild_Configuration));

            Assert.IsTrue(!results.Contains(false));
        }

        [TestMethod]
        public void Test_AreSubConfigProperties_Test2()
        {
            bool[] expected = new bool[] { true, false };

            var results = Dispatcher_Helpers.AreConfigSubProperties(typeof(Configuration), typeof(Child_Configuration), typeof(VaultApplication<object>));

            for (int i = 0; i < results.Length; i++)
            {
                Assert.AreEqual(expected[i], results[i]);
            }
        }
    }
}
