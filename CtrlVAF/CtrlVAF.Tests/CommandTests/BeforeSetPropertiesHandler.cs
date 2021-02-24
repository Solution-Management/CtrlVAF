using CtrlVAF.Events.Commands;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{

    public class BeforeSetPropertiesHandler : IEventHandler<BeforeSetPropertiesCommand<Configuration>>
    {
        public void Handle(BeforeSetPropertiesCommand<Configuration> command)
        {
            command.Env.CurrentUserID = command.Configuration.ID;
        }
       
    }
}
