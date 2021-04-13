using CtrlVAF.Events;
using CtrlVAF.Events.Attributes;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests.Subfolder
{
    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCheckInChanges)]
    class BeforeCheckinChangesHandler : EventHandler<Configuration, EventCommand>
    {
        public override void Handle(EventCommand command)
        {
            command.Env.Input = "BEFORECHECKINCHANGESHANDLER";
        }
    }
}
