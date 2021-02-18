using CtrlVAF.BackgroundOperations;
using CtrlVAF.Core.Attributes;

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

        internal TSecureConfiguration GetConfig()
        {
            return Configuration;
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

            Dispatcher<object> dispatcher = new BackgroundDispatcher<TSecureConfiguration>(this);

            if(this.GetType().IsDefined(typeof(UseLicensingAttribute)))
            {
                var content = License?.Content<LicenseContentBase>();

                dispatcher = new LicensedDispatcher<object>(dispatcher, content);
            }

            try
            {
                dispatcher.Dispatch();
            }
            catch(Exception e)
            {
                SysUtils.ReportErrorMessageToEventLog("Could not dispatch.", e);
                return;
            }

            base.StartApplication();
        }
    }
}
