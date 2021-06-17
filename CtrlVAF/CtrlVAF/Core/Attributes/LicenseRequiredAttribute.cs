using System;

namespace CtrlVAF.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class LicenseRequiredAttribute : Attribute
    {
        public string[] Modules { get; set; }
    }
}