
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

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher();
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand<Configuration> { Configuration = config, Vault = vault };

            var results = dispatcher.Dispatch(command);

            Assert.AreEqual(expected, results.Count());
        }

        [TestMethod]
        public void Assert_ResultsAreCached()
        {
            var expected = 1;

            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher();
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var command = new ValidatorCommand<Configuration> { Configuration = config, Vault = vault };

            dispatcher.Dispatch(command);

            var results = dispatcher.GetCachedResults(typeof(ConfigurationValidator));

            Assert.AreEqual(expected, results.Count());
        }


    }
}
