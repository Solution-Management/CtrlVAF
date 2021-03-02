using CtrlVAF.Events.Handlers;
using CtrlVAF.Core;
using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using MFiles.VAF.Common;
using CtrlVAF.Events.Attributes;
using CtrlVAF.Validation;

namespace CtrlVAF.Events
{
    //    /// <summary>
    //    /// Main command dispatcher entry method. Typical usage of this method would be inside an event handler method inside the vault application base class.
    //    /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
    //    /// </summary>
    //    /// <typeparam name="TCommand">The type of command for the dispatcher to located</typeparam>
    //    /// <param name="command">The actual command itself</param>
    //    /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
    //    /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>

    public class EventDispatcher<TConfig> : Dispatcher
        where TConfig : class, new()
    {
        private ConfigurableVaultApplicationBase<TConfig> vaultApplication;

        /// <summary>
        /// Typical usage of this class would be inside an event handler method inside the vault application base class.
        /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
        /// </summary>
        /// <param name="command">A command of type TCommand that has inherited from <see cref="Commands.IEventCommand{T}"/></param>
        /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
        /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>
        public EventDispatcher(ConfigurableVaultApplicationBase<TConfig> vaultApplication)
        {
            this.vaultApplication = vaultApplication;
        }

        /// <inheritdoc/>
        public override void Dispatch(params ICtrlVAFCommand[] commands)
        {
            commands = commands.Where(cmd => 
                cmd.GetType() == typeof(EventCommand) ||
                cmd.GetType().BaseType == typeof(EventCommand)
                )
                .ToArray();
            
            IncludeAssemblies(Assembly.GetCallingAssembly());

            var concreteTypes = GetTypes(commands);

            HandleConcreteTypes(concreteTypes, commands);
        }

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            // Instantiate a handlerType according to the TCommand type provided
            Type abstractHandlerType = typeof(EventHandler<,>);
            List<Type> dispatchableHandlerTypes = new List<Type>();

            List<Type> parsedCommands = new List<Type>();

            foreach (EventCommand command in commands)
            {
                Type commandType = command.GetType();

                if (parsedCommands.Contains(commandType))
                    continue;

                //If the concrete types have already been retrieved and cached before, simply handle those
                if (TypeCache.TryGetValue(commandType, out var cachedTypes))
                {
                    var handlerTypes = cachedTypes.Where(t =>
                        t.GetCustomAttributes<EventCommandHandlerAttribute>().Any(a =>
                            a.EventHandlerTypeMatches(command.Env.EventType)
                            )
                        );

                    dispatchableHandlerTypes.AddRange(handlerTypes);
                    continue;
                }

                //get the event handlers that can handle this command type and are designated to handle the event described in the commands Env
                List<Type> allCommandHandlerTypes = new List<Type>();
                foreach (Assembly assembly in Assemblies)
                {
                    var commandHandlerTypes = assembly.GetTypes().Where(t =>
                        t.IsClass &&
                        t.BaseType.IsGenericType &&
                        t.BaseType.GetGenericTypeDefinition() == abstractHandlerType &&
                        t.BaseType.GenericTypeArguments[1] == commandType); ;

                    allCommandHandlerTypes.AddRange(commandHandlerTypes);
                }

                //Was there a reason for this Distinct? The same assembly is not included twice.
                TypeCache.TryAdd(commandType, allCommandHandlerTypes.Distinct());

                var eventHandlerTypes = allCommandHandlerTypes.Where(t =>
                        t.GetCustomAttributes<EventCommandHandlerAttribute>().Any(a =>
                            a.EventHandlerTypeMatches(command.Env.EventType)
                            )
                        );

                dispatchableHandlerTypes.AddRange(eventHandlerTypes);

                parsedCommands.Add(commandType);
            }

            return dispatchableHandlerTypes.Distinct();
        }

        protected internal override void HandleConcreteTypes(IEnumerable<Type> types, params ICtrlVAFCommand[] commands)
        {
            // If none, return
            if (!types.Any()) return;

            List<Type> handledTypes = new List<Type>();

            foreach (ICtrlVAFCommand command in commands)
            {
                var commandType = command.GetType();

                if (TypeCache.TryGetValue(commandType, out IEnumerable<Type> concreteHandlerTypes))
                {
                    foreach (Type concreteHandlerType in concreteHandlerTypes)
                    {
                        if (handledTypes.Contains(concreteHandlerType) || !types.Contains(concreteHandlerType))
                            continue;

                        var concreteHandler = Activator.CreateInstance(concreteHandlerType) as Handlers.EventHandler;

                        var subConfigType = concreteHandlerType.BaseType.GenericTypeArguments[0];

                        //Set the configuration
                        var configProperty = concreteHandlerType.GetProperty(nameof(ICommandHandler<object>.Configuration));
                        var subConfig = Dispatcher_Helpers.GetConfigSubProperty(vaultApplication.GetConfig(), subConfigType);
                        configProperty.SetValue(concreteHandler, subConfig);


                        //Set the configuration independent variables
                        concreteHandler.PermanentVault = vaultApplication.PermanentVault;
                        concreteHandler.OnDemandBackgroundOperations = vaultApplication.OnDemandBackgroundOperations;
                        concreteHandler.RecurringBackgroundOperations = vaultApplication.RecurringBackgroundOperations;

                        var keys = vaultApplication.ValidationResults.Keys.ToArray();
                        var found = Dispatcher_Helpers.AreConfigSubProperties(subConfigType, keys);
                        for (int i = 0; i < keys.Length; i++)
                        {
                            if (found[i])
                                concreteHandler.ValidationResults.AddResults(vaultApplication.ValidationResults[keys[i]]);
                        }


                        var handleMethod = concreteHandlerType
                            .GetMethod(nameof(EventHandler<object, EventCommand>.Handle), new Type[] { commandType });

                        try
                        {
                            handleMethod.Invoke(concreteHandler, new object[] { command });
                        }
                        catch (TargetInvocationException te)
                        {
                            ExceptionDispatchInfo.Capture(te.InnerException).Throw();
                        }
                        catch (Exception e)
                        {
                            throw e;
                        }

                        handledTypes.Add(concreteHandlerType);
                    }
                }
            }

            return;
        }
    }
}
