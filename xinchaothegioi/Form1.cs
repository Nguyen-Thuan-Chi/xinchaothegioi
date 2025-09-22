using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace xinchaothegioi
{
    public partial class frmMain1 : Form
    {
        private List<Button> selectedSeats = new List<Button>();
        private List<Button> allSeats = new List<Button>();
        private const int SeatPrice = 80000;

        public frmMain1()
        {
            InitializeComponent();
            HideAllExceptMenu();
            InitializeSeats();
            InitializeDataGridView();
            InitializeRegionComboBox();
            
            // Khởi tạo DateTimePicker với ngày hiện tại
            dtpSaleDate.Value = DateTime.Now;
            
            // Gắn sự kiện kiểm tra thông tin
            txtName.TextChanged += (s, e) => CheckCustomerInfoAndToggleSeats();
            txtPhone.TextChanged += (s, e) => CheckCustomerInfoAndToggleSeats();
            cboRegion.SelectedIndexChanged += (s, e) => {
                CheckCustomerInfoAndToggleSeats();
                UpdateSeatStatus(); // Cập nhật trạng thái ghế khi thay đổi khu vực
                ClearSelectedSeats(); // Xóa ghế đã chọn khi đổi khu vực
            };
            rdoMale.CheckedChanged += (s, e) => CheckCustomerInfoAndToggleSeats();
            rdoFemale.CheckedChanged += (s, e) => CheckCustomerInfoAndToggleSeats();

            // Thiết lập properties cho form
            this.Text = "Hệ thống bán vé xem phim";
            this.WindowState = FormWindowState.Maximized;
        }

        private void ClearSelectedSeats()
        {
            // Xóa tất cả ghế đang chọn (màu vàng) khi đổi khu vực
            foreach (var seat in allSeats)
            {
                if (seat.BackColor == Color.Yellow)
                {
                    seat.BackColor = SystemColors.Control;
                }
            }
            selectedSeats.Clear();
            UpdateTotal();
        }

        private void InitializeSeats()
        {
            // Add all 24 seat buttons to the list for easy management
            allSeats.AddRange(new Button[] {
                btnSeat1, button1, button2, button3, button4, button5, button6, button7,
                button8, button9, button10, button11, button12, button13, button14, button15,
                button16, button17, button18, button19, button20, button21, button22, button23
            });

            foreach (var seat in allSeats)
            {
                seat.BackColor = SystemColors.Control;
                seat.Visible = false; // Hide at startup
                seat.Font = new Font("Arial", 8, FontStyle.Bold);
                
                // Thêm tooltip để hiển thị giá vé và vị trí
                ToolTip tooltip = new ToolTip();
                if (int.TryParse(seat.Text, out int seatNum))
                {
                    int price = AppConstants.GetSeatPrice(seatNum);
                    string description = AppConstants.GetSeatDescription(seatNum);
                    tooltip.SetToolTip(seat, $"Ghế {seatNum}\n{description}");
                }
            }
        }

        private void Seat_Click(object sender, EventArgs e)
        {
            // Kiểm tra thông tin khách hàng
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string region = cboRegion.SelectedItem?.ToString() ?? "";
            string gender = rdoMale.Checked ? "Nam" : (rdoFemale.Checked ? "Nữ" : "");

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }
            if (string.IsNullOrEmpty(region))
            {
                MessageBox.Show("Vui lòng chọn khu vực.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRegion.Focus();
                return;
            }
            if (string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Đủ thông tin, xử lý chọn ghế
            var seatBtn = sender as Button;
            if (seatBtn == null) return;

            if (seatBtn.BackColor == Color.Yellow)
            {
                seatBtn.BackColor = SystemColors.Control;
                selectedSeats.Remove(seatBtn);
            }
            else
            {
                seatBtn.BackColor = Color.Yellow;
                if (!selectedSeats.Contains(seatBtn))
                    selectedSeats.Add(seatBtn);
            }
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            int total = 0;
            foreach (var seat in selectedSeats)
            {
                total += GetSeatPrice(seat.Text);
            }
            txtTotal.Text = total.ToString("N0") + " VND";
            // Enable btnSelect chỉ khi có ghế đang chọn
            btnSelect.Enabled = selectedSeats.Count > 0;
        }

        // Thêm hàm tính giá ghế theo vị trí - sử dụng AppConstants
        private int GetSeatPrice(string seatNumber)
        {
            if (!int.TryParse(seatNumber, out int seat))
                return AppConstants.PRICE_COLUMN_1_4_ROW_1_2; // Giá mặc định
                
            return AppConstants.GetSeatPrice(seat);
        }

        // Hàm tính tổng tiền dựa trên danh sách ghế
        private int CalculateTotalAmount(string seatList)
        {
            if (string.IsNullOrEmpty(seatList))
                return 0;

            var seats = seatList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int total = 0;
            
            foreach (var seat in seats)
            {
                string seatNumber = seat.Trim();
                total += GetSeatPrice(seatNumber);
            }
            
            return total;
        }

        // Hàm đếm số lượng vé từ danh sách ghế
        private int CountTickets(string seatList)
        {
            if (string.IsNullOrEmpty(seatList))
                return 0;

            var seats = seatList.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return seats.Length;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            // Validate required fields
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string region = cboRegion.SelectedItem?.ToString() ?? "";
            string gender = rdoMale.Checked ? "Nam" : (rdoFemale.Checked ? "Nữ" : "");
            List<string> seatNames = new List<string>();
            foreach (var seat in selectedSeats)
            {
                if (seat.BackColor == Color.Yellow)
                {
                    seatNames.Add(seat.Text);
                }
            }

            // Check for missing info
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }
            if (string.IsNullOrEmpty(region))
            {
                MessageBox.Show("Vui lòng chọn khu vực.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboRegion.Focus();
                return;
            }
            if (string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng chọn giới tính.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (seatNames.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ghế ngồi!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validation số điện thoại
            if (!IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Số điện thoại không hợp lệ! Vui lòng nhập số điện thoại 10-11 chữ số.", 
                    "Thông tin không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPhone.Focus();
                return;
            }

            // Tính toán dữ liệu
            string seatList = string.Join(", ", seatNames.OrderBy(s => int.Parse(s)));
            int ticketCount = CountTickets(seatList);
            int totalAmount = CalculateTotalAmount(seatList);
            string saleDate = dtpSaleDate.Value.ToString("dd/MM/yyyy");

            // Add to DataGridView
            dgvInformaton.Rows.Add(
                GenerateInvoiceId(), // Mã hóa đơn
                name,                      // Tên khách hàng
                gender,                    // Giới tính
                phone,                     // SĐT
                region,                    // Khu vực
                seatList,                  // Ghế ngồi
                ticketCount.ToString(),    // Số lượng vé
                totalAmount.ToString("N0") + " VND", // Thành tiền
                saleDate                   // Ngày bán vé
            );

            // Show success message
            MessageBox.Show($"Đặt vé thành công!\nKhách hàng: {name}\nGhế: {seatList}\nTổng tiền: {totalAmount:N0} VND", 
                "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Reset form
            ClearForm();
            UpdateSeatStatus();
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

        private string GenerateInvoiceId()
        {
            // Tạo mã hóa đơn theo format: HD + YYYYMMDD + số thứ tự trong ngày
            string dateStr = DateTime.Now.ToString("yyyyMMdd");
            int orderInDay = dgvInformaton.Rows.Count + 1;
            return $"HD{dateStr}{orderInDay:D3}";
        }

        private void ClearForm()
        {
            // Reset trạng thái ghế đã chọn
            foreach (var seat in selectedSeats)
            {
                if (seat.BackColor == Color.Yellow)
                {
                    seat.BackColor = SystemColors.Control;
                }
            }
            selectedSeats.Clear();
            
            // Clear form fields
            txtName.Clear();
            txtPhone.Clear();
            txtTotal.Text = "0 VND";
            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            
            // Reset date to today
            dtpSaleDate.Value = DateTime.Now;
            
            // Disable btnSelect
            btnSelect.Enabled = false;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            foreach (var seat in allSeats)
            {
                if (seat.BackColor == Color.Yellow)
                {
                    seat.BackColor = SystemColors.Control;
                }
            }
            selectedSeats.Clear();
            UpdateTotal();
        }

        private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Temporarily disable login and just show controls
            SetControlsVisible(true);
            MessageBox.Show("Đăng nhập thành công (demo mode)!", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HideAllExceptMenu()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (!(ctrl is MenuStrip))
                    ctrl.Visible = false;
            }
        }

        private void SetControlsVisible(bool visible)
        {
            mnuSystem.Visible = true;
            mnuFunctions.Visible = visible;

            groupBox1.Visible = visible;
            groupBox2.Visible = visible;

            lblScreen.Visible = visible;
            lblCustomerInfo.Visible = visible;
            lblname.Visible = visible;
            lblgender.Visible = visible;
            lblPhone.Visible = visible;
            lblRegion.Visible = visible;
            lblTotal.Visible = visible;

            txtName.Visible = visible;
            txtPhone.Visible = visible;
            txtTotal.Visible = visible;

            rdoMale.Visible = visible;
            rdoFemale.Visible = visible;

            cboRegion.Visible = visible;
            btnAddRegion.Visible = visible;
            
            // Hiển thị DateTimePicker ngày bán vé
            dtpSaleDate.Visible = visible;

            dgvInformaton.Visible = visible;

            foreach (var seat in allSeats)
                seat.Visible = visible;

            btnSelect.Visible = visible;
            btnCancel.Visible = visible;
            btnExit.Visible = visible;

            btnEdit.Visible = visible;
            btnDelete.Visible = visible;
        }

        private void InitializeDataGridView()
        {
            dgvInformaton.Columns.Clear();
            dgvInformaton.Columns.Add("colInvoiceId", "Mã hóa đơn");
            dgvInformaton.Columns.Add("colCustomerName", "Tên khách hàng");
            dgvInformaton.Columns.Add("colGender", "Giới tính");
            dgvInformaton.Columns.Add("colPhone", "SĐT");
            dgvInformaton.Columns.Add("colRegion", "Khu vực");
            dgvInformaton.Columns.Add("colSeat", "Ghế ngồi");
            dgvInformaton.Columns.Add("colTicketCount", "Số lượng vé");
            dgvInformaton.Columns.Add("colTotalAmount", "Thành tiền");
            dgvInformaton.Columns.Add("colSaleDate", "Ngày bán vé");

            // Thiết lập properties cho DataGridView
            dgvInformaton.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInformaton.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInformaton.MultiSelect = false;
            dgvInformaton.AllowUserToAddRows = false;
            dgvInformaton.ReadOnly = true;
        }

        private void btnAddRegion_Click(object sender, EventArgs e)
        {
            using (var regionForm = new frmMain2())
            {
                regionForm.ShowDialog();
                // After closing Form2, update cboRegion
                string currentSelection = cboRegion.SelectedItem?.ToString();
                cboRegion.Items.Clear();
                cboRegion.Items.AddRange(RegionManager.Regions.ToArray());
                
                // Giữ lại lựa chọn hiện tại nếu có
                if (!string.IsNullOrEmpty(currentSelection) && cboRegion.Items.Contains(currentSelection))
                {
                    cboRegion.SelectedItem = currentSelection;
                }
                else if (cboRegion.Items.Count > 0)
                {
                    cboRegion.SelectedIndex = 0;
                }
                
                // Cập nhật trạng thái ghế theo khu vực mới
                UpdateSeatStatus();
            }
        }

        private void InitializeRegionComboBox()
        {
            cboRegion.Items.Clear();
            cboRegion.Items.AddRange(RegionManager.Regions.ToArray());
            if (cboRegion.Items.Count > 0)
            {
                cboRegion.SelectedIndex = 0;
                // Cập nhật trạng thái ghế cho khu vực đầu tiên
                UpdateSeatStatus();
            }
        }

        private void chứcNăngToolStripMenuItem_Click(object sender, EventArgs e) { }
        private void groupBox1_Enter(object sender, EventArgs e) { }
        private void mnuSystem_Click(object sender, EventArgs e) { }
        private void btnChooseSeats_Click(object sender, EventArgs e)
        {
            var seatBtn = sender as Button;
            if (seatBtn == null) return;

            // Kiểm tra ghế đã mua trong khu vực hiện tại
            if (IsSeatPurchased(seatBtn.Text))
            {
                MessageBox.Show($"Ghế này đã được mua trong khu vực {cboRegion.SelectedItem?.ToString()}!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Gọi hàm Seat_Click để xử lý
            Seat_Click(sender, e);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            // Xác nhận trước khi thoát
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát ứng dụng?", 
                "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dgvInformaton_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Có thể thêm chức năng xem chi tiết khi click vào cell
        }

        private bool IsSeatPurchased(string seatName)
        {
            // Lấy khu vực hiện tại được chọn
            string currentRegion = cboRegion.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(currentRegion)) return false;

            foreach (DataGridViewRow row in dgvInformaton.Rows)
            {
                if (row.IsNewRow) continue;
                
                // Kiểm tra cả ghế và khu vực
                var seatCell = row.Cells["colSeat"].Value as string;
                var regionCell = row.Cells["colRegion"].Value as string;
                
                if (seatCell != null && regionCell == currentRegion)
                {
                    var seats = seatCell.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var s in seats)
                    {
                        if (s.Trim() == seatName) return true;
                    }
                }
            }
            return false;
        }

        private void UpdateSeatStatus()
        {
            // Lấy khu vực hiện tại được chọn
            string currentRegion = cboRegion.SelectedItem?.ToString() ?? "";
            
            for (int i = 0; i < allSeats.Count; i++)
            {
                var seat = allSeats[i];
                int seatNumber = i + 1; // Ghế số 1-24
                
                if (string.IsNullOrEmpty(currentRegion))
                {
                    // Nếu chưa chọn khu vực, disable tất cả ghế
                    seat.BackColor = AppConstants.SEAT_AVAILABLE;
                    seat.Enabled = false;
                    seat.Text = seatNumber.ToString();
                }
                else if (IsSeatPurchased(seatNumber.ToString()))
                {
                    // Ghế đã được mua trong khu vực này
                    seat.BackColor = AppConstants.SEAT_OCCUPIED;
                    seat.Enabled = false;
                    seat.Text = "X";
                }
                else
                {
                    // Ghế trống trong khu vực này
                    seat.BackColor = AppConstants.SEAT_AVAILABLE;
                    seat.Enabled = true;
                    seat.Text = seatNumber.ToString();
                }
            }
            
            // Kiểm tra lại thông tin khách hàng để enable/disable ghế
            CheckCustomerInfoAndToggleSeats();
        }

        private void CheckCustomerInfoAndToggleSeats()
        {
            string name = txtName.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string region = cboRegion.SelectedItem?.ToString() ?? "";
            bool genderSelected = rdoMale.Checked || rdoFemale.Checked;

            bool infoValid = !string.IsNullOrEmpty(name)
                && !string.IsNullOrEmpty(phone)
                && !string.IsNullOrEmpty(region)
                && genderSelected;

            foreach (var seat in allSeats)
            {
                // Chỉ enable ghế nếu thông tin đầy đủ VÀ ghế chưa được mua trong khu vực này
                if (infoValid && !IsSeatPurchased(seat.Text))
                {
                    seat.Enabled = true;
                }
                else if (IsSeatPurchased(seat.Text))
                {
                    seat.Enabled = false; // Ghế đã mua
                }
                else
                {
                    seat.Enabled = false; // Thông tin chưa đầy đủ
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvInformaton.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa vé này?", 
                    "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    foreach (DataGridViewRow row in dgvInformaton.SelectedRows)
                    {
                        if (!row.IsNewRow)
                            dgvInformaton.Rows.Remove(row);
                    }
                    UpdateSeatStatus();
                    MessageBox.Show("Đã xóa vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvInformaton.SelectedRows.Count == 1)
            {
                var row = dgvInformaton.SelectedRows[0];
                var invoiceId = row.Cells["colInvoiceId"].Value?.ToString() ?? "";
                var name = row.Cells["colCustomerName"].Value?.ToString() ?? "";
                var gender = row.Cells["colGender"].Value?.ToString() ?? "";
                var phone = row.Cells["colPhone"].Value?.ToString() ?? "";
                var region = row.Cells["colRegion"].Value?.ToString() ?? "";
                var seats = row.Cells["colSeat"].Value?.ToString() ?? "";

                using (var editForm = new Editgatagridview(name, gender, phone, region, seats))
                {
                    if (editForm.ShowDialog() == DialogResult.OK)
                    {
                        // Cập nhật lại thông tin lên dòng đã chọn
                        row.Cells["colCustomerName"].Value = editForm.CustomerName;
                        row.Cells["colGender"].Value = editForm.Gender;
                        row.Cells["colPhone"].Value = editForm.Phone;
                        row.Cells["colRegion"].Value = editForm.Region;
                        row.Cells["colSeat"].Value = editForm.Seats;
                        
                        // Tính lại số lượng vé và thành tiền
                        int ticketCount = CountTickets(editForm.Seats);
                        int totalAmount = CalculateTotalAmount(editForm.Seats);
                        row.Cells["colTicketCount"].Value = ticketCount.ToString();
                        row.Cells["colTotalAmount"].Value = totalAmount.ToString("N0") + " VND";
                        
                        // Cập nhật trạng thái ghế cho tất cả khu vực
                        UpdateSeatStatus();
                        
                        // Nếu khu vực hiện tại trong ComboBox trùng với khu vực vừa sửa, cập nhật lại
                        string currentRegion = cboRegion.SelectedItem?.ToString() ?? "";
                        if (currentRegion == editForm.Region || currentRegion == region)
                        {
                            ClearSelectedSeats(); // Xóa ghế đang chọn
                        }
                        
                        MessageBox.Show("Đã cập nhật thông tin vé thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn 1 dòng để sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void frmMain1_Load(object sender, EventArgs e)
        {

        }

        private void cboRegion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void mnuViewRevenue_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu trước khi mở form
            if (dgvInformaton.Rows.Count == 0 || (dgvInformaton.Rows.Count == 1 && dgvInformaton.AllowUserToAddRows))
            {
                MessageBox.Show("Chưa có dữ liệu vé để xem doanh thu!\nVui lòng bán vé trước khi xem doanh thu.", 
                    "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Mở form xem doanh thu
            ViewRevenue revenueForm = new ViewRevenue();
            
            // Truyền dữ liệu từ DataGridView sang ViewRevenue
            revenueForm.SetRevenueData(dgvInformaton);
            
            revenueForm.ShowDialog();
        }

        private void lblSelectMovie_Click(object sender, EventArgs e)
        {

        }

        // Thêm phương thức để lấy dữ liệu doanh thu
        public DataGridView GetRevenueDataGridView()
        {
            return dgvInformaton;
        }
    }
}
