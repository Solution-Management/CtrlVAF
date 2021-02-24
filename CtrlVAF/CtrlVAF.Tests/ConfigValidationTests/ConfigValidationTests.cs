
using CtrlVAF.Core;
using CtrlVAF.Validators;

using MFiles.VAF.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Collections.Generic;
using System.Linq;

namespace CtrlVAF.Tests.ConfigValidationTests
{
    [TestClass]
    public class ConfigValidationTests
    {
        [TestMethod]
        public void Assert_BaseCase()
        {
            var expected = 1;

            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            var va = new VaultApplication();
            va.SetConfig(config);
            va.StartOperations(vault);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = va.ValidatorDispatcher;

            var command = new ValidatorCommand { Vault = vault };

            var results = dispatcher.Dispatch(command);

            Assert.AreEqual(expected, results.Count());
        }

        [TestMethod]
        public void Assert_ChildConfiguration()
        {
            var expected = 1;

            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "Blabla", ID = 42, ChildConfig = new Child_Configuration {Name = "" } };

            var va = new VaultApplication();
            va.SetConfig(config);
            va.StartOperations(vault);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = va.ValidatorDispatcher;

            var command = new ValidatorCommand { Vault = vault };

            var results = dispatcher.Dispatch(command);

            Assert.AreEqual(expected, results.Count());

        }

        [TestMethod]
        public void Assert_ResultsAreCached()
        {
            var expected = 1;

            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            var va = new VaultApplication();
            va.SetConfig(config);

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher<Configuration>(va);
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand { Vault = vault };

            dispatcher.Dispatch(command);

            var results = dispatcher.GetCachedResults(typeof(ConfigurationValidator));

            Assert.AreEqual(expected, results.Count());
        }


    }
}
