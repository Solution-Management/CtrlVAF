using MFiles.VAF.Common;
using MFiles.VAF.Extensions.MultiServerMode;
using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CtrlVAF.BackgroundOperations
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

        protected override void StartApplication()
        {
            TaskQueueBackgroundOperationManager = new TaskQueueBackgroundOperationManager(
                this,
                this.GetType().FullName.Replace(".", "-") + "-BackgroundOperations"
                );

            var dispatcher = new BackgroundDispatcher();

            try
            {
                dispatcher.Dispatch(this);
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
