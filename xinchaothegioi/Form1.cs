using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using xinchaothegioi.Data;
using xinchaothegioi.Services;
using xinchaothegioi.Models;
using xinchaothegioi.Entities;

namespace xinchaothegioi
{
    public partial class frmMain1 : Form
    {
        private List<Button> selectedSeats = new List<Button>();
        private List<Button> allSeats = new List<Button>();
        private const int SeatPrice = 80000;
        private MovieSummary _selectedMovie; // phim hiện tại
        private ToolTip _movieToolTip = new ToolTip();

        // EF + Service
        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private SalesService _sales;

        public frmMain1()
        {
            InitializeComponent();
            _sales = new SalesService(_db);

            HideAllExceptMenu();
            InitializeSeats();
            InitializeDataGridView();
            InitializeRegionComboBox();
            LoadInvoicesToGrid();
            
            // Đặt ô tổng tiền chỉ đọc để tránh chỉnh sửa thủ công
            txtTotal.ReadOnly = true;
            txtTotal.TabStop = false; // bỏ focus tab
            // Có thể đặt màu nền giống control readonly chuẩn
            txtTotal.BackColor = SystemColors.ControlLight;
            
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

        private string TruncateTitle(string title, int max = 16)
        {
            if (string.IsNullOrEmpty(title)) return "(Không có)";
            return title.Length > max ? title.Substring(0, max - 3) + "..." : title;
        }

        private void UpdateSelectedMovieUI()
        {
            if (_selectedMovie == null)
            {
                btnSelectMovie.Text = "Chọn Phim";
                _movieToolTip.SetToolTip(btnSelectMovie, "Chọn phim từ TMDB");
            }
            else
            {
                btnSelectMovie.Text = TruncateTitle(_selectedMovie.title);
                _movieToolTip.SetToolTip(btnSelectMovie, $"{_selectedMovie.title}\nĐiểm: {_selectedMovie.vote_average}/10\nPhát hành: {_selectedMovie.release_date}");
            }
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

            if (string.IsNullOrEmpty(name)) { MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtName.Focus(); return; }
            if (string.IsNullOrEmpty(phone)) { MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPhone.Focus(); return; }
            if (string.IsNullOrEmpty(region)) { MessageBox.Show("Vui lòng chọn khu vực.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); cboRegion.Focus(); return; }
            if (string.IsNullOrEmpty(gender)) { MessageBox.Show("Vui lòng chọn giới tính.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (seatNames.Count == 0) { MessageBox.Show("Vui lòng chọn ghế ngồi!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (_selectedMovie == null) { MessageBox.Show("Vui lòng chọn phim trước khi bán vé.", "Thiếu phim", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!IsValidPhoneNumber(phone)) { MessageBox.Show("Số điện thoại không hợp lệ!", "Thông tin không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning); txtPhone.Focus(); return; }

            try
            {
                // Lưu xuống DB
                var kh = _sales.GetOrCreateCustomer(name, phone, gender, region);
                var invoice = _sales.CreateInvoice(kh, _selectedMovie?.title ?? string.Empty, seatNames, dtpSaleDate.Value, AppConstants.GetSeatPrice);

                MessageBox.Show($"Đặt vé thành công!\nMã HĐ: HD{invoice.HoaDonId}\nPhim: {invoice.TenPhim}\nKhách hàng: {kh.Ten}\nGhế: {invoice.DanhSachGhe}\nTổng tiền: {invoice.SoTien:N0} VND", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh grid + UI
                ClearForm();
                LoadInvoicesToGrid();
                UpdateSeatStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            using (var login = new frmLogin())
            {
                if (login.ShowDialog() == DialogResult.OK)
                {
                    SetControlsVisible(true);
                    LoadInvoicesToGrid();
                    UpdateSeatStatus();
                }
                else
                {
                    SetControlsVisible(false);
                    MessageBox.Show("Đăng nhập thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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
            dgvInformaton.AutoGenerateColumns = false;
            dgvInformaton.Columns.Clear();

            // Define columns with DataPropertyName to bind DTO
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colInvoiceId", HeaderText = "Mã hóa đơn", DataPropertyName = "MaHoaDon" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCustomerName", HeaderText = "Tên khách hàng", DataPropertyName = "TenKhachHang" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGender", HeaderText = "Giới tính", DataPropertyName = "GioiTinh" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPhone", HeaderText = "SĐT", DataPropertyName = "SoDienThoai" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRegion", HeaderText = "Khu vực", DataPropertyName = "KhuVuc" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMovieTitle", HeaderText = "Phim", DataPropertyName = "TenPhim" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSeat", HeaderText = "Ghế ngồi", DataPropertyName = "DanhSachGhe" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTicketCount", HeaderText = "Số lượng vé", DataPropertyName = "SoLuongVe" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotalAmount", HeaderText = "Thành tiền", DataPropertyName = "ThanhTienHienThi" });
            dgvInformaton.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSaleDate", HeaderText = "Ngày bán vé", DataPropertyName = "NgayBan" });

            dgvInformaton.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInformaton.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvInformaton.MultiSelect = false;
            dgvInformaton.AllowUserToAddRows = false;
            dgvInformaton.ReadOnly = true;
        }

        private void LoadInvoicesToGrid()
        {
            try
            {
                var data = _sales.GetInvoices()
                    .Select(h => new InvoiceRow
                    {
                        MaHoaDon = "HD" + h.HoaDonId,
                        TenKhachHang = h.KhachHang?.Ten,
                        GioiTinh = h.KhachHang?.GioiTinh,
                        SoDienThoai = h.KhachHang?.SoDienThoai,
                        KhuVuc = h.KhachHang?.KhuVuc,
                        TenPhim = h.TenPhim,
                        DanhSachGhe = h.DanhSachGhe,
                        SoLuongVe = h.ChiTietHoaDons?.Count ?? 0,
                        ThanhTienHienThi = (h.SoTien).ToString("N0") + " VND",
                        NgayBan = h.NgayMua
                    })
                    .ToList();
                dgvInformaton.DataSource = data;
            }
            catch
            {
                // ignore if DB not ready
            }
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

            // Kiểm tra ghế đã mua trong khu vực hiện tại (DB)
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
            string currentRegion = cboRegion.SelectedItem?.ToString() ?? "";
            if (string.IsNullOrEmpty(currentRegion)) return false;

            // Query DB to check if seat is already purchased in current region
            try
            {
                return _sales.IsSeatPurchasedInRegion(currentRegion, seatName);
            }
            catch
            {
                return false;
            }
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

        private bool TryParseInvoiceIdFromRow(DataGridViewRow row, out int id)
        {
            id = 0;
            var val = row.Cells["colInvoiceId"].Value?.ToString();
            if (string.IsNullOrEmpty(val)) return false;
            // expected format: HD<number>
            var digits = new string(val.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out id);
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
                        if (row.IsNewRow) continue;
                        if (TryParseInvoiceIdFromRow(row, out int id))
                        {
                            var inv = _db.HoaDons.FirstOrDefault(h => h.HoaDonId == id);
                            if (inv != null)
                            {
                                _db.HoaDons.Remove(inv);
                            }
                        }
                    }
                    _db.SaveChanges();
                    LoadInvoicesToGrid();
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
            // Giữ nguyên xử lý hiện có (cập nhật trên lưới). Lưu ý: chưa đồng bộ DB.
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
                        // Cập nhật lại thông tin lên dòng đã chọn (UI-only)
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
                        
                        // Cập nhật trạng thái ghế từ DB
                        UpdateSeatStatus();
                        
                        MessageBox.Show("Đã cập nhật thông tin vé (chỉ hiển thị). Chức năng lưu DB chưa được triển khai trong bản này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void mnuReports_Click(object sender, EventArgs e)
        {
            // Mở form báo cáo tổng hợp (frmReport)
            if (dgvInformaton.Rows.Count == 0 || (dgvInformaton.Rows.Count == 1 && dgvInformaton.AllowUserToAddRows))
            {
                MessageBox.Show("Chưa có dữ liệu vé để xem báo cáo!\nVui lòng bán vé trước.", "Không có dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                using (var rpt = new frmReport())
                {
                    rpt.SetSourceGrid(dgvInformaton);
                    rpt.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblSaleDate_Click(object sender, EventArgs e)
        {

        }

        private void lvSelectMovie_DoubleClick(object sender, EventArgs e)
        {

        }

        private void btnSelectMovie_Click(object sender, EventArgs e)
        {
            using (var movieForm = new frmMovie())
            {
                var result = movieForm.ShowDialog(this);
                if (result == DialogResult.OK && movieForm.SelectedMovie != null)
                {
                    _selectedMovie = movieForm.SelectedMovie;
                    UpdateSelectedMovieUI();
                    MessageBox.Show($"Đã chọn phim: {_selectedMovie.title}", "Phim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
