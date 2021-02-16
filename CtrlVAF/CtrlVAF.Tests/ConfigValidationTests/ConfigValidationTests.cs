using CtrlVAF.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            var results = ValidationDispatcher.Dispatch(new MFilesAPI.Vault(), new Configuration {Name = "", ID = 42 });

            Assert.AreEqual(expected, results.Count());
        }

        [TestMethod]
        public void Assert_ChildConfiguration()
        {
            var expected = 1;

            Configuration config = new Configuration
            {
                Name = "name",
                ID = 42,
                ChildConfig = new Child_Configuration
                {
                    Name = ""
                }
            };

            var results = ValidationDispatcher.Dispatch(new MFilesAPI.Vault(), config);

            Assert.AreEqual(expected, results.Count());

        }
    }
}
