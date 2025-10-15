namespace xinchaothegioi
{
    partial class ViewRevenue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.cboRegionFilter = new System.Windows.Forms.ComboBox();
            this.btnView = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnToday = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.lblSelectMovie = new System.Windows.Forms.Label();
            this.cboSelectMovie = new System.Windows.Forms.ComboBox();
            this.lblTotalTickets = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.chartRevenuePie = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartRevenueColumn = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.colSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTicketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInvoiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvRevenue = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenuePie)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueColumn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenue)).BeginInit();
            this.SuspendLayout();
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(89, 32);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(235, 22);
            this.dtpFromDate.TabIndex = 2;
            this.dtpFromDate.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // cboRegionFilter
            // 
            this.cboRegionFilter.FormattingEnabled = true;
            this.cboRegionFilter.Location = new System.Drawing.Point(104, 89);
            this.cboRegionFilter.Name = "cboRegionFilter";
            this.cboRegionFilter.Size = new System.Drawing.Size(121, 24);
            this.cboRegionFilter.TabIndex = 3;
            this.cboRegionFilter.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btnView
            // 
            this.btnView.AutoSize = true;
            this.btnView.Location = new System.Drawing.Point(266, 87);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(113, 26);
            this.btnView.TabIndex = 4;
            this.btnView.Text = "Xem kết quả";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = true;
            this.btnExport.Location = new System.Drawing.Point(403, 87);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(113, 26);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "xuất báo cáo";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnToday
            // 
            this.btnToday.AutoSize = true;
            this.btnToday.Location = new System.Drawing.Point(754, 87);
            this.btnToday.Name = "btnToday";
            this.btnToday.Size = new System.Drawing.Size(113, 26);
            this.btnToday.TabIndex = 4;
            this.btnToday.Text = "Hôm nay";
            this.btnToday.UseVisualStyleBackColor = true;
            this.btnToday.Click += new System.EventHandler(this.btnToday_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Chọn khu vực";
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(12, 34);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(59, 16);
            this.lblFromDate.TabIndex = 5;
            this.lblFromDate.Text = "Từ ngày:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(431, 32);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(235, 22);
            this.dtpToDate.TabIndex = 2;
            this.dtpToDate.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(348, 34);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(67, 16);
            this.lblToDate.TabIndex = 5;
            this.lblToDate.Text = "Đến ngày:";
            // 
            // lblSelectMovie
            // 
            this.lblSelectMovie.AutoSize = true;
            this.lblSelectMovie.Location = new System.Drawing.Point(531, 92);
            this.lblSelectMovie.Name = "lblSelectMovie";
            this.lblSelectMovie.Size = new System.Drawing.Size(70, 16);
            this.lblSelectMovie.TabIndex = 5;
            this.lblSelectMovie.Text = "Chọn phim";
            // 
            // cboSelectMovie
            // 
            this.cboSelectMovie.FormattingEnabled = true;
            this.cboSelectMovie.Location = new System.Drawing.Point(617, 89);
            this.cboSelectMovie.Name = "cboSelectMovie";
            this.cboSelectMovie.Size = new System.Drawing.Size(121, 24);
            this.cboSelectMovie.TabIndex = 3;
            this.cboSelectMovie.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblTotalTickets
            // 
            this.lblTotalTickets.AutoSize = true;
            this.lblTotalTickets.Location = new System.Drawing.Point(546, 148);
            this.lblTotalTickets.Name = "lblTotalTickets";
            this.lblTotalTickets.Size = new System.Drawing.Size(120, 16);
            this.lblTotalTickets.TabIndex = 5;
            this.lblTotalTickets.Text = "Tổng số vé đã bán";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Location = new System.Drawing.Point(778, 139);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(100, 16);
            this.lblTotalRevenue.TabIndex = 5;
            this.lblTotalRevenue.Text = "Tổng doanh thu";
            // 
            // chartRevenuePie
            // 
            chartArea1.Name = "ChartArea1";
            this.chartRevenuePie.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartRevenuePie.Legends.Add(legend1);
            this.chartRevenuePie.Location = new System.Drawing.Point(947, 21);
            this.chartRevenuePie.Name = "chartRevenuePie";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartRevenuePie.Series.Add(series1);
            this.chartRevenuePie.Size = new System.Drawing.Size(233, 134);
            this.chartRevenuePie.TabIndex = 7;
            this.chartRevenuePie.Text = "chart1";
            this.chartRevenuePie.Click += new System.EventHandler(this.chart1_Click);
            // 
            // chartRevenueColumn
            // 
            chartArea2.Name = "ChartArea1";
            this.chartRevenueColumn.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartRevenueColumn.Legends.Add(legend2);
            this.chartRevenueColumn.Location = new System.Drawing.Point(522, 181);
            this.chartRevenueColumn.Name = "chartRevenueColumn";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartRevenueColumn.Series.Add(series2);
            this.chartRevenueColumn.Size = new System.Drawing.Size(427, 283);
            this.chartRevenueColumn.TabIndex = 1;
            this.chartRevenueColumn.Text = "chart1";
            // 
            // colSaleDate
            // 
            this.colSaleDate.HeaderText = "Ngày bán vé";
            this.colSaleDate.MinimumWidth = 6;
            this.colSaleDate.Name = "colSaleDate";
            this.colSaleDate.Width = 125;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.HeaderText = "Thành tiền";
            this.colTotalAmount.MinimumWidth = 6;
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.Width = 125;
            // 
            // colTicketCount
            // 
            this.colTicketCount.HeaderText = "Số lượng vé";
            this.colTicketCount.MinimumWidth = 6;
            this.colTicketCount.Name = "colTicketCount";
            this.colTicketCount.Width = 125;
            // 
            // colSeat
            // 
            this.colSeat.HeaderText = "Ghế ngồi";
            this.colSeat.MinimumWidth = 6;
            this.colSeat.Name = "colSeat";
            this.colSeat.Width = 125;
            // 
            // colRegion
            // 
            this.colRegion.HeaderText = "Khu vực";
            this.colRegion.MinimumWidth = 6;
            this.colRegion.Name = "colRegion";
            this.colRegion.Width = 125;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "Sđt";
            this.colPhone.MinimumWidth = 6;
            this.colPhone.Name = "colPhone";
            this.colPhone.Width = 125;
            // 
            // colGender
            // 
            this.colGender.HeaderText = "Giới tính";
            this.colGender.MinimumWidth = 6;
            this.colGender.Name = "colGender";
            this.colGender.Width = 125;
            // 
            // colCustomerName
            // 
            this.colCustomerName.HeaderText = "Tên khách hàng";
            this.colCustomerName.MinimumWidth = 6;
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.Width = 125;
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.HeaderText = "Mã hóa đơn";
            this.colInvoiceId.MinimumWidth = 6;
            this.colInvoiceId.Name = "colInvoiceId";
            this.colInvoiceId.Width = 125;
            // 
            // dgvRevenue
            // 
            this.dgvRevenue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRevenue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceId,
            this.colCustomerName,
            this.colGender,
            this.colPhone,
            this.colRegion,
            this.colSeat,
            this.colTicketCount,
            this.colTotalAmount,
            this.colSaleDate});
            this.dgvRevenue.Location = new System.Drawing.Point(12, 165);
            this.dgvRevenue.Name = "dgvRevenue";
            this.dgvRevenue.RowHeadersWidth = 51;
            this.dgvRevenue.RowTemplate.Height = 24;
            this.dgvRevenue.Size = new System.Drawing.Size(504, 323);
            this.dgvRevenue.TabIndex = 0;
            // 
            // ViewRevenue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1240, 500);
            this.Controls.Add(this.chartRevenuePie);
            this.Controls.Add(this.lblToDate);
            this.Controls.Add(this.lblFromDate);
            this.Controls.Add(this.lblSelectMovie);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblTotalTickets);
            this.Controls.Add(this.lblTotalRevenue);
            this.Controls.Add(this.btnToday);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.cboSelectMovie);
            this.Controls.Add(this.cboRegionFilter);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.chartRevenueColumn);
            this.Controls.Add(this.dgvRevenue);
            this.Name = "ViewRevenue";
            this.Text = "ViewRevenue";
            this.Load += new System.EventHandler(this.ViewRevenue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenuePie)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRevenueColumn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRevenue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.ComboBox cboRegionFilter;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnToday;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.Label lblSelectMovie;
        private System.Windows.Forms.ComboBox cboSelectMovie;
        private System.Windows.Forms.Label lblTotalTickets;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenuePie;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRevenueColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTicketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridView dgvRevenue;
    }
}