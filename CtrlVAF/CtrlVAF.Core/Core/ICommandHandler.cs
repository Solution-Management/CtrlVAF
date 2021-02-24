using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
    public interface ICommandHandler<TConfiguration> where TConfiguration: class, new()
    {
        TConfiguration Configuration { get; }

    }
}
