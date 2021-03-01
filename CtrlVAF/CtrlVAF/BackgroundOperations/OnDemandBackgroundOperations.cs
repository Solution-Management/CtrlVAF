using MFiles.VAF.Extensions.MultiServerMode;
using MFiles.VAF.MultiserverMode;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    public class OnDemandBackgroundOperations: IEnumerable<string>
    {
        private Dictionary<string, OperationInfo> BackgroundOperations { get; set; }
            = new Dictionary<string, OperationInfo>();

        public bool ContainsOperation(string name)
        {
            return BackgroundOperations.Keys.Contains(name);
        }

        internal void AddBackgroundOperation(string name, TaskQueueBackgroundOperation operation)
        {
            BackgroundOperations.Add(name, new OperationInfo
            {
                Operation = operation
            });
        }

        public void RunOnce(string name, DateTime? runAt = null, TaskQueueDirective directive = null)
        {
            if (!BackgroundOperations.Keys.Contains(name))
                return;

            BackgroundOperations[name].Operation.RunOnce(runAt, directive);
        }

        public void RunAtIntervals(string name, TimeSpan interval, TaskQueueDirective directive = null)
        {
            if (!BackgroundOperations.Keys.Contains(name))
                return;

            BackgroundOperations[name].Operation.RunAtIntervals(interval, directive);
        }

        public void StopRunningAtIntervals(string name)
        {
            if (!BackgroundOperations.Keys.Contains(name))
                return;

            BackgroundOperations[name].Operation.StopRunningAtIntervals();
        }

        public IEnumerator<string> GetEnumerator()
        {
            return BackgroundOperations.Keys.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class OperationInfo
        {
            public TaskQueueBackgroundOperation Operation { get; set; }
        }
    }
}
