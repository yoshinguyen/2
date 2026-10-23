using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form quản lý danh mục dịch vụ y tế (chẩn đoán hình ảnh, siêu âm, xét nghiệm, phẫu thuật, ...):
    /// - Quản lý thông tin: Mã dịch vụ, Tên dịch vụ, Đơn giá gốc, Phần trăm Bảo hiểm Y tế (% BHYT) chi trả, và Ghi chú.
    /// - Hỗ trợ đầy đủ chức năng Thêm mới, Sửa thông tin, Xóa và Tìm kiếm nhiều tiêu chí (Mã DV, Tên DV).
    /// </summary>
    public partial class DichVuForm : BaseForm
    {
        /// <summary>
        /// initiallize
        /// </summary>
        public DichVuForm()
        {
            InitializeComponent();
            LoadData();

            // dgv fill
            dgv.SelectionChanged += (s, e) => FillInputs();

            // enter keypress for searching
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
        }

        /// <summary>
        /// search btn
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// refresh btn
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            LoadData();
        }

        /// <summary>
        /// search filter
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT MaDV AS [Mã DV], TenDV AS [Tên dịch vụ],
                       DonGia AS [Đơn giá], PT_BaoHiem AS [% BH],
                       GhiChu AS [Ghi chú]
                FROM DICHVU
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(txtTimMa.Text))
            {
                sql += " AND MaDV LIKE @ma ";
                ps.Add(DatabaseHelper.Param("@ma", "%" + txtTimMa.Text.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(txtTimTen.Text))
            {
                sql += " AND TenDV LIKE @ten ";
                ps.Add(DatabaseHelper.Param("@ten", "%" + txtTimTen.Text.Trim() + "%"));
            }

            sql += " ORDER BY MaDV ";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            FormatColumn(dgv, "Đơn giá", "N0");
        }

        /// <summary>
        /// fill input function
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;
            txtMa.Text = GetText(r, "Mã DV");
            txtTen.Text = GetText(r, "Tên dịch vụ");
            txtDonGia.Text = GetText(r, "Đơn giá");
            txtBH.Text = GetText(r, "% BH");
            txtGhiChu.Text = GetText(r, "Ghi chú");
        }

        /// <summary>
        /// clear
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtDonGia.Text = txtBH.Text = txtGhiChu.Text = "";
        }

        /// <summary>
        /// refresh all
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        /// <summary>
        /// reset() call
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e) { Reset(); }

        /// <summary>
        /// validity check
        /// name required
        ///  >= 0.
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") { ShowWarning("Vui lòng nhập tên dịch vụ."); txtTen.Focus(); return false; }
            decimal dg;
            if (!decimal.TryParse(txtDonGia.Text, out dg) || dg < 0)
            { ShowWarning("Đơn giá phải là số >= 0."); txtDonGia.Focus(); return false; }
            return true;
        }

        /// <summary>
        /// sql param for INSERT/UPDATE.
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            double? bh = null;
            double bhVal;
            if (double.TryParse(txtBH.Text, out bhVal)) bh = bhVal;

            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@gia", decimal.Parse(txtDonGia.Text)),
                DatabaseHelper.Param("@bh", (object)bh ?? DBNull.Value),
                DatabaseHelper.Param("@gc", txtGhiChu.Text.Trim())
            };
        }

        /// <summary>
        /// add
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            if (!Valid()) return;
            try
            {
                string ma = DatabaseHelper.NextId("DICHVU", "MaDV", "DV");
                DatabaseHelper.ExecuteNonQuery(
                    "INSERT INTO DICHVU(MaDV, TenDV, DonGia, PT_BaoHiem, GhiChu) VALUES(@ma, @ten, @gia, @bh, @gc)", Ps(ma));
                Reset();
                ShowInfo("Đã thêm dịch vụ " + ma + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// mod
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn dịch vụ cần sửa."); return; }
            if (!Valid()) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE DICHVU SET TenDV=@ten, DonGia=@gia, PT_BaoHiem=@bh, GhiChu=@gc WHERE MaDV=@ma", Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật dịch vụ " + txtMa.Text + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// del
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn dịch vụ cần xóa."); return; }
            if (!Ask("Xóa dịch vụ " + txtMa.Text + " - " + txtTen.Text + "?")) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM DICHVU WHERE MaDV=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Dịch vụ này đang được sử dụng trong lịch điều trị nên không thể xóa.");
            }
        }
    }
}
