using MFiles.VAF.MultiserverMode;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BackgroundOperationAttribute : Attribute
    {
        
        public BackgroundOperationAttribute(string name, bool showRunCommandInDashboard = false, bool showBackgroundOperationInDashboard = true)
        {
            Name = name;
            ShowRunCommandInDashboard = showRunCommandInDashboard;
            ShowBackgroundOperationInDashboard = showBackgroundOperationInDashboard;
        }

        public string Name { get; private set; }

        public bool ShowRunCommandInDashboard { get; private set; }

        public bool ShowBackgroundOperationInDashboard { get; private set; }
    }
}
