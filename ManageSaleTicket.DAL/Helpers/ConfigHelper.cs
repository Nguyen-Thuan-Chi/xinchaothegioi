using System;
// using System.Configuration;

namespace xinchaothegioi.Helpers
{
    public static class ConfigHelper
    {
        public static string GetConnectionString(string name)
        {
            return Environment.GetEnvironmentVariable(name);
        }
        
        public static string GetAppSetting(string key)
        {
            return Environment.GetEnvironmentVariable(key);
        }
    }
}