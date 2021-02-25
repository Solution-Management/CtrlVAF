using MFiles.VAF.Common;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests
{
    class TestEventHandlerEnvironment: EventHandlerEnvironment
    {
        new public MFEventHandlerType EventType { get; set; }
    }
}
