using System;
using System.Windows.Forms;

namespace xinchaothegioi
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
            this.Text = "??ng nh?p h? th?ng";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui l?ng nh?p ??y ?? th?ng tin ??ng nh?p!", "Thi?u th?ng tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ki?m tra th?ng tin ??ng nh?p
            if (username == AppConstants.DEFAULT_USERNAME && password == AppConstants.DEFAULT_PASSWORD)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnLogin.PerformClick();
            }
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtPassword.Focus();
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();
            
            // Hi?n th? th?ng tin ??ng nh?p m?c ??nh ?? demo
            lblInfo.Text = $"Th?ng tin ??ng nh?p:\nT?i kho?n: {AppConstants.DEFAULT_USERNAME}\nM?t kh?u: {AppConstants.DEFAULT_PASSWORD}";
        }
    }
}