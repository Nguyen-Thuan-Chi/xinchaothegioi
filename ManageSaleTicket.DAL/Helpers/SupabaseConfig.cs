using System;

namespace xinchaothegioi.Helpers
{
    public static class SupabaseConfig
    {
        public static string Url 
        { 
            get 
            {
                var v = ConfigHelper.GetAppSetting("SUPABASE_URL");
                if (!string.IsNullOrWhiteSpace(v)) return v;
                return Environment.GetEnvironmentVariable("SUPABASE_URL") ?? "https://vmzldhyhyutpohorxniw.supabase.co";
            }
        }
        
        public static string AnonKey 
        { 
            get 
            {
                var v = ConfigHelper.GetAppSetting("SUPABASE_ANON_KEY");
                if (!string.IsNullOrWhiteSpace(v)) return v;
                return Environment.GetEnvironmentVariable("SUPABASE_ANON_KEY") ?? string.Empty;
            }
        }
        
        public static string ServiceKey
        {
            get
            {
                var v = ConfigHelper.GetAppSetting("SUPABASE_SERVICE_KEY");
                if (!string.IsNullOrWhiteSpace(v)) return v;
                return Environment.GetEnvironmentVariable("SUPABASE_SERVICE_KEY");
            }
        }
        
        public static bool IsConfigured => !string.IsNullOrEmpty(Url) && !string.IsNullOrEmpty(AnonKey);
            
        // Ki?m tra k?t n?i Supabase
        public static bool IsSupabaseConnection()
        {
            return !string.IsNullOrEmpty(Url) && Url.Contains("supabase.co");
        }
        
        // L?y th?ng tin debug an to?n
        public static string GetDebugInfo()
        {
            return $"URL: {Url}\nAnon Key: ***configured***\nIs Configured: {IsConfigured}";
        }
    }
}