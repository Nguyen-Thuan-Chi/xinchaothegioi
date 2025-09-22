using System;
using System.Windows.Forms;
using System.Linq;

namespace xinchaothegioi
{
    public partial class Editgatagridview : Form
    {
        private string originalRegion; // Lưu khu vực ban đầu
        private string originalSeats;  // Lưu ghế ban đầu
        
        public string CustomerName => txtName.Text.Trim();
        public string Gender => rdoMale.Checked ? "Nam" : "Nữ";
        public string Phone => txtPhone.Text.Trim();
        public new string Region => cboRegion.Text;
        public string Seats => txtSeats.Text.Trim();

        public Editgatagridview(string name, string gender, string phone, string region, string seats)
        {
            InitializeComponent();

            originalRegion = region; // Lưu khu vực ban đầu
            originalSeats = seats;   // Lưu ghế ban đầu
            
            InitializeForm();
            LoadData(name, gender, phone, region, seats);
        }

        private void InitializeForm()
        {
            this.Text = "Sửa thông tin vé";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            
            // Thêm tooltip
            ToolTip tooltip = new ToolTip();
            tooltip.SetToolTip(txtSeats, "Nhập số ghế, cách nhau bằng dấu phẩy (VD: 1, 2, 5)");
            tooltip.SetToolTip(txtPhone, "Nhập số điện thoại 10-11 chữ số");
        }

        private void LoadData(string name, string gender, string phone, string region, string seats)
        {
            txtName.Text = name;
            txtPhone.Text = phone;
            
            // Load regions
            cboRegion.Items.Clear();
            cboRegion.Items.AddRange(RegionManager.Regions.ToArray());
            cboRegion.Text = region;
            
            txtSeats.Text = seats;
            
            if (gender == "Nam") 
                rdoMale.Checked = true;
            else 
                rdoFemale.Checked = true;
        }

        private bool IsSeatTakenInRegion(string seatText, string regionToCheck)
        {
            // Tìm form chính để kiểm tra dữ liệu
            foreach (Form form in Application.OpenForms)
            {
                if (form is frmMain1 mainForm)
                {
                    var dgv = mainForm.Controls.Find("dgvInformaton", true)[0] as DataGridView;
                    if (dgv != null)
                    {
                        foreach (DataGridViewRow row in dgv.Rows)
                        {
                            if (row.IsNewRow) continue;
                            
                            var regionCell = row.Cells["colRegion"].Value?.ToString();
                            var seatCell = row.Cells["colSeat"].Value?.ToString();
                            
                            // Bỏ qua dòng hiện tại đang sửa (cùng khu vực và ghế ban đầu)
                            if (regionCell == originalRegion && seatCell == originalSeats)
                                continue;
                            
                            if (regionCell == regionToCheck && seatCell != null)
                            {
                                var seats = seatCell.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var seat in seats)
                                {
                                    if (seat.Trim() == seatText.Trim()) return true;
                                }
                            }
                        }
                    }
                    break;
                }
            }
            return false;
        }

        private bool IsValidPhoneNumber(string phone)
        {
            // Kiểm tra số điện thoại Việt Nam (10-11 chữ số, bắt đầu bằng 0)
            if (string.IsNullOrEmpty(phone)) return false;
            
            phone = phone.Replace(" ", "").Replace("-", "");
            
            if (phone.Length < 10 || phone.Length > 11) return false;
            if (!phone.StartsWith("0")) return false;
            
            return phone.All(char.IsDigit);
        }

        private bool ValidateSeats(string seats)
        {
            if (string.IsNullOrWhiteSpace(seats)) return false;
            
            var seatArray = seats.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var seat in seatArray)
            {
                string seatNum = seat.Trim();
                if (!int.TryParse(seatNum, out int num) || num < 1 || num > AppConstants.TOTAL_SEATS)
                {
                    return false;
                }
            }
            
            return true;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate tên khách hàng
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            
            // Validate số điện thoại
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }
            
            if (!IsValidPhoneNumber(txtPhone.Text.Trim()))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập số điện thoại 10-11 chữ số bắt đầu bằng 0.", 
                    "Thông tin không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }
            
            // Validate khu vực
            if (string.IsNullOrWhiteSpace(cboRegion.Text))
            {
                MessageBox.Show("Vui lòng chọn khu vực.", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRegion.Focus();
                return;
            }
            
            // Validate giới tính
            if (!rdoMale.Checked && !rdoFemale.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Validate ghế ngồi
            if (string.IsNullOrWhiteSpace(txtSeats.Text))
            {
                MessageBox.Show("Vui lòng nhập ghế ngồi.", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSeats.Focus();
                return;
            }
            
            if (!ValidateSeats(txtSeats.Text))
            {
                MessageBox.Show($"Ghế ngồi không hợp lệ! Vui lòng nhập số ghế từ 1-{AppConstants.TOTAL_SEATS}, cách nhau bằng dấu phẩy.", 
                    "Thông tin không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSeats.Focus();
                return;
            }

            // Kiểm tra ghế đã được mua trong khu vực mới chưa (nếu đổi khu vực hoặc đổi ghế)
            string newRegion = cboRegion.Text;
            string newSeats = txtSeats.Text.Trim();
            
            if (newRegion != originalRegion || newSeats != originalSeats)
            {
                var seats = newSeats.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var seat in seats)
                {
                    if (IsSeatTakenInRegion(seat.Trim(), newRegion))
                    {
                        MessageBox.Show($"Ghế {seat.Trim()} đã được mua trong khu vực {newRegion}!", 
                            "Ghế đã được mua", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        txtSeats.Focus();
                        return;
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép chữ cái, số và khoảng trắng
            if (!char.IsLetter(e.KeyChar) && !char.IsDigit(e.KeyChar) && 
                !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số và một số ký tự đặc biệt
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && 
                e.KeyChar != '-' && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtSeats_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép số, dấu phẩy và khoảng trắng
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && 
                e.KeyChar != ',' && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }
    }
}
