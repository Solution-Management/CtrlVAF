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
        
        public BackgroundOperationAttribute(string Name)
        {
            this.Name = Name;
        }

        public string Name { get; private set; }
    }
}
