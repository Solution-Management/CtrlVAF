using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LicenseRequiredAttribute: Attribute
    {
        public string[] Modules { get; set; }
    }
}
