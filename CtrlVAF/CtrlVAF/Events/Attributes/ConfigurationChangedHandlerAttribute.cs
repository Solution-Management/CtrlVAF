using System;

namespace CtrlVAF.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ConfigurationChangedHandlerAttribute : Attribute
    {
        public ConfigurationChangedHandlerAttribute()
        {
        }
    }
}