using CtrlVAF.Core;

using MFiles.VAF.Common;
using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    public interface IBackgroundTaskHandler<TConfig, TDirective> : ICommandHandler<TConfig>
                                                                where TDirective : TaskQueueDirective, new()
                                                                where TConfig : class, new()
    {
        void Task(TaskProcessorJob job, TDirective directive);
    }

    public abstract class BackgroundTaskHandler<TConfig, TDirective>: IBackgroundTaskHandler<TConfig, TDirective>
                                                                where TDirective : TaskQueueDirective, new()
                                                                where TConfig : class, new()
    {
        public TConfig Configuration { get; }

        public abstract void Task(TaskProcessorJob job, TDirective directive);
    }
}
