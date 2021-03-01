using CtrlVAF.Events;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.CommandTests
{
    class CustomCommand_1 : EventCommand
    {
        public CustomCommand_1(EventHandlerEnvironment env) : base(env)
        {
        }

        public string Name {get; set;}
    }

    class CustomCommand_2 : EventCommand
    {
        public CustomCommand_2(EventHandlerEnvironment env) : base(env)
        {
        }

        public int ID { get; set; }
    }

    class CustomCommand_3 : EventCommand
    {
        public CustomCommand_3(EventHandlerEnvironment env) : base(env)
        {
        }

        public int AddValue { get; set; }
    }

    
}
