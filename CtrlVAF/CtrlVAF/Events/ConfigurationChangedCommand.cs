using CtrlVAF.Models;
using MFiles.VAF.Configuration.AdminConfigurations;

namespace CtrlVAF.Events
{
    public class ConfigurationChangedCommand : ICtrlVAFCommand
    {
        private IConfigurationRequestContext Context;
        private ClientOperations ClientOperations;
        public object OldConfiguration;

        public ConfigurationChangedCommand(IConfigurationRequestContext context, ClientOperations clientOperations, object oldConfiguration)
        {
            Context = context;
            ClientOperations = clientOperations;
            OldConfiguration = oldConfiguration;
        }
    }
}