using MFiles.VAF.Extensions.MultiServerMode;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CtrlVAF.BackgroundOperations
{
    public class RecurringBackgroundOperations : IEnumerable<string>
    {
        private Dictionary<string, OperationInfo> BackgroundOperations { get; set; }
        = new Dictionary<string, OperationInfo>();

        public bool ContainsOperation(string name)
        {
            return BackgroundOperations.Keys.Contains(name);
        }

        public TaskQueueBackgroundOperation GetOperation(string name)
        {
            if (BackgroundOperations.ContainsKey(name))
                return BackgroundOperations[name].Operation;
            return null;
        }

        internal void AddBackgroundOperation(string name, TaskQueueBackgroundOperation operation, TimeSpan interval)
        {
            if (BackgroundOperations.ContainsKey(name))
                throw new InvalidOperationException("");

            BackgroundOperations.Add(name, new OperationInfo
            {
                Operation = operation,
                Interval = interval
            });

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
            public TimeSpan Interval { get; set; }
        }


    }
}
