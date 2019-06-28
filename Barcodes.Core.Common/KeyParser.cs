using System;
using System.Windows.Input;

namespace Barcodes.Core.Common
{
    public static class KeyParser
    {
        public static bool Check(string keys, Key key)
        {
            if (string.IsNullOrEmpty(keys))
            {
                return false;
            }

            foreach (var splittedKey in keys.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (Enum.TryParse(splittedKey.Trim(), out Key result) && key == result)
                {
                    return true;
                }
            }

            return false;
        }
    }
}