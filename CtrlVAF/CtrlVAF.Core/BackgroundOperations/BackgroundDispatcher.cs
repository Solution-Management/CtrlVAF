using CtrlVAF.Core;
using CtrlVAF.Models;

using MFiles.VAF.Common;
using MFiles.VAF.Extensions.MultiServerMode;
using MFiles.VAF.MultiserverMode;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CtrlVAF.BackgroundOperations
{
    public class BackgroundDispatcher<TConfig> : Dispatcher where TConfig : class, new()
    {
        private readonly Core.ConfigurableVaultApplicationBase<TConfig> vaultApplication;

        public BackgroundDispatcher(Core.ConfigurableVaultApplicationBase<TConfig> vaultApplication)
        {
            this.vaultApplication = vaultApplication;
        }

        public override void Dispatch(params ICtrlVAFCommand[] commands)
        {
            IncludeAssemblies(Assembly.GetCallingAssembly());

            var concreteTypes = GetTypes();

            if (!concreteTypes.Any())
                return;

            HandleConcreteTypes(concreteTypes);
        }

        

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            IncludeAssemblies(typeof(TConfig));

            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a.GetTypes().Where(t =>
                {
                    return t.IsClass &&
                           t.BaseType.IsGenericType &&
                           t.BaseType.GetGenericTypeDefinition() == typeof(BackgroundTaskHandler<,>) &&
                           t.IsDefined(typeof(BackgroundOperationAttribute));
                });
            });

            return concreteTypes;
        }

        protected internal override void HandleConcreteTypes(IEnumerable<Type> concreteTypes, params ICtrlVAFCommand[] commands)
        {
            List<string> PermanentBackgroundOperationNames = new List<string>();
            List<string> OnDemandBackgroundOperationNames = new List<string>();

            foreach (Type concreteType in concreteTypes)
            {
                var backgroundTaskHandler = Activator.CreateInstance(concreteType);

                //Get the right configuration subType and object
                TConfig config = vaultApplication.GetConfig();

                Type configSubType = concreteType.BaseType.GenericTypeArguments[0];

                object subConfig = Dispatcher_Helpers.GetConfigPropertyOfType(config, configSubType);

                //Set the configuration
                var configProperty = backgroundTaskHandler.GetType().GetProperty(nameof(IBackgroundTaskHandler<object, EmptyTQD>.Configuration));
                configProperty.SetValue(backgroundTaskHandler, subConfig);

                //Get the Task Action
                var taskMethod = backgroundTaskHandler.GetType().GetMethod(nameof(IBackgroundTaskHandler<object, EmptyTQD>.Task));

                Action<TaskProcessorJob, TaskQueueDirective> task = 
                    (Action<TaskProcessorJob, TaskQueueDirective>)
                    Delegate.CreateDelegate(typeof(Action<TaskProcessorJob, TaskQueueDirective>), concreteType, taskMethod);

                BackgroundOperationAttribute operationInfo = concreteType.GetCustomAttribute<BackgroundOperationAttribute>();

                if (concreteType.IsDefined(typeof(RecurringAttribute)))
                {
                    var attr = concreteType.GetCustomAttribute<RecurringAttribute>();

                    TimeSpan interval = TimeSpan.FromMinutes(attr.IntervalInMinutes);

                    TaskQueueBackgroundOperation operation = vaultApplication.TaskQueueBackgroundOperationManager.StartRecurringBackgroundOperation(
                        operationInfo.Name,
                        interval,
                        task
                        );

                    vaultApplication.RecurringBackgroundOperations.AddBackgroundOperation(operationInfo.Name, operation, interval);

                    PermanentBackgroundOperationNames.Add(concreteType.FullName);
                }
                else
                {
                    TaskQueueBackgroundOperation operation = vaultApplication.TaskQueueBackgroundOperationManager.CreateBackgroundOperation<TaskQueueDirective>(
                        operationInfo.Name,
                        task
                        );

                    vaultApplication.OnDemandBackgroundOperations.AddBackgroundOperation(operationInfo.Name, operation);

                    OnDemandBackgroundOperationNames.Add(concreteType.FullName);
                }
            }

            string message = "";

            if (PermanentBackgroundOperationNames.Any())
                message += $"Permanent background operation classes: " + Environment.NewLine +
                    JsonConvert.SerializeObject(PermanentBackgroundOperationNames, Formatting.Indented) + Environment.NewLine;

            if (OnDemandBackgroundOperationNames.Any())
                message += $"On demand background operation classes: " + Environment.NewLine +
                    JsonConvert.SerializeObject(OnDemandBackgroundOperationNames, Formatting.Indented) + Environment.NewLine;

            SysUtils.ReportInfoToEventLog(
                $"{vaultApplication.GetType().Name} - BackgroundOperations",
                message
                );

            return;
        }

        private class EmptyTQD : TaskQueueDirective
        {

        }

    }
}
