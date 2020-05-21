using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Barcodes.Utils
{
    public static class KeyParser
    {
        public static bool Check(string keys, Key key)
        {
            var keysToCheck = ExtractKeys(keys);
            return keysToCheck.Contains(key);
        }

        public static List<Key> ExtractKeys(string keys)
        {
            var result = new List<Key>();
            if (!string.IsNullOrEmpty(keys))
            {
                foreach (var splittedKey in keys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                {
                    if (Enum.TryParse(splittedKey.Trim(), out Key key))
                    {
                        result.Add(key);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid application settings key value");
                    }
                }
            }
            return result;
        }

        public static bool Validate(string keys)
        {
            try
            {
                ExtractKeys(keys);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}