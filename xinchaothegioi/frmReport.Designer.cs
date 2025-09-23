namespace xinchaothegioi
{
    partial class frmReport
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.tcReports = new System.Windows.Forms.TabControl();
            this.tcsynthetic = new System.Windows.Forms.TabPage();
            this.tpTop = new System.Windows.Forms.TabPage();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.lblTotalTickets = new System.Windows.Forms.Label();
            this.lblTotalRevenue = new System.Windows.Forms.Label();
            this.lblTotalCustomers = new System.Windows.Forms.Label();
            this.chartGenderRatio = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartSeatOccupancy = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tpCompare = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dgvTopCustomers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTopRegions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvTopDays = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chartRegionCompare = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartDayCompare = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tpExportExcel = new System.Windows.Forms.TabPage();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.tcReports.SuspendLayout();
            this.tcsynthetic.SuspendLayout();
            this.tpTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartGenderRatio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSeatOccupancy)).BeginInit();
            this.tpCompare.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRegionCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDayCompare)).BeginInit();
            this.tpExportExcel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Từ ngày";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(78, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 22);
            this.dateTimePicker1.TabIndex = 1;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(356, 17);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(199, 22);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Đến ngày";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // tcReports
            // 
            this.tcReports.Controls.Add(this.tcsynthetic);
            this.tcReports.Controls.Add(this.tpTop);
            this.tcReports.Controls.Add(this.tpCompare);
            this.tcReports.Controls.Add(this.tpExportExcel);
            this.tcReports.Location = new System.Drawing.Point(19, 50);
            this.tcReports.Name = "tcReports";
            this.tcReports.SelectedIndex = 0;
            this.tcReports.Size = new System.Drawing.Size(1063, 475);
            this.tcReports.TabIndex = 3;
            // 
            // tcsynthetic
            // 
            this.tcsynthetic.Controls.Add(this.chartSeatOccupancy);
            this.tcsynthetic.Controls.Add(this.chartGenderRatio);
            this.tcsynthetic.Controls.Add(this.lblTotalCustomers);
            this.tcsynthetic.Controls.Add(this.lblTotalRevenue);
            this.tcsynthetic.Controls.Add(this.lblTotalTickets);
            this.tcsynthetic.Location = new System.Drawing.Point(4, 25);
            this.tcsynthetic.Name = "tcsynthetic";
            this.tcsynthetic.Padding = new System.Windows.Forms.Padding(3);
            this.tcsynthetic.Size = new System.Drawing.Size(1055, 446);
            this.tcsynthetic.TabIndex = 0;
            this.tcsynthetic.Text = "Tổng hợp";
            this.tcsynthetic.UseVisualStyleBackColor = true;
            this.tcsynthetic.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // tpTop
            // 
            this.tpTop.Controls.Add(this.dataGridView1);
            this.tpTop.Location = new System.Drawing.Point(4, 25);
            this.tpTop.Name = "tpTop";
            this.tpTop.Padding = new System.Windows.Forms.Padding(3);
            this.tpTop.Size = new System.Drawing.Size(1055, 446);
            this.tpTop.TabIndex = 1;
            this.tpTop.Text = "Top";
            this.tpTop.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(620, 14);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(561, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Khu vực";
            this.label3.Click += new System.EventHandler(this.label2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(768, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Lọc";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lblTotalTickets
            // 
            this.lblTotalTickets.AutoSize = true;
            this.lblTotalTickets.Location = new System.Drawing.Point(40, 30);
            this.lblTotalTickets.Name = "lblTotalTickets";
            this.lblTotalTickets.Size = new System.Drawing.Size(78, 16);
            this.lblTotalTickets.TabIndex = 0;
            this.lblTotalTickets.Text = "Tổng số vé:";
            // 
            // lblTotalRevenue
            // 
            this.lblTotalRevenue.AutoSize = true;
            this.lblTotalRevenue.Location = new System.Drawing.Point(400, 30);
            this.lblTotalRevenue.Name = "lblTotalRevenue";
            this.lblTotalRevenue.Size = new System.Drawing.Size(103, 16);
            this.lblTotalRevenue.TabIndex = 0;
            this.lblTotalRevenue.Text = "Tổng doanh thu:";
            // 
            // lblTotalCustomers
            // 
            this.lblTotalCustomers.AutoSize = true;
            this.lblTotalCustomers.Location = new System.Drawing.Point(742, 30);
            this.lblTotalCustomers.Name = "lblTotalCustomers";
            this.lblTotalCustomers.Size = new System.Drawing.Size(132, 16);
            this.lblTotalCustomers.TabIndex = 0;
            this.lblTotalCustomers.Text = "Tổng số khách hàng:";
            // 
            // chartGenderRatio
            // 
            chartArea2.Name = "ChartArea1";
            this.chartGenderRatio.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chartGenderRatio.Legends.Add(legend2);
            this.chartGenderRatio.Location = new System.Drawing.Point(25, 90);
            this.chartGenderRatio.Name = "chartGenderRatio";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartGenderRatio.Series.Add(series2);
            this.chartGenderRatio.Size = new System.Drawing.Size(478, 271);
            this.chartGenderRatio.TabIndex = 1;
            this.chartGenderRatio.Text = "chart1";
            this.chartGenderRatio.Click += new System.EventHandler(this.chart1_Click);
            // 
            // chartSeatOccupancy
            // 
            chartArea1.Name = "ChartArea1";
            this.chartSeatOccupancy.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartSeatOccupancy.Legends.Add(legend1);
            this.chartSeatOccupancy.Location = new System.Drawing.Point(597, 90);
            this.chartSeatOccupancy.Name = "chartSeatOccupancy";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartSeatOccupancy.Series.Add(series1);
            this.chartSeatOccupancy.Size = new System.Drawing.Size(478, 271);
            this.chartSeatOccupancy.TabIndex = 1;
            this.chartSeatOccupancy.Text = "chart1";
            // 
            // tpCompare
            // 
            this.tpCompare.Controls.Add(this.chartDayCompare);
            this.tpCompare.Controls.Add(this.chartRegionCompare);
            this.tpCompare.Location = new System.Drawing.Point(4, 25);
            this.tpCompare.Name = "tpCompare";
            this.tpCompare.Size = new System.Drawing.Size(1055, 446);
            this.tpCompare.TabIndex = 2;
            this.tpCompare.Text = "So sánh";
            this.tpCompare.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvTopCustomers,
            this.dgvTopRegions,
            this.dgvTopDays});
            this.dataGridView1.Location = new System.Drawing.Point(6, 16);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1032, 427);
            this.dataGridView1.TabIndex = 0;
            // 
            // dgvTopCustomers
            // 
            this.dgvTopCustomers.HeaderText = "top khách hàng";
            this.dgvTopCustomers.MinimumWidth = 6;
            this.dgvTopCustomers.Name = "dgvTopCustomers";
            this.dgvTopCustomers.Width = 125;
            // 
            // dgvTopRegions
            // 
            this.dgvTopRegions.HeaderText = "Top khu vực";
            this.dgvTopRegions.MinimumWidth = 6;
            this.dgvTopRegions.Name = "dgvTopRegions";
            this.dgvTopRegions.Width = 125;
            // 
            // dgvTopDays
            // 
            this.dgvTopDays.HeaderText = "Top ngày bán chạy";
            this.dgvTopDays.MinimumWidth = 6;
            this.dgvTopDays.Name = "dgvTopDays";
            this.dgvTopDays.Width = 125;
            // 
            // chartRegionCompare
            // 
            chartArea4.Name = "ChartArea1";
            this.chartRegionCompare.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartRegionCompare.Legends.Add(legend4);
            this.chartRegionCompare.Location = new System.Drawing.Point(33, 62);
            this.chartRegionCompare.Name = "chartRegionCompare";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartRegionCompare.Series.Add(series4);
            this.chartRegionCompare.Size = new System.Drawing.Size(382, 305);
            this.chartRegionCompare.TabIndex = 0;
            this.chartRegionCompare.Text = "chart1";
            this.chartRegionCompare.Click += new System.EventHandler(this.chartRegionCompare_Click);
            // 
            // chartDayCompare
            // 
            chartArea3.Name = "ChartArea1";
            this.chartDayCompare.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartDayCompare.Legends.Add(legend3);
            this.chartDayCompare.Location = new System.Drawing.Point(622, 62);
            this.chartDayCompare.Name = "chartDayCompare";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartDayCompare.Series.Add(series3);
            this.chartDayCompare.Size = new System.Drawing.Size(382, 305);
            this.chartDayCompare.TabIndex = 0;
            this.chartDayCompare.Text = "chart1";
            // 
            // tpExportExcel
            // 
            this.tpExportExcel.Controls.Add(this.btnExportPdf);
            this.tpExportExcel.Controls.Add(this.btnExportExcel);
            this.tpExportExcel.Location = new System.Drawing.Point(4, 25);
            this.tpExportExcel.Name = "tpExportExcel";
            this.tpExportExcel.Size = new System.Drawing.Size(1055, 446);
            this.tpExportExcel.TabIndex = 3;
            this.tpExportExcel.Text = "Xuất báo cáo";
            this.tpExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.AutoSize = true;
            this.btnExportExcel.Location = new System.Drawing.Point(45, 28);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(79, 26);
            this.btnExportExcel.TabIndex = 0;
            this.btnExportExcel.Text = "Xuất Excel";
            this.btnExportExcel.UseVisualStyleBackColor = true;
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Location = new System.Drawing.Point(158, 28);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(75, 23);
            this.btnExportPdf.TabIndex = 0;
            this.btnExportPdf.Text = "Xuất PDF";
            this.btnExportPdf.UseVisualStyleBackColor = true;
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 537);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.tcReports);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "frmReport";
            this.Text = "frmReport";
            this.tcReports.ResumeLayout(false);
            this.tcsynthetic.ResumeLayout(false);
            this.tcsynthetic.PerformLayout();
            this.tpTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartGenderRatio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartSeatOccupancy)).EndInit();
            this.tpCompare.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartRegionCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDayCompare)).EndInit();
            this.tpExportExcel.ResumeLayout(false);
            this.tpExportExcel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tcReports;
        private System.Windows.Forms.TabPage tcsynthetic;
        private System.Windows.Forms.TabPage tpTop;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTotalRevenue;
        private System.Windows.Forms.Label lblTotalTickets;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartSeatOccupancy;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGenderRatio;
        private System.Windows.Forms.Label lblTotalCustomers;
        private System.Windows.Forms.TabPage tpCompare;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTopCustomers;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTopRegions;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvTopDays;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartDayCompare;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartRegionCompare;
        private System.Windows.Forms.TabPage tpExportExcel;
        private System.Windows.Forms.Button btnExportPdf;
        private System.Windows.Forms.Button btnExportExcel;
    }
}