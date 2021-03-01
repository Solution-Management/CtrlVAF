using CtrlVAF.Events.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Events.Handlers;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    [EventCommandHandler(MFEventHandlerType.MFEventHandlerAfterCheckInChanges)]
    public class AfterCheckInChangesFailureHandler : EventHandler<Configuration, EventCommand>
    {
        public override void Handle(EventCommand command)
        {
            throw new NotImplementedException();
        }
    }
}
