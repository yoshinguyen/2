namespace QuanLyNoiTru
{
    partial class BenhNhanForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblMa = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.lblTen = new System.Windows.Forms.Label();
            this.txtTen = new System.Windows.Forms.TextBox();
            this.lblNs = new System.Windows.Forms.Label();
            this.dtpNs = new System.Windows.Forms.DateTimePicker();
            this.lblGt = new System.Windows.Forms.Label();
            this.cmbGt = new System.Windows.Forms.ComboBox();
            this.lblCccd = new System.Windows.Forms.Label();
            this.txtCccd = new System.Windows.Forms.TextBox();
            this.lblSdt = new System.Windows.Forms.Label();
            this.txtSdt = new System.Windows.Forms.TextBox();
            this.lblNghe = new System.Windows.Forms.Label();
            this.txtNghe = new System.Windows.Forms.TextBox();
            this.lblDiaChi = new System.Windows.Forms.Label();
            this.txtDiaChi = new System.Windows.Forms.TextBox();
            this.lblNguoiThan = new System.Windows.Forms.Label();
            this.txtNguoiThan = new System.Windows.Forms.TextBox();
            this.lblSdtNT = new System.Windows.Forms.Label();
            this.txtSdtNT = new System.Windows.Forms.TextBox();
            this.lblTinhTrang = new System.Windows.Forms.Label();
            this.txtTinhTrang = new System.Windows.Forms.TextBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.lblTieuDeTim = new System.Windows.Forms.Label();
            this.lblTimMa = new System.Windows.Forms.Label();
            this.txtTimMa = new System.Windows.Forms.TextBox();
            this.lblTimTen = new System.Windows.Forms.Label();
            this.txtTimTen = new System.Windows.Forms.TextBox();
            this.lblTimGt = new System.Windows.Forms.Label();
            this.cmbTimGt = new System.Windows.Forms.ComboBox();
            this.lblTimSdt = new System.Windows.Forms.Label();
            this.txtTimSdt = new System.Windows.Forms.TextBox();
            this.btnTimKiem = new System.Windows.Forms.Button();
            this.btnLamMoiTim = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv
            // 
            this.dgv.AllowUserToAddRows = false;
            this.dgv.AllowUserToDeleteRows = false;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(247)))), ((int)(((byte)(252)))));
            this.dgv.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgv.BackgroundColor = System.Drawing.Color.White;
            this.dgv.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv.ColumnHeadersHeight = 34;
            this.dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv.Location = new System.Drawing.Point(0, 285);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1085, 365);
            this.dgv.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblMa);
            this.pnlTop.Controls.Add(this.txtMa);
            this.pnlTop.Controls.Add(this.lblTen);
            this.pnlTop.Controls.Add(this.txtTen);
            this.pnlTop.Controls.Add(this.lblNs);
            this.pnlTop.Controls.Add(this.dtpNs);
            this.pnlTop.Controls.Add(this.lblGt);
            this.pnlTop.Controls.Add(this.cmbGt);
            this.pnlTop.Controls.Add(this.lblCccd);
            this.pnlTop.Controls.Add(this.txtCccd);
            this.pnlTop.Controls.Add(this.lblSdt);
            this.pnlTop.Controls.Add(this.txtSdt);
            this.pnlTop.Controls.Add(this.lblNghe);
            this.pnlTop.Controls.Add(this.txtNghe);
            this.pnlTop.Controls.Add(this.lblDiaChi);
            this.pnlTop.Controls.Add(this.txtDiaChi);
            this.pnlTop.Controls.Add(this.lblNguoiThan);
            this.pnlTop.Controls.Add(this.txtNguoiThan);
            this.pnlTop.Controls.Add(this.lblSdtNT);
            this.pnlTop.Controls.Add(this.txtSdtNT);
            this.pnlTop.Controls.Add(this.lblTinhTrang);
            this.pnlTop.Controls.Add(this.txtTinhTrang);
            this.pnlTop.Controls.Add(this.btnThem);
            this.pnlTop.Controls.Add(this.btnSua);
            this.pnlTop.Controls.Add(this.btnXoa);
            this.pnlTop.Controls.Add(this.btnLamMoi);
            this.pnlTop.Controls.Add(this.lblTieuDeTim);
            this.pnlTop.Controls.Add(this.lblTimMa);
            this.pnlTop.Controls.Add(this.txtTimMa);
            this.pnlTop.Controls.Add(this.lblTimTen);
            this.pnlTop.Controls.Add(this.txtTimTen);
            this.pnlTop.Controls.Add(this.lblTimGt);
            this.pnlTop.Controls.Add(this.cmbTimGt);
            this.pnlTop.Controls.Add(this.lblTimSdt);
            this.pnlTop.Controls.Add(this.txtTimSdt);
            this.pnlTop.Controls.Add(this.btnTimKiem);
            this.pnlTop.Controls.Add(this.btnLamMoiTim);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1085, 285);
            this.pnlTop.TabIndex = 1;
            // 
            // lblMa
            // 
            this.lblMa.Location = new System.Drawing.Point(10, 16);
            this.lblMa.Name = "lblMa";
            this.lblMa.Size = new System.Drawing.Size(120, 24);
            this.lblMa.TabIndex = 2;
            this.lblMa.Text = "Mã BN";
            // 
            // txtMa
            // 
            this.txtMa.Location = new System.Drawing.Point(135, 10);
            this.txtMa.Name = "txtMa";
            this.txtMa.ReadOnly = true;
            this.txtMa.Size = new System.Drawing.Size(170, 34);
            this.txtMa.TabIndex = 3;
            // 
            // lblTen
            // 
            this.lblTen.Location = new System.Drawing.Point(10, 51);
            this.lblTen.Name = "lblTen";
            this.lblTen.Size = new System.Drawing.Size(120, 24);
            this.lblTen.TabIndex = 4;
            this.lblTen.Text = "Họ tên";
            // 
            // txtTen
            // 
            this.txtTen.Location = new System.Drawing.Point(135, 45);
            this.txtTen.Name = "txtTen";
            this.txtTen.Size = new System.Drawing.Size(170, 34);
            this.txtTen.TabIndex = 5;
            // 
            // lblNs
            // 
            this.lblNs.Location = new System.Drawing.Point(10, 86);
            this.lblNs.Name = "lblNs";
            this.lblNs.Size = new System.Drawing.Size(120, 24);
            this.lblNs.TabIndex = 6;
            this.lblNs.Text = "Ngày sinh";
            // 
            // dtpNs
            // 
            this.dtpNs.CustomFormat = "dd/MM/yyyy";
            this.dtpNs.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNs.Location = new System.Drawing.Point(135, 80);
            this.dtpNs.Name = "dtpNs";
            this.dtpNs.Size = new System.Drawing.Size(170, 34);
            this.dtpNs.TabIndex = 7;
            // 
            // lblGt
            // 
            this.lblGt.Location = new System.Drawing.Point(10, 121);
            this.lblGt.Name = "lblGt";
            this.lblGt.Size = new System.Drawing.Size(120, 24);
            this.lblGt.TabIndex = 8;
            this.lblGt.Text = "Giới tính";
            // 
            // cmbGt
            // 
            this.cmbGt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGt.FormattingEnabled = true;
            this.cmbGt.Items.AddRange(new object[] {
            "Nam",
            "Nữ"});
            this.cmbGt.Location = new System.Drawing.Point(135, 115);
            this.cmbGt.Name = "cmbGt";
            this.cmbGt.Size = new System.Drawing.Size(170, 36);
            this.cmbGt.TabIndex = 9;
            // 
            // lblCccd
            // 
            this.lblCccd.Location = new System.Drawing.Point(330, 16);
            this.lblCccd.Name = "lblCccd";
            this.lblCccd.Size = new System.Drawing.Size(120, 24);
            this.lblCccd.TabIndex = 10;
            this.lblCccd.Text = "CCCD";
            // 
            // txtCccd
            // 
            this.txtCccd.Location = new System.Drawing.Point(455, 10);
            this.txtCccd.Name = "txtCccd";
            this.txtCccd.Size = new System.Drawing.Size(170, 34);
            this.txtCccd.TabIndex = 11;
            // 
            // lblSdt
            // 
            this.lblSdt.Location = new System.Drawing.Point(330, 51);
            this.lblSdt.Name = "lblSdt";
            this.lblSdt.Size = new System.Drawing.Size(120, 24);
            this.lblSdt.TabIndex = 12;
            this.lblSdt.Text = "SĐT";
            // 
            // txtSdt
            // 
            this.txtSdt.Location = new System.Drawing.Point(455, 45);
            this.txtSdt.Name = "txtSdt";
            this.txtSdt.Size = new System.Drawing.Size(170, 34);
            this.txtSdt.TabIndex = 13;
            // 
            // lblNghe
            // 
            this.lblNghe.Location = new System.Drawing.Point(330, 86);
            this.lblNghe.Name = "lblNghe";
            this.lblNghe.Size = new System.Drawing.Size(120, 24);
            this.lblNghe.TabIndex = 14;
            this.lblNghe.Text = "Nghề nghiệp";
            // 
            // txtNghe
            // 
            this.txtNghe.Location = new System.Drawing.Point(455, 80);
            this.txtNghe.Name = "txtNghe";
            this.txtNghe.Size = new System.Drawing.Size(170, 34);
            this.txtNghe.TabIndex = 15;
            // 
            // lblDiaChi
            // 
            this.lblDiaChi.Location = new System.Drawing.Point(330, 121);
            this.lblDiaChi.Name = "lblDiaChi";
            this.lblDiaChi.Size = new System.Drawing.Size(120, 24);
            this.lblDiaChi.TabIndex = 16;
            this.lblDiaChi.Text = "Địa chỉ";
            // 
            // txtDiaChi
            // 
            this.txtDiaChi.Location = new System.Drawing.Point(455, 115);
            this.txtDiaChi.Name = "txtDiaChi";
            this.txtDiaChi.Size = new System.Drawing.Size(170, 34);
            this.txtDiaChi.TabIndex = 17;
            // 
            // lblNguoiThan
            // 
            this.lblNguoiThan.Location = new System.Drawing.Point(650, 16);
            this.lblNguoiThan.Name = "lblNguoiThan";
            this.lblNguoiThan.Size = new System.Drawing.Size(120, 24);
            this.lblNguoiThan.TabIndex = 18;
            this.lblNguoiThan.Text = "Người thân";
            // 
            // txtNguoiThan
            // 
            this.txtNguoiThan.Location = new System.Drawing.Point(775, 10);
            this.txtNguoiThan.Name = "txtNguoiThan";
            this.txtNguoiThan.Size = new System.Drawing.Size(230, 34);
            this.txtNguoiThan.TabIndex = 19;
            // 
            // lblSdtNT
            // 
            this.lblSdtNT.Location = new System.Drawing.Point(650, 51);
            this.lblSdtNT.Name = "lblSdtNT";
            this.lblSdtNT.Size = new System.Drawing.Size(120, 24);
            this.lblSdtNT.TabIndex = 20;
            this.lblSdtNT.Text = "SĐT người thân";
            // 
            // txtSdtNT
            // 
            this.txtSdtNT.Location = new System.Drawing.Point(775, 45);
            this.txtSdtNT.Name = "txtSdtNT";
            this.txtSdtNT.Size = new System.Drawing.Size(230, 34);
            this.txtSdtNT.TabIndex = 21;
            // 
            // lblTinhTrang
            // 
            this.lblTinhTrang.Location = new System.Drawing.Point(650, 86);
            this.lblTinhTrang.Name = "lblTinhTrang";
            this.lblTinhTrang.Size = new System.Drawing.Size(120, 24);
            this.lblTinhTrang.TabIndex = 22;
            this.lblTinhTrang.Text = "Tình trạng bệnh";
            // 
            // txtTinhTrang
            // 
            this.txtTinhTrang.Location = new System.Drawing.Point(775, 80);
            this.txtTinhTrang.Name = "txtTinhTrang";
            this.txtTinhTrang.Size = new System.Drawing.Size(230, 34);
            this.txtTinhTrang.TabIndex = 23;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(10, 165);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(90, 32);
            this.btnThem.TabIndex = 24;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = false;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(110, 165);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(90, 32);
            this.btnSua.TabIndex = 25;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.BackColor = System.Drawing.Color.Red;
            this.btnXoa.FlatAppearance.BorderSize = 0;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.ForeColor = System.Drawing.Color.White;
            this.btnXoa.Location = new System.Drawing.Point(210, 165);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(90, 32);
            this.btnXoa.TabIndex = 26;
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
            this.btnLamMoi.Location = new System.Drawing.Point(310, 165);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(90, 32);
            this.btnLamMoi.TabIndex = 27;
            this.btnLamMoi.Text = "Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = false;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // lblTieuDeTim
            // 
            this.lblTieuDeTim.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTieuDeTim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.lblTieuDeTim.Location = new System.Drawing.Point(10, 208);
            this.lblTieuDeTim.Name = "lblTieuDeTim";
            this.lblTieuDeTim.Size = new System.Drawing.Size(260, 24);
            this.lblTieuDeTim.TabIndex = 28;
            this.lblTieuDeTim.Text = "🔍 TÌM KIẾM BỆNH NHÂN";
            // 
            // lblTimMa
            // 
            this.lblTimMa.Location = new System.Drawing.Point(10, 243);
            this.lblTimMa.Name = "lblTimMa";
            this.lblTimMa.Size = new System.Drawing.Size(55, 24);
            this.lblTimMa.TabIndex = 29;
            this.lblTimMa.Text = "Mã BN";
            // 
            // txtTimMa
            // 
            this.txtTimMa.Location = new System.Drawing.Point(70, 238);
            this.txtTimMa.Name = "txtTimMa";
            this.txtTimMa.Size = new System.Drawing.Size(90, 34);
            this.txtTimMa.TabIndex = 30;
            // 
            // lblTimTen
            // 
            this.lblTimTen.Location = new System.Drawing.Point(170, 243);
            this.lblTimTen.Name = "lblTimTen";
            this.lblTimTen.Size = new System.Drawing.Size(70, 24);
            this.lblTimTen.TabIndex = 31;
            this.lblTimTen.Text = "Họ và tên";
            // 
            // txtTimTen
            // 
            this.txtTimTen.Location = new System.Drawing.Point(245, 238);
            this.txtTimTen.Name = "txtTimTen";
            this.txtTimTen.Size = new System.Drawing.Size(140, 34);
            this.txtTimTen.TabIndex = 32;
            // 
            // lblTimGt
            // 
            this.lblTimGt.Location = new System.Drawing.Point(395, 243);
            this.lblTimGt.Name = "lblTimGt";
            this.lblTimGt.Size = new System.Drawing.Size(65, 24);
            this.lblTimGt.TabIndex = 33;
            this.lblTimGt.Text = "Giới tính";
            // 
            // cmbTimGt
            // 
            this.cmbTimGt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimGt.FormattingEnabled = true;
            this.cmbTimGt.Items.AddRange(new object[] {
            "-- Tất cả --",
            "Nam",
            "Nữ"});
            this.cmbTimGt.Location = new System.Drawing.Point(465, 237);
            this.cmbTimGt.Name = "cmbTimGt";
            this.cmbTimGt.Size = new System.Drawing.Size(105, 36);
            this.cmbTimGt.TabIndex = 34;
            // 
            // lblTimSdt
            // 
            this.lblTimSdt.Location = new System.Drawing.Point(580, 243);
            this.lblTimSdt.Name = "lblTimSdt";
            this.lblTimSdt.Size = new System.Drawing.Size(104, 24);
            this.lblTimSdt.TabIndex = 35;
            this.lblTimSdt.Text = "Số điện thoại";
            // 
            // txtTimSdt
            // 
            this.txtTimSdt.Location = new System.Drawing.Point(690, 238);
            this.txtTimSdt.Name = "txtTimSdt";
            this.txtTimSdt.Size = new System.Drawing.Size(105, 34);
            this.txtTimSdt.TabIndex = 36;
            // 
            // btnTimKiem
            // 
            this.btnTimKiem.BackColor = System.Drawing.Color.SandyBrown;
            this.btnTimKiem.FlatAppearance.BorderSize = 0;
            this.btnTimKiem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTimKiem.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            this.btnTimKiem.ForeColor = System.Drawing.Color.White;
            this.btnTimKiem.Location = new System.Drawing.Point(805, 236);
            this.btnTimKiem.Name = "btnTimKiem";
            this.btnTimKiem.Size = new System.Drawing.Size(115, 31);
            this.btnTimKiem.TabIndex = 37;
            this.btnTimKiem.Text = "🔍 Tìm kiếm";
            this.btnTimKiem.UseVisualStyleBackColor = false;
            this.btnTimKiem.Click += new System.EventHandler(this.btnTimKiem_Click);
            // 
            // btnLamMoiTim
            // 
            this.btnLamMoiTim.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(242)))), ((int)(((byte)(248)))));
            this.btnLamMoiTim.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(200)))), ((int)(((byte)(225)))));
            this.btnLamMoiTim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLamMoiTim.Font = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnLamMoiTim.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnLamMoiTim.Location = new System.Drawing.Point(930, 236);
            this.btnLamMoiTim.Name = "btnLamMoiTim";
            this.btnLamMoiTim.Size = new System.Drawing.Size(105, 31);
            this.btnLamMoiTim.TabIndex = 38;
            this.btnLamMoiTim.Text = "🔄 Làm mới";
            this.btnLamMoiTim.UseVisualStyleBackColor = false;
            this.btnLamMoiTim.Click += new System.EventHandler(this.btnLamMoiTim_Click);
            // 
            // BenhNhanForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "BenhNhanForm";
            this.Text = "Quản lý bệnh nhân";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblMa;
        private System.Windows.Forms.TextBox txtMa;
        private System.Windows.Forms.Label lblTen;
        private System.Windows.Forms.TextBox txtTen;
        private System.Windows.Forms.Label lblNs;
        private System.Windows.Forms.DateTimePicker dtpNs;
        private System.Windows.Forms.Label lblGt;
        private System.Windows.Forms.ComboBox cmbGt;
        private System.Windows.Forms.Label lblCccd;
        private System.Windows.Forms.TextBox txtCccd;
        private System.Windows.Forms.Label lblSdt;
        private System.Windows.Forms.TextBox txtSdt;
        private System.Windows.Forms.Label lblNghe;
        private System.Windows.Forms.TextBox txtNghe;
        private System.Windows.Forms.Label lblDiaChi;
        private System.Windows.Forms.TextBox txtDiaChi;
        private System.Windows.Forms.Label lblNguoiThan;
        private System.Windows.Forms.TextBox txtNguoiThan;
        private System.Windows.Forms.Label lblSdtNT;
        private System.Windows.Forms.TextBox txtSdtNT;
        private System.Windows.Forms.Label lblTinhTrang;
        private System.Windows.Forms.TextBox txtTinhTrang;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Label lblTieuDeTim;
        private System.Windows.Forms.Label lblTimMa;
        private System.Windows.Forms.TextBox txtTimMa;
        private System.Windows.Forms.Label lblTimTen;
        private System.Windows.Forms.TextBox txtTimTen;
        private System.Windows.Forms.Label lblTimGt;
        private System.Windows.Forms.ComboBox cmbTimGt;
        private System.Windows.Forms.Label lblTimSdt;
        private System.Windows.Forms.TextBox txtTimSdt;
        private System.Windows.Forms.Button btnTimKiem;
        private System.Windows.Forms.Button btnLamMoiTim;
    }
}
