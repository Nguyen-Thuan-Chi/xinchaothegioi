using System;
using System.Data.SqlClient;
using System.Configuration;

namespace xinchaothegioi.Helpers
{
    public static class SqlServerHelper
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
        }

        public static bool TestConnection()
        {
            try
            {
                string connectionString = GetConnectionString();
                if (string.IsNullOrEmpty(connectionString))
                    return false;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Connection test error: {ex.Message}");
                return false;
            }
        }

        public static void CreateDatabaseIfNotExists()
        {
            try
            {
                // K?t n?i ??n master database ?? t?o database m?i
                string masterConnectionString = GetConnectionString().Replace("XinChaoTheGioiDB", "master");

                using (var connection = new SqlConnection(masterConnectionString))
                {
                    connection.Open();

                    // Ki?m tra database ?? t?n t?i ch?a
                    string checkDbQuery = "SELECT COUNT(*) FROM sys.databases WHERE name = 'XinChaoTheGioiDB'";
                    using (var cmd = new SqlCommand(checkDbQuery, connection))
                    {
                        int dbCount = (int)cmd.ExecuteScalar();
                        if (dbCount == 0)
                        {
                            // T?o database
                            string createDbQuery = "CREATE DATABASE XinChaoTheGioiDB";
                            using (var createCmd = new SqlCommand(createDbQuery, connection))
                            {
                                createCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine("Database created/verified successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Create database error: {ex.Message}");
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + "salt_xinchaothegioi"));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string hashOfInput = HashPassword(password);
            return string.Equals(hashOfInput, hashedPassword, StringComparison.Ordinal);
        }
    }
}