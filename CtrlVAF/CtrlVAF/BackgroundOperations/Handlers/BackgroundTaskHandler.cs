using CtrlVAF.Core;
using CtrlVAF.Validation;

using MFiles.VAF.MultiserverMode;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    public abstract class BackgroundTaskHandler<TConfig, TDirective> : BackgroundTaskHandler, IBackgroundTaskHandler<TConfig, TDirective>
                                                                where TDirective : TaskQueueDirective, new()
                                                                where TConfig : class, new()
    {
        public TConfig Configuration { get; internal set; }

        public abstract void Task(TaskProcessorJob job, TDirective directive);
    }

    public abstract class BackgroundTaskHandler : ICommandHandler
    {
        public Vault PermanentVault { get; internal set; }
        public ValidationResults ValidationResults { get; internal set; }
        public OnDemandBackgroundOperations OnDemandBackgroundOperations { get; internal set; }
        public RecurringBackgroundOperations RecurringBackgroundOperations { get; internal set; }

    }
}
