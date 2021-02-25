using System;
using CtrlVAF.Events;
using CtrlVAF.Core;
using CtrlVAF.Additional;

using MFiles.VAF.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

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
            var environment = new TestEventHandlerEnvironment() {EventType =  MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeSetProperties};

            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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

            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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

            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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

            var command = new EventCommand() { Env = environment };
            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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
            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_Added_Changes()
        {
            var expected = 1234;

            var conf = new Additional.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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


            var command = new EventCommand() { Env = environment };

            var va = Helpers.InitializeTestVA(conf);

            Dispatcher dispatcher = va.EventDispatcher;

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

            var va = Helpers.InitializeTestVA(Conf);

            Dispatcher dispatcher = va.EventDispatcher;

            var command_1 = new CustomCommand_1 {  Env = Env };

            var command_2 = new CustomCommand_1 { Env = Env };

            dispatcher.Dispatch(command_1, command_2);

            Assert.AreEqual(expected, Env.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerWasCalledTwice_WithDifferentCommands()
        {
            int expected = 20;

            var config = new Configuration();
            var env = new EventHandlerEnvironment { CurrentUserID = 0 };

            var command_3 = new CustomCommand_3 {  Env = env };

            var va = Helpers.InitializeTestVA(config);

            Dispatcher dispatcher = va.EventDispatcher;

            //The same handler is called twice, but in a different dispatch call, so it is executed twice
            dispatcher.Dispatch(command_3);

            Assert.AreEqual(expected, env.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerWasCalledOnce_WithDifferentCommands()
        {
            int expected = 10;

            var config = new Configuration();
            var env = new EventHandlerEnvironment { CurrentUserID = 0 };

            var command_3 = new CustomCommand_3 { Env = env };

            var va = Helpers.InitializeTestVA(config);

            Dispatcher dispatcher = va.EventDispatcher;

            //These commands call the same handler so the handler is only executed for the first command
            dispatcher.Dispatch(command_3);

            Assert.AreEqual(expected, env.CurrentUserID);
        }


    }
}

