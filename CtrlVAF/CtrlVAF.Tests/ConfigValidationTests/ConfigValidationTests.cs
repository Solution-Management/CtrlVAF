
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

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);
            dispatcher.IncludeAssemblies(typeof(Configuration));


            var results = dispatcher.Dispatch();

            Assert.AreEqual(expected, results.Count());
        }

        [TestMethod]
        public void Assert_ChildConfiguration()
        {
            var expected = 1;

            var vault = new MFilesAPI.Vault();

            Configuration config = new Configuration
            {
                Name = "name",
                ID = 42,
                ChildConfig = new Child_Configuration
                {
                    Name = ""
                }
            };

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);
            dispatcher.IncludeAssemblies(typeof(Configuration));

            var results = dispatcher.Dispatch();

            Assert.AreEqual(expected, results.Count());
        }
    }
}
