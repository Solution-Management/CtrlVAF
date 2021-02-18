using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RecurringAttribute : Attribute
    {
        
        public RecurringAttribute(int IntervalInMinutes)
        {
            this.IntervalInMinutes = IntervalInMinutes;
        }

        /// <summary>
        /// Interval for the permanent background operation
        /// </summary>
        public int IntervalInMinutes { get; private set; } = 10;
    }
}
