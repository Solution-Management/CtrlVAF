using MFiles.VAF.Configuration;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
    public class Dispatcher
    {
        public IEnumerable<ValidationFinding> Dispatch(Vault vault, object config)
        {
            var callingAssembly = config.GetType().Assembly;
            var allTypes = callingAssembly.GetTypes();

            Type[] concreteTypes = allTypes.Where(
                t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(typeof(ICustomValidator))
                )
                .ToArray();

            if (!concreteTypes.Any())
                yield break;

            foreach (Type concreteType in concreteTypes)
            {
                //Find config property (or sub-property) matching the generic argument of the basetype
                Type configSubType = concreteType.BaseType.GenericTypeArguments[0];

                var subConfig = GetConfigPropertyOfType(config, configSubType);

                if (subConfig == null)
                    continue;

                var concreteHandler = Activator.CreateInstance(concreteType) as ICustomValidator;

                foreach (var finding in concreteHandler.Validate(vault, subConfig))
                {
                    yield return finding;
                }
            }

        }

        private object GetConfigPropertyOfType(object config, Type configSubType)
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
