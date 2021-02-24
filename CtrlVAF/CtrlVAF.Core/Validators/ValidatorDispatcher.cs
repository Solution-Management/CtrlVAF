using MFiles.VAF.Configuration;

using CtrlVAF.Core;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CtrlVAF.Models;
using System.Runtime.ExceptionServices;

namespace CtrlVAF.Validators
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
                cmd.GetType() == typeof(ValidatorCommand) ||
                cmd.GetType().BaseType == typeof(ValidatorCommand)
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
                cmd.GetType() == typeof(ValidatorCommand) ||
                cmd.GetType().BaseType == typeof(ValidatorCommand)
                );

            if (validatorCommand == null)
                yield break;

            foreach (Type concreteValidatorType in concreteValidators)
            {
                if (!ResultsCache.TryGetValue(concreteValidatorType, out IEnumerable<ValidationFinding> findings))
                {
                    var concreteHandler = Activator.CreateInstance(concreteValidatorType);

                    //Set the configuration
                    var configProperty = concreteValidatorType.GetProperty(nameof(ICustomValidator<object, ValidatorCommand>.Configuration));

                    var subConfig = Dispatcher_Helpers.GetConfigPropertyOfType(vaultApplication.GetConfig(), typeof(TConfig));

                    configProperty.SetValue(concreteHandler, subConfig);

                    var validateMethod = concreteValidatorType.GetMethod(nameof(ICustomValidator<object, ValidatorCommand>.Validate));

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
