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
        public string Name {get; set;}
    }

    class CustomCommand_2 : EventCommand
    {
        public int ID { get; set; }
    }

    class CustomCommand_3 : EventCommand
    {
        public int AddValue { get; set; }
    }
}
