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

            var va = Helpers.InitializeTestVA(new Configuration());

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            var dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_InvalidLicense()
        {
            //Expects only the unlicensed class to be handled
            int expextedResult = 2; //3, 5 and 7 are not handled

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(false);

            var va = Helpers.InitializeLicensedTestVA(new Configuration(), licenseContent);

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_NoModules()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5 * 7;

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            var va = Helpers.InitializeLicensedTestVA(new Configuration(), licenseContent);

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_Module1()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5;

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module1" };

            var va = Helpers.InitializeLicensedTestVA(new Configuration(), licenseContent);

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_Module2()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 7;

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module2" };

            var va = Helpers.InitializeLicensedTestVA(new Configuration(), licenseContent);

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }

        [TestMethod]
        public void LicensedDispatcher_ValidLicense_BothModules()
        {
            //Expects all classes to be handled
            int expextedResult = 2 * 3 * 5 * 7;

            TestLicenseContent licenseContent = new TestLicenseContent();

            licenseContent.SetValidity(true);

            licenseContent.Modules = new List<string> { "Module1", "Module2" };

            var va = Helpers.InitializeLicensedTestVA(new Configuration(), licenseContent);

            var env = va.CreateEventHandlerEnvironment(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault);

            TestLicenseCommand command = new TestLicenseCommand(env)
            {
                Result = 1
            };

            Dispatcher dispatcher = va.EventDispatcher;

            dispatcher.Dispatch(command);

            Assert.AreEqual(expextedResult, command.Result);
        }
    }
}
