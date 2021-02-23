using CtrlVAF.Commands.Handlers;
using CtrlVAF.Core.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    [LicenseRequired(Modules = new string[] { "Module1" })]
    class LicensedCommandHandler_Module1 : IEventHandler<TestLicenseCommand>
    {
        public void Handle(TestLicenseCommand command)
        {
            command.Result *= 5;
        }
    }

}
