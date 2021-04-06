using CtrlVAF.Events;
using MFiles.VAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core.Events
{
    public class EventHandlerMethodInfo : IEventHandlerMethodInfo
	{
		public readonly Dispatcher EventDispatcher;

		public EventHandlerMethodInfo(Dispatcher eventDispatcher)
		{
			this.EventDispatcher = eventDispatcher
				?? throw new ArgumentNullException(nameof(eventDispatcher));
		}

		#region Implementation of IEventHandlerMethodInfo

		/// <inheritdoc />
		void IEventHandlerMethodInfo.Execute(MFiles.VAF.Common.EventHandlerEnvironment environment, IExecutionTrace trace)
		{
			// Create and dispatch the command.
			var command = new EventCommand(environment);
			this.EventDispatcher.Dispatch(command);
		}

		/// <inheritdoc />
		// TODO: Implement!
		string IMethodInfoBase.LogString => "";

		/// <inheritdoc />
		// TODO: Implement!
		int IMethodInfoBase.Priority => 0;

		#endregion
	}
}
