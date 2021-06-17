using CtrlVAF.Models;

using MFiles.VAF.Common;

namespace CtrlVAF.Events
{
    public class EventCommand : ICtrlVAFCommand
    {
        public EventCommand(EventHandlerEnvironment env)
        {
            Env = env;
        }

        public EventHandlerEnvironment Env { get; }
    }
}