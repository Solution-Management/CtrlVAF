using MFiles.VAF.Configuration;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CtrlVAF.Core
{
    public static class Dispatcher_Helpers
    {
        public static object GetConfigSubProperty(object config, Type configSubType)
        {
            if (config.GetType() == configSubType)
                return config;

            var configProperties = config.GetType().GetProperties();

            foreach (var property in configProperties)
            {
                if (!property.PropertyType.IsClass ||
                    property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(MFIdentifier) ||
                    typeof(ICollection).IsAssignableFrom(property.PropertyType)
                    )
                    continue;

                var subConfig = property.GetValue(config);

                if (property.PropertyType == configSubType)
                    return subConfig;

                var subsubConfig = GetConfigSubProperty(subConfig, configSubType);
                if (subsubConfig == null)
                    continue;
                else
                    return subsubConfig;
            }

            return null;
        }

        public static bool IsConfigSubProperty(Type parent, Type child)
        {
            if (parent == child)
                return false;

            foreach (PropertyInfo property in parent.GetProperties())
            {
                if (!property.PropertyType.IsClass)
                    continue;

                if (property.PropertyType == child)
                    return true;

                if (IsConfigSubProperty(property.PropertyType, child))
                    return true;
            }

            return false;
        }

        public static bool[] AreConfigSubProperties(Type parent, params Type[] children)
        {

            if (children.Count() != children.Distinct().Count())
                throw new InvalidOperationException("Found duplicate types in parameters for " + nameof(AreConfigSubProperties));

            Dictionary<Type, bool> foundChildren = new Dictionary<Type, bool>();

            foreach (var child in children)
            {
                foundChildren.Add(child, false);
            }

            if(children.Contains(parent))
            {
                foundChildren[parent] = true;
            }

            foreach (PropertyInfo property in parent.GetProperties())
            {
                if (!property.PropertyType.IsClass ||
                    property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(MFIdentifier) ||
                    typeof(ICollection).IsAssignableFrom(property.PropertyType)
                    )
                    continue;

                if (children.Contains(property.PropertyType))
                {
                    foundChildren[property.PropertyType] = true;
                }
                
                if (foundChildren.Values.Contains(false))
                {
                    Type[] unfoundChildren = foundChildren.Where(kv => !kv.Value).Select(kv => kv.Key).ToArray();
                    var results = AreConfigSubProperties(property.PropertyType, unfoundChildren);

                    for (int i = 0; i < unfoundChildren.Length; i++)
                    {
                        foundChildren[unfoundChildren[i]] = results[i];
                    }
                }
            }

            return foundChildren.Values.ToArray();
        }
    }
}
