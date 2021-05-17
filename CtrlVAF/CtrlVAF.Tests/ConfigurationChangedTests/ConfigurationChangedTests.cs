using CtrlVAF.Core;
using CtrlVAF.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.ConfigurationChangedTests
{
    [TestClass]
    public class ConfigurationChangedTests
    {

        [TestMethod]
        public void AssertThat_ConfigurationEventsPropagateToHandlers()
        {
            var oldConf = new Configuration() { Name = "Tester", ID = 1234 };
            var conf = new Configuration() { Name = "Tester", ID = 5678 };
            var va = Helpers.InitializeTestVA(conf);
            var command = new ConfigurationChangedCommand(null, null, oldConf);
            
            Dispatcher dispatcher = va.ConfigurationDistpacher;
            dispatcher.Dispatch(command);

            Assert.AreEqual(true, conf.Test);
        }

        [TestMethod]
        public void AssertThat_NoChange()
        {
            var oldConf = new Configuration() { Name = "Tester", ID = 1234 };
            var conf = new Configuration() { Name = "Tester", ID = 1234 };
            var va = Helpers.InitializeTestVA(conf);
            var command = new ConfigurationChangedCommand(null, null, oldConf);

            Dispatcher dispatcher = va.ConfigurationDistpacher;
            dispatcher.Dispatch(command);

            Assert.AreEqual(false, conf.Test);
        }

        [TestMethod]
        public void AssertThat_PreviousNullConfiguration_ReturnsAsChange()
        {
            var oldConf = new Configuration() { Name = "Tester", ID = 1234 };
            var conf = new Configuration() { Name = "Tester", ID = 1234, ChildConfig = new Child_Configuration() { Name = "James" } };
            var va = Helpers.InitializeTestVA(conf);
            var command = new ConfigurationChangedCommand(null, null, oldConf);

            Dispatcher dispatcher = va.ConfigurationDistpacher;
            dispatcher.Dispatch(command);

            Assert.AreEqual(true, conf.ChildConfig.Test);
        }

        [TestMethod]
        public void AssertThat_NewNullConfiguration_ReturnsAsChange()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234, ChildConfig = new Child_Configuration() };
            var oldConf = new Configuration() { Name = "Tester", ID = 1234, ChildConfig = new Child_Configuration() { Name = "James" } };
            var va = Helpers.InitializeTestVA(conf);
            var command = new ConfigurationChangedCommand(null, null, oldConf);

            Dispatcher dispatcher = va.ConfigurationDistpacher;
            dispatcher.Dispatch(command);

            Assert.AreEqual(true, conf.ChildConfig.Test);
        }

        [TestMethod]
        public void AssertThat_BothNullConfiguration_ReturnsAsNoChange()
        {
            var conf = new Configuration() { Name = "Tester", ID = 1234, ChildConfig = new Child_Configuration() };
            var oldConf = new Configuration() { Name = "Tester", ID = 1234, ChildConfig = new Child_Configuration() };
            var va = Helpers.InitializeTestVA(conf);
            var command = new ConfigurationChangedCommand(null, null, oldConf);

            Dispatcher dispatcher = va.ConfigurationDistpacher;
            dispatcher.Dispatch(command);

            Assert.AreEqual(false, conf.ChildConfig.Test);
        }
    }
}
