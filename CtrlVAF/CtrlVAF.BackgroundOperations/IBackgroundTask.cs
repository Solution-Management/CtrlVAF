using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    interface IBackgroundTask
    {
        object Config { set; }

        void Task(TaskProcessorJob job, TaskQueueDirective directive);
    }

    
}
