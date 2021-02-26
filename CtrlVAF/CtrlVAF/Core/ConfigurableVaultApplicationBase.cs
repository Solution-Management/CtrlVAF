using CtrlVAF.BackgroundOperations;
using CtrlVAF.Events;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Validation;

using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Configuration.AdminConfigurations;
using MFiles.VAF.Extensions.MultiServerMode;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;

namespace CtrlVAF.Core
{
    public abstract partial class ConfigurableVaultApplicationBase<TSecureConfiguration> :
        MFiles.VAF.Extensions.MultiServerMode.ConfigurableVaultApplicationBase<TSecureConfiguration> where TSecureConfiguration : class, new()
    {
        public TaskQueueBackgroundOperationManager TaskQueueBackgroundOperationManager { get; set; }

        public RecurringBackgroundOperations RecurringBackgroundOperations { get; }
             = new RecurringBackgroundOperations();

        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; }
             = new OnDemandBackgroundOperations();

        public Dispatcher BackgroundDispatcher { get; protected set; }

        public Dispatcher EventDispatcher { get; protected set; }

        public Dispatcher<IEnumerable<ValidationFinding>> ValidatorDispatcher { get; protected set; }

        public ConcurrentDictionary<Type, ValidationResults> ValidationResults { get; internal set; } = new ConcurrentDictionary<Type, ValidationResults>();

        internal TSecureConfiguration GetConfig()
        {
            return Configuration;
        }

        public override void StartOperations(Vault vaultPersistent)
        {
            BackgroundDispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);

            EventDispatcher = new EventDispatcher<TSecureConfiguration>(this);

            ValidatorDispatcher = new ValidatorDispatcher<TSecureConfiguration>(this);

            if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
            {
                var content = License?.Content<LicenseContentBase>();

                BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, content);

                EventDispatcher = new LicensedDispatcher(EventDispatcher, content);

                ValidatorDispatcher = new LicensedDispatcher<IEnumerable<ValidationFinding>>(ValidatorDispatcher, content);
            }

            base.StartOperations(vaultPersistent);
        }

        /// <summary>
        /// Registers background operations
        /// </summary>
        protected override void StartApplication()
        {
            TaskQueueBackgroundOperationManager = new TaskQueueBackgroundOperationManager(
                this,
                this.GetType().FullName.Replace(".", "-") + " - BackgroundOperations"
                );

            try
            {
                BackgroundDispatcher.Dispatch();
            }
            catch(Exception e)
            {
                SysUtils.ReportErrorMessageToEventLog("Could not dispatch the background operations.", e);
                return;
            }

            base.StartApplication();
        }

        public virtual ValidationCommand AddCustomValidationCommand(Vault vault)
        {
            return null;
        }

        protected override IEnumerable<ValidationFinding> CustomValidation(Vault vault, TSecureConfiguration config)
        {
            var command = new ValidationCommand(vault);

            var customCommand = AddCustomValidationCommand(vault);

            var findings =  ValidatorDispatcher.Dispatch(command, customCommand);

            if (findings == null)
                return base.CustomValidation(vault, config);

            return findings.Concat(base.CustomValidation(vault, config));
        }

        protected override void OnConfigurationUpdated(IConfigurationRequestContext context, ClientOperations clientOps, TSecureConfiguration oldConfiguration)
        {
            ValidatorDispatcher.ClearCache();
            ValidationResults = new ConcurrentDictionary<Type, ValidationResults>();

            base.OnConfigurationUpdated(context, clientOps, oldConfiguration);
        }
    }
}
