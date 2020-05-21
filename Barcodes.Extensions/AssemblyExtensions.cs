using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Barcodes.Extensions
{
    public static class AssemblyExtensions
    {
        public static List<Type> GetDerivedTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes()
                .Where(t => baseType.IsAssignableFrom(t) && t != baseType)
                .ToList();
        }
    }
}