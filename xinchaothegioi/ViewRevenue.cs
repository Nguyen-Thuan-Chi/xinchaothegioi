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
        private DataTable revenueData;
        private DataGridView sourceDataGridView;

        public ViewRevenue()
        {
            InitializeComponent();
            InitializeForm();
            InitializeControls();
            InitializeCharts();
        }

        private void InitializeForm()
        {
            this.Text = "Báo cáo doanh thu bán vé";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeControls()
        {
            // Setup region filter ComboBox
            cboRegionFilter.Items.Clear();
            cboRegionFilter.Items.Add("Tất cả");
            if (RegionManager.Regions != null)
            {
                cboRegionFilter.Items.AddRange(RegionManager.Regions.ToArray());
            }
            cboRegionFilter.SelectedIndex = 0;

            // Setup movie selection ComboBox (placeholder for future use)
            cboSelectMovie.Items.Clear();
            cboSelectMovie.Items.Add("Tất cả");
            cboSelectMovie.Items.Add("Phim hành động");
            cboSelectMovie.Items.Add("Phim tình cảm");
            cboSelectMovie.Items.Add("Phim kinh dị");
            cboSelectMovie.SelectedIndex = 0;

            // Setup DateTimePickers
            dtpFromDate.Value = DateTime.Now.AddMonths(-1);
            dtpToDate.Value = DateTime.Now;
            dtpFromDate.Format = DateTimePickerFormat.Short;
            dtpToDate.Format = DateTimePickerFormat.Short;

            // Setup DataGridView columns to match source
            SetupDataGridViewColumns();

            // Initialize summary labels
            lblTotalTickets.Text = "Tổng số vé: 0";
            lblTotalRevenue.Text = "Tổng doanh thu: 0 VND";
        }

        private void SetupDataGridViewColumns()
        {
            dgvRevenue.Columns.Clear();
            dgvRevenue.Columns.Add("colInvoiceId", "Mã hóa đơn");
            dgvRevenue.Columns.Add("colCustomerName", "Tên khách hàng");
            dgvRevenue.Columns.Add("colGender", "Giới tính");
            dgvRevenue.Columns.Add("colPhone", "SĐT");
            dgvRevenue.Columns.Add("colRegion", "Khu vực");
            dgvRevenue.Columns.Add("colSeat", "Ghế ngồi");
            dgvRevenue.Columns.Add("colTicketCount", "Số lượng vé");
            dgvRevenue.Columns.Add("colTotalAmount", "Thành tiền");
            dgvRevenue.Columns.Add("colSaleDate", "Ngày bán vé");

            // Configure DataGridView properties
            dgvRevenue.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRevenue.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRevenue.MultiSelect = false;
            dgvRevenue.AllowUserToAddRows = false;
            dgvRevenue.ReadOnly = true;
            dgvRevenue.RowHeadersVisible = false;
        }

        private void InitializeCharts()
        {
            InitializeColumnChart();
            InitializePieChart();
        }

        private void InitializeColumnChart()
        {
            chartRevenueColumn.Series.Clear();
            chartRevenueColumn.ChartAreas.Clear();
            
            // Create ChartArea
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Ngày";
            chartArea.AxisY.Title = "Doanh thu (VND)";
            chartArea.AxisX.Interval = 1;
            chartArea.AxisX.LabelStyle.Angle = -45;
            chartArea.AxisY.LabelStyle.Format = "N0";
            chartRevenueColumn.ChartAreas.Add(chartArea);
            
            // Create Series
            Series series = new Series("Doanh thu");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.SkyBlue;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";
            chartRevenueColumn.Series.Add(series);
            
            // Set title
            chartRevenueColumn.Titles.Clear();
            chartRevenueColumn.Titles.Add("Biểu đồ doanh thu theo ngày");
        }

        private void InitializePieChart()
        {
            chartRevenuePie.Series.Clear();
            chartRevenuePie.ChartAreas.Clear();
            
            // Create ChartArea
            ChartArea chartArea = new ChartArea("PieArea");
            chartRevenuePie.ChartAreas.Add(chartArea);
            
            // Create Series
            Series series = new Series("Doanh thu theo khu vực");
            series.ChartType = SeriesChartType.Pie;
            series.IsValueShownAsLabel = true;
            series.LabelFormat = "N0";
            series["PieLabelStyle"] = "Outside";
            chartRevenuePie.Series.Add(series);
            
            // Set title
            chartRevenuePie.Titles.Clear();
            chartRevenuePie.Titles.Add("Biểu đồ doanh thu theo khu vực");
        }

        public void SetRevenueData(DataGridView sourceGrid)
        {
            try
            {
                sourceDataGridView = sourceGrid;
                
                if (sourceGrid == null)
                {
                    MessageBox.Show("Không nhận được dữ liệu từ form chính!", "Lỗi", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int actualRowCount = 0;
                foreach (DataGridViewRow row in sourceGrid.Rows)
                {
                    if (!row.IsNewRow) actualRowCount++;
                }

                if (actualRowCount == 0)
                {
                    MessageBox.Show("Không có dữ liệu để hiển thị!", "Thông báo", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                LoadRevenueData();
                RefreshDisplay();
                
                MessageBox.Show($"Đã tải thành công {actualRowCount} bản ghi dữ liệu!", 
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRevenueData()
        {
            // Create DataTable to store revenue data
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

            int successCount = 0;
            int errorCount = 0;

            if (sourceDataGridView != null)
            {
                foreach (DataGridViewRow row in sourceDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;

                    try
                    {
                        // Get data from source DataGridView
                        string invoiceId = GetCellValue(row, "colInvoiceId");
                        string customerName = GetCellValue(row, "colCustomerName");
                        string gender = GetCellValue(row, "colGender");
                        string phone = GetCellValue(row, "colPhone");
                        string region = GetCellValue(row, "colRegion");
                        string seats = GetCellValue(row, "colSeat");
                        string ticketCountStr = GetCellValue(row, "colTicketCount");
                        string totalAmountStr = GetCellValue(row, "colTotalAmount");
                        string saleDateStr = GetCellValue(row, "colSaleDate");

                        // Parse data
                        if (DateTime.TryParse(saleDateStr, out DateTime saleDate) &&
                            int.TryParse(ticketCountStr, out int ticketCount) &&
                            !string.IsNullOrEmpty(totalAmountStr))
                        {
                            // Clean and parse amount (remove "VND", commas, dots)
                            string cleanAmount = totalAmountStr.Replace(" VND", "")
                                                                .Replace("VND", "")
                                                                .Replace(",", "")
                                                                .Replace(".", "")
                                                                .Trim();
                            
                            if (decimal.TryParse(cleanAmount, out decimal amount))
                            {
                                revenueData.Rows.Add(saleDate, ticketCount, amount, region, 
                                    invoiceId, customerName, gender, phone, seats);
                                successCount++;
                            }
                            else
                            {
                                errorCount++;
                            }
                        }
                        else
                        {
                            errorCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        System.Diagnostics.Debug.WriteLine($"Error processing row: {ex.Message}");
                    }
                }
            }

            if (errorCount > 0)
            {
                MessageBox.Show($"Đã xử lý {successCount} dòng thành công, {errorCount} dòng có lỗi", 
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row.Cells[columnName] != null && row.Cells[columnName].Value != null)
                {
                    return row.Cells[columnName].Value.ToString();
                }
                return "";
            }
            catch
            {
                return "";
            }
        }

        private void RefreshDisplay()
        {
            UpdateFilteredData();
            UpdateCharts();
            UpdateSummaryLabels();
        }

        private void UpdateFilteredData()
        {
            try
            {
                var filteredData = GetFilteredData();
                
                dgvRevenue.Rows.Clear();
                
                if (filteredData.Any())
                {
                    foreach (var row in filteredData)
                    {
                        dgvRevenue.Rows.Add(
                            row["Mã hóa đơn"].ToString(),
                            row["Tên khách hàng"].ToString(),
                            row["Giới tính"].ToString(),
                            row["SĐT"].ToString(),
                            row["Khu vực"].ToString(),
                            row["Ghế ngồi"].ToString(),
                            row["Số vé bán"].ToString(),
                            ((decimal)row["Doanh thu"]).ToString("N0") + " VND",
                            ((DateTime)row["Ngày"]).ToString("dd/MM/yyyy")
                        );
                    }
                }
                else
                {
                    // Show "no data" message
                    dgvRevenue.Rows.Add("Không có dữ liệu", "", "", "", "", "", "", "", "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi hiển thị dữ liệu: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateCharts()
        {
            UpdateColumnChart();
            UpdatePieChart();
        }

        private void UpdateColumnChart()
        {
            try
            {
                chartRevenueColumn.Series["Doanh thu"].Points.Clear();
                
                var filteredData = GetFilteredData();

                if (filteredData.Any())
                {
                    // Group by date and sum revenue
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
                else
                {
                    chartRevenueColumn.Series["Doanh thu"].Points.AddXY("Không có dữ liệu", 0);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating column chart: {ex.Message}");
            }
        }

        private void UpdatePieChart()
        {
            try
            {
                chartRevenuePie.Series["Doanh thu theo khu vực"].Points.Clear();
                
                var filteredData = GetFilteredData();

                if (filteredData.Any())
                {
                    // Group by region and sum revenue
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
                        point.Label = $"{item.Region}\n{item.Revenue:N0} VND";
                    }
                }
                else
                {
                    var point = chartRevenuePie.Series["Doanh thu theo khu vực"].Points.Add(1);
                    point.LegendText = "Không có dữ liệu";
                    point.Label = "Không có dữ liệu";
                    point.Color = Color.LightGray;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating pie chart: {ex.Message}");
            }
        }

        private DataRow[] GetFilteredData()
        {
            if (revenueData == null || revenueData.Rows.Count == 0)
            {
                return new DataRow[0];
            }

            var filteredData = revenueData.AsEnumerable();

            // Filter by date range
            DateTime fromDate = dtpFromDate.Value.Date;
            DateTime toDate = dtpToDate.Value.Date.AddDays(1).AddTicks(-1);
            
            filteredData = filteredData.Where(row => 
            {
                DateTime date = (DateTime)row["Ngày"];
                return date >= fromDate && date <= toDate;
            });

            // Filter by region
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
            try
            {
                var filteredData = GetFilteredData();

                if (filteredData.Any())
                {
                    int totalTickets = filteredData.Sum(row => (int)row["Số vé bán"]);
                    decimal totalRevenue = filteredData.Sum(row => (decimal)row["Doanh thu"]);
                    
                    lblTotalTickets.Text = $"Tổng số vé: {totalTickets:N0}";
                    lblTotalRevenue.Text = $"Tổng doanh thu: {totalRevenue:N0} VND";
                    
                    // Update chart titles with filter info
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating summary labels: {ex.Message}");
            }
        }

        // Event handlers
        private void ViewRevenue_Load(object sender, EventArgs e)
        {
            // Form loaded
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                RefreshDisplay();
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                RefreshDisplay();
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            // Chart click handler
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            if (revenueData != null)
            {
                RefreshDisplay();
                
                var filteredData = GetFilteredData();
                
                if (filteredData.Length > 0)
                {
                    MessageBox.Show($"Hiển thị {filteredData.Length} bản ghi doanh thu!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian đã chọn!", 
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu để hiển thị!", "Thông báo", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now.Date;
            dtpToDate.Value = DateTime.Now.Date;
            
            if (revenueData != null)
            {
                RefreshDisplay();
                
                var filteredData = GetFilteredData();
                MessageBox.Show($"Hiển thị dữ liệu hôm nay: {filteredData.Length} bản ghi", 
                    "Hôm nay", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "CSV Files|*.csv|Image Files (Column Chart)|*.png|Image Files (Pie Chart)|*.png";
                saveDialog.DefaultExt = "csv";
                saveDialog.FileName = $"BaoCaoDoanhThu_{DateTime.Now:yyyyMMdd_HHmmss}";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string extension = Path.GetExtension(saveDialog.FileName).ToLower();
                    
                    switch (extension)
                    {
                        case ".png":
                            DialogResult chartChoice = MessageBox.Show(
                                "Chọn 'Yes' để xuất biểu đồ cột, 'No' để xuất biểu đồ tròn", 
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
                            MessageBox.Show("Định dạng file không được hỗ trợ!", "Lỗi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                    }

                    MessageBox.Show($"Đã xuất báo cáo thành công!\nFile: {saveDialog.FileName}", 
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất báo cáo: {ex.Message}", "Lỗi", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExportToCSV(string fileName)
        {
            var filteredData = GetFilteredData();
            
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                // Write header
                writer.WriteLine("Mã hóa đơn,Tên khách hàng,Giới tính,SĐT,Khu vực,Ghế ngồi,Số vé bán,Doanh thu (VND),Ngày bán vé");
                
                // Write data
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
                
                // Write summary
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
    }
}
