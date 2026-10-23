namespace QuanLyNoiTru
{
    partial class BaoCaoForm
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
            this.lblBaoCao = new System.Windows.Forms.Label();
            this.cmbBC = new System.Windows.Forms.ComboBox();
            this.btnXem = new System.Windows.Forms.Button();
            this.btnNhapExcel = new System.Windows.Forms.Button();
            this.btnExcel = new System.Windows.Forms.Button();
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
            this.dgv.Location = new System.Drawing.Point(0, 56);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.ReadOnly = true;
            this.dgv.RowHeadersVisible = false;
            this.dgv.RowHeadersWidth = 62;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(1085, 594);
            this.dgv.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(248)))), ((int)(((byte)(252)))));
            this.pnlTop.Controls.Add(this.lblBaoCao);
            this.pnlTop.Controls.Add(this.cmbBC);
            this.pnlTop.Controls.Add(this.btnXem);
            this.pnlTop.Controls.Add(this.btnNhapExcel);
            this.pnlTop.Controls.Add(this.btnExcel);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1085, 56);
            this.pnlTop.TabIndex = 1;
            // 
            // lblBaoCao
            // 
            this.lblBaoCao.Location = new System.Drawing.Point(10, 16);
            this.lblBaoCao.Name = "lblBaoCao";
            this.lblBaoCao.Size = new System.Drawing.Size(70, 24);
            this.lblBaoCao.TabIndex = 2;
            this.lblBaoCao.Text = "Báo cáo";
            // 
            // cmbBC
            // 
            this.cmbBC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBC.FormattingEnabled = true;
            this.cmbBC.Location = new System.Drawing.Point(85, 12);
            this.cmbBC.Name = "cmbBC";
            this.cmbBC.Size = new System.Drawing.Size(390, 36);
            this.cmbBC.TabIndex = 3;
            // 
            // btnXem
            // 
            this.btnXem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnXem.FlatAppearance.BorderSize = 0;
            this.btnXem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXem.ForeColor = System.Drawing.Color.White;
            this.btnXem.Location = new System.Drawing.Point(490, 10);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(70, 32);
            this.btnXem.TabIndex = 4;
            this.btnXem.Text = "Xem";
            this.btnXem.UseVisualStyleBackColor = false;
            this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
            // 
            // btnNhapExcel
            // 
            this.btnNhapExcel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(101)))), ((int)(((byte)(164)))));
            this.btnNhapExcel.FlatAppearance.BorderSize = 0;
            this.btnNhapExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNhapExcel.ForeColor = System.Drawing.Color.White;
            this.btnNhapExcel.Location = new System.Drawing.Point(570, 10);
            this.btnNhapExcel.Name = "btnNhapExcel";
            this.btnNhapExcel.Size = new System.Drawing.Size(110, 32);
            this.btnNhapExcel.TabIndex = 5;
            this.btnNhapExcel.Text = "Nhập Excel";
            this.btnNhapExcel.UseVisualStyleBackColor = false;
            this.btnNhapExcel.Click += new System.EventHandler(this.btnNhapExcel_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.BackColor = System.Drawing.Color.Green;
            this.btnExcel.FlatAppearance.BorderSize = 0;
            this.btnExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcel.ForeColor = System.Drawing.Color.White;
            this.btnExcel.Location = new System.Drawing.Point(690, 10);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(110, 32);
            this.btnExcel.TabIndex = 6;
            this.btnExcel.Text = "Xuất Excel";
            this.btnExcel.UseVisualStyleBackColor = false;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // BaoCaoForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1085, 650);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.Name = "BaoCaoForm";
            this.Text = "Báo cáo";
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblBaoCao;
        private System.Windows.Forms.ComboBox cmbBC;
        private System.Windows.Forms.Button btnXem;
        private System.Windows.Forms.Button btnNhapExcel;
        private System.Windows.Forms.Button btnExcel;
    }
}
