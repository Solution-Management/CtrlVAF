using CtrlVAF.Core;
using CtrlVAF.Core.Models;
using CtrlVAF.Models;
using CtrlVAF.Validation;

using MFiles.VAF.Common;
using MFiles.VAF.Extensions;
using MFiles.VAF.MultiserverMode;
using MFilesAPI;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

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
            var permanentBackgroundOperationNames = new List<string>();
            var onDemandBackgroundOperationNames = new List<string>();

            foreach (Type concreteType in concreteTypes)
            {
                var operationInfo = concreteType.GetCustomAttribute<BackgroundOperationAttribute>();

                if (concreteType.IsDefined(typeof(RecurringAttribute)))
                {
                    var attr = concreteType.GetCustomAttribute<RecurringAttribute>();
                    var interval = TimeSpan.FromSeconds(attr.IntervalInSeconds);

                    var operation = vaultApplication.TaskQueueBackgroundOperationManager.StartRecurringBackgroundOperation(
                        operationInfo.Name,
                        interval,
                        GetBackgroundOperationFunction(concreteType)
                        );
                    operation.ShowRunCommandInDashboard = operationInfo.ShowRunCommandInDashboard;
                    operation.ShowBackgroundOperationInDashboard = operationInfo.ShowBackgroundOperationInDashboard;

                    vaultApplication.RecurringBackgroundOperations.AddBackgroundOperation(operationInfo.Name, operation, interval);
                    permanentBackgroundOperationNames.Add(concreteType.FullName);
                }
                else
                {
                    var operation = vaultApplication.TaskQueueBackgroundOperationManager.CreateBackgroundOperation<TaskQueueDirective>(
                        operationInfo.Name,
                        GetBackgroundOperationFunction(concreteType)
                        );
                    operation.ShowRunCommandInDashboard = operationInfo.ShowRunCommandInDashboard;
                    operation.ShowBackgroundOperationInDashboard = operationInfo.ShowBackgroundOperationInDashboard;
                    vaultApplication.OnDemandBackgroundOperations.AddBackgroundOperation(operationInfo.Name, operation);
                    onDemandBackgroundOperationNames.Add(concreteType.FullName);
                }
            }

            var message = "";

            if (permanentBackgroundOperationNames.Any())
                message += $"Permanent background operation classes: " + Environment.NewLine +
                    JsonConvert.SerializeObject(permanentBackgroundOperationNames, Formatting.Indented) + Environment.NewLine;

            if (onDemandBackgroundOperationNames.Any())
                message += $"On demand background operation classes: " + Environment.NewLine +
                    JsonConvert.SerializeObject(onDemandBackgroundOperationNames, Formatting.Indented) + Environment.NewLine;

            SysUtils.ReportInfoToEventLog(
                $"{vaultApplication.GetType().Name} - BackgroundOperations",
                message
                );
        }

        private BackgroundTaskHandler GetTaskHandler(Type concreteType)
        {
            var backgroundTaskHandler = Activator.CreateInstance(concreteType) as BackgroundTaskHandler;
            backgroundTaskHandler.PermanentVault = vaultApplication.PermanentVault;
            backgroundTaskHandler.OnDemandBackgroundOperations = vaultApplication.OnDemandBackgroundOperations;
            backgroundTaskHandler.RecurringBackgroundOperations = vaultApplication.RecurringBackgroundOperations;

            var config = vaultApplication.GetConfig();
            var subConfigType = concreteType.BaseType.GenericTypeArguments[0];
            object subConfig = Dispatcher_Helpers.GetConfigSubProperty(config, subConfigType);

            var configProperty = backgroundTaskHandler.GetType().GetProperty(nameof(IBackgroundTaskHandler<object, EmptyTaskQueueDirective>.Configuration));
            configProperty.SetValue(backgroundTaskHandler, subConfig);

            if (vaultApplication.ValidationResults.TryGetValue(subConfigType, out ValidationResults results))
                backgroundTaskHandler.ValidationResults = results;
            else
                backgroundTaskHandler.ValidationResults = null;

            return backgroundTaskHandler;
        }

        private Action<TaskProcessorJobEx, TaskQueueDirective> GetBackgroundOperationFunction(Type concreteType)
        {
            return (job, directive) =>
            {
                var backgroundTaskHandler = GetTaskHandler(concreteType);
                var taskMethod = backgroundTaskHandler.GetType().GetMethod(nameof(IBackgroundTaskHandler<object, EmptyTaskQueueDirective>.Task));

                try
                {
                    taskMethod.Invoke(backgroundTaskHandler, new object[] { job, directive, GetProgressFunction(job) });
                }
                catch (TargetInvocationException te)
                {
                    ExceptionDispatchInfo.Capture(te.InnerException).Throw();
                }
                catch (Exception e)
                {
                    throw e;
                }
            };
        }

        private Action<string, MFTaskState> GetProgressFunction(TaskProcessorJob job)
        {
            return (progress, taskState) =>
            {
                vaultApplication.TaskQueueBackgroundOperationManager.TaskProcessor.UpdateTaskInfo
                (
                    job,
                    taskState,
                    progress,
                    false
                );
            };
        }
    }
}