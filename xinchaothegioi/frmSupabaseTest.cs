using System;
using System.Windows.Forms;
using xinchaothegioi.Services;
using xinchaothegioi.Helpers;

namespace xinchaothegioi
{
    public partial class frmSupabaseTest : Form
    {
        private Label lblStatus;
        private Button btnTestConnection;
        private Button btnTestRegister;
        private Button btnTestLogin;
        private Button btnGetUsers;
        private Button btnClose;

        public frmSupabaseTest()
        {
            InitializeComponents();
            this.Text = "SQL Server Test";
        }

        private void InitializeComponents()
        {
            this.Size = new System.Drawing.Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblStatus = new Label();
            lblStatus.Location = new System.Drawing.Point(20, 20);
            lblStatus.Size = new System.Drawing.Size(450, 50);
            lblStatus.Text = "S?n s?ng...";
            lblStatus.BackColor = System.Drawing.Color.LightGray;
            lblStatus.BorderStyle = BorderStyle.FixedSingle;

            btnTestConnection = new Button();
            btnTestConnection.Text = "Test Connection";
            btnTestConnection.Location = new System.Drawing.Point(20, 90);
            btnTestConnection.Size = new System.Drawing.Size(120, 30);
            btnTestConnection.Click += btnTestConnection_Click;

            btnTestRegister = new Button();
            btnTestRegister.Text = "Test Register";
            btnTestRegister.Location = new System.Drawing.Point(160, 90);
            btnTestRegister.Size = new System.Drawing.Size(120, 30);
            btnTestRegister.Click += btnTestRegister_Click;

            btnTestLogin = new Button();
            btnTestLogin.Text = "Test Login";
            btnTestLogin.Location = new System.Drawing.Point(300, 90);
            btnTestLogin.Size = new System.Drawing.Size(120, 30);
            btnTestLogin.Click += btnTestLogin_Click;

            btnGetUsers = new Button();
            btnGetUsers.Text = "Get Users";
            btnGetUsers.Location = new System.Drawing.Point(20, 140);
            btnGetUsers.Size = new System.Drawing.Size(120, 30);
            btnGetUsers.Click += btnGetUsers_Click;

            btnClose = new Button();
            btnClose.Text = "Close";
            btnClose.Location = new System.Drawing.Point(300, 300);
            btnClose.Size = new System.Drawing.Size(120, 30);
            btnClose.Click += btnClose_Click;

            this.Controls.Add(lblStatus);
            this.Controls.Add(btnTestConnection);
            this.Controls.Add(btnTestRegister);
            this.Controls.Add(btnTestLogin);
            this.Controls.Add(btnGetUsers);
            this.Controls.Add(btnClose);
        }

        private void Form_Load(object sender, EventArgs e)
        {
            UpdateStatus("S?n s?ng test SQL Server...");
        }

        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(UpdateStatus), message);
                return;
            }
            
            lblStatus.Text = $"[{DateTime.Now:HH:mm:ss}] {message}";
            Application.DoEvents();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("?ang test k?t n?i SQL Server...");
                
                bool isConnected = SqlServerHelper.TestConnection();
                
                if (isConnected)
                {
                    UpdateStatus("? K?t n?i SQL Server th?nh c?ng!");
                    MessageBox.Show("K?t n?i SQL Server th?nh c?ng!", "Th?nh c?ng", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateStatus("? K?t n?i SQL Server th?t b?i!");
                    MessageBox.Show("K?t n?i SQL Server th?t b?i!\nKi?m tra l?i c?u h?nh.", "L?i", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"? L?i: {ex.Message}");
                MessageBox.Show($"L?i test connection:\n{ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestRegister_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("?ang test ??ng k? user...");
                
                string testUsername = "testuser_" + DateTime.Now.Ticks;
                string testPassword = "test123456";
                
                using (var userService = new UserService())
                {
                    bool success = userService.RegisterUser(testUsername, testPassword);
                    
                    if (success)
                    {
                        UpdateStatus($"? ??ng k? user '{testUsername}' th?nh c?ng!");
                        MessageBox.Show($"??ng k? user '{testUsername}' th?nh c?ng!", "Th?nh c?ng", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus($"? ??ng k? user '{testUsername}' th?t b?i!");
                        MessageBox.Show("??ng k? user th?t b?i!", "L?i", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"? L?i: {ex.Message}");
                MessageBox.Show($"L?i test register:\n{ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTestLogin_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("?ang test ??ng nh?p...");
                
                // Test v?i user admin m?c ??nh
                string username = "admin";
                string password = "admin123";
                
                using (var userService = new UserService())
                {
                    // Kh?i t?o database tr??c (s? t?o admin user t? ??ng)
                    userService.InitializeDatabase();
                    
                    bool isValid = userService.ValidateUser(username, password);
                    
                    if (isValid)
                    {
                        var user = userService.GetUser(username);
                        UpdateStatus($"? ??ng nh?p th?nh c?ng v?i user: {user?.Username} (Role: {user?.Role})");
                        MessageBox.Show($"??ng nh?p th?nh c?ng!\nUser: {user?.Username}\nRole: {user?.Role}", 
                            "Th?nh c?ng", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus("? ??ng nh?p th?t b?i!");
                        MessageBox.Show("??ng nh?p th?t b?i!", "L?i", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"? L?i: {ex.Message}");
                MessageBox.Show($"L?i test login:\n{ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGetUsers_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateStatus("?ang l?y danh s?ch users...");
                
                using (var userService = new UserService())
                {
                    // V? kh?ng c? GetAllUsers, ta s? th?ng b?o s? l??ng user th?ng qua test
                    var testUser = userService.GetUser("admin");
                    if (testUser != null)
                    {
                        UpdateStatus($"? T?m th?y user: {testUser.Username} (Created: {testUser.CreatedDate})");
                        MessageBox.Show($"Database ?ang ho?t ??ng!\nT?m th?y user: {testUser.Username}\nRole: {testUser.Role}\nCreated: {testUser.CreatedDate}", 
                            "Th?ng tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        UpdateStatus("?? Ch?a c? user n?o trong database");
                        MessageBox.Show("Ch?a c? user n?o trong database", "Th?ng tin", 
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"? L?i: {ex.Message}");
                MessageBox.Show($"L?i get users:\n{ex.Message}", "L?i", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}