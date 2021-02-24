using CtrlVAF.Core;
using CtrlVAF.Validators;

using MFiles.VAF.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CtrlVAF.Tests.ConfigValidationTests
{
    [TestClass]
    public class SpeedTests
    {
        [TestMethod]
        public void SpeedTest_10000Calls()
        {
            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            var va = new VaultApplication();
            va.SetConfig(config);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher<Configuration>(va);
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand {Vault = vault };

            for (int i = 0; i < 10000; i++)
            {
                var results = dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_50000Calls()
        {
            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            var va = new VaultApplication();
            va.SetConfig(config);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher<Configuration>(va);
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand { Vault = vault };

            for (int i = 0; i < 50000; i++)
            {
                var results = dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_100000Calls()
        {
            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            var va = new VaultApplication();
            va.SetConfig(config);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher<Configuration>(va);
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand { Vault = vault };

            for (int i = 0; i < 100000; i++)
            {
                var results = dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }
    }
}
