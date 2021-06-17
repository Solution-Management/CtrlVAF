using System;

namespace CtrlVAF.Utilities
{
    public static class ObjectUtilities
    {
        /// <summary>
        /// Method to obtain a value via a getter method and return it.
        /// </summary>
        /// <typeparam name="T">The type retrieved by the getter function</typeparam>
        /// <param name="PropertyGetter">The value getter function</param>
        /// <param name="value">The value retrieved by the getter</param>
        /// <returns>True if property has value, false otherwise</returns>
        public static bool HasValue<T>(Func<T> PropertyGetter, out T value)
        {
            value = GetPropertySafe(PropertyGetter);
            return value != null;
        }

        /// <summary>
        /// Null pointer safe method to retrive data via a property getter function
        /// </summary>
        /// <typeparam name="T">The type retrieved by the getter function</typeparam>
        /// <param name="PropertyGetter">The value getter function</param>
        /// <returns>The value of the property, default if null or an exception occurs</returns>
        public static T GetPropertySafe<T>(Func<T> PropertyGetter)
        {
            try
            {
                var value = PropertyGetter.Invoke();
                return value;
            }
            catch (Exception)
            {
                return default;
            }
        }

        /// <summary>
        /// Null pointer safe method to check if a value has changed from an old object to a new one.
        /// Used to compare two objects of the same type for changes.
        /// </summary>
        /// <typeparam name="T">The type retrieved by the getter functions</typeparam>
        /// <param name="PropertyGetterOld">The first value getter function</param>
        /// <param name="PropertyGetterNew">The second value getter function</param>
        /// <returns>True if the getter functions retrieve different values, false otherwise</returns>
        public static bool PropertyChangedSafe<T>(Func<T> PropertyGetterOld, Func<T> PropertyGetterNew)
        {
            var oldValue = GetPropertySafe(PropertyGetterOld);
            var newValue = GetPropertySafe(PropertyGetterNew);
            return !Equals(oldValue, newValue);
        }
    }
}