using System;
using System.Windows.Forms;
using System.Linq;

namespace xinchaothegioi
{
    public partial class frmMain2 : Form
    {
        public frmMain2()
        {
            InitializeComponent();
            this.Text = "Quản lý khu vực";
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Hiển thị danh sách khu vực hiện tại
            UpdateRegionsList();
        }

        private void UpdateRegionsList()
        {
            lblRegionsList.Text = "Các khu vực hiện có:\n" + string.Join("\n", RegionManager.Regions.Select(r => "• " + r));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string newRegion = txtName.Text.Trim();
            
            if (string.IsNullOrEmpty(newRegion))
            {
                MessageBox.Show("Vui lòng nhập tên khu vực!", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            
            if (RegionManager.Regions.Contains(newRegion))
            {
                MessageBox.Show("Khu vực này đã tồn tại!", "Trùng lặp", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                txtName.SelectAll();
                return;
            }

            // Thêm khu vực mới
            RegionManager.Regions.Add(newRegion);
            UpdateRegionsList();
            txtName.Clear();
            
            MessageBox.Show($"Đã thêm khu vực '{newRegion}' thành công!", "Thành công", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string regionToDelete = txtName.Text.Trim();
            
            if (string.IsNullOrEmpty(regionToDelete))
            {
                MessageBox.Show("Vui lòng nhập tên khu vực cần xóa!", "Thiếu thông tin", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            
            if (!RegionManager.Regions.Contains(regionToDelete))
            {
                MessageBox.Show("Khu vực này không tồn tại!", "Không tìm thấy", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            // Kiểm tra xem có thể xóa được không (ít nhất phải còn 1 khu vực)
            if (RegionManager.Regions.Count <= 1)
            {
                MessageBox.Show("Không thể xóa khu vực cuối cùng!", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khu vực '{regionToDelete}'?", 
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                RegionManager.Regions.Remove(regionToDelete);
                UpdateRegionsList();
                txtName.Clear();
                
                MessageBox.Show($"Đã xóa khu vực '{regionToDelete}' thành công!", "Thành công", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Cho phép nhấn Enter để thêm khu vực
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAdd.PerformClick();
            }
        }
    }
}
