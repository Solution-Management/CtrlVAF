using System;
using System.Reflection;

namespace CtrlVAF.Models
{
    public interface IDispatcher_Common
    {
        IDispatcher_Common IncludeAssemblies(params Assembly[] assemblies);

        IDispatcher_Common IncludeAssemblies(params Type[] types);
    }
}