using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{

    public class BeforeSetPropertiesHandler : CommandHandler<BeforeSetPropertiesCommand<Configuration>>
    {
        public override void Handle(BeforeSetPropertiesCommand<Configuration> command)
        {
            command.Env.CurrentUserID = command.Configuration.ID;
        }
    }
}
