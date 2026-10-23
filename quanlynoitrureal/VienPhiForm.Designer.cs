namespace QuanLyNoiTru
{
    partial class VienPhiForm
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
            this.lblMaVP = new System.Windows.Forms.Label();
            this.txtMaVP = new System.Windows.Forms.TextBox();
            this.lblSoNgay = new System.Windows.Forms.Label();
            this.txtSoNgay = new System.Windows.Forms.TextBox();
            this.lblDonGia = new System.Windows.Forms.Label();
            this.txtDonGia = new System.Windows.Forms.TextBox();
            this.lblTrangThai = new System.Windows.Forms.Label();
            this.txtTrangThai = new System.Windows.Forms.TextBox();
            this.lblPhong = new System.Windows.Forms.Label();
            this.txtPhong = new System.Windows.Forms.TextBox();
            this.lblThuoc = new System.Windows.Forms.Label();
            this.txtThuoc = new System.Windows.Forms.TextBox();
            this.lblTong = new System.Windows.Forms.Label();
            this.txtTong = new System.Windows.Forms.TextBox();
            this.lblKham = new System.Windows.Forms.Label();
            this.nudKham = new System.Windows.Forms.NumericUpDown();
            this.lblDV = new System.Windows.Forms.Label();
            this.nudDV = new System.Windows.Forms.NumericUpDown();
            this.btnTinhTien = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.btnThanhToan = new System.Windows.Forms.Button();
            this.btnInHoaDon = new System.Windows.Forms.Button();
            this.btnNhapExcel = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudKham)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDV)).BeginInit();
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
            this.dgv.Location = new System.Drawing.Point(0, 215);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1085, 435);
            this.dgv.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblNV);
            this.pnlTop.Controls.Add(this.cmbNV);
            this.pnlTop.Controls.Add(this.lblMaVP);
            this.pnlTop.Controls.Add(this.txtMaVP);
            this.pnlTop.Controls.Add(this.lblSoNgay);
            this.pnlTop.Controls.Add(this.txtSoNgay);
            this.pnlTop.Controls.Add(this.lblDonGia);
            this.pnlTop.Controls.Add(this.txtDonGia);
            this.pnlTop.Controls.Add(this.lblTrangThai);
            this.pnlTop.Controls.Add(this.txtTrangThai);
            this.pnlTop.Controls.Add(this.lblPhong);
            this.pnlTop.Controls.Add(this.txtPhong);
            this.pnlTop.Controls.Add(this.lblThuoc);
            this.pnlTop.Controls.Add(this.txtThuoc);
            this.pnlTop.Controls.Add(this.lblTong);
            this.pnlTop.Controls.Add(this.txtTong);
            this.pnlTop.Controls.Add(this.lblKham);
            this.pnlTop.Controls.Add(this.nudKham);
            this.pnlTop.Controls.Add(this.lblDV);
            this.pnlTop.Controls.Add(this.nudDV);
            this.pnlTop.Controls.Add(this.btnTinhTien);
            this.pnlTop.Controls.Add(this.btnLuu);
            this.pnlTop.Controls.Add(this.btnThanhToan);
            this.pnlTop.Controls.Add(this.btnInHoaDon);
            this.pnlTop.Controls.Add(this.btnNhapExcel);
            this.pnlTop.Controls.Add(this.btnLamMoi);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1085, 215);
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
            // lblMaVP
            // 
            this.lblMaVP.Location = new System.Drawing.Point(650, 16);
            this.lblMaVP.Name = "lblMaVP";
            this.lblMaVP.Size = new System.Drawing.Size(120, 24);
            this.lblMaVP.TabIndex = 4;
            this.lblMaVP.Text = "Mã hóa đơn";
            // 
            // txtMaVP
            // 
            this.txtMaVP.BackColor = System.Drawing.Color.White;
            this.txtMaVP.Location = new System.Drawing.Point(775, 10);
            this.txtMaVP.Name = "txtMaVP";
            this.txtMaVP.ReadOnly = true;
            this.txtMaVP.Size = new System.Drawing.Size(230, 34);
            this.txtMaVP.TabIndex = 5;
            // 
            // lblSoNgay
            // 
            this.lblSoNgay.Location = new System.Drawing.Point(10, 51);
            this.lblSoNgay.Name = "lblSoNgay";
            this.lblSoNgay.Size = new System.Drawing.Size(120, 24);
            this.lblSoNgay.TabIndex = 6;
            this.lblSoNgay.Text = "Số ngày nằm";
            // 
            // txtSoNgay
            // 
            this.txtSoNgay.BackColor = System.Drawing.Color.White;
            this.txtSoNgay.Location = new System.Drawing.Point(135, 45);
            this.txtSoNgay.Name = "txtSoNgay";
            this.txtSoNgay.ReadOnly = true;
            this.txtSoNgay.Size = new System.Drawing.Size(170, 34);
            this.txtSoNgay.TabIndex = 7;
            // 
            // lblDonGia
            // 
            this.lblDonGia.Location = new System.Drawing.Point(330, 51);
            this.lblDonGia.Name = "lblDonGia";
            this.lblDonGia.Size = new System.Drawing.Size(120, 24);
            this.lblDonGia.TabIndex = 8;
            this.lblDonGia.Text = "Đơn giá phòng";
            // 
            // txtDonGia
            // 
            this.txtDonGia.BackColor = System.Drawing.Color.White;
            this.txtDonGia.Location = new System.Drawing.Point(455, 45);
            this.txtDonGia.Name = "txtDonGia";
            this.txtDonGia.ReadOnly = true;
            this.txtDonGia.Size = new System.Drawing.Size(170, 34);
            this.txtDonGia.TabIndex = 9;
            // 
            // lblTrangThai
            // 
            this.lblTrangThai.Location = new System.Drawing.Point(650, 51);
            this.lblTrangThai.Name = "lblTrangThai";
            this.lblTrangThai.Size = new System.Drawing.Size(120, 24);
            this.lblTrangThai.TabIndex = 10;
            this.lblTrangThai.Text = "Trạng thái";
            // 
            // txtTrangThai
            // 
            this.txtTrangThai.BackColor = System.Drawing.Color.White;
            this.txtTrangThai.Location = new System.Drawing.Point(775, 45);
            this.txtTrangThai.Name = "txtTrangThai";
            this.txtTrangThai.ReadOnly = true;
            this.txtTrangThai.Size = new System.Drawing.Size(230, 34);
            this.txtTrangThai.TabIndex = 11;
            // 
            // lblPhong
            // 
            this.lblPhong.Location = new System.Drawing.Point(10, 86);
            this.lblPhong.Name = "lblPhong";
            this.lblPhong.Size = new System.Drawing.Size(120, 24);
            this.lblPhong.TabIndex = 12;
            this.lblPhong.Text = "Tiền phòng";
            // 
            // txtPhong
            // 
            this.txtPhong.BackColor = System.Drawing.Color.White;
            this.txtPhong.Location = new System.Drawing.Point(135, 80);
            this.txtPhong.Name = "txtPhong";
            this.txtPhong.ReadOnly = true;
            this.txtPhong.Size = new System.Drawing.Size(170, 34);
            this.txtPhong.TabIndex = 13;
            // 
            // lblThuoc
            // 
            this.lblThuoc.Location = new System.Drawing.Point(330, 86);
            this.lblThuoc.Name = "lblThuoc";
            this.lblThuoc.Size = new System.Drawing.Size(120, 24);
            this.lblThuoc.TabIndex = 14;
            this.lblThuoc.Text = "Tiền thuốc";
            // 
            // txtThuoc
            // 
            this.txtThuoc.BackColor = System.Drawing.Color.White;
            this.txtThuoc.Location = new System.Drawing.Point(455, 80);
            this.txtThuoc.Name = "txtThuoc";
            this.txtThuoc.ReadOnly = true;
            this.txtThuoc.Size = new System.Drawing.Size(170, 34);
            this.txtThuoc.TabIndex = 15;
            // 
            // lblTong
            // 
            this.lblTong.Location = new System.Drawing.Point(650, 86);
            this.lblTong.Name = "lblTong";
            this.lblTong.Size = new System.Drawing.Size(120, 24);
            this.lblTong.TabIndex = 16;
            this.lblTong.Text = "TỔNG TIỀN";
            // 
            // txtTong
            // 
            this.txtTong.BackColor = System.Drawing.Color.White;
            this.txtTong.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.txtTong.ForeColor = System.Drawing.Color.Firebrick;
            this.txtTong.Location = new System.Drawing.Point(775, 80);
            this.txtTong.Name = "txtTong";
            this.txtTong.ReadOnly = true;
            this.txtTong.Size = new System.Drawing.Size(230, 37);
            this.txtTong.TabIndex = 17;
            // 
            // lblKham
            // 
            this.lblKham.Location = new System.Drawing.Point(10, 121);
            this.lblKham.Name = "lblKham";
            this.lblKham.Size = new System.Drawing.Size(120, 24);
            this.lblKham.TabIndex = 18;
            this.lblKham.Text = "Tiền khám";
            // 
            // nudKham
            // 
            this.nudKham.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudKham.Location = new System.Drawing.Point(135, 115);
            this.nudKham.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.nudKham.Name = "nudKham";
            this.nudKham.Size = new System.Drawing.Size(170, 34);
            this.nudKham.TabIndex = 19;
            this.nudKham.ThousandsSeparator = true;
            // 
            // lblDV
            // 
            this.lblDV.Location = new System.Drawing.Point(330, 121);
            this.lblDV.Name = "lblDV";
            this.lblDV.Size = new System.Drawing.Size(120, 24);
            this.lblDV.TabIndex = 20;
            this.lblDV.Text = "Tiền dịch vụ";
            // 
            // nudDV
            // 
            this.nudDV.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudDV.Location = new System.Drawing.Point(455, 115);
            this.nudDV.Maximum = new decimal(new int[] {
            2000000000,
            0,
            0,
            0});
            this.nudDV.Name = "nudDV";
            this.nudDV.Size = new System.Drawing.Size(170, 34);
            this.nudDV.TabIndex = 21;
            this.nudDV.ThousandsSeparator = true;
            // 
            // btnTinhTien
            // 
            this.btnTinhTien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnTinhTien.FlatAppearance.BorderSize = 0;
            this.btnTinhTien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTinhTien.ForeColor = System.Drawing.Color.White;
            this.btnTinhTien.Location = new System.Drawing.Point(10, 165);
            this.btnTinhTien.Name = "btnTinhTien";
            this.btnTinhTien.Size = new System.Drawing.Size(100, 32);
            this.btnTinhTien.TabIndex = 22;
            this.btnTinhTien.Text = "Tính tiền";
            this.btnTinhTien.UseVisualStyleBackColor = false;
            this.btnTinhTien.Click += new System.EventHandler(this.btnTinhTien_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnLuu.FlatAppearance.BorderSize = 0;
            this.btnLuu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(120, 165);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(120, 32);
            this.btnLuu.TabIndex = 23;
            this.btnLuu.Text = "Lưu hóa đơn";
            this.btnLuu.UseVisualStyleBackColor = false;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // btnThanhToan
            // 
            this.btnThanhToan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnThanhToan.FlatAppearance.BorderSize = 0;
            this.btnThanhToan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThanhToan.ForeColor = System.Drawing.Color.White;
            this.btnThanhToan.Location = new System.Drawing.Point(250, 165);
            this.btnThanhToan.Name = "btnThanhToan";
            this.btnThanhToan.Size = new System.Drawing.Size(110, 32);
            this.btnThanhToan.TabIndex = 24;
            this.btnThanhToan.Text = "Thanh toán";
            this.btnThanhToan.UseVisualStyleBackColor = false;
            this.btnThanhToan.Click += new System.EventHandler(this.btnThanhToan_Click);
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.BackColor = System.Drawing.Color.Green;
            this.btnInHoaDon.FlatAppearance.BorderSize = 0;
            this.btnInHoaDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInHoaDon.ForeColor = System.Drawing.Color.White;
            this.btnInHoaDon.Location = new System.Drawing.Point(370, 165);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(125, 32);
            this.btnInHoaDon.TabIndex = 25;
            this.btnInHoaDon.Text = "Xuất Excel";
            this.btnInHoaDon.UseVisualStyleBackColor = false;
            this.btnInHoaDon.Click += new System.EventHandler(this.btnInHoaDon_Click);
            // 
            // btnNhapExcel
            // 
            this.btnNhapExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnNhapExcel.FlatAppearance.BorderSize = 0;
            this.btnNhapExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNhapExcel.ForeColor = System.Drawing.Color.White;
            this.btnNhapExcel.Location = new System.Drawing.Point(505, 165);
            this.btnNhapExcel.Name = "btnNhapExcel";
            this.btnNhapExcel.Size = new System.Drawing.Size(110, 32);
            this.btnNhapExcel.TabIndex = 26;
            this.btnNhapExcel.Text = "Nhập Excel";
            this.btnNhapExcel.UseVisualStyleBackColor = false;
            this.btnNhapExcel.Click += new System.EventHandler(this.btnNhapExcel_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnLamMoi.FlatAppearance.BorderSize = 0;
            this.btnLamMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoi.ForeColor = System.Drawing.Color.White;
            this.btnLamMoi.Location = new System.Drawing.Point(625, 165);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(90, 32);
            this.btnLamMoi.TabIndex = 27;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // VienPhiForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "VienPhiForm";
            this.Text = "Viện phí";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudKham)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblNV;
        private System.Windows.Forms.ComboBox cmbNV;
        private System.Windows.Forms.Label lblMaVP;
        private System.Windows.Forms.TextBox txtMaVP;
        private System.Windows.Forms.Label lblSoNgay;
        private System.Windows.Forms.TextBox txtSoNgay;
        private System.Windows.Forms.Label lblDonGia;
        private System.Windows.Forms.TextBox txtDonGia;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.TextBox txtTrangThai;
        private System.Windows.Forms.Label lblPhong;
        private System.Windows.Forms.TextBox txtPhong;
        private System.Windows.Forms.Label lblThuoc;
        private System.Windows.Forms.TextBox txtThuoc;
        private System.Windows.Forms.Label lblTong;
        private System.Windows.Forms.TextBox txtTong;
        private System.Windows.Forms.Label lblKham;
        private System.Windows.Forms.NumericUpDown nudKham;
        private System.Windows.Forms.Label lblDV;
        private System.Windows.Forms.NumericUpDown nudDV;
        private System.Windows.Forms.Button btnTinhTien;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnThanhToan;
        private System.Windows.Forms.Button btnInHoaDon;
        private System.Windows.Forms.Button btnNhapExcel;
        private System.Windows.Forms.Button btnLamMoi;
    }
}
