using System;
using System.Configuration;

namespace xinchaothegioi.Helpers
{
    public static class SupabaseConfig
    {
        public static string Url 
        { 
            get 
            {
                try
                {
                    return ConfigurationManager.AppSettings["SUPABASE_URL"];
                }
                catch
                {
                    return "https://vmzldhyhyutpohorxniw.supabase.co"; // Fallback
                }
            }
        }
        
        public static string AnonKey 
        { 
            get 
            {
                try
                {
                    return ConfigurationManager.AppSettings["SUPABASE_ANON_KEY"];
                }
                catch
                {
                    return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZtemxkaHloeXV0cG9ob3J4bml3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3NTkxOTQyMjIsImV4cCI6MjA3NDc3MDIyMn0.cKSznrERTkgbEFsozWHU5fmACm3F5HQh02EjNJa24jk";
                }
            }
        }
        
        public static string ServiceKey => ConfigurationManager.AppSettings["SUPABASE_SERVICE_KEY"];
        
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