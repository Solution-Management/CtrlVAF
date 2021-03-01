using CtrlVAF.Events.Attributes;
using CtrlVAF.Events.Handlers;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.LicensingTests
{
    [EventCommandHandler(MFEventHandlerType.MFEventHandlerBeforeLoginToVault)]
    class UnlicensedCommandHandler: EventHandler<Configuration, TestLicenseCommand>
    {
        public override void Handle(TestLicenseCommand command)
        {
            command.Result *= 2; 
        }
    }
}
