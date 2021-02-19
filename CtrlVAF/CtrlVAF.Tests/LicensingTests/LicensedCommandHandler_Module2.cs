﻿using CtrlVAF.Commands.Handlers;
using CtrlVAF.Core.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    [LicenseRequired(Modules = new string[] { "Module2" })]
    class LicensedCommandHandler_Module2: ICommandHandler<TestLicenseCommand>
    {
        public void Handle(TestLicenseCommand command)
        {
            command.Result *= 7;
        }
    }
}