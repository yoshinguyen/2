namespace QuanLyNoiTru
{
    partial class DonThuocForm
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
            this.lblBA = new System.Windows.Forms.Label();
            this.cmbBA = new System.Windows.Forms.ComboBox();
            this.lblDon = new System.Windows.Forms.Label();
            this.cmbDon = new System.Windows.Forms.ComboBox();
            this.btnTaoDon = new System.Windows.Forms.Button();
            this.lblGhiChuDon = new System.Windows.Forms.Label();
            this.txtGhiChuDon = new System.Windows.Forms.TextBox();
            this.lblThuoc = new System.Windows.Forms.Label();
            this.cmbThuoc = new System.Windows.Forms.ComboBox();
            this.lblSL = new System.Windows.Forms.Label();
            this.nudSL = new System.Windows.Forms.NumericUpDown();
            this.lblDonGiaTen = new System.Windows.Forms.Label();
            this.lblGia = new System.Windows.Forms.Label();
            this.lblLieu = new System.Windows.Forms.Label();
            this.txtLieu = new System.Windows.Forms.TextBox();
            this.lblCach = new System.Windows.Forms.Label();
            this.txtCach = new System.Windows.Forms.TextBox();
            this.btnThemThuoc = new System.Windows.Forms.Button();
            this.btnCapNhat = new System.Windows.Forms.Button();
            this.btnXoaThuoc = new System.Windows.Forms.Button();
            this.lblTong = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSL)).BeginInit();
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
            this.dgv.Location = new System.Drawing.Point(0, 240);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1085, 410);
            this.dgv.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblBA);
            this.pnlTop.Controls.Add(this.cmbBA);
            this.pnlTop.Controls.Add(this.lblDon);
            this.pnlTop.Controls.Add(this.cmbDon);
            this.pnlTop.Controls.Add(this.btnTaoDon);
            this.pnlTop.Controls.Add(this.lblGhiChuDon);
            this.pnlTop.Controls.Add(this.txtGhiChuDon);
            this.pnlTop.Controls.Add(this.lblThuoc);
            this.pnlTop.Controls.Add(this.cmbThuoc);
            this.pnlTop.Controls.Add(this.lblSL);
            this.pnlTop.Controls.Add(this.nudSL);
            this.pnlTop.Controls.Add(this.lblDonGiaTen);
            this.pnlTop.Controls.Add(this.lblGia);
            this.pnlTop.Controls.Add(this.lblLieu);
            this.pnlTop.Controls.Add(this.txtLieu);
            this.pnlTop.Controls.Add(this.lblCach);
            this.pnlTop.Controls.Add(this.txtCach);
            this.pnlTop.Controls.Add(this.btnThemThuoc);
            this.pnlTop.Controls.Add(this.btnCapNhat);
            this.pnlTop.Controls.Add(this.btnXoaThuoc);
            this.pnlTop.Controls.Add(this.lblTong);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1085, 240);
            this.pnlTop.TabIndex = 1;
            // 
            // lblBA
            // 
            this.lblBA.Location = new System.Drawing.Point(10, 16);
            this.lblBA.Name = "lblBA";
            this.lblBA.Size = new System.Drawing.Size(120, 24);
            this.lblBA.TabIndex = 2;
            this.lblBA.Text = "Bệnh án";
            // 
            // cmbBA
            // 
            this.cmbBA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBA.FormattingEnabled = true;
            this.cmbBA.Location = new System.Drawing.Point(135, 10);
            this.cmbBA.Name = "cmbBA";
            this.cmbBA.Size = new System.Drawing.Size(470, 36);
            this.cmbBA.TabIndex = 3;
            // 
            // lblDon
            // 
            this.lblDon.Location = new System.Drawing.Point(630, 16);
            this.lblDon.Name = "lblDon";
            this.lblDon.Size = new System.Drawing.Size(90, 24);
            this.lblDon.TabIndex = 4;
            this.lblDon.Text = "Đơn thuốc";
            // 
            // cmbDon
            // 
            this.cmbDon.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDon.FormattingEnabled = true;
            this.cmbDon.Location = new System.Drawing.Point(725, 10);
            this.cmbDon.Name = "cmbDon";
            this.cmbDon.Size = new System.Drawing.Size(140, 36);
            this.cmbDon.TabIndex = 5;
            // 
            // btnTaoDon
            // 
            this.btnTaoDon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnTaoDon.FlatAppearance.BorderSize = 0;
            this.btnTaoDon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTaoDon.ForeColor = System.Drawing.Color.White;
            this.btnTaoDon.Location = new System.Drawing.Point(880, 8);
            this.btnTaoDon.Name = "btnTaoDon";
            this.btnTaoDon.Size = new System.Drawing.Size(125, 32);
            this.btnTaoDon.TabIndex = 6;
            this.btnTaoDon.Text = "Tạo đơn mới";
            this.btnTaoDon.UseVisualStyleBackColor = false;
            this.btnTaoDon.Click += new System.EventHandler(this.btnTaoDon_Click);
            // 
            // lblGhiChuDon
            // 
            this.lblGhiChuDon.Location = new System.Drawing.Point(10, 51);
            this.lblGhiChuDon.Name = "lblGhiChuDon";
            this.lblGhiChuDon.Size = new System.Drawing.Size(120, 24);
            this.lblGhiChuDon.TabIndex = 7;
            this.lblGhiChuDon.Text = "Ghi chú đơn";
            // 
            // txtGhiChuDon
            // 
            this.txtGhiChuDon.Location = new System.Drawing.Point(135, 45);
            this.txtGhiChuDon.Name = "txtGhiChuDon";
            this.txtGhiChuDon.Size = new System.Drawing.Size(470, 34);
            this.txtGhiChuDon.TabIndex = 8;
            // 
            // lblThuoc
            // 
            this.lblThuoc.Location = new System.Drawing.Point(10, 96);
            this.lblThuoc.Name = "lblThuoc";
            this.lblThuoc.Size = new System.Drawing.Size(120, 24);
            this.lblThuoc.TabIndex = 9;
            this.lblThuoc.Text = "Thuốc";
            // 
            // cmbThuoc
            // 
            this.cmbThuoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbThuoc.FormattingEnabled = true;
            this.cmbThuoc.Location = new System.Drawing.Point(135, 90);
            this.cmbThuoc.Name = "cmbThuoc";
            this.cmbThuoc.Size = new System.Drawing.Size(330, 36);
            this.cmbThuoc.TabIndex = 10;
            // 
            // lblSL
            // 
            this.lblSL.Location = new System.Drawing.Point(490, 96);
            this.lblSL.Name = "lblSL";
            this.lblSL.Size = new System.Drawing.Size(70, 24);
            this.lblSL.TabIndex = 11;
            this.lblSL.Text = "Số lượng";
            // 
            // nudSL
            // 
            this.nudSL.Location = new System.Drawing.Point(565, 90);
            this.nudSL.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudSL.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSL.Name = "nudSL";
            this.nudSL.Size = new System.Drawing.Size(90, 34);
            this.nudSL.TabIndex = 12;
            this.nudSL.ThousandsSeparator = true;
            this.nudSL.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblDonGiaTen
            // 
            this.lblDonGiaTen.Location = new System.Drawing.Point(680, 96);
            this.lblDonGiaTen.Name = "lblDonGiaTen";
            this.lblDonGiaTen.Size = new System.Drawing.Size(70, 24);
            this.lblDonGiaTen.TabIndex = 13;
            this.lblDonGiaTen.Text = "Đơn giá";
            // 
            // lblGia
            // 
            this.lblGia.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblGia.Location = new System.Drawing.Point(755, 96);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(150, 24);
            this.lblGia.TabIndex = 14;
            this.lblGia.Text = "0 đ";
            // 
            // lblLieu
            // 
            this.lblLieu.Location = new System.Drawing.Point(10, 131);
            this.lblLieu.Name = "lblLieu";
            this.lblLieu.Size = new System.Drawing.Size(120, 24);
            this.lblLieu.TabIndex = 15;
            this.lblLieu.Text = "Liều dùng";
            // 
            // txtLieu
            // 
            this.txtLieu.Location = new System.Drawing.Point(135, 125);
            this.txtLieu.Name = "txtLieu";
            this.txtLieu.Size = new System.Drawing.Size(330, 34);
            this.txtLieu.TabIndex = 16;
            // 
            // lblCach
            // 
            this.lblCach.Location = new System.Drawing.Point(490, 131);
            this.lblCach.Name = "lblCach";
            this.lblCach.Size = new System.Drawing.Size(70, 24);
            this.lblCach.TabIndex = 17;
            this.lblCach.Text = "Cách dùng";
            // 
            // txtCach
            // 
            this.txtCach.Location = new System.Drawing.Point(565, 125);
            this.txtCach.Name = "txtCach";
            this.txtCach.Size = new System.Drawing.Size(440, 34);
            this.txtCach.TabIndex = 18;
            // 
            // btnThemThuoc
            // 
            this.btnThemThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnThemThuoc.FlatAppearance.BorderSize = 0;
            this.btnThemThuoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThemThuoc.ForeColor = System.Drawing.Color.White;
            this.btnThemThuoc.Location = new System.Drawing.Point(10, 170);
            this.btnThemThuoc.Name = "btnThemThuoc";
            this.btnThemThuoc.Size = new System.Drawing.Size(110, 32);
            this.btnThemThuoc.TabIndex = 19;
            this.btnThemThuoc.Text = "Thêm thuốc";
            this.btnThemThuoc.UseVisualStyleBackColor = false;
            this.btnThemThuoc.Click += new System.EventHandler(this.btnThemThuoc_Click);
            // 
            // btnCapNhat
            // 
            this.btnCapNhat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnCapNhat.FlatAppearance.BorderSize = 0;
            this.btnCapNhat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCapNhat.ForeColor = System.Drawing.Color.White;
            this.btnCapNhat.Location = new System.Drawing.Point(130, 170);
            this.btnCapNhat.Name = "btnCapNhat";
            this.btnCapNhat.Size = new System.Drawing.Size(100, 32);
            this.btnCapNhat.TabIndex = 20;
            this.btnCapNhat.Text = "Cập nhật";
            this.btnCapNhat.UseVisualStyleBackColor = false;
            this.btnCapNhat.Click += new System.EventHandler(this.btnCapNhat_Click);
            // 
            // btnXoaThuoc
            // 
            this.btnXoaThuoc.BackColor = System.Drawing.Color.Red;
            this.btnXoaThuoc.FlatAppearance.BorderSize = 0;
            this.btnXoaThuoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoaThuoc.ForeColor = System.Drawing.Color.White;
            this.btnXoaThuoc.Location = new System.Drawing.Point(240, 170);
            this.btnXoaThuoc.Name = "btnXoaThuoc";
            this.btnXoaThuoc.Size = new System.Drawing.Size(100, 32);
            this.btnXoaThuoc.TabIndex = 21;
            this.btnXoaThuoc.Text = "Xóa thuốc";
            this.btnXoaThuoc.UseVisualStyleBackColor = false;
            this.btnXoaThuoc.Click += new System.EventHandler(this.btnXoaThuoc_Click);
            // 
            // lblTong
            // 
            this.lblTong.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTong.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.lblTong.Location = new System.Drawing.Point(370, 176);
            this.lblTong.Name = "lblTong";
            this.lblTong.Size = new System.Drawing.Size(400, 24);
            this.lblTong.TabIndex = 22;
            this.lblTong.Text = "Tổng tiền đơn: 0 đ";
            // 
            // DonThuocForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "DonThuocForm";
            this.Text = "Đơn thuốc";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSL)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblBA;
        private System.Windows.Forms.ComboBox cmbBA;
        private System.Windows.Forms.Label lblDon;
        private System.Windows.Forms.ComboBox cmbDon;
        private System.Windows.Forms.Button btnTaoDon;
        private System.Windows.Forms.Label lblGhiChuDon;
        private System.Windows.Forms.TextBox txtGhiChuDon;
        private System.Windows.Forms.Label lblThuoc;
        private System.Windows.Forms.ComboBox cmbThuoc;
        private System.Windows.Forms.Label lblSL;
        private System.Windows.Forms.NumericUpDown nudSL;
        private System.Windows.Forms.Label lblDonGiaTen;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.Label lblLieu;
        private System.Windows.Forms.TextBox txtLieu;
        private System.Windows.Forms.Label lblCach;
        private System.Windows.Forms.TextBox txtCach;
        private System.Windows.Forms.Button btnThemThuoc;
        private System.Windows.Forms.Button btnCapNhat;
        private System.Windows.Forms.Button btnXoaThuoc;
        private System.Windows.Forms.Label lblTong;
    }
}
