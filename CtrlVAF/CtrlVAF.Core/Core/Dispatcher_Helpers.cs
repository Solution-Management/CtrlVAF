using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
    static class Dispatcher_Helpers
    {
        public static object GetConfigPropertyOfType(object config, Type configSubType)
        {
            if (config.GetType() == configSubType)
                return config;

            var configProperties = config.GetType().GetProperties();

            foreach (var configProperty in configProperties)
            {
                if (!configProperty.PropertyType.IsClass)
                    continue;

                var subConfig = configProperty.GetValue(config);

                if (configProperty.PropertyType == configSubType)
                    return subConfig;
            }

            foreach (var configProperty in configProperties)
            {
                if (!configProperty.PropertyType.IsClass)
                    continue;

                var subConfig = configProperty.GetValue(config);

                var subsubConfig = GetConfigPropertyOfType(subConfig, configSubType);
                if (subsubConfig == null)
                    continue;
                else
                    return subsubConfig;
            }

            return null;
        }
    }
}
