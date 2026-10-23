namespace QuanLyNoiTru
{
    partial class MainForm
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
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.btnDangXuat = new System.Windows.Forms.Button();
            this.btnBaoCao = new System.Windows.Forms.Button();
            this.btnBacSi = new System.Windows.Forms.Button();
            this.btnDichVu = new System.Windows.Forms.Button();
            this.btnThuoc = new System.Windows.Forms.Button();
            this.btnGiuong = new System.Windows.Forms.Button();
            this.btnPhong = new System.Windows.Forms.Button();
            this.btnKhoa = new System.Windows.Forms.Button();
            this.btnXuatVien = new System.Windows.Forms.Button();
            this.btnVienPhi = new System.Windows.Forms.Button();
            this.btnDonThuoc = new System.Windows.Forms.Button();
            this.btnBenhAn = new System.Windows.Forms.Button();
            this.btnNhapVien = new System.Windows.Forms.Button();
            this.btnBenhNhan = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContent
            // 
            this.pnlContent.BackColor = System.Drawing.Color.White;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(200, 48);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(1100, 672);
            this.pnlContent.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblUser);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(200, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1100, 48);
            this.pnlHeader.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(760, 48);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlMenu
            // 
            this.pnlMenu.AutoScroll = true;
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.pnlMenu.Controls.Add(this.btnDangXuat);
            this.pnlMenu.Controls.Add(this.btnBaoCao);
            this.pnlMenu.Controls.Add(this.btnBacSi);
            this.pnlMenu.Controls.Add(this.btnDichVu);
            this.pnlMenu.Controls.Add(this.btnThuoc);
            this.pnlMenu.Controls.Add(this.btnGiuong);
            this.pnlMenu.Controls.Add(this.btnPhong);
            this.pnlMenu.Controls.Add(this.btnKhoa);
            this.pnlMenu.Controls.Add(this.btnXuatVien);
            this.pnlMenu.Controls.Add(this.btnVienPhi);
            this.pnlMenu.Controls.Add(this.btnDonThuoc);
            this.pnlMenu.Controls.Add(this.btnBenhAn);
            this.pnlMenu.Controls.Add(this.btnNhapVien);
            this.pnlMenu.Controls.Add(this.btnBenhNhan);
            this.pnlMenu.Controls.Add(this.lblLogo);
            this.pnlMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMenu.Location = new System.Drawing.Point(0, 0);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(200, 720);
            this.pnlMenu.TabIndex = 4;
            // 
            // btnDangXuat
            // 
            this.btnDangXuat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnDangXuat.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnDangXuat.FlatAppearance.BorderSize = 0;
            this.btnDangXuat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDangXuat.ForeColor = System.Drawing.Color.White;
            this.btnDangXuat.Location = new System.Drawing.Point(0, 674);
            this.btnDangXuat.Name = "btnDangXuat";
            this.btnDangXuat.Size = new System.Drawing.Size(200, 46);
            this.btnDangXuat.TabIndex = 5;
            this.btnDangXuat.Text = "Đăng xuất";
            this.btnDangXuat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDangXuat.UseVisualStyleBackColor = false;
            this.btnDangXuat.Click += new System.EventHandler(this.btnDangXuat_Click);
            // 
            // btnBaoCao
            // 
            this.btnBaoCao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnBaoCao.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBaoCao.FlatAppearance.BorderSize = 0;
            this.btnBaoCao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBaoCao.ForeColor = System.Drawing.Color.White;
            this.btnBaoCao.Location = new System.Drawing.Point(0, 612);
            this.btnBaoCao.Name = "btnBaoCao";
            this.btnBaoCao.Size = new System.Drawing.Size(200, 46);
            this.btnBaoCao.TabIndex = 6;
            this.btnBaoCao.Text = "Quản lý báo cáo";
            this.btnBaoCao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBaoCao.UseVisualStyleBackColor = false;
            this.btnBaoCao.Click += new System.EventHandler(this.btnBaoCao_Click);
            // 
            // btnBacSi
            // 
            this.btnBacSi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnBacSi.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBacSi.FlatAppearance.BorderSize = 0;
            this.btnBacSi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBacSi.ForeColor = System.Drawing.Color.White;
            this.btnBacSi.Location = new System.Drawing.Point(0, 566);
            this.btnBacSi.Name = "btnBacSi";
            this.btnBacSi.Size = new System.Drawing.Size(200, 46);
            this.btnBacSi.TabIndex = 11;
            this.btnBacSi.Text = "Quản lý bác sĩ";
            this.btnBacSi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBacSi.UseVisualStyleBackColor = false;
            this.btnBacSi.Click += new System.EventHandler(this.btnBacSi_Click);
            // 
            // btnDichVu
            // 
            this.btnDichVu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnDichVu.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDichVu.FlatAppearance.BorderSize = 0;
            this.btnDichVu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDichVu.ForeColor = System.Drawing.Color.White;
            this.btnDichVu.Location = new System.Drawing.Point(0, 520);
            this.btnDichVu.Name = "btnDichVu";
            this.btnDichVu.Size = new System.Drawing.Size(200, 46);
            this.btnDichVu.TabIndex = 12;
            this.btnDichVu.Text = "Quản lý dịch vụ";
            this.btnDichVu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDichVu.UseVisualStyleBackColor = false;
            this.btnDichVu.Click += new System.EventHandler(this.btnDichVu_Click);
            // 
            // btnThuoc
            // 
            this.btnThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnThuoc.FlatAppearance.BorderSize = 0;
            this.btnThuoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThuoc.ForeColor = System.Drawing.Color.White;
            this.btnThuoc.Location = new System.Drawing.Point(0, 474);
            this.btnThuoc.Name = "btnThuoc";
            this.btnThuoc.Size = new System.Drawing.Size(200, 46);
            this.btnThuoc.TabIndex = 13;
            this.btnThuoc.Text = "Quản lý thuốc";
            this.btnThuoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThuoc.UseVisualStyleBackColor = false;
            this.btnThuoc.Click += new System.EventHandler(this.btnThuoc_Click);
            // 
            // btnGiuong
            // 
            this.btnGiuong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnGiuong.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnGiuong.FlatAppearance.BorderSize = 0;
            this.btnGiuong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGiuong.ForeColor = System.Drawing.Color.White;
            this.btnGiuong.Location = new System.Drawing.Point(0, 428);
            this.btnGiuong.Name = "btnGiuong";
            this.btnGiuong.Size = new System.Drawing.Size(200, 46);
            this.btnGiuong.TabIndex = 14;
            this.btnGiuong.Text = "Quản lý giường";
            this.btnGiuong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGiuong.UseVisualStyleBackColor = false;
            this.btnGiuong.Click += new System.EventHandler(this.btnGiuong_Click);
            // 
            // btnPhong
            // 
            this.btnPhong.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnPhong.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPhong.FlatAppearance.BorderSize = 0;
            this.btnPhong.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPhong.ForeColor = System.Drawing.Color.White;
            this.btnPhong.Location = new System.Drawing.Point(0, 382);
            this.btnPhong.Name = "btnPhong";
            this.btnPhong.Size = new System.Drawing.Size(200, 46);
            this.btnPhong.TabIndex = 15;
            this.btnPhong.Text = "Quản lý phòng";
            this.btnPhong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPhong.UseVisualStyleBackColor = false;
            this.btnPhong.Click += new System.EventHandler(this.btnPhong_Click);
            // 
            // btnKhoa
            // 
            this.btnKhoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnKhoa.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnKhoa.FlatAppearance.BorderSize = 0;
            this.btnKhoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKhoa.ForeColor = System.Drawing.Color.White;
            this.btnKhoa.Location = new System.Drawing.Point(0, 336);
            this.btnKhoa.Name = "btnKhoa";
            this.btnKhoa.Size = new System.Drawing.Size(200, 46);
            this.btnKhoa.TabIndex = 16;
            this.btnKhoa.Text = "Quản lý khoa";
            this.btnKhoa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnKhoa.UseVisualStyleBackColor = false;
            this.btnKhoa.Click += new System.EventHandler(this.btnKhoa_Click);
            // 
            // btnXuatVien
            // 
            this.btnXuatVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnXuatVien.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnXuatVien.FlatAppearance.BorderSize = 0;
            this.btnXuatVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXuatVien.ForeColor = System.Drawing.Color.White;
            this.btnXuatVien.Location = new System.Drawing.Point(0, 290);
            this.btnXuatVien.Name = "btnXuatVien";
            this.btnXuatVien.Size = new System.Drawing.Size(200, 46);
            this.btnXuatVien.TabIndex = 17;
            this.btnXuatVien.Text = "Quản lý xuất viện";
            this.btnXuatVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXuatVien.UseVisualStyleBackColor = false;
            this.btnXuatVien.Click += new System.EventHandler(this.btnXuatVien_Click);
            // 
            // btnVienPhi
            // 
            this.btnVienPhi.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnVienPhi.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnVienPhi.FlatAppearance.BorderSize = 0;
            this.btnVienPhi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVienPhi.ForeColor = System.Drawing.Color.White;
            this.btnVienPhi.Location = new System.Drawing.Point(0, 244);
            this.btnVienPhi.Name = "btnVienPhi";
            this.btnVienPhi.Size = new System.Drawing.Size(200, 46);
            this.btnVienPhi.TabIndex = 7;
            this.btnVienPhi.Text = "Quản lý viện phí";
            this.btnVienPhi.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnVienPhi.UseVisualStyleBackColor = false;
            this.btnVienPhi.Click += new System.EventHandler(this.btnVienPhi_Click);
            // 
            // btnDonThuoc
            // 
            this.btnDonThuoc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnDonThuoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDonThuoc.FlatAppearance.BorderSize = 0;
            this.btnDonThuoc.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDonThuoc.ForeColor = System.Drawing.Color.White;
            this.btnDonThuoc.Location = new System.Drawing.Point(0, 198);
            this.btnDonThuoc.Name = "btnDonThuoc";
            this.btnDonThuoc.Size = new System.Drawing.Size(200, 46);
            this.btnDonThuoc.TabIndex = 8;
            this.btnDonThuoc.Text = "Quản lý đơn thuốc";
            this.btnDonThuoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDonThuoc.UseVisualStyleBackColor = false;
            this.btnDonThuoc.Click += new System.EventHandler(this.btnDonThuoc_Click);
            // 
            // btnBenhAn
            // 
            this.btnBenhAn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnBenhAn.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBenhAn.FlatAppearance.BorderSize = 0;
            this.btnBenhAn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBenhAn.ForeColor = System.Drawing.Color.White;
            this.btnBenhAn.Location = new System.Drawing.Point(0, 152);
            this.btnBenhAn.Name = "btnBenhAn";
            this.btnBenhAn.Size = new System.Drawing.Size(200, 46);
            this.btnBenhAn.TabIndex = 9;
            this.btnBenhAn.Text = "Quản lý bệnh án";
            this.btnBenhAn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBenhAn.UseVisualStyleBackColor = false;
            this.btnBenhAn.Click += new System.EventHandler(this.btnBenhAn_Click);
            // 
            // btnNhapVien
            // 
            this.btnNhapVien.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnNhapVien.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNhapVien.FlatAppearance.BorderSize = 0;
            this.btnNhapVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNhapVien.ForeColor = System.Drawing.Color.White;
            this.btnNhapVien.Location = new System.Drawing.Point(0, 106);
            this.btnNhapVien.Name = "btnNhapVien";
            this.btnNhapVien.Size = new System.Drawing.Size(200, 46);
            this.btnNhapVien.TabIndex = 10;
            this.btnNhapVien.Text = "Quản lý nhập viện";
            this.btnNhapVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNhapVien.UseVisualStyleBackColor = false;
            this.btnNhapVien.Click += new System.EventHandler(this.btnNhapVien_Click);
            // 
            // btnBenhNhan
            // 
            this.btnBenhNhan.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.btnBenhNhan.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBenhNhan.FlatAppearance.BorderSize = 0;
            this.btnBenhNhan.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBenhNhan.ForeColor = System.Drawing.Color.White;
            this.btnBenhNhan.Location = new System.Drawing.Point(0, 60);
            this.btnBenhNhan.Name = "btnBenhNhan";
            this.btnBenhNhan.Size = new System.Drawing.Size(200, 46);
            this.btnBenhNhan.TabIndex = 12;
            this.btnBenhNhan.Text = "Quản lý bệnh nhân";
            this.btnBenhNhan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBenhNhan.UseVisualStyleBackColor = false;
            this.btnBenhNhan.Click += new System.EventHandler(this.btnBenhNhan_Click);
            // 
            // lblLogo
            // 
            this.lblLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(0, 0);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(200, 60);
            this.lblLogo.TabIndex = 13;
            this.lblLogo.Text = "QUẢN LÝ NỘI TRÚ";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUser
            // 
            this.lblUser.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblUser.Location = new System.Drawing.Point(760, 0);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(340, 48);
            this.lblUser.TabIndex = 3;
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1300, 720);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.MinimumSize = new System.Drawing.Size(1150, 650);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống quản lý bệnh nhân nội trú";
            this.pnlHeader.ResumeLayout(false);
            this.pnlMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Button btnDangXuat;
        private System.Windows.Forms.Button btnBaoCao;
        private System.Windows.Forms.Button btnVienPhi;
        private System.Windows.Forms.Button btnXuatVien;
        private System.Windows.Forms.Button btnDonThuoc;
        private System.Windows.Forms.Button btnBenhAn;
        private System.Windows.Forms.Button btnNhapVien;
        private System.Windows.Forms.Button btnBacSi;
        private System.Windows.Forms.Button btnKhoa;
        private System.Windows.Forms.Button btnPhong;
        private System.Windows.Forms.Button btnGiuong;
        private System.Windows.Forms.Button btnThuoc;
        private System.Windows.Forms.Button btnDichVu;
        private System.Windows.Forms.Button btnBenhNhan;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblUser;
    }
}
