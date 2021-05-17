using CtrlVAF.Models;

using MFiles.VAF.Common;
using MFiles.VAF.Configuration.AdminConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Events
{
    public class ConfigurationChangedCommand : ICtrlVAFCommand
    {
        IConfigurationRequestContext Context;
        ClientOperations ClientOperations;
        public object OldConfiguration;

        public ConfigurationChangedCommand(IConfigurationRequestContext context, ClientOperations clientOperations, object oldConfiguration)
        {
            Context = context;
            ClientOperations = clientOperations;
            OldConfiguration = oldConfiguration;
        }
    }
}
