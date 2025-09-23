using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace xinchaothegioi
{
    public partial class frmReport : Form
    {
        private DataGridView _sourceGrid; // Grid nguồn từ frmMain1
        private readonly List<SaleRecord> _allRecords = new List<SaleRecord>();
        private readonly CultureInfo _vn = new CultureInfo("vi-VN");
        private bool _initialized;

        // Bản ghi vé bán ra
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
        }

        // Cho phép form khác truyền vào DataGridView nguồn
        public void SetSourceGrid(DataGridView dgv)
        {
            _sourceGrid = dgv;
            LoadSourceData();
            RefreshAll();
        }

        private void InitForm()
        {
            Text = "Báo cáo tổng hợp";
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
            dateTimePicker1.ValueChanged += (s, e) => { if (_initialized) RefreshAll(); };
            dateTimePicker2.ValueChanged += (s, e) => { if (_initialized) RefreshAll(); };
            comboBox1.SelectedIndexChanged += (s, e) => { if (_initialized) RefreshAll(); };
            button1.Click += (s, e) => RefreshAll(); // Lọc
        }

        private void ConfigureCharts()
        {
            // Giới tính - Pie
            chartGenderRatio.Series.Clear();
            var genderSeries = new Series("Giới tính");
            genderSeries.ChartType = SeriesChartType.Pie;
            genderSeries.IsValueShownAsLabel = true;
            chartGenderRatio.Series.Add(genderSeries);
            chartGenderRatio.Titles.Clear();
            chartGenderRatio.Titles.Add("Tỷ lệ giới tính");

            // Tỷ lệ sử dụng ghế - Doughnut
            chartSeatOccupancy.Series.Clear();
            var occSeries = new Series("Sử dụng ghế");
            occSeries.ChartType = SeriesChartType.Doughnut;
            occSeries.IsValueShownAsLabel = true;
            chartSeatOccupancy.Series.Add(occSeries);
            chartSeatOccupancy.Titles.Clear();
            chartSeatOccupancy.Titles.Add("Tỷ lệ sử dụng ghế");

            // So sánh khu vực - Column
            chartRegionCompare.Series.Clear();
            var regSeries = new Series("Khu vực");
            regSeries.ChartType = SeriesChartType.Column;
            regSeries.IsValueShownAsLabel = true;
            chartRegionCompare.Series.Add(regSeries);
            chartRegionCompare.ChartAreas[0].AxisX.Interval = 1;
            chartRegionCompare.Titles.Clear();
            chartRegionCompare.Titles.Add("Top khu vực bán chạy (vé)");

            // So sánh ngày - Column
            chartDayCompare.Series.Clear();
            var daySeries = new Series("Ngày");
            daySeries.ChartType = SeriesChartType.Column;
            daySeries.IsValueShownAsLabel = true;
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
                    string invoiceId = Convert.ToString(row.Cells["colInvoiceId"].Value);
                    string name = Convert.ToString(row.Cells["colCustomerName"].Value);
                    string gender = Convert.ToString(row.Cells["colGender"].Value);
                    string phone = Convert.ToString(row.Cells["colPhone"].Value);
                    string region = Convert.ToString(row.Cells["colRegion"].Value);
                    string seats = Convert.ToString(row.Cells["colSeat"].Value);
                    string ticketStr = Convert.ToString(row.Cells["colTicketCount"].Value);
                    string amountStr = Convert.ToString(row.Cells["colTotalAmount"].Value);
                    string dateStr = Convert.ToString(row.Cells["colSaleDate"].Value);

                    int ticketCount;
                    int.TryParse(ticketStr, out ticketCount);

                    decimal amount = ParseAmount(amountStr);
                    DateTime saleDate;
                    DateTime.TryParseExact(dateStr, AppConstants.DATE_FORMAT, _vn, DateTimeStyles.None, out saleDate);

                    if (saleDate == DateTime.MinValue) continue; // bỏ qua lỗi

                    _allRecords.Add(new SaleRecord
                    {
                        InvoiceId = invoiceId,
                        CustomerName = name,
                        Gender = gender,
                        Phone = phone,
                        Region = region,
                        Seats = seats,
                        TicketCount = ticketCount,
                        Amount = amount,
                        SaleDate = saleDate
                    });
                }
                catch { /* bỏ lỗi từng dòng */ }
            }
        }

        private decimal ParseAmount(string amountStr)
        {
            if (string.IsNullOrWhiteSpace(amountStr)) return 0m;
            // Ví dụ: "80,000 VND"
            amountStr = amountStr.Replace("VND", string.Empty).Trim();
            decimal val;
            if (decimal.TryParse(amountStr, NumberStyles.Any, _vn, out val)) return val;
            // Thử bỏ dấu phẩy
            amountStr = amountStr.Replace(",", "");
            decimal.TryParse(amountStr, NumberStyles.Any, CultureInfo.InvariantCulture, out val);
            return val;
        }

        private IEnumerable<SaleRecord> ApplyFilters()
        {
            DateTime from = dateTimePicker1.Value.Date;
            DateTime to = dateTimePicker2.Value.Date;
            if (to < from) to = from;
            string region = comboBox1.SelectedItem != null ? comboBox1.SelectedItem.ToString() : "Tất cả";
            var q = _allRecords.Where(r => r.SaleDate.Date >= from && r.SaleDate.Date <= to);
            if (!string.IsNullOrEmpty(region) && region != "Tất cả")
                q = q.Where(r => r.Region == region);
            return q.ToList();
        }

        private void RefreshAll()
        {
            var filtered = ApplyFilters().ToList();
            UpdateSummary(filtered);
            UpdateGenderChart(filtered);
            UpdateOccupancyChart(filtered);
            UpdateCompareCharts(filtered);
            UpdateTopGrid(filtered);
        }

        private void UpdateSummary(IEnumerable<SaleRecord> data)
        {
            int totalTickets = data.Sum(r => r.TicketCount);
            decimal totalRevenue = data.Sum(r => r.Amount);
            int totalCustomers = data.Select(r => r.CustomerName + r.Phone).Distinct().Count();

            lblTotalTickets.Text = "Tổng số vé: " + totalTickets.ToString("N0", _vn);
            lblTotalRevenue.Text = "Tổng doanh thu: " + totalRevenue.ToString("N0", _vn) + " VND";
            lblTotalCustomers.Text = "Tổng số khách hàng: " + totalCustomers.ToString("N0", _vn);
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
                p.Color = Color.LightGray;
                return;
            }

            if (male > 0)
            {
                var p = series.Points.Add(male);
                p.LegendText = "Nam";
                p.Label = "Nam: " + male;
            }
            if (female > 0)
            {
                var p = series.Points.Add(female);
                p.LegendText = "Nữ";
                p.Label = "Nữ: " + female;
            }
        }

        private void UpdateOccupancyChart(IEnumerable<SaleRecord> data)
        {
            var series = chartSeatOccupancy.Series[0];
            series.Points.Clear();
            int sold = data.Sum(r => r.TicketCount);
            // Giả định: mỗi ngày và mỗi khu vực có tối đa AppConstants.TOTAL_SEATS lượt chỗ (1 suất chiếu)
            int distinctDays = data.Select(r => r.SaleDate.Date).Distinct().Count();
            int distinctRegions = data.Select(r => r.Region).Distinct().Count();
            int capacity = AppConstants.TOTAL_SEATS * Math.Max(1, distinctDays) * Math.Max(1, distinctRegions);
            int remaining = Math.Max(0, capacity - sold);

            if (capacity == 0)
            {
                var p = series.Points.Add(1);
                p.LegendText = "Không có dữ liệu";
                p.Label = "Không có dữ liệu";
                p.Color = Color.LightGray;
                return;
            }

            var s1 = series.Points.Add(sold);
            s1.LegendText = "Đã bán";
            s1.Label = sold.ToString("N0", _vn);
            s1.Color = Color.SteelBlue;

            var s2 = series.Points.Add(remaining);
            s2.LegendText = "Còn lại";
            s2.Label = remaining.ToString("N0", _vn);
            s2.Color = Color.LightGray;
        }

        private void UpdateCompareCharts(IEnumerable<SaleRecord> data)
        {
            // Khu vực
            var regSeries = chartRegionCompare.Series[0];
            regSeries.Points.Clear();
            var regionGroup = data.GroupBy(r => r.Region)
                .Select(g => new { Region = g.Key, Tickets = g.Sum(x => x.TicketCount) })
                .OrderByDescending(x => x.Tickets)
                .ToList();
            if (regionGroup.Count == 0)
            {
                var p = regSeries.Points.Add(1);
                p.AxisLabel = "Không có dữ liệu";
                p.Label = "0";
                p.Color = Color.LightGray;
            }
            else
            {
                foreach (var item in regionGroup)
                {
                    var p = regSeries.Points.Add(item.Tickets);
                    p.AxisLabel = item.Region;
                    p.Label = item.Tickets.ToString("N0", _vn);
                }
            }

            // Ngày
            var daySeries = chartDayCompare.Series[0];
            daySeries.Points.Clear();
            var dayGroup = data.GroupBy(r => r.SaleDate.Date)
                .Select(g => new { Day = g.Key, Tickets = g.Sum(x => x.TicketCount) })
                .OrderByDescending(x => x.Tickets)
                .ThenBy(x => x.Day)
                .ToList();
            if (dayGroup.Count == 0)
            {
                var p = daySeries.Points.Add(1);
                p.AxisLabel = "Không có dữ liệu";
                p.Label = "0";
                p.Color = Color.LightGray;
            }
            else
            {
                foreach (var item in dayGroup)
                {
                    var p = daySeries.Points.Add(item.Tickets);
                    p.AxisLabel = item.Day.ToString(AppConstants.DATE_FORMAT);
                    p.Label = item.Tickets.ToString("N0", _vn);
                }
            }
        }

        private void UpdateTopGrid(IEnumerable<SaleRecord> data)
        {
            dataGridView1.Rows.Clear();
            // Top khách hàng theo vé
            var topCustomers = data.GroupBy(r => r.CustomerName + "|" + r.Phone)
                .Select(g => new { Key = g.Key, Tickets = g.Sum(x => x.TicketCount) })
                .OrderByDescending(x => x.Tickets).Take(10).ToList();
            // Top khu vực
            var topRegions = data.GroupBy(r => r.Region)
                .Select(g => new { Region = g.Key, Tickets = g.Sum(x => x.TicketCount) })
                .OrderByDescending(x => x.Tickets).Take(10).ToList();
            // Top ngày
            var topDays = data.GroupBy(r => r.SaleDate.Date)
                .Select(g => new { Day = g.Key, Tickets = g.Sum(x => x.TicketCount) })
                .OrderByDescending(x => x.Tickets).ThenBy(x => x.Day).Take(10).ToList();

            int maxRows = Math.Max(topCustomers.Count, Math.Max(topRegions.Count, topDays.Count));
            for (int i = 0; i < maxRows; i++)
            {
                string c = i < topCustomers.Count ? FormatCustomer(topCustomers[i].Key, topCustomers[i].Tickets) : string.Empty;
                string r = i < topRegions.Count ? topRegions[i].Region + " (" + topRegions[i].Tickets + ")" : string.Empty;
                string d = i < topDays.Count ? topDays[i].Day.ToString(AppConstants.DATE_FORMAT) + " (" + topDays[i].Tickets + ")" : string.Empty;
                dataGridView1.Rows.Add(c, r, d);
            }

            if (maxRows == 0)
            {
                dataGridView1.Rows.Add("Không có dữ liệu", "", "");
            }
        }

        private string FormatCustomer(string key, int tickets)
        {
            var parts = key.Split('|');
            string name = parts.Length > 0 ? parts[0] : key;
            string phone = parts.Length > 1 ? parts[1] : string.Empty;
            return name + (string.IsNullOrEmpty(phone) ? string.Empty : (" - " + phone)) + " (" + tickets + ")";
        }

        // Sự kiện Designer đã gán (giữ lại tránh lỗi Designer)
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void chart1_Click(object sender, EventArgs e) { }
        private void chartRegionCompare_Click(object sender, EventArgs e) { }
    }
}
