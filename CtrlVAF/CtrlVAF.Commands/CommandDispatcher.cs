using CtrlVAF.Commands.Handlers;
using System;
using System.Linq;
using System.Reflection;

namespace CtrlVAF.Commands
{
    public static class CommandDispatcher
    {
        /// <summary>
        /// Main command dispatcher entry method. Typical usage of this method would be inside an event handler method inside the vault application base class.
        /// Once called, the dispatcher will locate any ICommandHandlers using the same TCommand interface and invoke their handle method.
        /// </summary>
        /// <typeparam name="TCommand">The type of command for the dispatcher to located</typeparam>
        /// <param name="command">The actual command itself</param>
        /// <param name="throwExceptions">Whether or not to stop executing ICommandHandlers upon exceptions and throw the exception</param>
        /// <param name="exceptionHandler">An exception handler to pass along or handle any ICommandHandler exceptions</param>
        public static void Dispatch<TCommand>(TCommand command, bool throwExceptions = false, Action<Exception> exceptionHandler = null) where TCommand : class
        {
            // Instantiate a handlerType according to the TCommand type provided
            Type handler = typeof(ICommandHandler<>);
            Type handlerType = handler.MakeGenericType(command.GetType());

            // Obtain the types of the executing assembly
            var executingAssembly = Assembly.GetCallingAssembly();
            var types = executingAssembly.GetTypes();

            // Obtain the concrete types in the assembly where the handleType is included as an interface
            Type[] concreteTypes = types.Where(
                t =>
                    t.IsClass &&
                    t.GetInterfaces().Contains(handlerType)
                    )
                .ToArray();

            // If none, return
            if (!concreteTypes.Any()) return;

            foreach (Type type in concreteTypes)
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
