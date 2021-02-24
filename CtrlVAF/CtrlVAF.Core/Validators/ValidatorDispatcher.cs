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

            IncludeAssemblies(configType);

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
                return new ValidationFinding[0];

            //Get any validator command
            var validatorCommand = commands.FirstOrDefault(
                cmd =>
                cmd.GetType().IsGenericType &&
                cmd.GetType().GetGenericTypeDefinition() == typeof(ValidatorCommand<>)
                );

            if (validatorCommand == null)
                return new ValidationFinding[0];

            List<ValidationFinding> allFindings = new List<ValidationFinding>();

            foreach (Type concreteValidator in concreteValidators)
            {
                if (!ResultsCache.TryGetValue(concreteValidator, out IEnumerable<ValidationFinding> findings))
                {
                    var concreteHandler = Activator.CreateInstance(concreteValidator) as ICustomValidator;

                    findings = concreteHandler.Validate(validatorCommand);

                    ResultsCache.TryAdd(concreteValidator, findings.ToList());
                }

                allFindings.AddRange(findings);
            }

            return allFindings;
        }
    }
}
