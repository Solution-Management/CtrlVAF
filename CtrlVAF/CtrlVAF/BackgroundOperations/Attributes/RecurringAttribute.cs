using CtrlVAF.Core.Models;
using MFiles.VAF.Common;
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
        /// <summary>
        /// Sets the interval of the background operation using the interval and interval kind.
        /// Setting it to 1.5 days for instance will set the interval to 129600 seconds (Equal to the seconds in 1.5 days)
        /// NOTE: There is a minimum interval currently set to 30 seconds. Any intervals below this will default to a 10 minute interval.
        /// </summary>
        /// <param name="interval">The interval number</param>
        /// <param name="intervalKind">The kind of interval (seconds, minutes, hours, days)</param>
        public RecurringAttribute(double interval, IntervalKind intervalKind)
        {
            IntervalInSeconds = IntervalToSeconds(interval, intervalKind);
        }

        /// <summary>
        /// Sets the interval of the background operation in minutes.
        /// </summary>
        /// <param name="minutesInterval">The interval in minutes</param>
        public RecurringAttribute(double minutesInterval)
        {
            IntervalInSeconds = IntervalToSeconds(minutesInterval, IntervalKind.Minutes);
        }

        /// <summary>
        /// Interval for the permanent background operation
        /// </summary>
        public int IntervalInSeconds { get; private set; } = 600;
        public bool debug = false;

        public int IntervalToSeconds(double interval, IntervalKind intervalKind)
        {
            if (interval <= 0)
            {
                if (!debug)
                {
                    SysUtils.ReportErrorToEventLog($"Invalid interval of {interval} {intervalKind} used for background job. Using 10 Minute interval instead.");
                }
                return 600;
            }
            int seconds;

            switch (intervalKind)
            {
                case IntervalKind.Seconds:
                    seconds = (int)TimeSpan.FromSeconds(interval).TotalSeconds;
                    break;
                case IntervalKind.Minutes:
                    seconds = (int)TimeSpan.FromMinutes(interval).TotalSeconds;
                    break;
                case IntervalKind.Hours:
                    seconds = (int)TimeSpan.FromHours(interval).TotalSeconds;
                    break;
                case IntervalKind.Days:
                    seconds = (int)TimeSpan.FromDays(interval).TotalSeconds;
                    break;
                default:
                    // Default to minutes
                    seconds = (int)TimeSpan.FromMinutes(interval).TotalSeconds;
                    break;
            }

            if (seconds < 30.0)
            {
                if (!debug)
                {
                    SysUtils.ReportErrorToEventLog($"Too low interval seconds of {seconds} calculated from interval of {interval} {intervalKind} for background job. Using 10 Minute interval instead.");
                }
                return 600;
            }
            return seconds;
        }
    }
}
