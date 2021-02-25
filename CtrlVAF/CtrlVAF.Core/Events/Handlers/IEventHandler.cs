using CtrlVAF.Core;
using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Events.Handlers
{
    public interface IEventHandler<TConfig, TCommand> : ICommandHandler<TConfig>
        where TConfig: class, new()
        where TCommand : EventCommand
    {
        void Handle(TCommand command);
    }

}
