using CtrlVAF.Events.Handlers;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommandHandler_1 : IEventHandler<CustomCommand_1>
    {
        public void Handle(CustomCommand_1 command)
        {
            command.Env.CurrentUserID += 1;
        }
    }

    class CustomCommandHandler_2 : IEventHandler<CustomCommand_2>
    {
        public void Handle(CustomCommand_2 command)
        {
            command.Env.CurrentUserID += 10;
        }
    }

    class CustomCommandHandler : IEventHandler<CustomCommand_3>, IEventHandler<CustomCommand_4>
    {
        public void Handle(CustomCommand_3 command)
        {
            command.Env.CurrentUserID += 10;
        }

        public void Handle(CustomCommand_4 command)
        {
            CustomCommand_3 cmd = new CustomCommand_3
            {
                Env = command.Env,
                Configuration = command.Configuration
            };

            Handle(cmd);
        }
    }
}
