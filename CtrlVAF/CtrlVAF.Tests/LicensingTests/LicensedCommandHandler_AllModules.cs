using CtrlVAF.Events.Handlers;
using CtrlVAF.Core.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CtrlVAF.Events.Attributes;

namespace CtrlVAF.Tests.LicensingTests
{
    [LicenseRequired]
    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerBeforeLoginToVault)]
    class LicensedCommandHandler_AllModules : EventHandler<Configuration, TestLicenseCommand>
    {
        public override void Handle(TestLicenseCommand command)
        {
            command.Result *= 3;
        }
    }
}
