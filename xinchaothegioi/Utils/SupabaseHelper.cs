using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using xinchaothegioi.Helpers;
using xinchaothegioi.Models;
using Npgsql;

namespace xinchaothegioi.Utilities
{
    public static class SupabaseHelper
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        
        static SupabaseHelper()
        {
            _httpClient.DefaultRequestHeaders.Add("apikey", SupabaseConfig.AnonKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {SupabaseConfig.AnonKey}");
            _httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");
            _httpClient.DefaultRequestHeaders.Add("Prefer", "return=representation");
        }
        
        /// <summary>
        /// Test k?t n?i ??n Supabase database
        /// </summary>
        public static bool TestSupabaseConnection()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT 1", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine("Supabase connection test successful");
                        return result != null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Supabase connection test failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// Test k?t n?i ??n Supabase REST API
        /// </summary>
        public static async Task<bool> TestSupabaseRestApiAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{SupabaseConfig.Url}/rest/v1/");
                System.Diagnostics.Debug.WriteLine($"Supabase REST API test: {response.StatusCode}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Supabase REST API test failed: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// T?o b?ng users n?u ch?a t?n t?i
        /// </summary>
        public static bool CreateUsersTableIfNotExists()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    
                    // Ki?m tra xem b?ng users ?? t?n t?i ch?a
                    string checkTableQuery = @"
                        SELECT EXISTS (
                            SELECT FROM information_schema.tables 
                            WHERE table_schema = 'public' 
                            AND table_name = 'users'
                        );";
                    
                    bool tableExists = false;
                    using (var checkCmd = new NpgsqlCommand(checkTableQuery, connection))
                    {
                        tableExists = (bool)checkCmd.ExecuteScalar();
                    }
                    
                    if (!tableExists)
                    {
                        string createUsersTable = @"
                            CREATE TABLE users (
                                user_id SERIAL PRIMARY KEY,
                                username VARCHAR(50) NOT NULL UNIQUE,
                                password VARCHAR(255) NOT NULL,
                                role VARCHAR(20) NOT NULL DEFAULT 'User',
                                full_name VARCHAR(100),
                                phone_number VARCHAR(15),
                                email VARCHAR(100),
                                created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
                                is_active BOOLEAN DEFAULT TRUE
                            );
                            
                            CREATE INDEX idx_users_username ON users(username);
                            CREATE INDEX idx_users_email ON users(email);";
                        
                        using (var cmd = new NpgsqlCommand(createUsersTable, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        
                        System.Diagnostics.Debug.WriteLine("B?ng users ?? ???c t?o th?nh c?ng");
                        
                        // T?o admin user m?c ??nh
                        CreateDefaultAdminUser(connection);
                        
                        return true;
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("B?ng users ?? t?n t?i");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"L?i t?o b?ng users: {ex.Message}");
                return false;
            }
        }
        
        /// <summary>
        /// T?o admin user m?c ??nh
        /// </summary>
        private static void CreateDefaultAdminUser(NpgsqlConnection connection)
        {
            try
            {
                // Ki?m tra xem admin ?? t?n t?i ch?a
                string checkAdmin = "SELECT COUNT(*) FROM users WHERE username = 'admin'";
                using (var cmd = new NpgsqlCommand(checkAdmin, connection))
                {
                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count == 0)
                    {
                        string hashedPassword = DatabaseHelper.HashPassword("admin123");
                        string insertAdmin = @"
                            INSERT INTO users (username, password, role, full_name, email) 
                            VALUES (@username, @password, 'Admin', 'Administrator', 'admin@cinema.com')";
                        
                        using (var insertCmd = new NpgsqlCommand(insertAdmin, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@username", "admin");
                            insertCmd.Parameters.AddWithValue("@password", hashedPassword);
                            insertCmd.ExecuteNonQuery();
                        }
                        
                        System.Diagnostics.Debug.WriteLine("T?i kho?n admin ?? ???c t?o (username: admin, password: admin123)");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"L?i t?o admin user: {ex.Message}");
            }
        }
        
        /// <summary>
        /// L?y th?ng tin phi?n b?n PostgreSQL
        /// </summary>
        public static string GetPostgreSQLVersion()
        {
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT version()", connection))
                    {
                        return cmd.ExecuteScalar()?.ToString() ?? "Unknown";
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        
        /// <summary>
        /// L?y th?ng tin t?t c? b?ng trong database
        /// </summary>
        public static List<string> GetAllTables()
        {
            var tables = new List<string>();
            try
            {
                using (var connection = DatabaseHelper.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT table_name 
                        FROM information_schema.tables 
                        WHERE table_schema = 'public' 
                        ORDER BY table_name";
                    
                    using (var cmd = new NpgsqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(reader.GetString(0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting tables: {ex.Message}");
            }
            
            return tables;
        }
        
        /// <summary>
        /// L?y th?ng tin debug v? Supabase
        /// </summary>
        public static string GetSupabaseDebugInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== SUPABASE CONNECTION INFO ===");
            sb.AppendLine($"URL: {SupabaseConfig.Url}");
            sb.AppendLine($"Anon Key: {(string.IsNullOrEmpty(SupabaseConfig.AnonKey) ? "NOT SET" : "SET")}");
            sb.AppendLine($"Is Configured: {SupabaseConfig.IsConfigured}");
            sb.AppendLine($"Connection String: {DatabaseHelper.GetSafeConnectionString()}");
            sb.AppendLine($"PostgreSQL Version: {GetPostgreSQLVersion()}");
            
            var tables = GetAllTables();
            sb.AppendLine($"Tables Count: {tables.Count}");
            if (tables.Count > 0)
            {
                sb.AppendLine("Tables:");
                foreach (var table in tables)
                {
                    sb.AppendLine($"  - {table}");
                }
            }
            
            return sb.ToString();
        }
    }
}