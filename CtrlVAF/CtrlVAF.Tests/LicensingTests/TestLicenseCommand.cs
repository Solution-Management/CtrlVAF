using CtrlVAF.Events;

using MFiles.VAF.Common;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    class TestLicenseCommand: EventCommand
    {
        public int Result { get; set; } = 1;
    }
}
