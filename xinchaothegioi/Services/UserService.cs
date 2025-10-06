using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using xinchaothegioi.Data;
using xinchaothegioi.Models;
using xinchaothegioi.Helpers;

namespace xinchaothegioi.Services
{
    public class UserService : IDisposable
    {
        private readonly ApplicationDbContext _context;

        public UserService()
        {
            _context = new ApplicationDbContext();
        }

        public bool TestConnection()
        {
            try
            {
                // Test kết nối bằng cách kiểm tra database tồn tại
                return _context.Database.Exists();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Connection test error: {ex.Message}");
                return false;
            }
        }

        public bool RegisterUser(string username, string password)
        {
            try
            {
                // Kiểm tra username đã tồn tại
                if (_context.Users.Any(u => u.Username.ToLower() == username.ToLower()))
                {
                    return false;
                }

                // Hash password bằng SqlServerHelper
                string hashedPassword = SqlServerHelper.HashPassword(password);

                // Tạo user mới
                var user = new User
                {
                    Username = username,
                    Password = hashedPassword,
                    Role = "User",
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };

                _context.Users.Add(user);
                int result = _context.SaveChanges();

                return result > 0;
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
                var user = _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.IsActive);
                if (user == null) return false;

                return SqlServerHelper.VerifyPassword(password, user.Password);
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
                return _context.Users.FirstOrDefault(u => u.Username.ToLower() == username.ToLower() && u.IsActive);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Get user error: {ex.Message}");
                return null;
            }
        }

        public void InitializeDatabase()
        {
            try
            {
                // Tạo database nếu chưa tồn tại
                SqlServerHelper.CreateDatabaseIfNotExists();
                
                // Tạo bảng nếu chưa tồn tại
                if (_context.Database.Exists())
                {
                    _context.Database.CreateIfNotExists();
                    
                    // Tạo admin user mặc định nếu chưa có
                    if (!_context.Users.Any(u => u.Username.ToLower() == "admin"))
                    {
                        var adminUser = new User
                        {
                            Username = "admin",
                            Password = SqlServerHelper.HashPassword("admin123"),
                            Role = "Admin",
                            CreatedDate = DateTime.Now,
                            IsActive = true
                        };
                        
                        _context.Users.Add(adminUser);
                        _context.SaveChanges();
                        
                        System.Diagnostics.Debug.WriteLine("Admin user created successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Initialize database error: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}