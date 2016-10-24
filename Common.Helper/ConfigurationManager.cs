using System;
using System.Configuration;

namespace Common.Helper
{
    public static class WebConfigurationManager
    {
        public static string GetValue(string key)
        {
            return GetValue(key, typeof(string));
        }

        public static string GetValue(string key, Type type)
        {
            var value = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrEmpty(value)) return value;

            if (type == typeof(int) || type == typeof(Int64) || type == typeof(double) || type == typeof(decimal) || type == typeof(float))
                return "0";
            else
                return string.Empty;
        }

        public static bool GetBoolean(string key)
        {
            bool value;
            bool.TryParse(ConfigurationManager.AppSettings[key], out value);
            return value;
        }
    }
}
