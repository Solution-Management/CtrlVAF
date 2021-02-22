using System;
using CtrlVAF.Commands;
using CtrlVAF.Commands.Commands;
using CtrlVAF.Core;
using CtrlVAF.Additional;

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

            var dispatcher = new EventDispatcher();

            dispatcher.Dispatch(command);

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

            Dispatcher dispatcher = new EventDispatcher();

            dispatcher.Dispatch(command);

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);
        }

        //[TestMethod]
        //public void AssertThat_FailuresInDefaultDispatchMethod_ThrowsNoException()
        //{
        //    var conf = new Configuration() { };
        //    var environment = new EventHandlerEnvironment();

        //    var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
        //    Dispatcher dispatcher = new CommandDispatcher();

        //    dispatcher.Dispatch(command);

        //    Assert.IsTrue(true);
        //}

        [TestMethod]
        public void AssertThat_FailuresInExceptionHandlingDispatcher_ThrowsException()
        {

            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Dispatcher dispatcher = new EventDispatcher();
                

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch((e) => throw e, command);
            });
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionThrowingDispatcher_ThrowsException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
            var dispatcher = new EventDispatcher();

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch(command);
            });
        }

        

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_NotAdded_NoChanges()
        {
            var expected = 0;

            var conf = new Additional.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeCheckInChangesCommand<Additional.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new EventDispatcher();

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_Added_Changes()
        {
            var expected = 1234;

            var conf = new Additional.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeCheckInChangesCommand<Additional.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new EventDispatcher();

            dispatcher.IncludeAssemblies(typeof(Additional.TestConfiguration).Assembly);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_AddedType_Changes()
        {
            var expected = 1234;

            var conf = new Additional.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();

            
            var command = new BeforeCheckInChangesCommand<Additional.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new EventDispatcher();

            dispatcher.IncludeAssemblies(typeof(Additional.TestConfiguration).Assembly);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_MultipleCommandsWereCalled()
        {
            int expected = 11;

            var Conf = new Configuration();

            EventHandlerEnvironment Env = new EventHandlerEnvironment { CurrentUserID = 0 };

            var dispatcher = new EventDispatcher();

            var command_1 = new CustomCommand_1 { Configuration = Conf, Env = Env };

            var command_2 = new CustomCommand_2 { Configuration = Conf, Env = Env };

            dispatcher.Dispatch(command_1, command_2);

            Assert.AreEqual(expected, Env.CurrentUserID);
        }

        
    }
}

