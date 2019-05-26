using System.Configuration;
using System.Runtime.CompilerServices;

namespace Barcodes.Startup
{
    public static class AppConfig
    {
        public static bool MultiInst
        {
            get
            {
                var value = GetConfigValue();
                if (bool.TryParse(value, out bool result))
                {
                    return result;
                }
                return false;
            }
        }

        private static string GetConfigValue([CallerMemberName] string key = null)
        {
            return ConfigurationManager.AppSettings.Get(key);
        }
    }
}