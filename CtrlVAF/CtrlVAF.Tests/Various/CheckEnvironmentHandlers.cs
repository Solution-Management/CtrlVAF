using CtrlVAF.Events.Attributes;
using CtrlVAF.Events.Handlers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.Various
{
    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterBringOnline)]
    class CheckEnvironmentHandler : EventHandler<Configuration, TestEnvironmentCommand>
    {
        public override void Handle(TestEnvironmentCommand command)
        {
            if (Configuration?.ID == 1234)
                command.TestedEnvironmentProperties += 1;
            if (ValidationResults?.Count() > 0)
                command.TestedEnvironmentProperties += 10;
            if (PermanentVault != null)
                command.TestedEnvironmentProperties += 100;
        }
    }

    [EventCommandHandler(MFilesAPI.MFEventHandlerType.MFEventHandlerAfterCancelCheckout)]
    class CheckValidationResults: EventHandler<Configuration, TestEnvironmentCommand>
    {
        public override void Handle(TestEnvironmentCommand command)
        {
            //We're expecting both Configuration and Child_Configuration results to be available
            if (ValidationResults.Count() == 2)
                command.TestedEnvironmentProperties += 10;
        }
    }
}
