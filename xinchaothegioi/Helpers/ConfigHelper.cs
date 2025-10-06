using System;
using System.Configuration;

namespace xinchaothegioi.Helpers
{
    public static class ConfigHelper
    {
        public static string GetConnectionString(string name)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[name]?.ConnectionString;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading config: {ex.Message}");
                return null;
            }
        }
        
        public static string GetAppSetting(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading app setting: {ex.Message}");
                return null;
            }
        }
    }
}