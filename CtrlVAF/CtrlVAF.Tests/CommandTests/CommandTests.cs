using System;
using CtrlVAF.Commands;
using CtrlVAF.Commands.Commands;
using CtrlVAF.Core;

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

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.Dispatch();

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

            Dispatcher<object> dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.Dispatch();

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);
        }

        [TestMethod]
        public void AssertThat_FailuresInDefaultDispatchMethod_ThrowsNoException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
            Dispatcher<object> dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.Dispatch();

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionHandlingDispatcher_ThrowsException()
        {

            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Dispatcher<object> dispatcher = new CommandDispatcher(
               false,
                (e) => throw e
                );

            dispatcher.AddCommand(command);

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch();
            });
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionThrowingDispatcher_ThrowsException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
            var dispatcher = new CommandDispatcher( true);

            dispatcher.AddCommand(command);

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch();
            });
        }

        

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_NotAdded_NoChanges()
        {
            var expected = 0;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.Dispatch();

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_Added_Changes()
        {
            var expected = 1234;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);

            dispatcher.Dispatch();

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_AddedType_Changes()
        {
            var expected = 1234;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();

            
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);

            dispatcher.Dispatch();

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_MultipleCommandsWereCalled()
        {
            int expected = 11;

            var Conf = new Configuration();

            EventHandlerEnvironment Env = new EventHandlerEnvironment { CurrentUserID = 0 };

            var dispatcher = new CommandDispatcher();

            var command_1 = new CustomCommand_1 { Configuration = Conf, Env = Env };

            var command_2 = new CustomCommand_2 { Configuration = Conf, Env = Env };


            dispatcher.AddCommand(command_1);

            dispatcher.AddCommand(command_2);

            dispatcher.Dispatch();

            Assert.AreEqual(expected, Env.CurrentUserID);
        }

        
    }
}

