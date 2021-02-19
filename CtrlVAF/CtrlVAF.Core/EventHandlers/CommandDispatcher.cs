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

    public class CommandDispatcher<TCommand> : Dispatcher<object> where TCommand : class, new()
    {
        private readonly TCommand command;
        private readonly bool throwExceptions = false;
        private readonly Action<Exception> exceptionHandler = null;

        /// <summary>
        /// Typical usage of this class would be inside an event handler method inside the vault application base class.
        /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
        /// </summary>
        /// <param name="command">A command of type TCommand that has inherited from <see cref="Commands.IEventHandlerCommand{T}"/></param>
        /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
        /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>
        public CommandDispatcher(TCommand command, bool throwExceptions = false, Action<Exception> exceptionHandler = null)
        {
            this.command = command;
            this.throwExceptions = throwExceptions;
            this.exceptionHandler = exceptionHandler;

            IncludeAssemblies(Assembly.GetCallingAssembly());
        }

        /// <inheritdoc/>
        public override object Dispatch()
        {
            var concreteTypes = GetTypes();

            // If none, return
            if (!concreteTypes.Any()) return null;

            return HandleConcreteTypes(concreteTypes);
        }

        protected internal override IEnumerable<Type> GetTypes()
        {
            // Instantiate a handlerType according to the TCommand type provided
            var handler = typeof(ICommandHandler<>);
            var handlerType = handler.MakeGenericType(command.GetType());

            //If the concrete types have already been retrieved and cached before, simply handle those
            if (TypeCache.TryGetValue(handlerType, out var cachedTypes))
            {
                HandleConcreteTypes(cachedTypes);
                return cachedTypes.ToArray();
            }

            //Obtain the types of the executing assembly
            var concreteTypes = Assemblies.SelectMany(a =>
            {
                return a.GetTypes().Where(t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(handlerType)
                    );
            });

            TypeCache.TryAdd(handlerType, concreteTypes);

            return concreteTypes;

        }

        protected internal override object HandleConcreteTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
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
            return null;
        }
    }
}
