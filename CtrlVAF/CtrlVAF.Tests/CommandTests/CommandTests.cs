using System;
using CtrlVAF.Events;
using CtrlVAF.Core;
using CtrlVAF.Additional;

using MFiles.VAF.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using CtrlVAF.Validation;
using MFilesAPI;

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
            var va = Helpers.InitializeTestVA(conf);

            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeSetProperties);
            var command = new EventCommand(environment);

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
            var va = Helpers.InitializeTestVA(conf);

            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterSetProperties);

            var command = new EventCommand(environment);


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
            var va = Helpers.InitializeTestVA(conf);

            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCheckInChanges);

            var command = new EventCommand(environment);

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
            var va = Helpers.InitializeTestVA(conf);

            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCheckInChanges);

            var command = new EventCommand(environment);

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
            var va = Helpers.InitializeTestVA(conf);
            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize);
            var command = new EventCommand(environment);

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_Added_Changes()
        {
            var expected = 1234;

            var conf = new Additional.TestConfiguration() { id = 1234 };
            var va = Helpers.InitializeTestVA(conf);
            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize);
            var command = new EventCommand(environment);

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
            var va = Helpers.InitializeTestVA(conf);
            var environment = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize);
            var command = new EventCommand(environment);

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.IncludeAssemblies(typeof(Additional.TestConfiguration));

            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_CustomCommandWasHandled()
        {
            string expected = "Tester";

            var Conf = new Configuration();
            var va = Helpers.InitializeTestVA(Conf);
            var Env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateView);

            Dispatcher dispatcher = va.EventDispatcher;

            var command_1 = new CustomCommand_1(Env) { Name = expected };

            dispatcher.Dispatch(command_1);

            Assert.AreEqual(expected, Env.Input);
        }

        [TestMethod]
        public void AssertThat_MultipleCommandsWereCalled()
        {
            int expectedID = 1234;
            string expectedName = "Tester";

            var Conf = new Configuration();
            var va = Helpers.InitializeTestVA(Conf);
            var Env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateView);

            Dispatcher dispatcher = va.EventDispatcher;

            var command_1 = new CustomCommand_1(Env) { Name = expectedName };

            var command_2 = new CustomCommand_2(Env) { ID = expectedID };

            dispatcher.Dispatch(command_1, command_2);

            Assert.AreEqual(expectedID, Env.CurrentUserID);
            Assert.AreEqual(expectedName, Env.Input);
        }

        [TestMethod]
        public void AssertThat_HandlerWasCalledTwice_DifferentEvent_TwoCalls()
        {
            //The same handler can be called from two different events
            int expected = 15;

            var Conf = new Configuration();
            var va = Helpers.InitializeTestVA(Conf);
            Dispatcher dispatcher = va.EventDispatcher;

            var Env_BeforeCheckout = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCheckOut);

            var command_3_BeforeCheckOut = new CustomCommand_3(Env_BeforeCheckout) { AddValue = 10 };

            dispatcher.Dispatch(command_3_BeforeCheckOut);

            //Execute the same call with a different EventType
            var Env_AfterCheckOut = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCheckOut);
            Env_AfterCheckOut.CurrentUserID = Env_BeforeCheckout.CurrentUserID;

            var command_3_AfterCheckOut = new CustomCommand_3(Env_AfterCheckOut) { AddValue = 5 };

            dispatcher.Dispatch(command_3_AfterCheckOut);

            Assert.AreEqual(expected, Env_AfterCheckOut.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerWasCalledOnce_DifferentEvent_OneCall()
        {
            //Because this combination of command and event type leads to the same event handler, the event handler is only handled once
            int expectedChanged = 10;
            int expectedUnchanged = 0;

            var Conf = new Configuration();
            var va = Helpers.InitializeTestVA(Conf);
            Dispatcher dispatcher = va.EventDispatcher;

            var Env_BeforeCheckout = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCheckOut);
            var command_3_BeforeCheckOut = new CustomCommand_3(Env_BeforeCheckout) { AddValue = 10 };

            var Env_AfterCheckOut = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCheckOut);
            Env_AfterCheckOut.CurrentUserID = Env_BeforeCheckout.CurrentUserID;

            var command_3_AfterCheckOut = new CustomCommand_3(Env_AfterCheckOut) { AddValue = 5 };

            dispatcher.Dispatch(command_3_BeforeCheckOut, command_3_AfterCheckOut);

            Assert.AreEqual(expectedChanged, Env_BeforeCheckout.CurrentUserID);
            Assert.AreEqual(expectedUnchanged, Env_AfterCheckOut.CurrentUserID);
        }


    }
}

