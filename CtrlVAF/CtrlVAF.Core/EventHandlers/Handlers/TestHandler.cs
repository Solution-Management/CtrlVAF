using CtrlVAF.Commands.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Commands.Handlers
{
    public class TestConfiguration
    {
        public int id = 0;
    }

    public class TestHandler : ICommandHandler<BeforeCheckInChangesCommand<TestConfiguration>>
    {
        public override void Handle(BeforeCheckInChangesCommand<TestConfiguration> command)
        {
            command.Env.CurrentUserID = command.Configuration.id;
        }
    }
}
