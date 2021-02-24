using CtrlVAF.Events.Commands;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommand_1: IEventCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }

    class CustomCommand_2 : IEventCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }
    class CustomCommand_3 : IEventCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }

    class CustomCommand_4 : IEventCommand<Configuration>
    {
        public EventHandlerEnvironment Env { get; set; }

        public Configuration Configuration { get; set; }
    }
}
