using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

namespace xinchaothegioi.Helpers
{
    public static class DatabaseHelper
    {
        // Hard-code connection string ?? tr?nh l?i ConfigurationManager
        private static readonly string _connectionString = 
            "Host=db.vmzldhyhyutpohorxniw.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=Jonhejan14@gmail.com;SSL Mode=Require;Trust Server Certificate=true;";
        
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
        
        public static bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    using (var cmd = new NpgsqlCommand("SELECT version()", connection))
                    {
                        var version = cmd.ExecuteScalar();
                        System.Diagnostics.Debug.WriteLine($"PostgreSQL Version: {version}");
                    }
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Test connection failed: {ex.Message}");
                return false;
            }
        }
        
        public static void CreateTablesIfNotExists()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    
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
                            CREATE INDEX idx_users_username ON users(username);";
                        
                        using (var cmd = new NpgsqlCommand(createUsersTable, connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        
                        System.Diagnostics.Debug.WriteLine("B?ng users ?? ???c t?o th?nh c?ng");
                    }
                    
                    string checkAdmin = "SELECT COUNT(*) FROM users WHERE username = 'admin'";
                    using (var cmd = new NpgsqlCommand(checkAdmin, connection))
                    {
                        var count = Convert.ToInt32(cmd.ExecuteScalar());
                        if (count == 0)
                        {
                            string hashedPassword = HashPassword("admin123");
                            string insertAdmin = @"
                                INSERT INTO users (username, password, role, full_name, email) 
                                VALUES (@username, @password, 'Admin', 'Administrator', 'admin@cinema.com')";
                            
                            using (var insertCmd = new NpgsqlCommand(insertAdmin, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@username", "admin");
                                insertCmd.Parameters.AddWithValue("@password", hashedPassword);
                                insertCmd.ExecuteNonQuery();
                            }
                            
                            System.Diagnostics.Debug.WriteLine("T?i kho?n admin ?? ???c t?o");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"L?i t?o b?ng: {ex.Message}");
            }
        }
        
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password + "CinemaApp2024"));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, hashedPassword) == 0;
        }
        
        public static string GetSafeConnectionString()
        {
            return "Host=db.vmzldhyhyutpohorxniw.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=***;SSL Mode=Require;Trust Server Certificate=true;";
        }
        
        public static bool IsUsingSupabase()
        {
            return _connectionString.Contains("supabase.co");
        }
    }
}