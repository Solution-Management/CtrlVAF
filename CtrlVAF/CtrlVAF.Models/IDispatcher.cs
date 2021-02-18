using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Models
{
    public abstract class IDispatcher
    {
        /// <summary>
        /// Cache to hold the concrete types found for each type of dispatcher execution type
        /// </summary>
        protected ConcurrentDictionary<Type, IEnumerable<Type>> _typeCache = new ConcurrentDictionary<Type, IEnumerable<Type>>();

        /// <summary>
        /// List of additional assemblies to look through for concrete types.
        /// Always contains the calling assembly!
        /// </summary>
        protected List<Assembly> _assemblies = new List<Assembly>() { Assembly.GetCallingAssembly() };

        /// <summary>
        /// Method to include additional assemblies in which to look for ICommandHandlers. The calling assembly is always included.
        /// </summary>
        /// <param name="assemblies">The assemblies in which to look for</param>
        /// <returns>The same CommandDispatcher</returns>
        public IDispatcher IncludeAssemblies(params Assembly[] assemblies)
        {
            _assemblies.AddRange(assemblies);
            _assemblies = _assemblies.Distinct().ToList();
            return this;
        }

        /// <summary>
        /// Overload method to IncludeAssmblies to allow for adding types directly and converting the Assmblies from these.
        /// </summary>
        /// <param name="types">The types of which to get the assemblies from</param>
        /// <returns>The same CommandDispatcher</returns>
        public IDispatcher IncludeAssemblies(params Type[] types)
        {
            var assemblies = types.Select(t => { return t.Assembly; }).ToArray();
            return IncludeAssemblies(assemblies);
        }
    }
}
