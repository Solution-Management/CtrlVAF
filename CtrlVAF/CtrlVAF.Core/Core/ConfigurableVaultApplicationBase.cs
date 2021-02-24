using CtrlVAF.BackgroundOperations;
using CtrlVAF.Events;
using CtrlVAF.Events.Commands;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Validators;

using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Configuration.AdminConfigurations;
using MFiles.VAF.Extensions.MultiServerMode;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


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

        public Dispatcher BackgroundDispatcher { get; private set; }

        public Dispatcher EventDispatcher { get; private set; }

        public Dispatcher<IEnumerable<ValidationFinding>> ValidatorDispatcher { get; private set; }

        internal TSecureConfiguration GetConfig()
        {
            return Configuration;
        }

        public override void StartOperations(Vault vaultPersistent)
        {
            BackgroundDispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);

            EventDispatcher = new EventDispatcher();

            ValidatorDispatcher = new ValidatorDispatcher();

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

        protected override IEnumerable<ValidationFinding> CustomValidation(Vault vault, TSecureConfiguration config)
        {
            var command = new ValidatorCommand<TSecureConfiguration> { Vault = vault, Configuration = config };

            var findings =  ValidatorDispatcher.Dispatch(command);

            return findings.Concat(base.CustomValidation(vault, config));
        }

        protected override void OnConfigurationUpdated(IConfigurationRequestContext context, ClientOperations clientOps, TSecureConfiguration oldConfiguration)
        {
            ValidatorDispatcher.ClearCache();

            base.OnConfigurationUpdated(context, clientOps, oldConfiguration);
        }

        
    }
}
