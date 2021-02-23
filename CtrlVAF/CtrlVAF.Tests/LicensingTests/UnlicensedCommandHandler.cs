using CtrlVAF.Commands.Handlers;
using CtrlVAF.Commands.Commands;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    class UnlicensedCommandHandler: IEventHandler<TestLicenseCommand>
    {
        public void Handle(TestLicenseCommand command)
        {
            command.Result *= 2; 
        }
    }
}
