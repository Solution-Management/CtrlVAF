﻿using CtrlVAF.Commands.Commands;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommand_1: IEventHandlerCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }

    class CustomCommand_2 : IEventHandlerCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }
}