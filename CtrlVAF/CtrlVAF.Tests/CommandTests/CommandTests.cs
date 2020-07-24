using System;
using CtrlVAF.Commands;
using CtrlVAF.Commands.Commands;
using MFiles.VAF.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CtrlVAF.Tests.CommandTests
{
    [TestClass]
    public class CommandTests
    {
        [TestMethod]
        public void AssertThat_ChangesMadeInHandler_PropagateToEnvironment()
        {
            var expected = 1234;

            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new Dispatcher();
            dispatcher.Dispatch(new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf });

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_ChangesMadeInMultipleHandlers_PropagateToEnvironment()
        {
            var expectedID = 1234;
            var expectedName = "Tester";

            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new Dispatcher();
            var command = new AfterSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);
        }
    }
}
