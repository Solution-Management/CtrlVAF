using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;
using CtrlVAF.Core;
using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace CtrlVAF.Commands
{

    //    /// <summary>
    //    /// Main command dispatcher entry method. Typical usage of this method would be inside an event handler method inside the vault application base class.
    //    /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
    //    /// </summary>
    //    /// <typeparam name="TCommand">The type of command for the dispatcher to located</typeparam>
    //    /// <param name="command">The actual command itself</param>
    //    /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
    //    /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>

    public class EventDispatcher : Dispatcher
    {

        /// <summary>
        /// Typical usage of this class would be inside an event handler method inside the vault application base class.
        /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
        /// </summary>
        /// <param name="command">A command of type TCommand that has inherited from <see cref="Commands.IEventCommand{T}"/></param>
        /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
        /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>
        public EventDispatcher()
        {
        }

        /// <inheritdoc/>
        public override void Dispatch(params ICtrlVAFCommand[] commands)
        {
            IncludeAssemblies(Assembly.GetCallingAssembly());

            var concreteTypes = GetTypes(commands);

            HandleConcreteTypes(concreteTypes, commands);
        }

        protected internal override IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands)
        {
            // Instantiate a handlerType according to the TCommand type provided
            Type handlerType = typeof(IEventHandler<>);
            List<Type> dispatchableHandlerTypes = new List<Type>();

            List<Type> parsedCommands = new List<Type>();

            foreach (ICtrlVAFCommand command in commands)
            {
                Type commandType = command.GetType();

                if (parsedCommands.Contains(commandType))
                    continue;

                //If the concrete types have already been retrieved and cached before, simply handle those
                if (TypeCache.TryGetValue(commandType, out var cachedTypes))
                {
                    dispatchableHandlerTypes.AddRange(cachedTypes);
                    continue;
                }

                List<Type> handlerTypes = new List<Type>();

                foreach (Assembly assembly in Assemblies)
                {
                    var types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.IsClass)
                        {
                            var commandHandlerInterfaces = type.GetInterfaces().Where(t =>
                            t.IsGenericType &&
                            t.GetGenericTypeDefinition() == handlerType &&
                            t.GenericTypeArguments[0] == commandType);

                            if (commandHandlerInterfaces.Any())
                                handlerTypes.Add(type);
                        }
                    }
                }

                TypeCache.TryAdd(commandType, handlerTypes.Distinct());

                dispatchableHandlerTypes.AddRange(handlerTypes);

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

                if(TypeCache.TryGetValue(commandType, out IEnumerable<Type> concreteHandlerTypes))
                {
                    foreach (Type concreteHandlerType in concreteHandlerTypes)
                    {
                        if (handledTypes.Contains(concreteHandlerType) || !types.Contains(concreteHandlerType))
                            continue;

                        var concreteHandler = Activator.CreateInstance(concreteHandlerType);

                        var handleMethod = concreteHandlerType.GetMethod(nameof(IEventHandler<object>.Handle), new Type[] { commandType });

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
