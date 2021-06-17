using CtrlVAF.Core;
using MFiles.VAF.Extensions;
using MFiles.VAF.MultiserverMode;

using MFilesAPI;

using System;

namespace CtrlVAF.BackgroundOperations
{
    public interface IBackgroundTaskHandler<TConfig, TDirective> : ICommandHandler<TConfig>
                                                                where TDirective : TaskQueueDirective, new()
                                                                where TConfig : class, new()
    {
        void Task(TaskProcessorJobEx job, TDirective directive, Action<string, MFTaskState> progressCallback);
    }
}