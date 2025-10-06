using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Security.Cryptography;

namespace xinchaothegioi
{
    public partial class frmReport : Form
    {
        private DataGridView _sourceGrid;
        private readonly List<SaleRecord> _allRecords = new List<SaleRecord>();
        private readonly CultureInfo _vn = new CultureInfo("vi-VN");
        private bool _initialized;
        
        // Smart refresh mechanism
        private string _lastDataHash = string.Empty;
        private DateTime _lastRefreshTime = DateTime.MinValue;
        private readonly TimeSpan _minimumRefreshInterval = TimeSpan.FromSeconds(5); // Tối thiểu 5 giây giữa các lần refresh
        private System.Windows.Forms.Timer _changeDetectionTimer;

        private class SaleRecord
        {
            public string InvoiceId { get; set; }
            public string CustomerName { get; set; }
            public string Gender { get; set; }
            public string Phone { get; set; }
            public string Region { get; set; }
            public string Seats { get; set; }
            public int TicketCount { get; set; }
            public decimal Amount { get; set; }
            public DateTime SaleDate { get; set; }
        }

        public frmReport()
        {
            InitializeComponent();
            InitForm();
            InitEvents();
            InitializeSmartRefresh();
        }

        public void SetSourceGrid(DataGridView dgv)
        {
            _sourceGrid = dgv;
            LoadSourceData();
            RefreshAll();
        }

        private void InitializeSmartRefresh()
        {
            // Timer để kiểm tra thay đổi dữ liệu nguồn thay vì refresh liên tục
            _changeDetectionTimer = new System.Windows.Forms.Timer();
            _changeDetectionTimer.Interval = 2000; // Kiểm tra mỗi 2 giây
            _changeDetectionTimer.Tick += ChangeDetectionTimer_Tick;
            _changeDetectionTimer.Start();
        }

        private void ChangeDetectionTimer_Tick(object sender, EventArgs e)
        {
            if (_sourceGrid == null || !_initialized) return;

            // Kiểm tra xem đã đủ thời gian để refresh chưa
            if (DateTime.Now - _lastRefreshTime < _minimumRefreshInterval) return;

            // Tính hash của dữ liệu hiện tại
            var currentHash = ComputeDataHash();
            
            // Chỉ refresh khi dữ liệu thực sự thay đổi
            if (currentHash != _lastDataHash)
            {
                _lastDataHash = currentHash;
                _lastRefreshTime = DateTime.Now;
                
                // Thực hiện refresh trong background thread để không block UI
                RefreshDataAsync();
            }
        }

        private string ComputeDataHash()
        {
            if (_sourceGrid == null) return string.Empty;
            
            try
            {
                var dataSignature = new StringBuilder();
                dataSignature.Append($"RowCount:{_sourceGrid.Rows.Count}|");
                
                // Lấy mẫu một vài dòng để tạo signature (không cần check tất cả để tối ưu performance)
                var sampleRows = Math.Min(_sourceGrid.Rows.Count, 10);
                for (int i = 0; i < sampleRows; i++)
                {
                    var row = _sourceGrid.Rows[i];
                    if (row.IsNewRow) continue;
                    
                    for (int j = 0; j < Math.Min(row.Cells.Count, 5); j++) // Chỉ lấy 5 cột đầu
                    {
                        dataSignature.Append($"{row.Cells[j].Value}|");
                    }
                }
                
                // Thêm thông tin filter hiện tại
                dataSignature.Append($"From:{dateTimePicker1.Value:yyyyMMdd}|");
                dataSignature.Append($"To:{dateTimePicker2.Value:yyyyMMdd}|");
                dataSignature.Append($"Region:{comboBox1.SelectedItem}|");
                
                using (var sha256 = SHA256.Create())
                {
                    var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(dataSignature.ToString()));
                    return Convert.ToBase64String(hash);
                }
            }
            catch
            {
                return Guid.NewGuid().ToString(); // Fallback để force refresh nếu có lỗi
            }
        }

        private async void RefreshDataAsync()
        {
            try
            {
                // Sử dụng Task.Run để không block UI thread
                await System.Threading.Tasks.Task.Run(() =>
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() =>
                        {
                            LoadSourceData();
                            RefreshAll();
                        }));
                    }
                    else
                    {
                        LoadSourceData();
                        RefreshAll();
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SmartRefresh] Error: {ex.Message}");
            }
        }

        private void InitForm()
        {
            Text = "Báo cáo tổng hợp - Smart Refresh";
            StartPosition = FormStartPosition.CenterParent;
            comboBox1.Items.Clear();
            comboBox1.Items.Add("Tất cả");
            comboBox1.Items.AddRange(RegionManager.Regions.ToArray());
            if (comboBox1.Items.Count > 0) comboBox1.SelectedIndex = 0;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = AppConstants.DATE_FORMAT;
            dateTimePicker2.CustomFormat = AppConstants.DATE_FORMAT;
            dateTimePicker2.Value = DateTime.Now.Date;
            dateTimePicker1.Value = DateTime.Now.Date.AddMonths(-1);
            ConfigureCharts();
            _initialized = true;
        }

        private void InitEvents()
        {
            // Sử dụng debounced refresh để tránh refresh quá nhiều khi user thay đổi filter
            dateTimePicker1.ValueChanged += (s, e) => { if (_initialized) ScheduleDebouncedRefresh(); };
            dateTimePicker2.ValueChanged += (s, e) => { if (_initialized) ScheduleDebouncedRefresh(); };
            comboBox1.SelectedIndexChanged += (s, e) => { if (_initialized) ScheduleDebouncedRefresh(); };
            button1.Click += (s, e) => RefreshAll(); // Manual refresh button
            if (btnExportExcel != null) btnExportExcel.Click += BtnExportExcel_Click;
            if (btnExportPdf != null) btnExportPdf.Click += BtnExportPdf_Click;
        }

        private System.Windows.Forms.Timer _debounceTimer;
        private void ScheduleDebouncedRefresh()
        {
            // Debounce mechanism: chờ 500ms sau thay đổi cuối cùng mới refresh
            _debounceTimer?.Stop();
            _debounceTimer?.Dispose();
            
            _debounceTimer = new System.Windows.Forms.Timer();
            _debounceTimer.Interval = 500;
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                _debounceTimer.Dispose();
                _debounceTimer = null;
                
                // Force refresh vì user thay đổi filter
                _lastDataHash = string.Empty; 
                RefreshDataAsync();
            };
            _debounceTimer.Start();
        }

        private void ConfigureCharts()
        {
            chartGenderRatio.Series.Clear();
            var genderSeries = new Series("Giới tính") { ChartType = SeriesChartType.Pie, IsValueShownAsLabel = true };
            chartGenderRatio.Series.Add(genderSeries);
            chartGenderRatio.Titles.Clear();
            chartGenderRatio.Titles.Add("Tỷ lệ giới tính");

            chartSeatOccupancy.Series.Clear();
            var occSeries = new Series("Sử dụng ghế") { ChartType = SeriesChartType.Doughnut, IsValueShownAsLabel = true };
            chartSeatOccupancy.Series.Add(occSeries);
            chartSeatOccupancy.Titles.Clear();
            chartSeatOccupancy.Titles.Add("Tỷ lệ sử dụng ghế");

            chartRegionCompare.Series.Clear();
            var regSeries = new Series("Khu vực") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true };
            chartRegionCompare.Series.Add(regSeries);
            chartRegionCompare.ChartAreas[0].AxisX.Interval = 1;
            chartRegionCompare.Titles.Clear();
            chartRegionCompare.Titles.Add("Top khu vực bán chạy (vé)");

            chartDayCompare.Series.Clear();
            var daySeries = new Series("Ngày") { ChartType = SeriesChartType.Column, IsValueShownAsLabel = true };
            chartDayCompare.Series.Add(daySeries);
            chartDayCompare.ChartAreas[0].AxisX.Interval = 1;
            chartDayCompare.Titles.Clear();
            chartDayCompare.Titles.Add("Top ngày bán chạy (vé)");
        }

        private void LoadSourceData()
        {
            _allRecords.Clear();
            if (_sourceGrid == null) return;
            foreach (DataGridViewRow row in _sourceGrid.Rows)
            {
                if (row.IsNewRow) continue;
                try
                {
                    string invoiceId = SafeStr(row.Cells["colInvoiceId"].Value);
                    string name = SafeStr(row.Cells["colCustomerName"].Value);
                    string gender = SafeStr(row.Cells["colGender"].Value);
                    string phone = SafeStr(row.Cells["colPhone"].Value);
                    string region = SafeStr(row.Cells["colRegion"].Value);
                    string seats = SafeStr(row.Cells["colSeat"].Value);

                    // Ticket count
                    int ticketCount = 0;
                    var tcObj = row.Cells["colTicketCount"].Value;
                    if (tcObj is int) ticketCount = (int)tcObj; else int.TryParse(SafeStr(tcObj), out ticketCount);

                    // Amount
                    decimal amount = 0m;
                    var amtObj = row.Cells["colTotalAmount"].Value;
                    if (amtObj is decimal) amount = (decimal)amtObj;
                    else if (amtObj is double) amount = Convert.ToDecimal((double)amtObj);
                    else amount = ParseAmount(SafeStr(amtObj));

                    // Date
                    DateTime saleDate = DateTime.MinValue;
                    var dateObj = row.Cells["colSaleDate"].Value;
                    if (dateObj is DateTime dt) saleDate = dt.Date;
                    else
                    {
                        var s = SafeStr(dateObj);
                        if (!DateTime.TryParseExact(s, AppConstants.DATE_FORMAT, _vn, DateTimeStyles.None, out saleDate))
                            DateTime.TryParse(s, _vn, DateTimeStyles.None, out saleDate);
                        saleDate = saleDate.Date;
                    }

                    if (saleDate == DateTime.MinValue) continue;
                    _allRecords.Add(new SaleRecord { InvoiceId = invoiceId, CustomerName = name, Gender = gender, Phone = phone, Region = region, Seats = seats, TicketCount = ticketCount, Amount = amount, SaleDate = saleDate });
                }
                catch { }
            }
        }

        private string SafeStr(object v) => v == null ? string.Empty : Convert.ToString(v, _vn);

        private decimal ParseAmount(string amountStr)
        {
            if (string.IsNullOrWhiteSpace(amountStr)) return 0m;
            var s = amountStr.Replace("VND", string.Empty).Trim();
            // Try VN culture with thousands separator and space
            if (decimal.TryParse(s, NumberStyles.Any, _vn, out var valVN)) return valVN;
            // Strip non-digits then parse invariant
            var digits = new string(s.Where(char.IsDigit).ToArray());
            if (decimal.TryParse(digits, NumberStyles.None, CultureInfo.InvariantCulture, out var valInvariant)) return valInvariant;
            // Fallback remove commas
            s = s.Replace(",", "").Replace(".", "");
            decimal.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out var val);
            return val;
        }

        private IEnumerable<SaleRecord> ApplyFilters()
        {
            DateTime from = dateTimePicker1.Value.Date;
            DateTime to = dateTimePicker2.Value.Date;
            if (to < from) to = from;
            string region = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "Tất cả";
            var q = _allRecords.Where(r => r.SaleDate.Date >= from && r.SaleDate.Date <= to);
            if (!string.IsNullOrEmpty(region) && region != "Tất cả") q = q.Where(r => r.Region == region);
            return q.ToList();
        }

        private void RefreshAll()
        {
            try
            {
                var filtered = ApplyFilters().ToList();
                UpdateSummary(filtered);
                UpdateGenderChart(filtered);
                UpdateOccupancyChart(filtered);
                UpdateCompareCharts(filtered);
                UpdateTopGrid(filtered);
                
                // Cập nhật title để hiển thị thời gian refresh cuối
                this.Text = $"Báo cáo tổng hợp - Smart Refresh (Cập nhật: {DateTime.Now:HH:mm:ss})";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[RefreshAll] Error: {ex.Message}");
            }
        }

        private void UpdateSummary(IEnumerable<SaleRecord> data)
        {
            int totalTickets = data.Sum(r => r.TicketCount);
            decimal totalRevenue = data.Sum(r => r.Amount);
            int totalCustomers = data.Select(r => r.CustomerName + r.Phone).Distinct().Count();
            lblTotalTickets.Text = "Tổng số vé: " + totalTickets.ToString("N3", _vn);
            lblTotalRevenue.Text = "Tổng doanh thu: " + totalRevenue.ToString("N3", _vn) + " VND";
            lblTotalCustomers.Text = "Tổng số khách hàng: " + totalCustomers.ToString("N3", _vn);
        }

        private void UpdateGenderChart(IEnumerable<SaleRecord> data)
        {
            var series = chartGenderRatio.Series[0];
            series.Points.Clear();
            int male = data.Count(r => string.Equals(r.Gender, "Nam", StringComparison.OrdinalIgnoreCase));
            int female = data.Count(r => string.Equals(r.Gender, "Nữ", StringComparison.OrdinalIgnoreCase));
            if (male + female == 0)
            {
                var p = series.Points.Add(1);
                p.LegendText = "Không có dữ liệu";
                p.Label = "Không có dữ liệu";
                p.Color = System.Drawing.Color.LightGray;
                return;
            }
            if (male > 0) { var p = series.Points.Add(male); p.LegendText = "Nam"; p.Label = "Nam: " + male; }
            if (female > 0) { var p = series.Points.Add(female); p.LegendText = "Nữ"; p.Label = "Nữ: " + female; }
        }

        private void UpdateOccupancyChart(IEnumerable<SaleRecord> data)
        {
            var series = chartSeatOccupancy.Series[0];
            series.Points.Clear();
            int sold = data.Sum(r => r.TicketCount);
            int distinctDays = data.Select(r => r.SaleDate.Date).Distinct().Count();
            int distinctRegions = data.Select(r => r.Region).Distinct().Count();
            int capacity = AppConstants.TOTAL_SEATS * Math.Max(1, distinctDays) * Math.Max(1, distinctRegions);
            int remaining = Math.Max(0, capacity - sold);
            if (capacity == 0)
            {
                var p = series.Points.Add(1);
                p.LegendText = "Không có dữ liệu";
                p.Label = "Không có dữ liệu";
                p.Color = System.Drawing.Color.LightGray;
                return;
            }
            var s1 = series.Points.Add(sold); s1.LegendText = "Đã bán"; s1.Label = sold.ToString("N3", _vn); s1.Color = System.Drawing.Color.SteelBlue;
            var s2 = series.Points.Add(remaining); s2.LegendText = "Còn lại"; s2.Label = remaining.ToString("N3", _vn); s2.Color = System.Drawing.Color.LightGray;
        }

        private void UpdateCompareCharts(IEnumerable<SaleRecord> data)
        {
            var regSeries = chartRegionCompare.Series[0];
            regSeries.Points.Clear();
            var regionGroup = data.GroupBy(r => r.Region).Select(g => new { Region = g.Key, Tickets = g.Sum(x => x.TicketCount) }).OrderByDescending(x => x.Tickets).ToList();
            if (regionGroup.Count == 0)
            {
                var p = regSeries.Points.Add(1);
                p.AxisLabel = "Không có dữ liệu";
                p.Label = "0";
                p.Color = System.Drawing.Color.LightGray;
            }
            else
            {
                foreach (var item in regionGroup)
                {
                    var p = regSeries.Points.Add(item.Tickets);
                    p.AxisLabel = item.Region;
                    p.Label = item.Tickets.ToString("N3", _vn);
                }
            }

            var daySeries = chartDayCompare.Series[0];
            daySeries.Points.Clear();
            var dayGroup = data.GroupBy(r => r.SaleDate.Date).Select(g => new { Day = g.Key, Tickets = g.Sum(x => x.TicketCount) }).OrderByDescending(x => x.Tickets).ThenBy(x => x.Day).ToList();
            if (dayGroup.Count == 0)
            {
                var p = daySeries.Points.Add(1);
                p.AxisLabel = "Không có dữ liệu";
                p.Label = "0";
                p.Color = System.Drawing.Color.LightGray;
            }
            else
            {
                foreach (var item in dayGroup)
                {
                    var p = daySeries.Points.Add(item.Tickets);
                    p.AxisLabel = item.Day.ToString(AppConstants.DATE_FORMAT);
                    p.Label = item.Tickets.ToString("N3", _vn);
                }
            }
        }

        private void UpdateTopGrid(IEnumerable<SaleRecord> data)
        {
            dataGridView1.Rows.Clear();
            var topCustomers = data.GroupBy(r => r.CustomerName + "|" + r.Phone).Select(g => new { Key = g.Key, Tickets = g.Sum(x => x.TicketCount) }).OrderByDescending(x => x.Tickets).Take(10).ToList();
            var topRegions = data.GroupBy(r => r.Region).Select(g => new { Region = g.Key, Tickets = g.Sum(x => x.TicketCount) }).OrderByDescending(x => x.Tickets).Take(10).ToList();
            var topDays = data.GroupBy(r => r.SaleDate.Date).Select(g => new { Day = g.Key, Tickets = g.Sum(x => x.TicketCount) }).OrderByDescending(x => x.Tickets).ThenBy(x => x.Day).Take(10).ToList();
            int maxRows = Math.Max(topCustomers.Count, Math.Max(topRegions.Count, topDays.Count));
            for (int i = 0; i < maxRows; i++)
            {
                string c = i < topCustomers.Count ? FormatCustomer(topCustomers[i].Key, topCustomers[i].Tickets) : string.Empty;
                string r = i < topRegions.Count ? topRegions[i].Region + " (" + topRegions[i].Tickets + ")" : string.Empty;
                string d = i < topDays.Count ? topDays[i].Day.ToString(AppConstants.DATE_FORMAT) + " (" + topDays[i].Tickets + ")" : string.Empty;
                dataGridView1.Rows.Add(c, r, d);
            }
            if (maxRows == 0) dataGridView1.Rows.Add("Không có dữ liệu", "", "");
        }

        private string FormatCustomer(string key, int tickets)
        {
            var parts = key.Split('|');
            string name = parts.Length > 0 ? parts[0] : key;
            string phone = parts.Length > 1 ? parts[1] : string.Empty;
            return name + (string.IsNullOrEmpty(phone) ? string.Empty : (" - " + phone)) + " (" + tickets + ")";
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            // Cleanup timers
            _changeDetectionTimer?.Stop();
            _changeDetectionTimer?.Dispose();
            _debounceTimer?.Stop();
            _debounceTimer?.Dispose();
            
            base.OnFormClosed(e);
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ApplyFilters().ToList();
                if (data.Count == 0) { MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "CSV (Excel)|*.csv";
                    dlg.FileName = "BaoCaoTongHop_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".csv";
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    ExportCsv(dlg.FileName, data);
                }
                MessageBox.Show("Xuất CSV thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xuất CSV: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ExportCsv(string filePath, IEnumerable<SaleRecord> data)
        {
            using (var w = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                w.WriteLine("Mã hóa đơn,Tên khách hàng,Giới tính,SĐT,Khu vực,Ghế,Tổng vé,Thành tiền (VND),Ngày bán vé");
                foreach (var r in data)
                {
                    w.WriteLine(string.Join(",", new string[] { Csv(r.InvoiceId), Csv(r.CustomerName), Csv(r.Gender), Csv(r.Phone), Csv(r.Region), Csv(r.Seats), r.TicketCount.ToString(), r.Amount.ToString("F0"), r.SaleDate.ToString(AppConstants.DATE_FORMAT) }));
                }
                w.WriteLine();
                w.WriteLine("Tổng vé," + data.Sum(x => x.TicketCount));
                w.WriteLine("Tổng doanh thu," + data.Sum(x => x.Amount).ToString("F0"));
                w.WriteLine("Từ ngày," + dateTimePicker1.Value.ToString(AppConstants.DATE_FORMAT));
                w.WriteLine("Đến ngày," + dateTimePicker2.Value.ToString(AppConstants.DATE_FORMAT));
                if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() != "Tất cả") w.WriteLine("Khu vực," + comboBox1.SelectedItem);
            }
        }
        private string Csv(string s) { if (s == null) return string.Empty; if (s.Contains(",") || s.Contains("\"")) return "\"" + s.Replace("\"", "\"\"") + "\""; return s; }

        private void BtnExportPdf_Click(object sender, EventArgs e)
        {
            try
            {
                var data = ApplyFilters().ToList();
                if (data.Count == 0) { MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "PDF file|*.pdf";
                    dlg.FileName = "BaoCaoTongHop_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".pdf";
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    ExportPdf(dlg.FileName, data);
                }
                MessageBox.Show("Xuất PDF thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { MessageBox.Show("Lỗi xuất PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private iTextSharp.text.Image ChartToITextImage(Chart chart)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                chart.SaveImage(ms, ChartImageFormat.Png);
                return iTextSharp.text.Image.GetInstance(ms.ToArray());
            }
        }

        private void ExportPdf(string path, List<SaleRecord> data)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                var doc = new Document(PageSize.A4.Rotate(), 20f, 20f, 25f, 25f);
                PdfWriter.GetInstance(doc, fs);
                doc.Open();
                var fontHeader = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16f);
                var fontNormal = FontFactory.GetFont(FontFactory.HELVETICA, 10f);
                var fontSmall = FontFactory.GetFont(FontFactory.HELVETICA, 8f, iTextSharp.text.Font.ITALIC, BaseColor.DARK_GRAY);
                doc.Add(new Paragraph("BÁO CÁO TỔNG HỢP", fontHeader) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 10f });
                doc.Add(new Paragraph(string.Format("Khoảng thời gian: {0} - {1}", dateTimePicker1.Value.ToString(AppConstants.DATE_FORMAT), dateTimePicker2.Value.ToString(AppConstants.DATE_FORMAT)), fontNormal));
                doc.Add(new Paragraph("Khu vực: " + (comboBox1.SelectedItem ?? "Tất cả"), fontNormal));
                doc.Add(new Paragraph(string.Format("Tổng vé: {0:N3}    Tổng doanh thu: {1:N0} VND", data.Sum(x => x.TicketCount), data.Sum(x => x.Amount)), fontNormal) { SpacingAfter = 10f });
                PdfPTable table = new PdfPTable(8) { WidthPercentage = 100f, SpacingBefore = 5f, SpacingAfter = 10f };
                table.SetWidths(new float[] { 10, 18, 8, 12, 12, 15, 8, 12 });
                AddHeaderCell(table, "Mã"); AddHeaderCell(table, "Khách hàng"); AddHeaderCell(table, "GT"); AddHeaderCell(table, "SĐT"); AddHeaderCell(table, "Khu vực"); AddHeaderCell(table, "Ghế"); AddHeaderCell(table, "Vé"); AddHeaderCell(table, "Tiền (VND)");
                int maxRows = 200;
                foreach (var r in data.Take(maxRows))
                {
                    AddBodyCell(table, r.InvoiceId, fontSmall); AddBodyCell(table, r.CustomerName, fontSmall); AddBodyCell(table, r.Gender, fontSmall);
                    AddBodyCell(table, r.Phone, fontSmall); AddBodyCell(table, r.Region, fontSmall); AddBodyCell(table, r.Seats, fontSmall);
                    AddBodyCell(table, r.TicketCount.ToString(), fontSmall, Element.ALIGN_RIGHT); AddBodyCell(table, r.Amount.ToString("N3", _vn), fontSmall, Element.ALIGN_RIGHT);
                }
                if (data.Count > maxRows)
                { var more = new PdfPCell(new Phrase($"... còn {data.Count - maxRows} dòng", fontSmall)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, BackgroundColor = new BaseColor(240, 240, 240) }; table.AddCell(more); }
                doc.Add(table);
                var imgGender = ChartToITextImage(chartGenderRatio); imgGender.ScaleToFit(350f, 300f);
                var imgSeat = ChartToITextImage(chartSeatOccupancy); imgSeat.ScaleToFit(350f, 300f);
                var imgRegion = ChartToITextImage(chartRegionCompare); imgRegion.ScaleToFit(350f, 300f);
                var imgDay = ChartToITextImage(chartDayCompare); imgDay.ScaleToFit(350f, 300f);
                PdfPTable chartTable1 = new PdfPTable(2) { WidthPercentage = 100f, SpacingBefore = 5f, SpacingAfter = 5f }; chartTable1.SetWidths(new float[] { 50, 50 }); chartTable1.AddCell(WrapImageCell(imgGender)); chartTable1.AddCell(WrapImageCell(imgSeat)); doc.Add(chartTable1);
                PdfPTable chartTable2 = new PdfPTable(2) { WidthPercentage = 100f, SpacingBefore = 5f, SpacingAfter = 5f }; chartTable2.SetWidths(new float[] { 50, 50 }); chartTable2.AddCell(WrapImageCell(imgRegion)); chartTable2.AddCell(WrapImageCell(imgDay)); doc.Add(chartTable2);
                doc.Close();
            }
        }

        private PdfPCell WrapImageCell(iTextSharp.text.Image img) => new PdfPCell(img) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, Padding = 4f, Border = Rectangle.NO_BORDER };
        private void AddHeaderCell(PdfPTable tbl, string text) { var f = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9f, BaseColor.WHITE); var c = new PdfPCell(new Phrase(text, f)) { BackgroundColor = new BaseColor(70, 130, 180), HorizontalAlignment = Element.ALIGN_CENTER, Padding = 4f }; tbl.AddCell(c); }
        private void AddBodyCell(PdfPTable tbl, string text, iTextSharp.text.Font f, int align = Element.ALIGN_LEFT) { var c = new PdfPCell(new Phrase(text ?? string.Empty, f)) { HorizontalAlignment = align, Padding = 3f }; tbl.AddCell(c); }

        // Designer placeholder handlers
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
        private void chartRegionCompare_Click(object sender, EventArgs e) { }
    }
}
