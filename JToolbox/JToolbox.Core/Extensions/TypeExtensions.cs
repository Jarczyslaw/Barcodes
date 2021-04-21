using System;
using System.Linq;

namespace JToolbox.Core.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsSimpleType(this Type @this)
        {
            var additionalTypes = new Type[]
            {
                typeof(string),
                typeof(decimal),
                typeof(DateTime),
                typeof(DateTimeOffset),
                typeof(TimeSpan),
                typeof(Guid)
            };
            return @this.IsPrimitive
                || additionalTypes.Contains(@this) || @this.IsEnum || Convert.GetTypeCode(@this) != TypeCode.Object
                || (@this.IsGenericType && @this.GetGenericTypeDefinition() == typeof(Nullable<>) && IsSimpleType(@this.GetGenericArguments()[0]));
        }
    }
}