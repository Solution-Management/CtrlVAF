﻿using CtrlVAF.Core.Attributes;

using MFiles.VAF.Configuration;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CtrlVAF.Core
{
    /// <summary>
    /// Decorator for a <see cref="Dispatcher{TReturn}"/> that checks the application's licensing status.
    /// Works with <see cref="LicenseRequiredAttribute"/>
    /// </summary>
    /// <typeparam name="TReturn">The return type of the dispatcher</typeparam>
    public class LicensedDispatcher<TReturn> : Dispatcher<TReturn>
    {
        private LicenseContentBase license;
        private Dispatcher<TReturn> dispatcher;

        /// <summary>
        /// Wraps a dispatcher with logic checking the application's licensing status when evaluating types.
        /// </summary>
        /// <param name="dispatcher">the wrapped dispatcher</param>
        /// <param name="license">the license contents for the VaultApplication</param>
        public LicensedDispatcher(Dispatcher<TReturn> dispatcher, LicenseContentBase license = null)
        {
            this.license = license;
            this.dispatcher = dispatcher;
        }

        public override TReturn Dispatch()
        {
            var types =  GetTypes();

            if (!types.Any())
                return default;

            return HandleConcreteTypes(types);
        }

        protected internal override IEnumerable<Type> GetTypes()
        {
            var types = dispatcher.GetTypes();


            //If the license is not valid, remove all classes with the attribute [LicenseRequired]
            if (license == null || !license.IsValid)
            {
                var filteredTypes = types
                .Where(t =>
                   !t.IsDefined(typeof(LicenseRequiredAttribute), false)
                )
                .ToArray();

                return filteredTypes;
            }

            //If the license is valid, and the license has modules, 
            //remove classes which required licensing AND are in modules not contained in the licensed modules
            if (license?.Modules?.Any() == true)
            {
                var filteredTypes = types
                    .Where(t =>
                    {
                        //Keep types that don't require a license
                        if (!t.IsDefined(typeof(LicenseRequiredAttribute), false))
                            return true;

                        string[] modules = t.GetCustomAttribute<LicenseRequiredAttribute>().Modules;

                        //If it has no modules specified, keep it
                        if (modules == null || !modules.Any())
                            return true;
                        //Keep it only if one of the specified modules is licensed.
                        else
                            return modules.Intersect(license.Modules).Any();
                    }
                    );

                return filteredTypes;
            }

            return types;
        }

        protected internal override TReturn HandleConcreteTypes(IEnumerable<Type> types)
        {
            return dispatcher.HandleConcreteTypes(types);
        }
    }
}