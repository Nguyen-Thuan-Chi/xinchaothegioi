using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xinchaothegioi
{
    public partial class frmRegister : Form
    {
        public frmRegister()
        {
            InitializeComponent();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ form
                string username = txtName.Text.Trim();
                string password = txtPassword.Text;
                string confirmPassword = txtConfirmPassword.Text;

                // Validation
                if (string.IsNullOrEmpty(username))
                {
                    MessageBox.Show("Vui lòng nhập tên đăng nhập!", "Thiếu thông tin", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Vui lòng nhập mật khẩu!", "Thiếu thông tin", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (password != confirmPassword)
                {
                    MessageBox.Show("Mật khẩu xác nhận không khớp!", "Lỗi xác nhận", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConfirmPassword.Focus();
                    return;
                }

                if (password.Length < 6)
                {
                    MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự!", "Mật khẩu không hợp lệ", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }

                if (username.Length < 3)
                {
                    MessageBox.Show("Tên đăng nhập phải có ít nhất 3 ký tự!", "Tên đăng nhập không hợp lệ", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                // Sử dụng UserService để đăng ký với SQL Server
                using (var userService = new Services.UserService())
                {
                    // Kiểm tra kết nối database
                    if (!userService.TestConnection())
                    {
                        MessageBox.Show("Không thể kết nối đến cơ sở dữ liệu SQL Server!\nVui lòng kiểm tra:\n" +
                            "- SQL Server đang chạy\n" +
                            "- Thông tin kết nối trong App.config\n" +
                            "- Quyền truy cập database", 
                            "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Khởi tạo database và bảng nếu cần
                    userService.InitializeDatabase();

                    // Thực hiện đăng ký
                    bool success = userService.RegisterUser(username, password);
                    
                    if (success)
                    {
                        MessageBox.Show($"Đăng ký tài khoản '{username}' thành công!\nBạn có thể đăng nhập ngay bây giờ.", 
                            "Đăng ký thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // Clear form và đóng form đăng ký
                        ClearForm();
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Đăng ký thất bại!\nTên đăng nhập '{username}' đã tồn tại.", 
                            "Đăng ký thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtName.Focus();
                        txtName.SelectAll();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi đăng ký:\n{ex.Message}", 
                    "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            txtName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                // Hiển thị thông báo đang test
                this.Cursor = Cursors.WaitCursor;
                btnTestConnection.Enabled = false;
                btnTestConnection.Text = "Testing...";
                Application.DoEvents();
                
                // Test connection qua UserService
                using (var userService = new Services.UserService())
                {
                    bool isConnected = userService.TestConnection();
                    
                    if (isConnected)
                    {
                        MessageBox.Show("✅ Kết nối SQL Server thành công!\n\n" +
                            "Server: LAPTOP-PN16MELH\n" +
                            "Database: XinChaoTheGioiDB\n" +
                            "Authentication: Windows Authentication\n" +
                            "Status: Connected", 
                            "Kết nối thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("❌ Kết nối SQL Server thất bại!\n\n" +
                            "Kiểm tra lại:\n" +
                            "1. SQL Server Service đang chạy\n" +
                            "2. Server name: LAPTOP-PN16MELH\n" +
                            "3. Windows Authentication được bật\n" +
                            "4. Firewall không block SQL Server\n" +
                            "5. SQL Server Browser Service đang chạy", 
                            "Kết nối thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kiểm tra kết nối:\n{ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Khôi phục trạng thái button
                this.Cursor = Cursors.Default;
                btnTestConnection.Enabled = true;
                btnTestConnection.Text = "Test Connection";
            }
        }
    }
}
