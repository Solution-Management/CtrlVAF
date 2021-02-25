using MFiles.VAF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Events.Commands
{
    public class AfterBeginTransactionCommand : IEventCommand
    {
        public EventHandlerEnvironment Env { get; set; }
    }
}
