using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Commands.Handlers
{
    public abstract class CommandHandler<TCommand> : ICommandHandler where TCommand : class, new()
    {
        public abstract void Handle(TCommand command);

        public void Handle(ICtrlVAFCommand command)
        {
            if (command.GetType() != typeof(TCommand))
                return;
            else
                Handle(command as TCommand);
        }
    }

    public interface ICommandHandler
    {
        void Handle(ICtrlVAFCommand command);
    }
}
