using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using xinchaothegioi.Helpers;
using xinchaothegioi.Models;

namespace xinchaothegioi.Services
{
    /// <summary>
    /// UserService s? d?ng SqlConnection tr?c ti?p thay v? Entity Framework
    /// Gi?i ph?p thay th? khi Package Manager Console b? l?i
    /// </summary>
    public class PlainSqlUserService : IDisposable
    {
        private readonly string _connectionString;

        public PlainSqlUserService()
        {
            _connectionString = SqlServerHelper.GetConnectionString();
        }

        public bool TestConnection()
        {
            return SqlServerHelper.TestConnection();
        }

        public void InitializeDatabase()
        {
            try
            {
                // T?o database n?u ch?a c?
                SqlServerHelper.CreateDatabaseIfNotExists();
                
                // T?o b?ng Users n?u ch?a c?
                CreateUsersTableIfNotExists();
                
                // T?o admin user m?c ??nh
                CreateDefaultAdminUser();
                
                System.Diagnostics.Debug.WriteLine("Database initialized successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Initialize database error: {ex.Message}");
                throw;
            }
        }

        private void CreateUsersTableIfNotExists()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string checkTableQuery = @"
                        SELECT COUNT(*) 
                        FROM INFORMATION_SCHEMA.TABLES 
                        WHERE TABLE_NAME = 'Users'";
                    
                    using (var cmd = new SqlCommand(checkTableQuery, connection))
                    {
                        int tableCount = (int)cmd.ExecuteScalar();
                        
                        if (tableCount == 0)
                        {
                            string createTableQuery = @"
                                CREATE TABLE Users (
                                    UserId INT IDENTITY(1,1) PRIMARY KEY,
                                    Username NVARCHAR(50) NOT NULL UNIQUE,
                                    Password NVARCHAR(255) NOT NULL,
                                    Role NVARCHAR(20) NOT NULL DEFAULT 'User',
                                    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
                                    IsActive BIT NOT NULL DEFAULT 1
                                )";
                            
                            using (var createCmd = new SqlCommand(createTableQuery, connection))
                            {
                                createCmd.ExecuteNonQuery();
                            }
                            
                            System.Diagnostics.Debug.WriteLine("Users table created successfully");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Create table error: {ex.Message}");
                throw;
            }
        }

        private void CreateDefaultAdminUser()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Ki?m tra admin user ?? t?n t?i ch?a
                    string checkAdminQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using (var cmd = new SqlCommand(checkAdminQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", "admin");
                        int adminCount = (int)cmd.ExecuteScalar();
                        
                        if (adminCount == 0)
                        {
                            string insertAdminQuery = @"
                                INSERT INTO Users (Username, Password, Role, CreatedDate, IsActive) 
                                VALUES (@username, @password, @role, @createdDate, @isActive)";
                            
                            using (var insertCmd = new SqlCommand(insertAdminQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@username", "admin");
                                insertCmd.Parameters.AddWithValue("@password", SqlServerHelper.HashPassword("admin123"));
                                insertCmd.Parameters.AddWithValue("@role", "Admin");
                                insertCmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                                insertCmd.Parameters.AddWithValue("@isActive", true);
                                
                                insertCmd.ExecuteNonQuery();
                            }
                            
                            System.Diagnostics.Debug.WriteLine("Default admin user created");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Create admin user error: {ex.Message}");
            }
        }

        public bool RegisterUser(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    // Ki?m tra username ?? t?n t?i ch?a
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                    using (var checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int userCount = (int)checkCmd.ExecuteScalar();
                        
                        if (userCount > 0)
                        {
                            return false; // Username ?? t?n t?i
                        }
                    }
                    
                    // Th?m user m?i
                    string insertQuery = @"
                        INSERT INTO Users (Username, Password, Role, CreatedDate, IsActive) 
                        VALUES (@username, @password, @role, @createdDate, @isActive)";
                    
                    using (var insertCmd = new SqlCommand(insertQuery, connection))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@password", SqlServerHelper.HashPassword(password));
                        insertCmd.Parameters.AddWithValue("@role", "User");
                        insertCmd.Parameters.AddWithValue("@createdDate", DateTime.Now);
                        insertCmd.Parameters.AddWithValue("@isActive", true);
                        
                        int rowsAffected = insertCmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Register user error: {ex.Message}");
                return false;
            }
        }

        public bool ValidateUser(string username, string password)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = "SELECT Password FROM Users WHERE Username = @username AND IsActive = 1";
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        
                        var storedPassword = cmd.ExecuteScalar() as string;
                        if (storedPassword != null)
                        {
                            return SqlServerHelper.VerifyPassword(password, storedPassword);
                        }
                        
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Validate user error: {ex.Message}");
                return false;
            }
        }

        public User GetUser(string username)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        SELECT UserId, Username, Role, CreatedDate, IsActive 
                        FROM Users 
                        WHERE Username = @username AND IsActive = 1";
                    
                    using (var cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new User
                                {
                                    UserId = reader.GetInt32("UserId"),
                                    Username = reader.GetString("Username"),
                                    Role = reader.GetString("Role"),
                                    CreatedDate = reader.GetDateTime("CreatedDate"),
                                    IsActive = reader.GetBoolean("IsActive")
                                };
                            }
                        }
                    }
                }
                
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Get user error: {ex.Message}");
                return null;
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    
                    string query = @"
                        SELECT UserId, Username, Role, CreatedDate, IsActive 
                        FROM Users 
                        ORDER BY CreatedDate DESC";
                    
                    using (var cmd = new SqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetInt32("UserId"),
                                Username = reader.GetString("Username"),
                                Role = reader.GetString("Role"),
                                CreatedDate = reader.GetDateTime("CreatedDate"),
                                IsActive = reader.GetBoolean("IsActive")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Get all users error: {ex.Message}");
            }
            
            return users;
        }

        public void Dispose()
        {
            // Nothing to dispose for plain SQL connections
        }
    }
}