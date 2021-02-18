<<<<<<< Updated upstream
﻿using System;
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

            var dispatcher = new CommandDispatcher();
            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
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

            var dispatcher = new CommandDispatcher();
            var command = new AfterSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);
        }

        [TestMethod]
        public void AssertThat_ChangesMadeInMultipleTypesOfHandlers_PropagateToEnvironment()
        {
            var expectedID = 1234;
            var expectedName = "Tester";

            var expectedID2 = 4321;
            var expectedName2 = "Tester";

            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            var command = new AfterSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expectedID, environment.CurrentUserID);
            Assert.AreEqual(expectedName, environment.Input);

            var conf2 = new Configuration() { Name = "Tester", ID = 4321 };
            var command2 = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf2 };
            dispatcher.Dispatch(command2);

            Assert.AreEqual(expectedID2, environment.CurrentUserID);
            Assert.AreEqual(expectedName2, environment.Input);
        }


        [TestMethod]
        public void AssertThat_FailuresInDefaultDispatchMethod_ThrowsNoException()
        {
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);
=======
﻿using System;
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

            Dispatcher<object> dispatcher = new CommandDispatcher<BeforeSetPropertiesCommand<Configuration>>(command);

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

            Dispatcher<object> dispatcher = new CommandDispatcher<AfterSetPropertiesCommand<Configuration>>(command);
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
            Dispatcher<object> dispatcher = new CommandDispatcher<AfterCheckInChangesCommand<Configuration>>(command);
            dispatcher.Dispatch();
>>>>>>> Stashed changes

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionHandlingDispatcher_ThrowsException()
        {
<<<<<<< Updated upstream
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch(command, false, (e) => throw e);
            });
=======
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Dispatcher<object> dispatcher = new CommandDispatcher<AfterCheckInChangesCommand<Configuration>>(
                command,
                false,
                (e) => throw e
                );

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch();
            });
>>>>>>> Stashed changes
        }

        [TestMethod]
        public void AssertThat_FailuresInExceptionThrowingDispatcher_ThrowsException()
        {
<<<<<<< Updated upstream
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch(command, true);
            });
        }

        [TestMethod]
        public void SpeedTest_50000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();
            var dispatcher = new CommandDispatcher();
            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };

            for (int i = 0; i < 50000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_100000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();
            var dispatcher = new CommandDispatcher();
            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_NotAdded_NoChanges()
        {
            var expected = 0;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_Added_Changes()
        {
            var expected = 1234;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void AssertThat_HandlerInDifferentAssembly_AddedType_Changes()
        {
            var expected = 1234;

            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();

            var dispatcher = new CommandDispatcher();
            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };
            dispatcher.Dispatch(command);

            Assert.AreEqual(expected, environment.CurrentUserID);
        }

        [TestMethod]
        public void SpeedTest_AdditionalAssembly_100000Calls()
        {
            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var dispatcher = new CommandDispatcher();
            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch(command);
            }

            Assert.IsTrue(true);
        }
    }
}
=======
            var conf = new Configuration() { };
            var environment = new EventHandlerEnvironment();

            var command = new AfterCheckInChangesCommand<Configuration>() { Env = environment, Configuration = conf };

            Dispatcher<object> dispatcher = new CommandDispatcher<AfterCheckInChangesCommand<Configuration>>(command, true);
            

            Assert.ThrowsException<NotImplementedException>(() =>
            {
                dispatcher.Dispatch();
            });
        }
    }
}
>>>>>>> Stashed changes
