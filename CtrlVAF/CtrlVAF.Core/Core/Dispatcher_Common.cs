using CtrlVAF.Models;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
    public abstract class Dispatcher_Common : IDispatcher_Common
    {
        /// <summary>
        /// List of additional assemblies to look through for concrete types.
        /// Always contains the calling assembly!
        /// </summary>
        protected List<Assembly> Assemblies = new List<Assembly>() { };

        /// <summary>
        /// Cache to hold the concrete types found for each type of dispatcher execution type
        /// </summary>
        protected ConcurrentDictionary<Type, IEnumerable<Type>> TypeCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        /// <summary>
        /// Method to include additional assemblies in which to look for ICommandHandlers. The calling assembly is always included.
        /// </summary>
        /// <param name="assemblies">The assemblies in which to look for</param>
        /// <returns>The same CommandDispatcher</returns>
        public IDispatcher_Common IncludeAssemblies(params Assembly[] assemblies)
        {
            Assemblies.AddRange(assemblies);
            //Never search CtrlVAF.Core
            Assemblies.RemoveAll(a => a == GetType().Assembly);
            Assemblies = Assemblies.Distinct().ToList();
            return this;
        }

        /// <summary>
        /// Overload method to IncludeAssemblies to allow for adding types directly and converting the Assmblies from these.
        /// </summary>
        /// <param name="types">The types of which to get the assemblies from</param>
        /// <returns>The same CommandDispatcher</returns>
        public IDispatcher_Common IncludeAssemblies(params Type[] types)
        {
            var assemblies = types.Select(t => { return t.Assembly; }).ToArray();
            return IncludeAssemblies(assemblies);
        }

        /// <summary>
        /// Searches for suitable types
        /// </summary>
        /// <returns>Array of suitable types</returns>
        protected internal abstract IEnumerable<Type> GetTypes(params ICtrlVAFCommand[] commands);
    }
}
