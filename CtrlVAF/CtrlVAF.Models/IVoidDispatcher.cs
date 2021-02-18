using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Models
{
    public abstract class IVoidDispatcher : IDispatcher
    {
        public abstract void Dispatch<TCommand>(TCommand command, bool throwExceptions = false, Action<Exception> exceptionHandler = null) where TCommand : class;
    }
}
