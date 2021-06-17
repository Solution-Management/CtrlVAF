using MFiles.VAF.Configuration;

using CtrlVAF.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CtrlVAF.Models;
using System.Runtime.ExceptionServices;

namespace CtrlVAF.Validation
{
    public class ValidatorDispatcher<TConfig> : Dispatcher<IEnumerable<ValidationFinding>>
                                                where TConfig : class, new()
    {
        private ConfigurableVaultApplicationBase<TConfig> vaultApplication;

        public ValidatorDispatcher(ConfigurableVaultApplicationBase<TConfig> vaultApplication)
        {
            this.vaultApplication = vaultApplication;
        }

        public override IEnumerable<ValidationFinding> Dispatch(params ICtrlVAFCommand[] commands)
        {
            if (commands == null) return Enumerable.Empty<ValidationFinding>();

            commands = commands.Where(cmd =>
                cmd.GetType() == typeof(ValidationCommand) ||
                cmd.GetType().BaseType == typeof(ValidationCommand)
                )
                .ToArray();

            IncludeAssemblies(Assembly.GetCallingAssembly());

            var types = GetTypes(commands);

            return HandleConcreteTypes(types, commands);
        }

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            if (!commands.Any()) return Enumerable.Empty<Type>();

            var validatorCommandTypes = commands.Select(cmd => cmd.GetType());
            var configType = typeof(TConfig);

            if (TypeCache.TryGetValue(configType, out var cachedTypes))
            {
                return cachedTypes.Distinct();
            }

            IncludeAssemblies(configType);
            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(CustomValidator<,>) &&
                    t.BaseType.GenericTypeArguments.Intersect(validatorCommandTypes).Any()
                    );
            });

            TypeCache.TryAdd(configType, concreteTypes);

            return concreteTypes;
        }

        protected internal override IEnumerable<ValidationFinding> HandleConcreteTypes(IEnumerable<Type> concreteValidators, params ICtrlVAFCommand[] commands)
        {
            if (!concreteValidators.Any() || !commands.Any()) return Enumerable.Empty<ValidationFinding>();

            var findings = new List<ValidationFinding>();

            foreach (Type concreteValidatorType in concreteValidators)
            {
                var results = Enumerable.Empty<ValidationFinding>();
                var validateMethod = concreteValidatorType.GetMethod(nameof(ICustomValidator<object, ValidationCommand>.Validate));
                var subConfigType = concreteValidatorType.BaseType.GenericTypeArguments[0];
                var concreteHandler = Activator.CreateInstance(concreteValidatorType) as CustomValidator;
                concreteHandler.PermanentVault = vaultApplication.PermanentVault;
                concreteHandler.OnDemandBackgroundOperations = vaultApplication.OnDemandBackgroundOperations;
                concreteHandler.RecurringBackgroundOperations = vaultApplication.RecurringBackgroundOperations;

                //Set the configuration
                var configProperty = concreteValidatorType
                    .GetProperty(nameof(ICustomValidator<object, ValidationCommand>.Configuration));
                var subConfig = Dispatcher_Helpers.GetConfigSubProperty(vaultApplication.GetConfig(), subConfigType);
                configProperty.SetValue(concreteHandler, subConfig);

                try
                {
                    var validatorCommand = commands.FirstOrDefault(cmd => cmd.GetType() == concreteValidatorType.BaseType.GenericTypeArguments[1]);

                    results = (validateMethod.Invoke(concreteHandler, new object[] { validatorCommand })
                                    as IEnumerable<ValidationFinding>).ToArray();
                    findings.AddRange(results);
                }
                catch (TargetInvocationException te)
                {
                    ExceptionDispatchInfo.Capture(te.InnerException).Throw();
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (!vaultApplication.ValidationResults.TryAdd(subConfigType, new ValidationResults(results)))
                {
                    vaultApplication.ValidationResults[subConfigType].AddResults(results);
                }
            }

            return findings;
        }
    }
}
