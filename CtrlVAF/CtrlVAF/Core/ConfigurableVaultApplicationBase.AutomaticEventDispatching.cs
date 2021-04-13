using CtrlVAF.Events;
using CtrlVAF.Events.Attributes;
using MFiles.VAF;
using MFiles.VAF.Common;
using MFilesAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
	public abstract partial class ConfigurableVaultApplicationBase<TSecureConfiguration>
		where TSecureConfiguration : class, new()
	{
		/// <inheritdoc />
		protected override void LoadHandlerMethods(Vault vault)
		{
			// Use the base implementation in case of old-style registrations.
			base.LoadHandlerMethods(vault);

			// Identify/register event handlers.
			try
			{
				this.RegisterEventHandlers
				(
					this.GetClassesWithAttribute<EventCommandHandlerAttribute>()
						.SelectMany(kvp => kvp.Value)
				);
			}
			catch(Exception e)
			{
				SysUtils.ReportErrorMessageToEventLog("Could not load event handlers", e);
			}
		}

		/// <summary>
		/// Registers the event command 
		/// </summary>
		/// <param name="keyValuePair"></param>
		protected void RegisterEventHandlers
		(
			IEnumerable<EventCommandHandlerAttribute> attributes
		)
		{
			// Sanity.
			attributes = attributes ?? new List<EventCommandHandlerAttribute>();

			// Find the unique types and add event handler method info for each.
			var eventHandlerInfo = EventDispatcher.CreateEventHandlerMethodInfo();
			foreach (var eventType in attributes.Select(a => a.MFEvent).Distinct())
			{
				// Check if the event type already has a collection.
				if (!this.eventHandlerMethods.ContainsKey(eventType))
					this.eventHandlerMethods.Add(eventType, new List<IEventHandlerMethodInfo>());

				// Create the delegate for the event handler method and store it to the collection.
				this.eventHandlerMethods[eventType].Add(eventHandlerInfo);
			}

		}

		/// <summary>
		/// Returns all classes in assemblies provided by <see cref="GetAssembliesForAnalysis"/>
		/// which have at least one <typeparamref name="T"/> attribute.
		/// </summary>
		/// <typeparam name="T">The type of attribute to retrieve.</typeparam>
		/// <returns>A collection of types that implement the attribute, along with the attributes.</returns>
		protected Dictionary<Type, List<T>> GetClassesWithAttribute<T>()
			where T : Attribute
		{
			// Create our dictionary of items to process.
			var dict = new Dictionary<Type, List<T>>();

			// Populate the dictionary with classes that have this type attribute.
			foreach (var a in this.IncludeAssemblies())
			{
				foreach (var c in a.GetTypes().Where(t => t.IsClass && false == t.IsAbstract))
				{
					// Get the appropriate attributes (e.g. EventCommandHandlerAttribute).
					var appropriateAttributes = c.GetCustomAttributes<T>();

					// If this class does not have the attribute then die.
					if (false == appropriateAttributes.Any())
						continue;
					
					// Add the attributes to our dictionary for further processing.
					dict.Add(c, appropriateAttributes.ToList());
				}
			}

			// Return our classes.
			return dict;
		}
	}
}
