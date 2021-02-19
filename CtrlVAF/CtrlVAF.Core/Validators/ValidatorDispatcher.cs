using MFiles.VAF.Configuration;

using CtrlVAF.Core;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;

namespace CtrlVAF.Validators
{
    public class ValidatorDispatcher : Dispatcher<IEnumerable<ValidationFinding>>
    {
        private Vault Vault;
        private object Config;

        public ValidatorDispatcher(Vault vault, object config)
        {
            Vault = vault;
            Config = config;
        }

        public override IEnumerable<ValidationFinding> Dispatch()
        {
            var types = GetTypes();

            return HandleConcreteTypes(types);
        }

        protected internal override IEnumerable<Type> GetTypes()
        {
            var handlerType = Config.GetType();

            // Attempt to get types from the cache
            if (TypeCache.TryGetValue(handlerType, out var cachedTypes))
            {
                return cachedTypes;
            }

            var concreteTypes = Assemblies.SelectMany(a => {
                return a
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(typeof(ICustomValidator))
                    );
            }); 
            
            TypeCache.TryAdd(handlerType, concreteTypes);

            return concreteTypes;
        }

        protected internal override IEnumerable<ValidationFinding> HandleConcreteTypes(IEnumerable<Type> types)
        {
            if (!types.Any())
                yield break;

            foreach (Type concreteType in types)
            {
                //Find config property (or sub-property) matching the generic argument of the basetype
                Type configSubType = concreteType.BaseType.GenericTypeArguments[0];

                var subConfig = GetConfigPropertyOfType(Config, configSubType);

                if (subConfig == null)
                    continue;

                var concreteHandler = Activator.CreateInstance(concreteType) as ICustomValidator;

                foreach (var finding in concreteHandler.Validate(Vault, subConfig))
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
