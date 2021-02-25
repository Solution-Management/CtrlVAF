using CtrlVAF.Events;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.Various
{
    class TestEnvironmentCommand : EventCommand
    {
        public int TestedEnvironmentProperties = 0;

        public TestEnvironmentCommand(EventHandlerEnvironment env) : base(env)
        {
        }
    }
}
