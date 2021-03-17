using CtrlVAF.Core.Models;
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
        public RecurringAttribute(double interval, IntervalKind intervalKind)
        {
            IntervalInMinutes = IntervalToMinutes(interval, intervalKind);
        }

        public RecurringAttribute(int IntervalInMinutes)
        {
            this.IntervalInMinutes = IntervalInMinutes;
        }

        /// <summary>
        /// Interval for the permanent background operation
        /// </summary>
        public int IntervalInMinutes { get; private set; } = 10;

        public int IntervalToMinutes(double interval, IntervalKind intervalKind)
        {
            var minutes = 0;

            switch (intervalKind)
            {
                case IntervalKind.Minutes:
                    minutes = (int)TimeSpan.FromMinutes(interval).TotalMinutes;
                    break;
                case IntervalKind.Hours:
                    minutes = (int)TimeSpan.FromHours(interval).TotalMinutes;
                    break;
                case IntervalKind.Days:
                    minutes = (int)TimeSpan.FromDays(interval).TotalMinutes;
                    break;
                default:
                    // Default to minutes
                    minutes = (int)TimeSpan.FromMinutes(interval).TotalMinutes;
                    break;
            }

            return minutes;
        }
    }
}
