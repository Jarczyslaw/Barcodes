using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Utils
{
    public static class Serializer
    {
        public static void ToFile<T>(T obj, string filePath)
        {
            var serialized = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, serialized);
        }

        public static T FromFile<T>(string filePath)
        {
            var serialized = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(serialized);
        }
    }
}
