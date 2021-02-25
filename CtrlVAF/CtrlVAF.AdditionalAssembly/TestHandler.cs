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
        public int id = 0;
    }

    public class TestHandler : EventHandler<TestConfiguration, EventCommand>
    {
        public override void Handle(EventCommand command)
        {
            command.Env.CurrentUserID = Configuration.id;
        }
    }
}
