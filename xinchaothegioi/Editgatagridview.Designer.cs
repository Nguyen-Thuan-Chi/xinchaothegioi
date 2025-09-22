namespace xinchaothegioi
{
    partial class Editgatagridview
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
            this.lblEditName = new System.Windows.Forms.Label();
            this.lblEditPhone = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboRegion = new System.Windows.Forms.ComboBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cboAddRegion = new System.Windows.Forms.Button();
            this.lblEditGender = new System.Windows.Forms.Label();
            this.rdoMale = new System.Windows.Forms.RadioButton();
            this.rdoFemale = new System.Windows.Forms.RadioButton();
            this.txtSeats = new System.Windows.Forms.TextBox();
            this.lblEditSeat = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblEditName
            // 
            this.lblEditName.AutoSize = true;
            this.lblEditName.Location = new System.Drawing.Point(89, 76);
            this.lblEditName.Name = "lblEditName";
            this.lblEditName.Size = new System.Drawing.Size(103, 16);
            this.lblEditName.TabIndex = 0;
            this.lblEditName.Text = "Tên khách hàng";
            // 
            // lblEditPhone
            // 
            this.lblEditPhone.AutoSize = true;
            this.lblEditPhone.Location = new System.Drawing.Point(107, 125);
            this.lblEditPhone.Name = "lblEditPhone";
            this.lblEditPhone.Size = new System.Drawing.Size(34, 16);
            this.lblEditPhone.TabIndex = 1;
            this.lblEditPhone.Text = "SĐT";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(328, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Khu vực";
            // 
            // cboRegion
            // 
            this.cboRegion.FormattingEnabled = true;
            this.cboRegion.Location = new System.Drawing.Point(404, 119);
            this.cboRegion.Name = "cboRegion";
            this.cboRegion.Size = new System.Drawing.Size(113, 24);
            this.cboRegion.TabIndex = 2;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(165, 119);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(132, 22);
            this.txtPhone.TabIndex = 3;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(213, 76);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(168, 22);
            this.txtName.TabIndex = 3;
            // 
            // cboAddRegion
            // 
            this.cboAddRegion.Location = new System.Drawing.Point(542, 113);
            this.cboAddRegion.Name = "cboAddRegion";
            this.cboAddRegion.Size = new System.Drawing.Size(41, 34);
            this.cboAddRegion.TabIndex = 4;
            this.cboAddRegion.Text = "+";
            this.cboAddRegion.UseVisualStyleBackColor = true;
            // 
            // lblEditGender
            // 
            this.lblEditGender.AutoSize = true;
            this.lblEditGender.Location = new System.Drawing.Point(401, 79);
            this.lblEditGender.Name = "lblEditGender";
            this.lblEditGender.Size = new System.Drawing.Size(54, 16);
            this.lblEditGender.TabIndex = 0;
            this.lblEditGender.Text = "Giới tính";
            // 
            // rdoMale
            // 
            this.rdoMale.AutoSize = true;
            this.rdoMale.Checked = true;
            this.rdoMale.Location = new System.Drawing.Point(475, 79);
            this.rdoMale.Name = "rdoMale";
            this.rdoMale.Size = new System.Drawing.Size(57, 20);
            this.rdoMale.TabIndex = 5;
            this.rdoMale.TabStop = true;
            this.rdoMale.Text = "Nam";
            this.rdoMale.UseVisualStyleBackColor = true;
            // 
            // rdoFemale
            // 
            this.rdoFemale.AutoSize = true;
            this.rdoFemale.Location = new System.Drawing.Point(538, 79);
            this.rdoFemale.Name = "rdoFemale";
            this.rdoFemale.Size = new System.Drawing.Size(45, 20);
            this.rdoFemale.TabIndex = 5;
            this.rdoFemale.Text = "Nữ";
            this.rdoFemale.UseVisualStyleBackColor = true;
            // 
            // txtSeats
            // 
            this.txtSeats.Location = new System.Drawing.Point(165, 169);
            this.txtSeats.Name = "txtSeats";
            this.txtSeats.Size = new System.Drawing.Size(100, 22);
            this.txtSeats.TabIndex = 6;
            // 
            // lblEditSeat
            // 
            this.lblEditSeat.AutoSize = true;
            this.lblEditSeat.Location = new System.Drawing.Point(89, 169);
            this.lblEditSeat.Name = "lblEditSeat";
            this.lblEditSeat.Size = new System.Drawing.Size(60, 16);
            this.lblEditSeat.TabIndex = 1;
            this.lblEditSeat.Text = "sửa Seat";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(165, 221);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "Lưu";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(380, 221);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Hủy";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // Editgatagridview
            // 
            this.ClientSize = new System.Drawing.Size(664, 343);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtSeats);
            this.Controls.Add(this.rdoFemale);
            this.Controls.Add(this.rdoMale);
            this.Controls.Add(this.cboAddRegion);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.cboRegion);
            this.Controls.Add(this.lblEditSeat);
            this.Controls.Add(this.lblEditPhone);
            this.Controls.Add(this.lblEditGender);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblEditName);
            this.Name = "Editgatagridview";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEditName;
        private System.Windows.Forms.Label lblEditPhone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboRegion;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button cboAddRegion;
        private System.Windows.Forms.Label lblEditGender;
        private System.Windows.Forms.RadioButton rdoMale;
        private System.Windows.Forms.RadioButton rdoFemale;
        private System.Windows.Forms.TextBox txtSeats;
        private System.Windows.Forms.Label lblEditSeat;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}