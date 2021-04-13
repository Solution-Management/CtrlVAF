using MFiles.VAF.Common;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Events.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class EventCommandHandlerAttribute: Attribute
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
