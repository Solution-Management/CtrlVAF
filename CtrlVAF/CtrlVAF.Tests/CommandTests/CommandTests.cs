using System;
using CtrlVAF.Commands;
using CtrlVAF.Commands.Commands;
using MFiles.VAF.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
            CommandDispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_ChangesMadeInMultipleHandlers_PropagateToEnvironment()
        {
            var expectedID = 1234;
            var expectedName = "Tester";

            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var command = new AfterSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
            CommandDispatcher.Dispatch(command);

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);
        }

        [TestMethod]
        public void AssertThat_FailuresInDefaultDispatchMethod_ThrowsNoException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
            CommandDispatcher.Dispatch(command);

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionHandlingDispatcher_ThrowsException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                CommandDispatcher.Dispatch(command, false, (e) => throw e);
            });
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionThrowingDispatcher_ThrowsException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                CommandDispatcher.Dispatch(command, true);
            });
        }
    }
}
