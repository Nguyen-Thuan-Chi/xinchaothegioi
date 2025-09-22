namespace xinchaothegioi
{
    partial class frmMain1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnuSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSystem_Login = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSystem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFunctions = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFunctions_SellTickets = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFunctions_ViewRevenue = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFunctions_Reports = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button23 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSeat1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dtpSaleDate = new System.Windows.Forms.DateTimePicker();
            this.dgvInformaton = new System.Windows.Forms.DataGridView();
            this.colInvoiceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCustomerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPhone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRegion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTicketCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSaleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddRegion = new System.Windows.Forms.Button();
            this.cboSelectMovie = new System.Windows.Forms.ComboBox();
            this.cboRegion = new System.Windows.Forms.ComboBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblSaleDate = new System.Windows.Forms.Label();
            this.lblSelectMovie = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.rdoFemale = new System.Windows.Forms.RadioButton();
            this.rdoMale = new System.Windows.Forms.RadioButton();
            this.lblgender = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblname = new System.Windows.Forms.Label();
            this.lblScreen = new System.Windows.Forms.Label();
            this.lblCustomerInfo = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotal = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInformaton)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSystem,
            this.mnuFunctions});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1099, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnuSystem
            // 
            this.mnuSystem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSystem_Login,
            this.mnuSystem_Exit});
            this.mnuSystem.Name = "mnuSystem";
            this.mnuSystem.Size = new System.Drawing.Size(85, 26);
            this.mnuSystem.Text = "Hệ thống";
            this.mnuSystem.Click += new System.EventHandler(this.mnuSystem_Click);
            // 
            // mnuSystem_Login
            // 
            this.mnuSystem_Login.Name = "mnuSystem_Login";
            this.mnuSystem_Login.Size = new System.Drawing.Size(165, 26);
            this.mnuSystem_Login.Text = "Đăng nhập";
            this.mnuSystem_Login.Click += new System.EventHandler(this.đăngNhậpToolStripMenuItem_Click);
            // 
            // mnuSystem_Exit
            // 
            this.mnuSystem_Exit.Name = "mnuSystem_Exit";
            this.mnuSystem_Exit.Size = new System.Drawing.Size(165, 26);
            this.mnuSystem_Exit.Text = "Thoát";
            // 
            // mnuFunctions
            // 
            this.mnuFunctions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFunctions_SellTickets,
            this.mnuFunctions_ViewRevenue,
            this.mnuFunctions_Reports});
            this.mnuFunctions.Name = "mnuFunctions";
            this.mnuFunctions.Size = new System.Drawing.Size(93, 24);
            this.mnuFunctions.Text = "Chức năng";
            this.mnuFunctions.Visible = false;
            this.mnuFunctions.Click += new System.EventHandler(this.chứcNăngToolStripMenuItem_Click);
            // 
            // mnuFunctions_SellTickets
            // 
            this.mnuFunctions_SellTickets.Name = "mnuFunctions_SellTickets";
            this.mnuFunctions_SellTickets.Size = new System.Drawing.Size(208, 26);
            this.mnuFunctions_SellTickets.Text = "Bán vé";
            // 
            // mnuFunctions_ViewRevenue
            // 
            this.mnuFunctions_ViewRevenue.Name = "mnuFunctions_ViewRevenue";
            this.mnuFunctions_ViewRevenue.Size = new System.Drawing.Size(208, 26);
            this.mnuFunctions_ViewRevenue.Text = "xem doanh thu";
            this.mnuFunctions_ViewRevenue.Click += new System.EventHandler(this.mnuViewRevenue_Click);
            // 
            // mnuFunctions_Reports
            // 
            this.mnuFunctions_Reports.Name = "mnuFunctions_Reports";
            this.mnuFunctions_Reports.Size = new System.Drawing.Size(208, 26);
            this.mnuFunctions_Reports.Text = "báo cáo thống kê";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button23);
            this.groupBox1.Controls.Add(this.button19);
            this.groupBox1.Controls.Add(this.button15);
            this.groupBox1.Controls.Add(this.button11);
            this.groupBox1.Controls.Add(this.button7);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button22);
            this.groupBox1.Controls.Add(this.button18);
            this.groupBox1.Controls.Add(this.button14);
            this.groupBox1.Controls.Add(this.button10);
            this.groupBox1.Controls.Add(this.button6);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button21);
            this.groupBox1.Controls.Add(this.button20);
            this.groupBox1.Controls.Add(this.button17);
            this.groupBox1.Controls.Add(this.button16);
            this.groupBox1.Controls.Add(this.button13);
            this.groupBox1.Controls.Add(this.button12);
            this.groupBox1.Controls.Add(this.button9);
            this.groupBox1.Controls.Add(this.button8);
            this.groupBox1.Controls.Add(this.button5);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.btnSeat1);
            this.groupBox1.Location = new System.Drawing.Point(31, 77);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(428, 292);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "thông tin vị trí ghế ngồi";
            this.groupBox1.Visible = false;
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(316, 252);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(75, 23);
            this.button23.TabIndex = 15;
            this.button23.Text = "24";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Visible = false;
            this.button23.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(316, 210);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(75, 23);
            this.button19.TabIndex = 15;
            this.button19.Text = "20";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Visible = false;
            this.button19.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(316, 163);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 15;
            this.button15.Text = "16";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Visible = false;
            this.button15.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(316, 118);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 15;
            this.button11.Text = "12";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Visible = false;
            this.button11.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(316, 76);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 15;
            this.button7.Text = "8";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Visible = false;
            this.button7.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(316, 41);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "4";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Visible = false;
            this.button3.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(216, 252);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(75, 23);
            this.button22.TabIndex = 15;
            this.button22.Text = "23";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Visible = false;
            this.button22.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(216, 210);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(75, 23);
            this.button18.TabIndex = 15;
            this.button18.Text = "19";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Visible = false;
            this.button18.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(216, 163);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 15;
            this.button14.Text = "15";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Visible = false;
            this.button14.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(216, 118);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 15;
            this.button10.Text = "11";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Visible = false;
            this.button10.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(216, 76);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 15;
            this.button6.Text = "7";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Visible = false;
            this.button6.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(216, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "3";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(109, 252);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 15;
            this.button21.Text = "22";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Visible = false;
            this.button21.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(6, 252);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(75, 23);
            this.button20.TabIndex = 15;
            this.button20.Text = "21";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Visible = false;
            this.button20.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(109, 210);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 23);
            this.button17.TabIndex = 15;
            this.button17.Text = "18";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Visible = false;
            this.button17.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(6, 210);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 15;
            this.button16.Text = "17";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Visible = false;
            this.button16.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(109, 163);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 15;
            this.button13.Text = "14";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Visible = false;
            this.button13.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(6, 163);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 15;
            this.button12.Text = "13";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Visible = false;
            this.button12.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(109, 118);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 15;
            this.button9.Text = "10";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Visible = false;
            this.button9.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(6, 118);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = "9";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Visible = false;
            this.button8.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(109, 76);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 15;
            this.button5.Text = "6";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Visible = false;
            this.button5.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 76);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "5";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(109, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 15;
            this.button1.Text = "2";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // btnSeat1
            // 
            this.btnSeat1.Location = new System.Drawing.Point(6, 41);
            this.btnSeat1.Name = "btnSeat1";
            this.btnSeat1.Size = new System.Drawing.Size(75, 23);
            this.btnSeat1.TabIndex = 15;
            this.btnSeat1.Text = "1";
            this.btnSeat1.UseVisualStyleBackColor = true;
            this.btnSeat1.Visible = false;
            this.btnSeat1.Click += new System.EventHandler(this.btnChooseSeats_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dtpSaleDate);
            this.groupBox2.Controls.Add(this.dgvInformaton);
            this.groupBox2.Controls.Add(this.btnAddRegion);
            this.groupBox2.Controls.Add(this.cboSelectMovie);
            this.groupBox2.Controls.Add(this.cboRegion);
            this.groupBox2.Controls.Add(this.lblRegion);
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.lblSaleDate);
            this.groupBox2.Controls.Add(this.lblSelectMovie);
            this.groupBox2.Controls.Add(this.lblPhone);
            this.groupBox2.Controls.Add(this.rdoFemale);
            this.groupBox2.Controls.Add(this.rdoMale);
            this.groupBox2.Controls.Add(this.lblgender);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.lblname);
            this.groupBox2.Location = new System.Drawing.Point(554, 77);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(515, 358);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "thông tin khách hàng";
            this.groupBox2.Visible = false;
            // 
            // dtpSaleDate
            // 
            this.dtpSaleDate.Location = new System.Drawing.Point(299, 121);
            this.dtpSaleDate.Name = "dtpSaleDate";
            this.dtpSaleDate.Size = new System.Drawing.Size(200, 22);
            this.dtpSaleDate.TabIndex = 11;
            // 
            // dgvInformaton
            // 
            this.dgvInformaton.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInformaton.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInvoiceId,
            this.colCustomerName,
            this.colGender,
            this.colPhone,
            this.colRegion,
            this.colSeat,
            this.colTicketCount,
            this.colTotalAmount,
            this.colSaleDate});
            this.dgvInformaton.Location = new System.Drawing.Point(6, 163);
            this.dgvInformaton.Name = "dgvInformaton";
            this.dgvInformaton.RowHeadersWidth = 51;
            this.dgvInformaton.RowTemplate.Height = 24;
            this.dgvInformaton.Size = new System.Drawing.Size(515, 189);
            this.dgvInformaton.TabIndex = 10;
            this.dgvInformaton.Visible = false;
            this.dgvInformaton.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvInformaton_CellContentClick);
            // 
            // colInvoiceId
            // 
            this.colInvoiceId.HeaderText = "Mã hóa đơn";
            this.colInvoiceId.MinimumWidth = 6;
            this.colInvoiceId.Name = "colInvoiceId";
            this.colInvoiceId.Width = 125;
            // 
            // colCustomerName
            // 
            this.colCustomerName.HeaderText = "Tên khách hàng";
            this.colCustomerName.MinimumWidth = 6;
            this.colCustomerName.Name = "colCustomerName";
            this.colCustomerName.Width = 125;
            // 
            // colGender
            // 
            this.colGender.HeaderText = "Giới tính";
            this.colGender.MinimumWidth = 6;
            this.colGender.Name = "colGender";
            this.colGender.Width = 125;
            // 
            // colPhone
            // 
            this.colPhone.HeaderText = "SĐT";
            this.colPhone.MinimumWidth = 6;
            this.colPhone.Name = "colPhone";
            this.colPhone.Width = 125;
            // 
            // colRegion
            // 
            this.colRegion.HeaderText = "Khu vực";
            this.colRegion.MinimumWidth = 6;
            this.colRegion.Name = "colRegion";
            this.colRegion.Width = 125;
            // 
            // colSeat
            // 
            this.colSeat.HeaderText = "Ghế ngồi";
            this.colSeat.MinimumWidth = 6;
            this.colSeat.Name = "colSeat";
            this.colSeat.Width = 125;
            // 
            // colTicketCount
            // 
            this.colTicketCount.HeaderText = "Số lượng vé";
            this.colTicketCount.MinimumWidth = 6;
            this.colTicketCount.Name = "colTicketCount";
            this.colTicketCount.Width = 125;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.HeaderText = "Thành tiền";
            this.colTotalAmount.MinimumWidth = 6;
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.Width = 125;
            // 
            // colSaleDate
            // 
            this.colSaleDate.HeaderText = "Ngày bán vé";
            this.colSaleDate.MinimumWidth = 6;
            this.colSaleDate.Name = "colSaleDate";
            this.colSaleDate.Width = 125;
            // 
            // btnAddRegion
            // 
            this.btnAddRegion.Location = new System.Drawing.Point(401, 73);
            this.btnAddRegion.Name = "btnAddRegion";
            this.btnAddRegion.Size = new System.Drawing.Size(30, 23);
            this.btnAddRegion.TabIndex = 9;
            this.btnAddRegion.Text = "+";
            this.btnAddRegion.UseVisualStyleBackColor = true;
            this.btnAddRegion.Visible = false;
            this.btnAddRegion.Click += new System.EventHandler(this.btnAddRegion_Click);
            // 
            // cboSelectMovie
            // 
            this.cboSelectMovie.FormattingEnabled = true;
            this.cboSelectMovie.Location = new System.Drawing.Point(82, 117);
            this.cboSelectMovie.Name = "cboSelectMovie";
            this.cboSelectMovie.Size = new System.Drawing.Size(121, 24);
            this.cboSelectMovie.TabIndex = 8;
            this.cboSelectMovie.Visible = false;
            this.cboSelectMovie.SelectedIndexChanged += new System.EventHandler(this.cboRegion_SelectedIndexChanged);
            // 
            // cboRegion
            // 
            this.cboRegion.FormattingEnabled = true;
            this.cboRegion.Location = new System.Drawing.Point(265, 73);
            this.cboRegion.Name = "cboRegion";
            this.cboRegion.Size = new System.Drawing.Size(121, 24);
            this.cboRegion.TabIndex = 8;
            this.cboRegion.Visible = false;
            this.cboRegion.SelectedIndexChanged += new System.EventHandler(this.cboRegion_SelectedIndexChanged);
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(206, 76);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(53, 16);
            this.lblRegion.TabIndex = 7;
            this.lblRegion.Text = "Khu vực";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(73, 73);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(112, 22);
            this.txtPhone.TabIndex = 6;
            this.txtPhone.Visible = false;
            // 
            // lblSaleDate
            // 
            this.lblSaleDate.AutoSize = true;
            this.lblSaleDate.Location = new System.Drawing.Point(209, 125);
            this.lblSaleDate.Name = "lblSaleDate";
            this.lblSaleDate.Size = new System.Drawing.Size(84, 16);
            this.lblSaleDate.TabIndex = 5;
            this.lblSaleDate.Text = "Ngày bán vé";
            // 
            // lblSelectMovie
            // 
            this.lblSelectMovie.AutoSize = true;
            this.lblSelectMovie.Location = new System.Drawing.Point(6, 121);
            this.lblSelectMovie.Name = "lblSelectMovie";
            this.lblSelectMovie.Size = new System.Drawing.Size(70, 16);
            this.lblSelectMovie.TabIndex = 5;
            this.lblSelectMovie.Text = "Chọn phim";
            this.lblSelectMovie.Click += new System.EventHandler(this.lblSelectMovie_Click);
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Location = new System.Drawing.Point(18, 76);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(34, 16);
            this.lblPhone.TabIndex = 5;
            this.lblPhone.Text = "SĐT";
            // 
            // rdoFemale
            // 
            this.rdoFemale.AutoSize = true;
            this.rdoFemale.Location = new System.Drawing.Point(384, 38);
            this.rdoFemale.Name = "rdoFemale";
            this.rdoFemale.Size = new System.Drawing.Size(42, 20);
            this.rdoFemale.TabIndex = 4;
            this.rdoFemale.Text = "nữ";
            this.rdoFemale.UseVisualStyleBackColor = true;
            this.rdoFemale.Visible = false;
            // 
            // rdoMale
            // 
            this.rdoMale.AutoSize = true;
            this.rdoMale.Checked = true;
            this.rdoMale.Location = new System.Drawing.Point(321, 37);
            this.rdoMale.Name = "rdoMale";
            this.rdoMale.Size = new System.Drawing.Size(57, 20);
            this.rdoMale.TabIndex = 3;
            this.rdoMale.TabStop = true;
            this.rdoMale.Text = "Nam";
            this.rdoMale.UseVisualStyleBackColor = true;
            this.rdoMale.Visible = false;
            // 
            // lblgender
            // 
            this.lblgender.AutoSize = true;
            this.lblgender.Location = new System.Drawing.Point(254, 39);
            this.lblgender.Name = "lblgender";
            this.lblgender.Size = new System.Drawing.Size(52, 16);
            this.lblgender.TabIndex = 2;
            this.lblgender.Text = "giới tính";
            this.lblgender.Visible = false;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(123, 35);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(112, 22);
            this.txtName.TabIndex = 1;
            this.txtName.Visible = false;
            // 
            // lblname
            // 
            this.lblname.AutoSize = true;
            this.lblname.Location = new System.Drawing.Point(6, 41);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(97, 16);
            this.lblname.TabIndex = 0;
            this.lblname.Text = "tên khách hàng";
            this.lblname.Visible = false;
            // 
            // lblScreen
            // 
            this.lblScreen.AutoSize = true;
            this.lblScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreen.Location = new System.Drawing.Point(162, 45);
            this.lblScreen.Name = "lblScreen";
            this.lblScreen.Size = new System.Drawing.Size(122, 29);
            this.lblScreen.TabIndex = 3;
            this.lblScreen.Text = "MÀN ẢNH";
            this.lblScreen.Visible = false;
            // 
            // lblCustomerInfo
            // 
            this.lblCustomerInfo.AutoSize = true;
            this.lblCustomerInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerInfo.Location = new System.Drawing.Point(672, 45);
            this.lblCustomerInfo.Name = "lblCustomerInfo";
            this.lblCustomerInfo.Size = new System.Drawing.Size(309, 29);
            this.lblCustomerInfo.TabIndex = 4;
            this.lblCustomerInfo.Text = "THÔNG TIN KHÁCH HÀNG";
            this.lblCustomerInfo.Visible = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(259, 388);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(63, 16);
            this.lblTotal.TabIndex = 11;
            this.lblTotal.Text = "Tổng tiền";
            this.lblTotal.Visible = false;
            // 
            // txtTotal
            // 
            this.txtTotal.Location = new System.Drawing.Point(328, 382);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Size = new System.Drawing.Size(131, 22);
            this.txtTotal.TabIndex = 11;
            this.txtTotal.Visible = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(31, 414);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(112, 23);
            this.btnSelect.TabIndex = 12;
            this.btnSelect.Text = "Chọn";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Visible = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(184, 414);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(112, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(347, 414);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(112, 23);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Thoát";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Visible = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(37, 381);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 15;
            this.btnEdit.Text = "Sửa";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Visible = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(140, 381);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 16;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // frmMain1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 447);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblCustomerInfo);
            this.Controls.Add(this.lblScreen);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "frmMain1";
            this.Text = "Quản lý kkk";
            this.Load += new System.EventHandler(this.frmMain1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInformaton)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem_Login;
        private System.Windows.Forms.ToolStripMenuItem mnuSystem_Exit;
        private System.Windows.Forms.ToolStripMenuItem mnuFunctions;
        private System.Windows.Forms.ToolStripMenuItem mnuFunctions_SellTickets;
        private System.Windows.Forms.ToolStripMenuItem mnuFunctions_ViewRevenue;
        private System.Windows.Forms.ToolStripMenuItem mnuFunctions_Reports;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblScreen;
        private System.Windows.Forms.Label lblCustomerInfo;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblname;
        private System.Windows.Forms.Label lblgender;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.RadioButton rdoFemale;
        private System.Windows.Forms.RadioButton rdoMale;
        private System.Windows.Forms.Button btnAddRegion;
        private System.Windows.Forms.ComboBox cboRegion;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.DataGridView dgvInformaton;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.TextBox txtTotal;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnSeat1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Button button20;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Button button22;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInvoiceId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCustomerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGender;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPhone;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRegion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeat;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTicketCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSaleDate;
        private System.Windows.Forms.Label lblSelectMovie;
        private System.Windows.Forms.ComboBox cboSelectMovie;
        private System.Windows.Forms.DateTimePicker dtpSaleDate;
        private System.Windows.Forms.Label lblSaleDate;
    }
}

