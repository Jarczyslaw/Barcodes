using Microsoft.Win32;
using System.Linq;

namespace JToolbox.Desktop.Core
{
    public static class RegistryTools
    {
        private static readonly string runKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

        public static void SetStartup(string appName, string appPath)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(runKey, true))
            {
                key.SetValue(appName, appPath);
            }
        }

        public static void ClearStartup(string appName)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(runKey, true))
            {
                DeleteValue(key, appName);
            }
        }

        public static bool CheckStartup(string appName, string appPath)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(runKey, false))
            {
                return CheckValue(key, appName, appPath);
            }
        }

        public static void DeleteValue(RegistryKey registryKey, string valueName)
        {
            var values = registryKey.GetValueNames();
            if (values.Contains(valueName))
            {
                registryKey.DeleteValue(valueName);
            }
        }

        public static bool CheckValue(RegistryKey registryKey, string valueName, object valueData)
        {
            var values = registryKey.GetValueNames();
            if (values.Contains(valueName))
            {
                return Equals(registryKey.GetValue(valueName), valueData);
            }
            return false;
        }
    }
}