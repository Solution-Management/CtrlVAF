using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Commands.Handlers
{
    public interface IEventHandler<TCommand> where TCommand : class, new()
    {
        void Handle(TCommand command);
    }

}
