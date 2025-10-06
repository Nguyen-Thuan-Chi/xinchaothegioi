using System;
using System.Windows.Forms;
using xinchaothegioi.Services;
using xinchaothegioi.Helpers;

namespace xinchaothegioi
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            InitializeDatabase();
        }
        
        private void InitializeDatabase()
        {
            try
            {
                // Tạo database nếu chưa tồn tại
                SqlServerHelper.CreateDatabaseIfNotExists();
                
                // Kiểm tra kết nối và khởi tạo database nếu cần
                using (var userService = new UserService())
                {
                    if (userService.TestConnection())
                    {
                        // Database sẽ được tự động tạo bởi Entity Framework
                        System.Diagnostics.Debug.WriteLine("Database initialized successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo database:\n{ex.Message}", 
                    "Lỗi Database", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Text.Trim();
                
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Thiếu thông tin", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUsername.Focus();
                    return;
                }
                
                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu.", "Thiếu thông tin", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
                
                using (var userService = new UserService())
                {
                    // Kiểm tra kết nối trước
                    if (!userService.TestConnection())
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu SQL Server!\nVui lòng kiểm tra:\n" +
                            "- SQL Server đang chạy\n" +
                            "- Thông tin kết nối trong App.config\n" +
                            "- Quyền truy cập database", 
                            "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    
                    // Xác thực người dùng
                    if (userService.ValidateUser(username, password))
                    {
                        var user = userService.GetUser(username);
                        if (user != null)
                        {
                            MessageBox.Show($"Đăng nhập thành công!\nChào mừng {user.Username}\nQuyền: {user.Role}", 
                                "Đăng nhập thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Có lỗi xảy ra khi lấy thông tin người dùng.", 
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", 
                            "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtPassword.Clear();
                        txtUsername.Focus();
                        txtUsername.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi đăng nhập:\n{ex.Message}", 
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }
        
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmRegister registerForm = new frmRegister();
            registerForm.ShowDialog();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
