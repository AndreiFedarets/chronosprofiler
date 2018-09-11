using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Chronos
{
    public static class TypeExtensions
    {
        public static bool ImplementsInterface<T>(this Type type)
        {
            Type targetInterfaceType = typeof (T);
            return type.GetInterfaces().Any(x => x == targetInterfaceType);
        }

        public static Type GetFirstInterface(this Type type)
        {
            if (type.IsInterface)
            {
                return type;
            }
            Type[] interfaces = type.GetInterfaces();
            if (interfaces.Any())
            {
                return interfaces.First();
            }
            return null;
        }

        public static string GetAssemblyPath(this Type type)
        {
            Assembly assembly = type.Assembly;
            string location = assembly.Location;
            return Path.GetDirectoryName(location);
        }

        public static T GetCustomAttribute<T>(this MemberInfo element) where T : Attribute
        {
            object[] attributes = element.GetCustomAttributes(typeof (T), true);
            T attribute = default(T);
            if (attributes.Length > 0)
            {
                attribute = (T) attributes[0];
            }
            return attribute;
        }

        public static string GetFullyQualifiedName(this Type type)
        {
            string fullName = string.Format("{0}, {1}", type.FullName, type.Assembly.FullName);
            return fullName;
        }
    }
}
