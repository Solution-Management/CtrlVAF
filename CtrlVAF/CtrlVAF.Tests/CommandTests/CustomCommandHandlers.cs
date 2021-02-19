using CtrlVAF.Commands.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommandHandler_1 : ICommandHandler<CustomCommand_1>
    {
        public override void Handle(CustomCommand_1 command)
        {
            command.Env.CurrentUserID += 1;
        }
    }

    class CustomCommandHandler_2 : ICommandHandler<CustomCommand_2>
    {
        public override void Handle(CustomCommand_2 command)
        {
            command.Env.CurrentUserID += 10;
        }
    }
}
