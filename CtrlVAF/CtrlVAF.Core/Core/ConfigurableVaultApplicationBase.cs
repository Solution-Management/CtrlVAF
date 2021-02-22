using CtrlVAF.BackgroundOperations;
using CtrlVAF.Commands;
using CtrlVAF.Core.Attributes;
using CtrlVAF.Validators;

using MFiles.VAF.Common;
using MFiles.VAF.Configuration;
using MFiles.VAF.Extensions.MultiServerMode;

using System;
using System.Reflection;


namespace CtrlVAF.Core
{
    public abstract class ConfigurableVaultApplicationBase<TSecureConfiguration> :
        MFiles.VAF.Extensions.MultiServerMode.ConfigurableVaultApplicationBase<TSecureConfiguration> where TSecureConfiguration : class, new()
    {
        public TaskQueueBackgroundOperationManager TaskQueueBackgroundOperationManager { get; set; }

        public RecurringBackgroundOperations PermanentBackgroundOperations { get; }
             = new RecurringBackgroundOperations();

        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; }
             = new OnDemandBackgroundOperations();

        public Dispatcher BackgroundDispatcher { get; private set; }

        public Dispatcher EventDispatcher { get; private set; }

        internal TSecureConfiguration GetConfig()
        {
            return Configuration;
        }

        public ConfigurableVaultApplicationBase(){
            BackgroundDispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);

            EventDispatcher = new EventDispatcher();

            if (this.GetType().IsDefined(typeof(UseLicensingAttribute)))
            {
                var content = License?.Content<LicenseContentBase>();

                BackgroundDispatcher = new LicensedDispatcher(BackgroundDispatcher, content);

                EventDispatcher = new LicensedDispatcher(EventDispatcher, content);
            }
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
    }
}
