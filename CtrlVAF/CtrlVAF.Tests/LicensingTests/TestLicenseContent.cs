using MFiles.VAF.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    class TestLicenseContent: LicenseContentBase
    {
        public void SetValidity(bool isValid)
        {
            IsValid = isValid;
        }
        public override bool IsValid { get; protected set; }

        public override List<string> Modules { get; set; } = new List<string>();
    }
}
