using CtrlVAF.Events.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterSetProperties)]
    public class AfterSetPropertiesHandler1 : EventHandler<Configuration, EventCommand>
    {
        public override void Handle(EventCommand command)
        {
            command.Env.CurrentUserID = Configuration.ID;
        }
    }
}
