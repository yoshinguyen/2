using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Quản lý Khoa (KhoaForm):
    /// Quản lý danh mục các chuyên khoa trong bệnh viện (Cấp cứu, Nội tim mạch, Ngoại chấn thương...).
    /// Khoa là đơn vị quản lý cấp cao nhất cho các Phòng bệnh và Bác sĩ.
    /// Kế thừa BaseForm để tái sử dụng các hàm tiện ích đồ họa và xử lý lỗi SQL.
    /// </summary>
    public partial class KhoaForm : BaseForm
    {
        public KhoaForm()
        {
            InitializeComponent(); //init
            LoadData(); // fill dgv

            // dgv click fill
            dgv.SelectionChanged += (s, e) => FillInputs();

            // enter keydown for searching
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
        /// dgv search filter
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT MaKhoa AS [Mã khoa], TenKhoa AS [Tên khoa],
                       CASE WHEN TrangThai = 1 THEN N'Hoạt động' ELSE N'Ngừng' END AS [Trạng thái],
                       GhiChu AS [Ghi chú]
                FROM KHOA
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();
            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND MaKhoa LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }
            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND TenKhoa LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }
            sql += " ORDER BY MaKhoa";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
        }

        /// <summary>
        /// fill input function
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;

            txtMa.Text = GetText(r, "Mã khoa");
            txtTen.Text = GetText(r, "Tên khoa");
            chkTrangThai.Checked = (GetText(r, "Trạng thái") == "Hoạt động");
            txtGhiChu.Text = GetText(r, "Ghi chú");
        }

        /// <summary>
        /// clear
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtGhiChu.Text = "";
            chkTrangThai.Checked = true;
        }

        /// <summary>
        /// clear all
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        void btnLamMoi_Click(object s, EventArgs e) 
        { 
            Reset(); 
        }

        /// <summary>
        /// validity check, must not be blank
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") 
            { 
                ShowWarning("Vui lòng nhập tên khoa."); 
                txtTen.Focus(); 
                return false; 
            }
            return true;
        }

        /// <summary>
        /// sql param for stuff
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@tt", chkTrangThai.Checked),
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
                string ma = DatabaseHelper.NextId("KHOA", "MaKhoa", "KH");
                string sql = "INSERT INTO KHOA(MaKhoa, TenKhoa, TrangThai, GhiChu) VALUES(@ma, @ten, @tt, @gc)";
                DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
                Reset();
                ShowInfo("Đã thêm mới thành công khoa " + ma + ".");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        /// mod
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn khoa cần sửa trong danh sách."); 
                return; 
            }
            if (!Valid()) return;

            try
            {
                string sql = "UPDATE KHOA SET TenKhoa=@ten, TrangThai=@tt, GhiChu=@gc WHERE MaKhoa=@ma";
                DatabaseHelper.ExecuteNonQuery(sql, Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật thông tin khoa " + txtMa.Text + " thành công.");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        ///del
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn khoa cần xóa."); 
                return; 
            }
            if (!Ask("Bạn có chắc chắn muốn xóa khoa " + txtMa.Text + " - " + txtTen.Text + "?")) 
                return;

            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM KHOA WHERE MaKhoa=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
                ShowInfo("Đã xóa khoa thành công.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Khoa này đang có phòng bệnh hoặc bác sĩ trực thuộc nên không thể xóa.\n" +
                              "Gợi ý: Hãy bỏ chọn ô 'Hoạt động' rồi bấm 'Sửa' để chuyển khoa sang trạng thái ngừng hoạt động.");
            }
        }
    }
}
