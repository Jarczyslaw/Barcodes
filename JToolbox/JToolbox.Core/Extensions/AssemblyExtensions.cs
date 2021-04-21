using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace JToolbox.Core.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetTypesSubclassOf<T>(this Assembly assembly)
        {
            return assembly.GetTypesSubclassOf(typeof(T));
        }

        public static IEnumerable<Type> GetTypesSubclassOf(this Assembly assembly, Type type)
        {
            return assembly.GetTypes(t1 => t1.IsSubclassOf(type));
        }

        public static IEnumerable<Type> GetTypesImplements<T>(this Assembly assembly)
        {
            return assembly.GetTypesImplements(typeof(T));
        }

        public static IEnumerable<Type> GetTypesImplements(this Assembly assembly, Type type)
        {
            return assembly.GetTypes(t1 => t1.GetInterfaces().Contains(type));
        }

        public static IEnumerable<Type> GetTypes(this Assembly assembly, Predicate<Type> predicate)
        {
            return assembly.GetTypes()
                .Where(t => predicate(t));
        }
    }
}