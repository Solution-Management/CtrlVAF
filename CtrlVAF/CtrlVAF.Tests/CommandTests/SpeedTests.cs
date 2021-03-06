﻿using CtrlVAF.Events;

using MFiles.VAF.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace CtrlVAF.Tests.CommandTests
{
    [TestClass]
    public class SpeedTests
    {
        [TestMethod]
        public void SpeedTest_AdditionalAssembly_100000Calls()
        {
            var conf = new Additional.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new EventCommand(environment);

            var va = Helpers.InitializeTestVA(conf);

            var dispatcher = va.EventDispatcher;

            dispatcher.IncludeAssemblies(typeof(Additional.TestConfiguration).Assembly);

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_50000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var command = new EventCommand(environment);

            var va = Helpers.InitializeTestVA(conf);

            var dispatcher = va.EventDispatcher;

            for (int i = 0; i < 50000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_100000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new EventCommand(environment);

            var va = Helpers.InitializeTestVA(conf);

            var dispatcher = va.EventDispatcher;

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }
    }


}
