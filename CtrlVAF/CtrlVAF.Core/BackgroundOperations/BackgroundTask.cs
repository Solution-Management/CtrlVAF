using MFiles.VAF.Common;
using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    abstract public class BackgroundTask<TConfig, TDirective> : IBackgroundTask 
                                                                where TDirective : TaskQueueDirective, new()
                                                                where TConfig : class, new()
    {
        public TConfig Configuration { get; set; }

        public void Task(TaskProcessorJob job, TaskQueueDirective directive)
        {
            if (directive != null && directive.GetType() != typeof(TDirective))
            {
                SysUtils.ReportErrorToEventLog(
                    "Background Dispatcher",
                    $"Received {directive?.GetType()?.FullName} but expected {typeof(TDirective).FullName}"
                    );
                return;
            }

            var newDirective = (TDirective)directive;

            Task(job, newDirective);
        }

        public abstract void Task(TaskProcessorJob job, TDirective directive);
    }
}
