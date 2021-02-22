using MFiles.VAF.Configuration;

using CtrlVAF.Core;

using MFilesAPI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CtrlVAF.Models;
using System.Collections.Concurrent;

namespace CtrlVAF.Validators
{
    public class ValidatorDispatcher : Dispatcher<IEnumerable<ValidationFinding>>
    {
        

        public ValidatorDispatcher()
        {
        }

        public override IEnumerable<ValidationFinding> Dispatch(params ICtrlVAFCommand[] commands)
        {
            IncludeAssemblies(Assembly.GetCallingAssembly());

            var types = GetTypes(commands);

            return HandleConcreteTypes(types, commands);
        }

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            var validatorCommand = commands.FirstOrDefault(
                cmd => 
                cmd.GetType().IsGenericType && 
                cmd.GetType().GetGenericTypeDefinition() == typeof(ValidatorCommand<>)
                );

            if (validatorCommand == null)
                return new List<Type>();

            var configType = validatorCommand.GetType().GenericTypeArguments[0];

            // Attempt to get types from the cache
            if (TypeCache.TryGetValue(configType, out var cachedTypes))
            {
                return cachedTypes.Distinct();
            }

            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(typeof(ICustomValidator))
                    );
            });

            TypeCache.TryAdd(configType, concreteTypes);

            return concreteTypes;
        }

        protected internal override IEnumerable<ValidationFinding> HandleConcreteTypes(IEnumerable<Type> concreteValidators, params ICtrlVAFCommand[] commands)
        {
            if (!concreteValidators.Any())
                yield break;

            //Get any validator command
            var validatorCommand = commands.FirstOrDefault(
                cmd =>
                cmd.GetType().IsGenericType &&
                cmd.GetType().GetGenericTypeDefinition() == typeof(ValidatorCommand<>)
                );

            if (validatorCommand == null)
                yield break;


            foreach (Type concreteValidator in concreteValidators)
            {
                if (ResultsCache.TryGetValue(concreteValidator, out IEnumerable<ValidationFinding> findings))
                {
                    //Do nothing. We got the cached values already
                }
                else
                {
                    var concreteHandler = Activator.CreateInstance(concreteValidator) as ICustomValidator;

                    findings = concreteHandler.Validate(validatorCommand);

                    ResultsCache.TryAdd(concreteValidator, findings);
                }

                foreach (var finding in findings)
                {
                    yield return finding;
                }
            }

        }

        //private object GetConfigPropertyOfType(object config, Type configSubType)
        //{
        //    if (config.GetType() == configSubType)
        //        return config;

        //    var configProperties = config.GetType().GetProperties();

        //    foreach (var configProperty in configProperties)
        //    {
        //        if (!configProperty.PropertyType.IsClass)
        //            continue;

        //        var subConfig = configProperty.GetValue(config);

        //        if (configProperty.PropertyType == configSubType)
        //            return subConfig;
        //    }

        //    foreach (var configProperty in configProperties)
        //    {
        //        if (!configProperty.PropertyType.IsClass)
        //            continue;

        //        var subConfig = configProperty.GetValue(config);

        //        var subsubConfig = GetConfigPropertyOfType(subConfig, configSubType);
        //        if (subsubConfig == null)
        //            continue;
        //        else
        //            return subsubConfig;

        //    }

        //    return null;
        //}
    }
}
