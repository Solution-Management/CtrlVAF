﻿using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    public class AfterCheckInChangesFailureHandler : ICommandHandler<AfterCheckInChangesCommand<Configuration>>
    {
        public override void Handle(AfterCheckInChangesCommand<Configuration> command)
        {
            throw new NotImplementedException();
        }
    }
}
