using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace xinchaothegioi
{
    public partial class ViewRevenue : Form
    {
        private DataGridView sourceDataGridView;
        private DataTable revenueData;

        public ViewRevenue()
        {
            InitializeComponent();
            InitializeControls();
            InitializeCharts();
        }

        private void InitializeControls()
        {
            // Thiết lập ComboBox lọc khu vực
            cboRegionFilter.Items.Clear();
            cboRegionFilter.Items.Add("Tất cả");
            cboRegionFilter.Items.AddRange(RegionManager.Regions.ToArray());
            cboRegionFilter.SelectedIndex = 0;

            // Thiết lập ComboBox chọn phim
            cboSelectMovie.Items.Clear();
            cboSelectMovie.Items.Add("Tất cả");
            cboSelectMovie.Items.Add("Phim hành động");
            cboSelectMovie.Items.Add("Phim tình cảm");
            cboSelectMovie.Items.Add("Phim kinh dị");
            cboSelectMovie.SelectedIndex = 0;

            // Thiết lập DateTimePicker
            dtpFromDate.Value = DateTime.Now.AddDays(-30); // 30 ngày trước
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Format = DateTimePickerFormat.Short;

            // Ẩn dgvRevenue mặc định, chỉ hiển thị khi có data
            dgvRevenue.Visible = false;
        }

        private void InitializeCharts()
        {
            // Thiết lập biểu đồ cột
            InitializeColumnChart();
            
            // Thiết lập biểu đồ tròn
            InitializePieChart();
        }

        private void InitializeColumnChart()
        {
            chartRevenueColumn.Series.Clear();
            chartRevenueColumn.ChartAreas.Clear();
            
            // Tạo ChartArea
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Ngày";
            chartArea.AxisY.Title = "Doanh thu (VND)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartRevenueColumn.ChartAreas.Add(chartArea);
            
            // Tạo Series
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SkyBlue;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";
            chartRevenueColumn.Series.Add(series);
            
            // Thiết lập tiêu đề
            chartRevenueColumn.Titles.Clear();
            chartRevenueColumn.Titles.Add("Biểu đồ doanh thu theo ngày");
        }

        private void InitializePieChart()
        {
            chartRevenuePie.Series.Clear();
            chartRevenuePie.ChartAreas.Clear();
            
            // Tạo ChartArea
            ChartArea chartArea = new ChartArea("PieArea");
            chartRevenuePie.ChartAreas.Add(chartArea);
            
            // Tạo Series
            Series series = new Series("Doanh thu theo khu vực");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";
            chartRevenuePie.Series.Add(series);
            
            // Thiết lập tiêu đề
            chartRevenuePie.Titles.Clear();
            chartRevenuePie.Titles.Add("Biểu đồ doanh thu theo khu vực");
        }

        public void SetRevenueData(DataGridView sourceGrid)
        {
            sourceDataGridView = sourceGrid;
            LoadRevenueData();
            UpdateCharts();
            ShowFilteredData();
        }

        private void LoadRevenueData()
        {
            // Tạo DataTable để chứa dữ liệu doanh thu
            revenueData = new DataTable();
            revenueData.Columns.Add("Ngày", typeof(DateTime));
            revenueData.Columns.Add("Số vé bán", typeof(int));
            revenueData.Columns.Add("Doanh thu", typeof(decimal));
            revenueData.Columns.Add("Khu vực", typeof(string));
            revenueData.Columns.Add("Mã hóa đơn", typeof(string));
            revenueData.Columns.Add("Tên khách hàng", typeof(string));
            revenueData.Columns.Add("Giới tính", typeof(string));
            revenueData.Columns.Add("SĐT", typeof(string));
            revenueData.Columns.Add("Ghế ngồi", typeof(string));

            if (sourceDataGridView != null)
            {
                foreach (DataGridViewRow row in sourceDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    try
                    {
                        string dateStr = row.Cells["colSaleDate"].Value?.ToString();
                        string region = row.Cells["colRegion"].Value?.ToString();
                        string ticketCountStr = row.Cells["colTicketCount"].Value?.ToString();
                        string totalAmountStr = row.Cells["colTotalAmount"].Value?.ToString();
                        string invoiceId = row.Cells["colInvoiceId"].Value?.ToString();
                        string customerName = row.Cells["colCustomerName"].Value?.ToString();
                        string gender = row.Cells["colGender"].Value?.ToString();
                        string phone = row.Cells["colPhone"].Value?.ToString();
                        string seats = row.Cells["colSeat"].Value?.ToString();

                        if (DateTime.TryParse(dateStr, out DateTime saleDate) &&
                            int.TryParse(ticketCountStr, out int ticketCount) &&
                            !string.IsNullOrEmpty(totalAmountStr))
                        {
                            // Xử lý số tiền (loại bỏ " VND" và dấu phẩy)
                            string cleanAmount = totalAmountStr.Replace(" VND", "").Replace(",", "");
                            if (decimal.TryParse(cleanAmount, out decimal amount))
                            {
                                revenueData.Rows.Add(saleDate, ticketCount, amount, region, 
                                    invoiceId, customerName, gender, phone, seats);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error processing row: {ex.Message}");
                    }
                }
            }
        }

        private void UpdateCharts()
        {
            UpdateColumnChart();
            UpdatePieChart();
            UpdateSummaryLabels();
        }

        private void UpdateColumnChart()
        {
            chartRevenueColumn.Series["Doanh thu"].Points.Clear();
            
            var filteredData = GetFilteredData();

            // Nhóm theo ngày và tính tổng doanh thu
            var chartData = filteredData
                .GroupBy(row => ((DateTime)row["Ngày"]).Date)
                .Select(g => new { 
                    Date = g.Key, 
                    Revenue = g.Sum(row => (decimal)row["Doanh thu"]) 
                })
                .OrderBy(x => x.Date);

            foreach (var item in chartData)
            {
                chartRevenueColumn.Series["Doanh thu"].Points.AddXY(
                    item.Date.ToString("dd/MM"), (double)item.Revenue);
            }
        }

        private void UpdatePieChart()
        {
            chartRevenuePie.Series["Doanh thu theo khu vực"].Points.Clear();
            
            var filteredData = GetFilteredData();

            // Nhóm theo khu vực và tính tổng doanh thu
            var pieData = filteredData
                .GroupBy(row => row["Khu vực"].ToString())
                .Select(g => new { 
                    Region = g.Key, 
                    Revenue = g.Sum(row => (decimal)row["Doanh thu"]) 
                });

            foreach (var item in pieData)
            {
                var point = chartRevenuePie.Series["Doanh thu theo khu vực"].Points.Add((double)item.Revenue);
                point.LegendText = item.Region;
                point.Label = $"{item.Region}\n{item.Revenue:N0}";
            }
        }

        private DataRow[] GetFilteredData()
        {
            var filteredData = revenueData.AsEnumerable();

            // Lọc theo ngày
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
            
            filteredData = filteredData.Where(row => 
            {
                DateTime date = (DateTime)row["Ngày"];
                return date >= fromDate && date <= toDate;
            });

            // Lọc theo khu vực
            string selectedRegion = cboRegionFilter.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedRegion) && selectedRegion != "Tất cả")
            {
                filteredData = filteredData.Where(row => 
                    row["Khu vực"].ToString() == selectedRegion);
            }

            return filteredData.ToArray();
        }

        private void UpdateSummaryLabels()
        {
            var filteredData = GetFilteredData();

            if (filteredData.Any())
            {
                int totalTickets = filteredData.Sum(row => (int)row["Số vé bán"]);
                decimal totalRevenue = filteredData.Sum(row => (decimal)row["Doanh thu"]);
                
                lblTotalTickets.Text = $"Tổng số vé: {totalTickets:N0}";
                lblTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:N0} VND";
                
                // Cập nhật tiêu đề biểu đồ
                string filterInfo = $"Từ {dtpFromDate.Value:dd/MM/yyyy} đến {dtpToDate.Value:dd/MM/yyyy}";
                if (cboRegionFilter.SelectedItem?.ToString() != "Tất cả")
                {
                    filterInfo += $" - {cboRegionFilter.SelectedItem}";
                }
                
                chartRevenueColumn.Titles[0].Text = $"Doanh thu theo ngày\n{filterInfo}";
                chartRevenuePie.Titles[0].Text = $"Doanh thu theo khu vực\n{filterInfo}";
            }
            else
            {
                lblTotalTickets.Text = "Tổng số vé: 0";
                lblTotalRevenue.Text = "Tổng doanh thu: 0 VND";
                chartRevenueColumn.Titles[0].Text = "Không có dữ liệu trong khoảng thời gian này";
                chartRevenuePie.Titles[0].Text = "Không có dữ liệu trong khoảng thời gian này";
            }
        }

        private void ShowFilteredData()
        {
            // Hiển thị dữ liệu chi tiết trong DataGridView
            var filteredData = GetFilteredData();
            
            dgvRevenue.Rows.Clear();
            
            foreach (var row in filteredData)
            {
                dgvRevenue.Rows.Add(
                    row["Mã hóa đơn"],
                    row["Tên khách hàng"],
                    row["Giới tính"],
                    row["SĐT"],
                    row["Khu vực"],
                    row["Ghế ngồi"],
                    row["Số vé bán"],
                    ((decimal)row["Doanh thu"]).ToString("N0") + " VND",
                    ((DateTime)row["Ngày"]).ToString("dd/MM/yyyy")
                );
            }
            
            dgvRevenue.Visible = filteredData.Length > 0;
        }

        private void ViewRevenue_Load(object sender, EventArgs e)
        {
            this.Text = "Xem doanh thu bán vé";
            this.WindowState = FormWindowState.Maximized;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                UpdateCharts();
                ShowFilteredData();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                UpdateCharts();
                ShowFilteredData();
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Có thể thêm chức năng khi click vào biểu đồ
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Excel Files|*.xlsx|CSV Files|*.csv|Image Files (Column Chart)|*.png|Image Files (Pie Chart)|*.png";
                saveDialog.DefaultExt = "csv";
                saveDialog.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    
                    switch (extension)
                    {
                        case ".png":
                            // Hỏi người dùng muốn xuất biểu đồ nào
                            DialogResult chartChoice = MessageBox.Show("Chọn 'Yes' để xuất biểu đồ cột, 'No' để xuất biểu đồ tròn", 
                                "Chọn biểu đồ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            
                            if (chartChoice == DialogResult.Yes)
                                chartRevenueColumn.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                            else if (chartChoice == DialogResult.No)
                                chartRevenuePie.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                            else
                                return;
                            break;
                        case ".csv":
                            ExportToCSV(saveDialog.FileName);
                            break;
                        default:
                            MessageBox.Show("Định dạng file không được hỗ trợ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {saveDialog.FileName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string fileName)
        {
            var filteredData = GetFilteredData();
            
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                // Viết header
                writer.WriteLine("Mã hóa đơn,Tên khách hàng,Giới tính,SĐT,Khu vực,Ghế ngồi,Số vé bán,Doanh thu (VND),Ngày bán vé");
                
                // Viết dữ liệu
                foreach (DataRow row in filteredData)
                {
                    DateTime date = (DateTime)row["Ngày"];
                    int tickets = (int)row["Số vé bán"];
                    decimal revenue = (decimal)row["Doanh thu"];
                    string invoiceId = row["Mã hóa đơn"].ToString();
                    string customerName = row["Tên khách hàng"].ToString();
                    string gender = row["Giới tính"].ToString();
                    string phone = row["SĐT"].ToString();
                    string region = row["Khu vực"].ToString();
                    string seats = row["Ghế ngồi"].ToString();
                    
                    writer.WriteLine($"{invoiceId},{customerName},{gender},{phone},{region},{seats},{tickets},{revenue:F0},{date:dd/MM/yyyy}");
                }
                
                // Viết tổng kết
                writer.WriteLine();
                writer.WriteLine("TỔNG KẾT:");
                int totalTickets = filteredData.Sum(row => (int)row["Số vé bán"]);
                decimal totalRevenue = filteredData.Sum(row => (decimal)row["Doanh thu"]);
                writer.WriteLine($"Tổng số vé bán,{totalTickets}");
                writer.WriteLine($"Tổng doanh thu,{totalRevenue:F0}");
                writer.WriteLine($"Từ ngày,{dtpFromDate.Value:dd/MM/yyyy}");
                writer.WriteLine($"Đến ngày,{dtpToDate.Value:dd/MM/yyyy}");
                if (cboRegionFilter.SelectedItem?.ToString() != "Tất cả")
                {
                    writer.WriteLine($"Khu vực,{cboRegionFilter.SelectedItem}");
                }
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                UpdateCharts();
                ShowFilteredData();
                MessageBox.Show("Đã cập nhật dữ liệu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
