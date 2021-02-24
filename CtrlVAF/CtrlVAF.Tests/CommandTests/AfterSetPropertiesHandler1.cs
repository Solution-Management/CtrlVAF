using CtrlVAF.Events.Commands;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    public class AfterSetPropertiesHandler1 : IEventHandler<AfterSetPropertiesCommand<Configuration>>
    {
        public void Handle(AfterSetPropertiesCommand<Configuration> command)
        {
            command.Env.CurrentUserID = command.Configuration.ID;
        }
    }
}
