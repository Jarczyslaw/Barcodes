using Newtonsoft.Json;
using System.IO;

namespace Barcodes.Utils
{
    public static class Serializer
    {
        public static void ToFile<T>(T obj, string filePath)
        {
            var serialized = ToString(obj);
            File.WriteAllText(filePath, serialized);
        }

        public static T FromFile<T>(string filePath)
        {
            var serialized = File.ReadAllText(filePath);
            return FromString<T>(serialized);
        }

        public static string ToString<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static T FromString<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}