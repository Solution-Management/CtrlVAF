using CtrlVAF.Commands.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CtrlVAF.Commands
{
    public class Dispatcher
    {
        public void Dispatch<TCommand>(TCommand command) where TCommand : class
        {
            Type handler = typeof(ICommandHandler<>);
            Type handlerType = handler.MakeGenericType(command.GetType());

            var executingAssembly = Assembly.GetCallingAssembly();
            var types = executingAssembly.GetTypes();

            Type[] concreteTypes = types.Where(
                t => 
                    t.IsClass && 
                    t.GetInterfaces().Contains(handlerType)
                    )
                .ToArray();

            if (!concreteTypes.Any()) return;

            foreach(Type type in concreteTypes)
            {
                var concreteHandler = Activator.CreateInstance(type) as ICommandHandler<TCommand>;
                concreteHandler?.Handle(command);
            }
        }
    }
}
