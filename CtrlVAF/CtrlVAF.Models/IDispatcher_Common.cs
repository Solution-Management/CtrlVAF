using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Models
{
    public interface IDispatcher_Common
    {
        IDispatcher_Common IncludeAssemblies(params Assembly[] assemblies);

        IDispatcher_Common IncludeAssemblies(params Type[] types);
    }

    

}
