using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;
using CtrlVAF.Core;
using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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
        /// <param name="command">A command of type TCommand that has inherited from <see cref="Commands.IEventHandlerCommand{T}"/></param>
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
            Type handlerType = typeof(ICommandHandler);
            List<Type> dispatchableHandlerTypes = new List<Type>();

            foreach (ICtrlVAFCommand command in commands)
            {
                Type commandType = command.GetType();

                //If the concrete types have already been retrieved and cached before, simply handle those
                if (TypeCache.TryGetValue(commandType, out var cachedTypes))
                {
                    dispatchableHandlerTypes.AddRange(cachedTypes.Distinct());
                    continue;
                }

                List<Type> handlerTypes = new List<Type>();

                foreach (Assembly assembly in Assemblies)
                {
                    var types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.IsClass && 
                            type.GetInterfaces().Contains(handlerType) &&
                            type.BaseType.IsGenericType &&
                            type.BaseType.GetGenericArguments().Contains(commandType))
                            handlerTypes.Add(type);
                    }
                }

                TypeCache.TryAdd(commandType, handlerTypes);

                dispatchableHandlerTypes.AddRange(handlerTypes);
            }

            return dispatchableHandlerTypes;
        }

        protected internal override void HandleConcreteTypes(IEnumerable<Type> types, params ICtrlVAFCommand[] commands)
        {
            // If none, return
            if (!types.Any()) return;

            foreach (Type type in types)
            {
                //get the right command for the type from typecache
                Type commandType = TypeCache.FirstOrDefault(kv => kv.Value.Contains(type)).Key;

                if (commandType == default)
                    continue;

                var command = commands.FirstOrDefault(cmd => cmd.GetType() == commandType);
                
                    // Create instances of the concrete ICommandHandlers and handle them
                    var concreteHandler = Activator.CreateInstance(type) as ICommandHandler;
                    concreteHandler?.Handle(command);
            }
            return;
        }
    }
}
