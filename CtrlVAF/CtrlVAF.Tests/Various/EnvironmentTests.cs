using CtrlVAF.Core;
using CtrlVAF.Tests.CommandTests;
using CtrlVAF.Validation;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace CtrlVAF.Tests.Various
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestMethod]
        public void AssertThat_EnvironmentVariablesAreAvailable()
        {
            int expected = 111;

            var conf = new Configuration() { Name = "", ID = 1234 };
            var vault = new MFilesAPI.Vault();
            var va = Helpers.InitializeTestVA(conf);
            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterBringOnline);
            va.ValidatorDispatcher.Dispatch(new ValidationCommand(vault));

            var command = new TestEnvironmentCommand(env) { TestedEnvironmentProperties = 0 };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, command.TestedEnvironmentProperties);
        }

        [TestMethod]
        public void AssertThat_ChildValidationFindings_AreAvailable()
        {
            int expected = 10;

            var conf = new Configuration() 
            { 
                Name = "", 
                ID = 1234, 
                ChildConfig = new Child_Configuration 
                { 
                    Name = "", 
                    GrandChildConfig = new GrandChild_Configuration 
                    { 
                        Name = "" 
                    } 
                } 
            };
            var vault = new MFilesAPI.Vault();
            var va = Helpers.InitializeTestVA(conf);
            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCancelCheckout);
            va.ValidatorDispatcher.Dispatch(new ValidationCommand(vault));

            var command = new TestEnvironmentCommand(env) { TestedEnvironmentProperties = 0 };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, command.TestedEnvironmentProperties);
        }


    }
}
