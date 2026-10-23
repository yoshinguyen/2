namespace QuanLyNoiTru
{
    partial class BenhAnForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>Code do Visual Studio quản lý. Sửa giao diện bằng cách mở form trong Designer (kéo thả).</summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblNV = new System.Windows.Forms.Label();
            this.cmbNV = new System.Windows.Forms.ComboBox();
            this.lblMa = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.lblTrieuChung = new System.Windows.Forms.Label();
            this.txtTrieuChung = new System.Windows.Forms.TextBox();
            this.lblChanDoan = new System.Windows.Forms.Label();
            this.txtChanDoan = new System.Windows.Forms.TextBox();
            this.lblKetQua = new System.Windows.Forms.Label();
            this.txtKetQua = new System.Windows.Forms.TextBox();
            this.lblHuong = new System.Windows.Forms.Label();
            this.txtHuong = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersHeight = 34;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 255);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1085, 395);
            this.dgv.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblNV);
            this.pnlTop.Controls.Add(this.cmbNV);
            this.pnlTop.Controls.Add(this.lblMa);
            this.pnlTop.Controls.Add(this.txtMa);
            this.pnlTop.Controls.Add(this.lblTrieuChung);
            this.pnlTop.Controls.Add(this.txtTrieuChung);
            this.pnlTop.Controls.Add(this.lblChanDoan);
            this.pnlTop.Controls.Add(this.txtChanDoan);
            this.pnlTop.Controls.Add(this.lblKetQua);
            this.pnlTop.Controls.Add(this.txtKetQua);
            this.pnlTop.Controls.Add(this.lblHuong);
            this.pnlTop.Controls.Add(this.txtHuong);
            this.pnlTop.Controls.Add(this.btnThem);
            this.pnlTop.Controls.Add(this.btnCapNhat);
            this.pnlTop.Controls.Add(this.btnXoa);
            this.pnlTop.Controls.Add(this.btnLamMoi);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1085, 255);
            this.pnlTop.TabIndex = 1;
            // 
            // lblNV
            // 
            this.lblNV.Location = new System.Drawing.Point(10, 16);
            this.lblNV.Name = "lblNV";
            this.lblNV.Size = new System.Drawing.Size(120, 24);
            this.lblNV.TabIndex = 2;
            this.lblNV.Text = "Hồ sơ nhập viện";
            // 
            // cmbNV
            // 
            this.cmbNV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNV.FormattingEnabled = true;
            this.cmbNV.Location = new System.Drawing.Point(135, 10);
            this.cmbNV.Name = "cmbNV";
            this.cmbNV.Size = new System.Drawing.Size(470, 36);
            this.cmbNV.TabIndex = 3;
            // 
            // lblMa
            // 
            this.lblMa.Location = new System.Drawing.Point(650, 16);
            this.lblMa.Name = "lblMa";
            this.lblMa.Size = new System.Drawing.Size(120, 24);
            this.lblMa.TabIndex = 4;
            this.lblMa.Text = "Mã BA";
            // 
            // txtMa
            // 
            this.txtMa.Location = new System.Drawing.Point(775, 10);
            this.txtMa.Name = "txtMa";
            this.txtMa.ReadOnly = true;
            this.txtMa.Size = new System.Drawing.Size(130, 34);
            this.txtMa.TabIndex = 5;
            // 
            // lblTrieuChung
            // 
            this.lblTrieuChung.Location = new System.Drawing.Point(10, 60);
            this.lblTrieuChung.Name = "lblTrieuChung";
            this.lblTrieuChung.Size = new System.Drawing.Size(120, 24);
            this.lblTrieuChung.TabIndex = 6;
            this.lblTrieuChung.Text = "Triệu chứng";
            // 
            // txtTrieuChung
            // 
            this.txtTrieuChung.Location = new System.Drawing.Point(135, 54);
            this.txtTrieuChung.Multiline = true;
            this.txtTrieuChung.Name = "txtTrieuChung";
            this.txtTrieuChung.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTrieuChung.Size = new System.Drawing.Size(360, 65);
            this.txtTrieuChung.TabIndex = 7;
            // 
            // lblChanDoan
            // 
            this.lblChanDoan.Location = new System.Drawing.Point(520, 60);
            this.lblChanDoan.Name = "lblChanDoan";
            this.lblChanDoan.Size = new System.Drawing.Size(110, 24);
            this.lblChanDoan.TabIndex = 8;
            this.lblChanDoan.Text = "Chẩn đoán";
            // 
            // txtChanDoan
            // 
            this.txtChanDoan.Location = new System.Drawing.Point(635, 54);
            this.txtChanDoan.Multiline = true;
            this.txtChanDoan.Name = "txtChanDoan";
            this.txtChanDoan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtChanDoan.Size = new System.Drawing.Size(370, 65);
            this.txtChanDoan.TabIndex = 9;
            // 
            // lblKetQua
            // 
            this.lblKetQua.Location = new System.Drawing.Point(10, 135);
            this.lblKetQua.Name = "lblKetQua";
            this.lblKetQua.Size = new System.Drawing.Size(120, 24);
            this.lblKetQua.TabIndex = 10;
            this.lblKetQua.Text = "Kết quả khám";
            // 
            // txtKetQua
            // 
            this.txtKetQua.Location = new System.Drawing.Point(135, 129);
            this.txtKetQua.Multiline = true;
            this.txtKetQua.Name = "txtKetQua";
            this.txtKetQua.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKetQua.Size = new System.Drawing.Size(360, 65);
            this.txtKetQua.TabIndex = 11;
            // 
            // lblHuong
            // 
            this.lblHuong.Location = new System.Drawing.Point(520, 135);
            this.lblHuong.Name = "lblHuong";
            this.lblHuong.Size = new System.Drawing.Size(110, 24);
            this.lblHuong.TabIndex = 12;
            this.lblHuong.Text = "Hướng điều trị";
            // 
            // txtHuong
            // 
            this.txtHuong.Location = new System.Drawing.Point(635, 129);
            this.txtHuong.Multiline = true;
            this.txtHuong.Name = "txtHuong";
            this.txtHuong.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHuong.Size = new System.Drawing.Size(370, 65);
            this.txtHuong.TabIndex = 13;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(10, 208);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(130, 32);
            this.btnThem.TabIndex = 14;
            this.btnThem.Text = "Thêm lần khám";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnCapNhat.FlatAppearance.BorderSize = 0;
            this.btnCapNhat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapNhat.ForeColor = System.Drawing.Color.White;
            this.btnCapNhat.Location = new System.Drawing.Point(150, 208);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(100, 32);
            this.btnCapNhat.TabIndex = 15;
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.UseVisualStyleBackColor = false;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.Red;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(260, 208);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(80, 32);
            this.btnXoa.TabIndex = 16;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = false;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(350, 208);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(90, 32);
            this.btnLamMoi.TabIndex = 17;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // BenhAnForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "BenhAnForm";
            this.Text = "Hồ sơ bệnh án";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblNV;
        private System.Windows.Forms.ComboBox cmbNV;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtMa;
        private System.Windows.Forms.Label lblTrieuChung;
        private System.Windows.Forms.TextBox txtTrieuChung;
        private System.Windows.Forms.Label lblChanDoan;
        private System.Windows.Forms.TextBox txtChanDoan;
        private System.Windows.Forms.Label lblKetQua;
        private System.Windows.Forms.TextBox txtKetQua;
        private System.Windows.Forms.Label lblHuong;
        private System.Windows.Forms.TextBox txtHuong;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
