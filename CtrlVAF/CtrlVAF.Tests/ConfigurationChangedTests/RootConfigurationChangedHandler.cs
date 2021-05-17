using CtrlVAF.Events;
using CtrlVAF.Events.Attributes;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Tests.ConfigurationChangedTests
{
    [ConfigurationChangedHandler()]
    class RootConfigurationChangedHandler : ConfigurationChangedHandler<Configuration, ConfigurationChangedCommand>
    {
        public override void Handle(ConfigurationChangedCommand command)
        {
            if (OldConfiguration.ID != Configuration.ID) Configuration.Test = true;
        }
    }
}
