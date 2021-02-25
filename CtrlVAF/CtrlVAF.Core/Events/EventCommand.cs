using CtrlVAF.Models;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Events
{
    public class EventCommand: ICtrlVAFCommand
    {
        public EventCommand(EventHandlerEnvironment env)
        {
            Env = env;
        }

        public EventHandlerEnvironment Env { get; }
    }
}
