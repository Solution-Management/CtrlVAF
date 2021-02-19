using CtrlVAF.Commands;
using CtrlVAF.Commands.Commands;

using MFiles.VAF.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;

namespace CtrlVAF.Tests.CommandTests
{
    [TestClass]
    public class SpeedTests
    {
        [TestMethod]
        public void SpeedTest_AdditionalAssembly_100000Calls()
        {
            var conf = new Commands.Handlers.TestConfiguration() { id = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeCheckInChangesCommand<Commands.Handlers.TestConfiguration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.IncludeAssemblies(typeof(Commands.Handlers.TestConfiguration).Assembly);

            dispatcher.AddCommand(command);

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_50000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();

            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            for (int i = 0; i < 50000; i++)
            {
                dispatcher.Dispatch();
            }

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void SpeedTest_100000Calls()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var environment = new EventHandlerEnvironment();
            var command = new BeforeSetPropertiesCommand<Configuration>() { Env = environment, Configuration = conf };

            var dispatcher = new CommandDispatcher();

            dispatcher.AddCommand(command);

            for (int i = 0; i < 100000; i++)
            {
                dispatcher.Dispatch();
            }

            Assert.IsTrue(true);
        }
    }


}
