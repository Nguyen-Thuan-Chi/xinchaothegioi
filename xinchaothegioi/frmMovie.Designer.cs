namespace xinchaothegioi
{
    partial class frmMovie
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
            this.components = new System.ComponentModel.Container();
            this.txtQuery = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cboMode = new System.Windows.Forms.ComboBox();
            this.flowMovies = new System.Windows.Forms.FlowLayoutPanel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnAutoRefreshToggle = new System.Windows.Forms.Button();
            this.nudSeconds = new System.Windows.Forms.NumericUpDown();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.btnSelectMovie = new System.Windows.Forms.Button();
            this.btnCancelMovie = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // txtQuery
            // 
            this.txtQuery.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtQuery.Location = new System.Drawing.Point(224, 14);
            this.txtQuery.Name = "txtQuery";
            this.txtQuery.Size = new System.Drawing.Size(100, 22);
            this.txtQuery.TabIndex = 1;
            this.txtQuery.TextChanged += new System.EventHandler(this.txtQuery_TextChanged);
            this.txtQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtQuery_KeyDown);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(364, 14);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Tìm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // cboMode
            // 
            this.cboMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMode.FormattingEnabled = true;
            this.cboMode.Location = new System.Drawing.Point(28, 12);
            this.cboMode.Name = "cboMode";
            this.cboMode.Size = new System.Drawing.Size(158, 24);
            this.cboMode.TabIndex = 3;
            this.cboMode.SelectedIndexChanged += new System.EventHandler(this.cboMode_SelectedIndexChanged);
            // 
            // flowMovies
            // 
            this.flowMovies.AutoScroll = true;
            this.flowMovies.Location = new System.Drawing.Point(12, 63);
            this.flowMovies.Name = "flowMovies";
            this.flowMovies.Size = new System.Drawing.Size(1319, 359);
            this.flowMovies.TabIndex = 4;
            this.flowMovies.Click += new System.EventHandler(this.flowMovies_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(57, 425);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(16, 16);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "...";
            // 
            // btnAutoRefreshToggle
            // 
            this.btnAutoRefreshToggle.Location = new System.Drawing.Point(476, 14);
            this.btnAutoRefreshToggle.Name = "btnAutoRefreshToggle";
            this.btnAutoRefreshToggle.Size = new System.Drawing.Size(75, 23);
            this.btnAutoRefreshToggle.TabIndex = 2;
            this.btnAutoRefreshToggle.Text = "Start Auto";
            this.btnAutoRefreshToggle.UseVisualStyleBackColor = true;
            this.btnAutoRefreshToggle.Click += new System.EventHandler(this.btnAutoRefreshToggle_Click);
            // 
            // nudSeconds
            // 
            this.nudSeconds.Location = new System.Drawing.Point(599, 15);
            this.nudSeconds.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudSeconds.Name = "nudSeconds";
            this.nudSeconds.Size = new System.Drawing.Size(120, 22);
            this.nudSeconds.TabIndex = 6;
            this.nudSeconds.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nudSeconds.ValueChanged += new System.EventHandler(this.nudSeconds_ValueChanged);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 60000;
            this.timerRefresh.Tick += new System.EventHandler(this.timerRefresh_Tick);
            // 
            // btnSelectMovie
            // 
            this.btnSelectMovie.AutoSize = true;
            this.btnSelectMovie.Location = new System.Drawing.Point(761, 15);
            this.btnSelectMovie.Name = "btnSelectMovie";
            this.btnSelectMovie.Size = new System.Drawing.Size(103, 26);
            this.btnSelectMovie.TabIndex = 7;
            this.btnSelectMovie.Text = "Chọn phim";
            this.btnSelectMovie.UseVisualStyleBackColor = true;
            this.btnSelectMovie.Click += new System.EventHandler(this.btnSelectMovie_Click);
            // 
            // btnCancelMovie
            // 
            this.btnCancelMovie.AutoSize = true;
            this.btnCancelMovie.Location = new System.Drawing.Point(888, 14);
            this.btnCancelMovie.Name = "btnCancelMovie";
            this.btnCancelMovie.Size = new System.Drawing.Size(96, 26);
            this.btnCancelMovie.TabIndex = 7;
            this.btnCancelMovie.Text = "Hủy chọn";
            this.btnCancelMovie.UseVisualStyleBackColor = true;
            this.btnCancelMovie.Click += new System.EventHandler(this.btnCancelMovie_Click);
            // 
            // frmMovie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1343, 548);
            this.Controls.Add(this.btnCancelMovie);
            this.Controls.Add(this.btnSelectMovie);
            this.Controls.Add(this.nudSeconds);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.flowMovies);
            this.Controls.Add(this.cboMode);
            this.Controls.Add(this.btnAutoRefreshToggle);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtQuery);
            this.Name = "frmMovie";
            this.Text = "frmMovie";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMovie_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtQuery;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cboMode;
        private System.Windows.Forms.FlowLayoutPanel flowMovies;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnAutoRefreshToggle;
        private System.Windows.Forms.NumericUpDown nudSeconds;
        private System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Button btnSelectMovie;
        private System.Windows.Forms.Button btnCancelMovie;
    }
}