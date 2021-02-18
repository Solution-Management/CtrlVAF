using CtrlVAF.Core;

using MFiles.VAF.Common;
using MFiles.VAF.Extensions.MultiServerMode;
using MFiles.VAF.MultiserverMode;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.BackgroundOperations
{
    public class BackgroundDispatcher<TConfig> : Dispatcher<object> where TConfig : class, new()
    {
        private Core.ConfigurableVaultApplicationBase<TConfig> vaultApplication;

        public BackgroundDispatcher(Core.ConfigurableVaultApplicationBase<TConfig> vaultApplication)
        {
            this.vaultApplication = vaultApplication;
        }

        public override object Dispatch() 
        {
            Type[] concreteTypes = GetTypes();
                        
            if (!concreteTypes.Any())
                return null;

            return DispatchTypes(concreteTypes);
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

        protected internal override Type[] GetTypes()
        {
            var applicationAssembly = Assembly.GetAssembly(vaultApplication.GetType());
            var allTypes = applicationAssembly.GetTypes();

            Type[] concreteTypes = allTypes.Where(
                t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(typeof(IBackgroundTask)) &&
                    t.IsDefined(typeof(BackgroundOperationAttribute))
                )
                .ToArray();

            return concreteTypes;
        }

        protected internal override object DispatchTypes(Type[] concreteTypes)
        {
            List<string> PermanentBackgroundOperationNames = new List<string>();
            List<string> OnDemandBackgroundOperationNames = new List<string>();

            foreach (Type concreteType in concreteTypes)
            {
                var task = Activator.CreateInstance(concreteType) as IBackgroundTask;

                TConfig config = vaultApplication.GetConfig();

                Type configSubType = concreteType.BaseType.GenericTypeArguments[0];

                object subConfig = GetConfigPropertyOfType(config, configSubType);

                task.Config = subConfig;

                BackgroundOperationAttribute operationInfo = concreteType.GetCustomAttribute<BackgroundOperationAttribute>();

                if (concreteType.IsDefined(typeof(RecurringAttribute)))
                {
                    var attr = concreteType.GetCustomAttribute<RecurringAttribute>();

                    TimeSpan interval = TimeSpan.FromMinutes(attr.IntervalInMinutes);

                    TaskQueueBackgroundOperation operation = vaultApplication.TaskQueueBackgroundOperationManager.StartRecurringBackgroundOperation(
                        operationInfo.Name,
                        interval,
                        task.Task
                        );

                    vaultApplication.PermanentBackgroundOperations.AddBackgroundOperation(operationInfo.Name, operation, interval);

                    PermanentBackgroundOperationNames.Add(concreteType.FullName);
                }
                else
                {
                    TaskQueueBackgroundOperation operation = vaultApplication.TaskQueueBackgroundOperationManager.CreateBackgroundOperation<TaskQueueDirective>(
                        operationInfo.Name,
                        task.Task
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

            return null;
        }
    }
}
