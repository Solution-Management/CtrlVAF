using CtrlVAF.Events.Attributes;
using CtrlVAF.Events;
using CtrlVAF.Events.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Additional
{
    public class TestConfiguration
    {
        public int id = 50;
    }

    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeCreateNewObjectFinalize)]
    public class TestHandler : EventHandler<TestConfiguration, EventCommand>
    {
        public override void Handle(EventCommand command)
        {
            command.Env.CurrentUserID = Configuration.id;
        }
    }
}
