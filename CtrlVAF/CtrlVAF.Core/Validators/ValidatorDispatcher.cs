using CtrlVAF.Models;
using MFiles.VAF.Configuration;

using CtrlVAF.Core;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Validators
{
<<<<<<< Updated upstream:CtrlVAF/CtrlVAF.Validators/ValidationDispatcher.cs
    public class ValidationDispatcher : IDispatcher
    {
        public IEnumerable<ValidationFinding> Dispatch(Vault vault, object config)
        {
            var handlerType = typeof(ICustomValidator);
=======
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
            Type[] types = GetTypes();

            return DispatchTypes(types);
        }

        protected internal override Type[] GetTypes()
        {
            var callingAssembly = Config.GetType().Assembly;
            var allTypes = callingAssembly.GetTypes();
>>>>>>> Stashed changes:CtrlVAF/CtrlVAF.Core/Validators/ValidatorDispatcher.cs

            // Attempt to get types from the cache
            if (TypeCache.TryGetValue(handlerType, out var cachedTypes))
            {
                return HandleConcreteTypes(cachedTypes, vault, config);
            }

            // Obtain the types of the assemblies
            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a.GetTypes().Where(t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(handlerType)
                    );
            });

<<<<<<< Updated upstream:CtrlVAF/CtrlVAF.Validators/ValidationDispatcher.cs
            // Add the concrete types to cache
            TypeCache.TryAdd(handlerType, concreteTypes);

            return HandleConcreteTypes(concreteTypes, vault, config);
        }

        private IEnumerable<ValidationFinding> HandleConcreteTypes(IEnumerable<Type> concreteTypes, Vault vault, object config)
        {
            if (!concreteTypes.Any())
=======
            return concreteTypes;
        }

        protected internal override IEnumerable<ValidationFinding> DispatchTypes(Type[] types)
        {
            if (!types.Any())
>>>>>>> Stashed changes:CtrlVAF/CtrlVAF.Core/Validators/ValidatorDispatcher.cs
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
<<<<<<< Updated upstream:CtrlVAF/CtrlVAF.Validators/ValidationDispatcher.cs
            }
=======
            }
>>>>>>> Stashed changes:CtrlVAF/CtrlVAF.Core/Validators/ValidatorDispatcher.cs
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
