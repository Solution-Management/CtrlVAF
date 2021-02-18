using CtrlVAF.Core;
using CtrlVAF.Validators;

using MFiles.VAF.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections;
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

            //Runs ICustomValidator.Validate(vault, config) on all classes that implement CustomValidator<T> 
            //where T is the configuration class or a class used by it's members.
<<<<<<< Updated upstream
            var dispatcher = new ValidationDispatcher();
            dispatcher.IncludeAssemblies(typeof(Configuration));
            var results = dispatcher.Dispatch(new MFilesAPI.Vault(), new Configuration {Name = "", ID = 42 });
=======
            var vault = new MFilesAPI.Vault();
            var config = new Configuration { Name = "", ID = 42 };

            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);

            var results = dispatcher.Dispatch();
>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
            var dispatcher = new ValidationDispatcher();
            dispatcher.IncludeAssemblies(typeof(Configuration));
            var results = dispatcher.Dispatch(new MFilesAPI.Vault(), config);
=======
            Dispatcher<IEnumerable<ValidationFinding>> dispatcher = new ValidatorDispatcher(vault, config);

            var results = dispatcher.Dispatch();
>>>>>>> Stashed changes

            Assert.AreEqual(expected, results.Count());
        }
    }
}
