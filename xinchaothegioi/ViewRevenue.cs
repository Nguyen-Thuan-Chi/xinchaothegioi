using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Text;

namespace xinchaothegioi
{
    public partial class ViewRevenue : Form
    {
        // Strongly typed revenue record
        private class RevenueRecord
        {
            public DateTime SaleDate { get; set; }
            public int TicketCount { get; set; }
            public decimal Amount { get; set; }
            public string Region { get; set; }
            public string InvoiceId { get; set; }
            public string CustomerName { get; set; }
            public string Gender { get; set; }
            public string Phone { get; set; }
            public string Seats { get; set; }
        }

        private readonly List<RevenueRecord> _allRecords = new List<RevenueRecord>();
        private DataGridView _sourceGrid;
        private readonly CultureInfo _vn = new CultureInfo("vi-VN");
        private readonly string _dateFormat = "dd/MM/yyyy"; // Format bên Form1 dùng

        public ViewRevenue()
        {
            InitializeComponent();
            InitForm();
            InitControls();
            InitCharts();
        }

        private void InitForm()
        {
            Text = "Báo cáo doanh thu bán vé";
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitControls()
        {
            // Regions
            cboRegionFilter.Items.Clear();
            cboRegionFilter.Items.Add("Tất cả");
            cboRegionFilter.Items.AddRange(RegionManager.Regions.ToArray());
            cboRegionFilter.SelectedIndex = 0;

            // Movies (placeholder)
            cboSelectMovie.Items.Clear();
            cboSelectMovie.Items.Add("Tất cả");
            cboSelectMovie.Items.Add("Phim hành động");
            cboSelectMovie.Items.Add("Phim tình cảm");
            cboSelectMovie.Items.Add("Phim kinh dị");
            cboSelectMovie.SelectedIndex = 0;

            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpToDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = _dateFormat;
            dtpToDate.CustomFormat = _dateFormat;
            dtpToDate.Value = DateTime.Now.Date;
            dtpFromDate.Value = DateTime.Now.Date.AddMonths(-1);

            ConfigureGrid();
            UpdateSummary(Enumerable.Empty<RevenueRecord>());
        }

        private void ConfigureGrid()
        {
            dgvRevenue.Columns.Clear();
            dgvRevenue.ReadOnly = true;
            dgvRevenue.AllowUserToAddRows = false;
            dgvRevenue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRevenue.MultiSelect = false;
            dgvRevenue.RowHeadersVisible = false;
            dgvRevenue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvRevenue.Columns.Add("colInvoiceId", "Mã hóa đơn");
            dgvRevenue.Columns.Add("colCustomerName", "Tên khách hàng");
            dgvRevenue.Columns.Add("colGender", "Giới tính");
            dgvRevenue.Columns.Add("colPhone", "SĐT");
            dgvRevenue.Columns.Add("colRegion", "Khu vực");
            dgvRevenue.Columns.Add("colSeat", "Ghế ngồi");
            dgvRevenue.Columns.Add("colTicketCount", "Số vé bán");
            dgvRevenue.Columns.Add("colTotalAmount", "Doanh thu");
            dgvRevenue.Columns.Add("colSaleDate", "Ngày bán vé");
        }

        private void InitCharts()
        {
            InitColumnChart();
            InitPieChart();
        }

        private void InitColumnChart()
        {
            chartRevenueColumn.Series.Clear();
            chartRevenueColumn.ChartAreas.Clear();
            var area = new ChartArea("Main");
            area.AxisX.Title = "Ngày";
            area.AxisY.Title = "Doanh thu (VND)";
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -45;
            area.AxisY.LabelStyle.Format = "N0";
            chartRevenueColumn.ChartAreas.Add(area);

            var s = new Series("Doanh thu")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelFormat = "N0",
                Color = Color.SteelBlue
            };
            chartRevenueColumn.Series.Add(s);
            chartRevenueColumn.Titles.Clear();
            chartRevenueColumn.Titles.Add("Doanh thu theo ngày");
        }

        private void InitPieChart()
        {
            chartRevenuePie.Series.Clear();
            chartRevenuePie.ChartAreas.Clear();
            var area = new ChartArea("Pie");
            chartRevenuePie.ChartAreas.Add(area);
            var s = new Series("Doanh thu theo khu vực")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                LabelFormat = "N0"
            };
            s["PieLabelStyle"] = "Outside";
            chartRevenuePie.Series.Add(s);
            chartRevenuePie.Titles.Clear();
            chartRevenuePie.Titles.Add("Doanh thu theo khu vực");
        }

        // Public API --------------------------------------------------------
        public void SetRevenueData(DataGridView sourceGrid)
        {
            _sourceGrid = sourceGrid;
            _allRecords.Clear();
            if (sourceGrid == null)
            {
                MessageBox.Show("Không nhận được dữ liệu nguồn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int success = 0, fail = 0;
            foreach (DataGridViewRow r in sourceGrid.Rows)
            {
                if (r.IsNewRow) continue;
                if (TryParseRow(r, out var record))
                {
                    _allRecords.Add(record);
                    success++;
                }
                else fail++;
            }

            if (success == 0)
            {
                MessageBox.Show("Không thể đọc dữ liệu vé!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (fail > 0)
            {
                MessageBox.Show($"Đọc {success} dòng, lỗi {fail} dòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            RefreshAll();
        }

        // Parsing -----------------------------------------------------------
        private bool TryParseRow(DataGridViewRow row, out RevenueRecord rec)
        {
            rec = null;
            try
            {
                string invoiceId = AsString(row.Cells["colInvoiceId"].Value);
                string name = AsString(row.Cells["colCustomerName"].Value);
                string gender = AsString(row.Cells["colGender"].Value);
                string phone = AsString(row.Cells["colPhone"].Value);
                string region = AsString(row.Cells["colRegion"].Value);
                string seats = AsString(row.Cells["colSeat"].Value);

                // Ticket count can be int or string
                int ticketCount = 0;
                var tcObj = row.Cells["colTicketCount"].Value;
                if (tcObj is int) ticketCount = (int)tcObj;
                else int.TryParse(AsString(tcObj), out ticketCount);

                // Amount can be decimal/double or a string with VND
                decimal amount = 0m;
                var amtObj = row.Cells["colTotalAmount"].Value;
                if (amtObj is decimal) amount = (decimal)amtObj;
                else if (amtObj is double) amount = Convert.ToDecimal((double)amtObj);
                else
                {
                    string amountStr = AsString(amtObj);
                    string digits = Regex.Replace(amountStr, "[^0-9]", "");
                    if (!string.IsNullOrEmpty(digits)) decimal.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out amount);
                }

                // Sale date can be DateTime or string
                DateTime saleDate;
                var dateObj = row.Cells["colSaleDate"].Value;
                if (dateObj is DateTime dt)
                {
                    saleDate = dt.Date;
                }
                else
                {
                    string dateStr = AsString(dateObj);
                    if (!DateTime.TryParseExact(dateStr, _dateFormat, _vn, DateTimeStyles.None, out saleDate))
                    {
                        if (!DateTime.TryParse(dateStr, _vn, DateTimeStyles.None, out saleDate))
                            return false;
                    }
                    saleDate = saleDate.Date;
                }

                rec = new RevenueRecord
                {
                    SaleDate = saleDate,
                    TicketCount = ticketCount,
                    Amount = amount,
                    Region = region,
                    InvoiceId = invoiceId,
                    CustomerName = name,
                    Gender = gender,
                    Phone = phone,
                    Seats = seats
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string AsString(object val)
        {
            return val == null ? string.Empty : Convert.ToString(val, _vn);
        }

        // Filtering + Refresh -----------------------------------------------
        private IEnumerable<RevenueRecord> ApplyFilters()
        {
            if (_allRecords.Count == 0) return Enumerable.Empty<RevenueRecord>();
            DateTime from = dtpFromDate.Value.Date;
            DateTime to = dtpToDate.Value.Date; // inclusive day
            var q = _allRecords.Where(r => r.SaleDate >= from && r.SaleDate <= to);

            string region = cboRegionFilter.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(region) && region != "Tất cả")
                q = q.Where(r => r.Region == region);

            return q.ToList();
        }

        private void RefreshAll()
        {
            var filtered = ApplyFilters();
            FillGrid(filtered);
            UpdateCharts(filtered);
            UpdateSummary(filtered);
        }

        private void FillGrid(IEnumerable<RevenueRecord> data)
        {
            dgvRevenue.Rows.Clear();
            foreach (var r in data)
            {
                dgvRevenue.Rows.Add(
                    r.InvoiceId,
                    r.CustomerName,
                    r.Gender,
                    r.Phone,
                    r.Region,
                    r.Seats,
                    r.TicketCount.ToString(),
                    r.Amount.ToString("N0", _vn) + " VND",
                    r.SaleDate.ToString(_dateFormat)
                );
            }
            if (dgvRevenue.Rows.Count == 0)
            {
                dgvRevenue.Rows.Add("Không có dữ liệu", "", "", "", "", "", "", "", "");
            }
        }

        // Charts ------------------------------------------------------------
        private void UpdateCharts(IEnumerable<RevenueRecord> data)
        {
            UpdateColumnChart(data);
            UpdatePieChart(data);
        }

        private void UpdateColumnChart(IEnumerable<RevenueRecord> data)
        {
            var series = chartRevenueColumn.Series["Doanh thu"];
            series.Points.Clear();
            var grouped = data
                .GroupBy(r => r.SaleDate)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key, Sum = g.Sum(x => x.Amount) });
            foreach (var item in grouped)
                series.Points.AddXY(item.Date.ToString("dd/MM"), (double)item.Sum);
            if (!grouped.Any())
                series.Points.AddXY("Không có dữ liệu", 0);
            chartRevenueColumn.Titles[0].Text = BuildTitle("Doanh thu theo ngày");
        }

        private void UpdatePieChart(IEnumerable<RevenueRecord> data)
        {
            var series = chartRevenuePie.Series["Doanh thu theo khu vực"];
            series.Points.Clear();
            var grouped = data
                .GroupBy(r => string.IsNullOrEmpty(r.Region) ? "(Không xác định)" : r.Region)
                .Select(g => new { Region = g.Key, Sum = g.Sum(x => x.Amount) })
                .OrderByDescending(x => x.Sum);
            foreach (var item in grouped)
            {
                var p = series.Points.Add((double)item.Sum);
                p.LegendText = item.Region;
                p.Label = $"{item.Region}\n{item.Sum:N0}";
            }
            if (!grouped.Any())
            {
                var p = series.Points.Add(1);
                p.LegendText = "Không có dữ liệu";
                p.Label = "Không có dữ liệu";
                p.Color = Color.LightGray;
            }
            chartRevenuePie.Titles[0].Text = BuildTitle("Doanh thu theo khu vực");
        }

        private string BuildTitle(string baseTitle)
        {
            string region = cboRegionFilter.SelectedItem?.ToString();
            string extra = region != null && region != "Tất cả" ? " - " + region : string.Empty;
            return $"{baseTitle}\nTừ {dtpFromDate.Value.ToString(_dateFormat)} đến {dtpToDate.Value.ToString(_dateFormat)}{extra}";
        }

        // Summary -----------------------------------------------------------
        private void UpdateSummary(IEnumerable<RevenueRecord> data)
        {
            int totalTickets = data.Sum(r => r.TicketCount);
            decimal totalRevenue = data.Sum(r => r.Amount);
            lblTotalTickets.Text = $"Tổng số vé: {totalTickets:N0}";
            lblTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:N0} VND";
        }

        // Events ------------------------------------------------------------
        private void ViewRevenue_Load(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { if (_allRecords.Count > 0) RefreshAll(); }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { if (_allRecords.Count > 0) RefreshAll(); }
        private void chart1_Click(object sender, EventArgs e) { }

        private void btnView_Click(object sender, EventArgs e)
        {
            RefreshAll();
            var count = ApplyFilters().Count();
            MessageBox.Show($"Hiển thị {count} bản ghi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now.Date;
            dtpToDate.Value = DateTime.Now.Date;
            RefreshAll();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ApplyFilters();
                if (!data.Any())
                {
                    MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "CSV Files|*.csv|Ảnh biểu đồ (*.png)|*.png";
                    dlg.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}";
                    if (dlg.ShowDialog() != DialogResult.OK) return;

                    string ext = Path.GetExtension(dlg.FileName).ToLower();
                    if (ext == ".csv")
                    {
                        ExportCsv(dlg.FileName, data);
                    }
                    else if (ext == ".png")
                    {
                        var choice = MessageBox.Show("Yes = biểu đồ cột, No = biểu đồ tròn", "Chọn biểu đồ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                        if (choice == DialogResult.Yes)
                            chartRevenueColumn.SaveImage(dlg.FileName, ChartImageFormat.Png);
                        else if (choice == DialogResult.No)
                            chartRevenuePie.SaveImage(dlg.FileName, ChartImageFormat.Png);
                        else return;
                    }
                    else
                    {
                        MessageBox.Show("Định dạng không hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Xuất thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportCsv(string path, IEnumerable<RevenueRecord> data)
        {
            using (var w = new StreamWriter(path, false, Encoding.UTF8))
            {
                w.WriteLine("Mã hóa đơn,Tên khách hàng,Giới tính,SĐT,Khu vực,Ghế ngồi,Số vé bán,Doanh thu (VND),Ngày bán vé");
                foreach (var r in data)
                {
                    w.WriteLine($"{r.InvoiceId},{Escape(r.CustomerName)},{r.Gender},{r.Phone},{Escape(r.Region)},{Escape(r.Seats)},{r.TicketCount},{r.Amount:F0},{r.SaleDate.ToString(_dateFormat)}");
                }
                w.WriteLine();
                w.WriteLine("TỔNG KẾT");
                w.WriteLine($"Tổng số vé,{data.Sum(x => x.TicketCount)}");
                w.WriteLine($"Tổng doanh thu,{data.Sum(x => x.Amount):F0}");
                w.WriteLine($"Từ ngày,{dtpFromDate.Value.ToString(_dateFormat)}");
                w.WriteLine($"Đến ngày,{dtpToDate.Value.ToString(_dateFormat)}");
                if (cboRegionFilter.SelectedItem?.ToString() != "Tất cả")
                    w.WriteLine($"Khu vực,{cboRegionFilter.SelectedItem}");
            }
        }

        private string Escape(string s)
        {
            if (s == null) return string.Empty;
            if (s.Contains(",") || s.Contains("\""))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }
    }
}
