using System;
using System.Windows.Forms;
using xinchaothegioi.Helpers;
using xinchaothegioi.Services;

namespace xinchaothegioi
{
    public partial class Program
    {
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Test SQL Server connection tr??c khi start app
            TestSqlServerConnection();
            
            Application.Run(new frmLogin());
        }
        
        private static void TestSqlServerConnection()
        {
            try
            {
                Console.WriteLine("Testing SQL Server connection...");
                
                // Test connection
                bool canConnect = SqlServerHelper.TestConnection();
                Console.WriteLine($"SQL Server Connection: {(canConnect ? "SUCCESS" : "FAILED")}");
                
                if (canConnect)
                {
                    // Test database operations
                    using (var userService = new UserService())
                    {
                        // Try to create a test admin user
                        userService.RegisterUser("admin", "admin123", "Admin");
                        Console.WriteLine("Admin user created/verified successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"SQL Server Test Error: {ex.Message}");
            }
        }
    }
}