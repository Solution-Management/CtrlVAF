using MFilesAPI;

using System;

namespace CtrlVAF.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EventCommandHandlerAttribute : Attribute
    {
        internal readonly MFEventHandlerType MFEvent = MFEventHandlerType.MFEventHandlerTypeUndefined;

        public EventCommandHandlerAttribute(MFEventHandlerType MFEvent)
        {
            this.MFEvent = MFEvent;
        }

        public bool EventHandlerTypeMatches(MFEventHandlerType eventHandlerType)
        {
            return MFEvent == eventHandlerType;
        }
    }
}