using CtrlVAF.Commands.Commands;
using CtrlVAF.Commands.Handlers;
using CtrlVAF.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CtrlVAF.Commands
{
    public class CommandDispatcher : IVoidDispatcher
    {
        /// <summary>
        /// Main command dispatcher entry method. Typical usage of this method would be inside an event handler method inside the vault application base class.
        /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
        /// </summary>
        /// <typeparam name="TCommand">The type of command for the dispatcher to located</typeparam>
        /// <param name="command">The actual command itself</param>
        /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
        /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>
        public override void Dispatch<TCommand>(TCommand command, bool throwExceptions = false, Action<Exception> exceptionHandler = null)
        {
            // Instantiate a handlerType according to the TCommand type provided
            var handler = typeof(ICommandHandler<>);
            var handlerType = handler.MakeGenericType(command.GetType());

            // If the concrete types have already been retrieved and cached before, simply handle those
            if (TypeCache.TryGetValue(handlerType, out var cachedTypes))
            {
                HandleConcreteTypes(cachedTypes, command, throwExceptions, exceptionHandler);
                return;
            }

            // Obtain the types of the executing assembly
            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a.GetTypes().Where(t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(handlerType)
                    );
            });

            // Cache the concrete types
            TypeCache.TryAdd(handlerType, concreteTypes);

            HandleConcreteTypes(concreteTypes, command, throwExceptions, exceptionHandler);
        }

        private void HandleConcreteTypes<TCommand>(IEnumerable<Type> concreteTypes, TCommand command, bool throwExceptions = false, Action<Exception> exceptionHandler = null) where TCommand : class
        {
            // If none, return
            if (!concreteTypes.Any()) return;

            foreach (var type in concreteTypes)
            {
                try
                {
                    // Create instances of the concrete ICommandHandlers and handle them
                    var concreteHandler = Activator.CreateInstance(type) as ICommandHandler<TCommand>;
                    concreteHandler?.Handle(command);
                }
                catch (Exception e)
                {
                    // If anything happens during, let the exception handler do it for us
                    if (exceptionHandler != null)
                    {
                        exceptionHandler(e);
                    }

                    if (throwExceptions)
                    {
                        throw e;
                    }
                }
            }
        }
    }
}
