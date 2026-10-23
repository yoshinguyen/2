namespace QuanLyNoiTru
{
    partial class LoginForm
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
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.lblTaiKhoan = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblMatKhau = new System.Windows.Forms.Label();
            this.txtPass = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.lblGoiY = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(58)))), ((int)(((byte)(95)))));
            this.lblTieuDe.Location = new System.Drawing.Point(0, 20);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(400, 36);
            this.lblTieuDe.TabIndex = 0;
            this.lblTieuDe.Text = "ĐĂNG NHẬP HỆ THỐNG";
            this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTaiKhoan
            // 
            this.lblTaiKhoan.Location = new System.Drawing.Point(40, 82);
            this.lblTaiKhoan.Name = "lblTaiKhoan";
            this.lblTaiKhoan.Size = new System.Drawing.Size(90, 24);
            this.lblTaiKhoan.TabIndex = 1;
            this.lblTaiKhoan.Text = "Tài khoản";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(135, 79);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(220, 34);
            this.txtUser.TabIndex = 2;
            // 
            // lblMatKhau
            // 
            this.lblMatKhau.Location = new System.Drawing.Point(40, 122);
            this.lblMatKhau.Name = "lblMatKhau";
            this.lblMatKhau.Size = new System.Drawing.Size(90, 24);
            this.lblMatKhau.TabIndex = 3;
            this.lblMatKhau.Text = "Mật khẩu";
            // 
            // txtPass
            // 
            this.txtPass.Location = new System.Drawing.Point(135, 119);
            this.txtPass.Name = "txtPass";
            this.txtPass.Size = new System.Drawing.Size(220, 34);
            this.txtPass.TabIndex = 4;
            this.txtPass.UseSystemPasswordChar = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(135, 167);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(105, 34);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "Đăng nhập";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnThoat.Location = new System.Drawing.Point(250, 167);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(105, 34);
            this.btnThoat.TabIndex = 6;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            // 
            // lblGoiY
            // 
            this.lblGoiY.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblGoiY.ForeColor = System.Drawing.Color.Gray;
            this.lblGoiY.Location = new System.Drawing.Point(40, 215);
            this.lblGoiY.Name = "lblGoiY";
            this.lblGoiY.Size = new System.Drawing.Size(330, 40);
            this.lblGoiY.TabIndex = 7;
            this.lblGoiY.Text = "Tài khoản mẫu: 1 / 1";
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnThoat;
            this.ClientSize = new System.Drawing.Size(400, 270);
            this.Controls.Add(this.lblTieuDe);
            this.Controls.Add(this.lblTaiKhoan);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.lblMatKhau);
            this.Controls.Add(this.txtPass);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnThoat);
            this.Controls.Add(this.lblGoiY);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTaiKhoan;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblMatKhau;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Label lblGoiY;
    }
}
