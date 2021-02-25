using CtrlVAF.Events.Handlers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommandHandler_1 : EventHandler<Configuration, CustomCommand_1>
    {
        public override void Handle(CustomCommand_1 command)
        {
            command.Env.CurrentUserID += 1;
        }
    }

    class CustomCommandHandler_2 : EventHandler<Configuration, CustomCommand_2>
    {
        public override void Handle(CustomCommand_2 command)
        {
            command.Env.CurrentUserID += 10;
        }
    }

    class CustomCommandHandler : EventHandler<Configuration, CustomCommand_3>
    {
        public override void Handle(CustomCommand_3 command)
        {
            command.Env.CurrentUserID += 10;
        }
    }
}
