using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;

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

    public class TestHandler : IEventHandler<BeforeCheckInChangesCommand<TestConfiguration>>
    {
        public void Handle(BeforeCheckInChangesCommand<TestConfiguration> command)
        {
            command.Env.CurrentUserID = command.Configuration.id;
        }
    }
}
