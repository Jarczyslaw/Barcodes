using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace JToolbox.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string PublicPropertiesToString(this object @this)
        {
            return @this.PropertiesToString(BindingFlags.Public | BindingFlags.Instance);
        }

        public static string AllPropertiesToString(this object @this)
        {
            return @this.PropertiesToString(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static string PropertiesToString(this object @this, BindingFlags flags)
        {
            var result = new StringBuilder();
            var props = @this.GetType().GetProperties(flags);
            for (int i = 0; i < props.Length; i++)
            {
                var prop = props[i];
                var propVal = prop.GetValue(@this, null);
                var propStr = $"{prop.Name} = {(propVal != null ? propVal.ToString() : string.Empty)}";
                if (i != props.Length - 1)
                {
                    result.AppendLine(propStr);
                }
                else
                {
                    result.Append(propStr);
                }
            }
            return result.ToString();
        }

        public static T DeepClone<T>(this object obj)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}