using CtrlVAF.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
    /// <summary>
    /// Abstract class every dispatcher should inherit from. 
    /// </summary>
    /// <typeparam name="TReturn">The return type of the Dispatch method. If no return is expected this should be type object.</typeparam>
    public abstract class Dispatcher<TReturn> : IDispatcher
    {
        /// <summary>
        /// Main dispatcher entry method. Searches for suitable classes and instantiates them to execute some logic.
        /// </summary>
        /// <returns>Object of type <see cref="TReturn"/>. 
        /// In case no suitable types are found or no return is expected this will be <see cref="default"/>. </returns>
        public abstract TReturn Dispatch();

        /// <summary>
        /// Searches for suitable types
        /// </summary>
        /// <returns>Array of suitable types</returns>
        protected internal abstract IEnumerable<Type> GetTypes();

        /// <summary>
        /// Executes logic for an array of types, usually supplied by <see cref="GetTypes"/>
        /// </summary>
        /// <param name="types">a list of types which can be instantiated and will behave as expected</param>
        /// <returns>The result or <see cref="default"/> for no result.</returns>
        protected internal abstract TReturn HandleConcreteTypes(IEnumerable<Type> types);

        public List<ICtrlVAFCommand> Commands { get; set; } = new List<ICtrlVAFCommand>();


        /// <summary>
        /// Adds a command to be dispatched.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public override IDispatcher AddCommand(ICtrlVAFCommand command)
        {
            Commands.Add(command);
            Commands = Commands.Distinct().ToList();
            return this;
        }
    }
}
