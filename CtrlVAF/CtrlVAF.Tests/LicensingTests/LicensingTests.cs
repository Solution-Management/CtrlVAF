using CtrlVAF.Events;
using CtrlVAF.Core;

using MFiles.VAF.Configuration;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Collections.Generic;

namespace CtrlVAF.Tests.LicensingTests
{
    [TestClass]
    public class LicensingTests
    {
        [TestMethod]
        public void WithoutLicensedDispatcher_AllClassesAreLoaded()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5 * 7;

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_InvalidLicense()
        {
            //Expects only the unlicensed class to be handled
            int expextedResult = 2; //3, 5 and 7 are not handled

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(false);

            dispatcher = new LicensedDispatcher(dispatcher, licenseContent);

            dispatcher.Dispatch(commands: command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_NoModules()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5 * 7;

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            dispatcher = new LicensedDispatcher(dispatcher, licenseContent);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_Module1()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5;

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module1" };

            dispatcher = new LicensedDispatcher(dispatcher, licenseContent);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_Module2()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 7;

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module2" };

            dispatcher = new LicensedDispatcher(dispatcher, licenseContent);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_BothModules()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5 * 7;

            TestLicenseCommand command = new TestLicenseCommand
            {
                Env = new MFiles.VAF.Common.EventHandlerEnvironment(),
                Configuration = new Configuration(),
                Result = 1
            };

            Dispatcher dispatcher = new EventDispatcher();

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module1","Module2" };

            dispatcher = new LicensedDispatcher(dispatcher, licenseContent);

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }
    }
}
