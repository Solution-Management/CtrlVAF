using CtrlVAF.Events;
using CtrlVAF.Events.Attributes;
using CtrlVAF.Events.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CtrlVAF.Utilities;


namespace CtrlVAF.Tests.ConfigurationChangedTests
{
    [ConfigurationChangedHandler()]
    class ChildConfigurationChangedHandler : ConfigurationChangedHandler<Child_Configuration, ConfigurationChangedCommand>
    {
        public override void Handle(ConfigurationChangedCommand command)
        {
            // Checks a specific property, here the Name of a configuration, for any changes safely
            if (ObjectUtilities.PropertyChangedSafe(() => { return OldConfiguration.Name; }, () => { return Configuration.Name; }))
            {
                // Configuration was changed
                Configuration.Test = true;
            }
        }
    }
}
