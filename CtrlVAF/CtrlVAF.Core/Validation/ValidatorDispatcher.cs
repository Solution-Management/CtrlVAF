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
                                                where TConfig: class, new()
    {
        private ConfigurableVaultApplicationBase<TConfig> vaultApplication;

        public ValidatorDispatcher(ConfigurableVaultApplicationBase<TConfig> vaultApplication)
        {
            this.vaultApplication = vaultApplication; 
        }

        public override IEnumerable<ValidationFinding> Dispatch(params ICtrlVAFCommand[] commands)
        {
            IncludeAssemblies(Assembly.GetCallingAssembly());

            var types = GetTypes(commands);

            return HandleConcreteTypes(types, commands).ToArray();
        }

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            var validatorCommand = commands.FirstOrDefault(
                cmd => 
                cmd.GetType() == typeof(ValidationCommand) ||
                cmd.GetType().BaseType == typeof(ValidationCommand)
                );

            if (validatorCommand == null)
                return new List<Type>();

            var configType = typeof(TConfig);

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
                    t.BaseType.IsGenericType &&
                    t.BaseType.GetGenericTypeDefinition() == typeof(CustomValidator<,>)
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
                cmd.GetType() == typeof(ValidationCommand) ||
                cmd.GetType().BaseType == typeof(ValidationCommand)
                );

            if (validatorCommand == null)
                yield break;

            foreach (Type concreteValidatorType in concreteValidators)
            {
                if (!ResultsCache.TryGetValue(concreteValidatorType, out IEnumerable<ValidationFinding> findings))
                {
                    var concreteHandler = Activator.CreateInstance(concreteValidatorType) as CustomValidator;

                    var subConfigType = concreteValidatorType.BaseType.GenericTypeArguments[0];

                    //Set the configuration
                    var configProperty = concreteValidatorType.GetProperty(nameof(ICustomValidator<object, ValidationCommand>.Configuration));
                    var subConfig = Dispatcher_Helpers.GetConfigPropertyOfType(vaultApplication.GetConfig(), subConfigType);
                    configProperty.SetValue(concreteHandler, subConfig);

                    //Set the configuration independent variables
                    concreteHandler.PermanentVault = vaultApplication.PermanentVault;
                    concreteHandler.OnDemandBackgroundOperations = vaultApplication.OnDemandBackgroundOperations;
                    concreteHandler.RecurringBackgroundOperations = vaultApplication.RecurringBackgroundOperations;
                    

                    var validateMethod = concreteValidatorType.GetMethod(nameof(ICustomValidator<object, ValidationCommand>.Validate));

                    try
                    {
                        findings = validateMethod.Invoke(concreteHandler, new object[] { validatorCommand })
                                        as IEnumerable<ValidationFinding>;
                    }
                    catch (TargetInvocationException te)
                    {
                        ExceptionDispatchInfo.Capture(te.InnerException).Throw();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                    ResultsCache.TryAdd(concreteValidatorType, findings.ToList());
                }

                foreach (var finding in findings)
                {
                    yield return finding;
                }
            }
        }
    }
}
