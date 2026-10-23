using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Quản lý Giường bệnh nội trú:
    /// - Quản lý thông tin từng giường bệnh trong các phòng điều trị.
    /// - Mỗi giường thuộc về một phòng nhất định, có đơn giá riêng và trạng thái ('Trống' hoặc 'Đang sử dụng').
    /// - Cung cấp đầy đủ các thao tác Thêm mới, Sửa thông tin, Xóa giường và Tìm kiếm theo nhiều điều kiện độc lập (Mã giường, Tên giường, Phòng, Trạng thái).
    /// </summary>
    public partial class GiuongForm : BaseForm
    {
        /// <summary>
        /// initiallize
        /// </summary>
        public GiuongForm()
        {
            InitializeComponent();
            var dtPhong = DatabaseHelper.GetData("SELECT MaPhong, TenPhong FROM PHONG ORDER BY MaPhong");
            FillCombo(cmbPhong, dtPhong, "TenPhong", "MaPhong");

            // fill combox search, default '-- Tất cả --'
            var dtTimPhong = dtPhong.Copy();
            var rAll = dtTimPhong.NewRow();
            rAll["MaPhong"] = "";
            rAll["TenPhong"] = "-- Tất cả --";
            dtTimPhong.Rows.InsertAt(rAll, 0);
            FillCombo(cmbTimPhong, dtTimPhong, "TenPhong", "MaPhong");
            cmbTimTT.SelectedIndex = 0; // default'-- Tất cả --'

            LoadData();

            // fill textbox khi click dgv
            dgv.SelectionChanged += (s, e) => FillInputs();

            // enter de tim kiem
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
        }

        /// <summary>
        /// search
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// refresh
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimPhong.Items.Count > 0) cmbTimPhong.SelectedIndex = 0;
            if (cmbTimTT.Items.Count > 0) cmbTimTT.SelectedIndex = 0;
            LoadData();
        }

        /// <summary>
        /// fill giuong from db
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT g.MaGiuong AS [Mã giường], g.TenGiuong AS [Tên giường],
                       g.MaPhong AS [Mã phòng], p.TenPhong AS [Phòng],
                       g.DonGia AS [Đơn giá],
                       g.TrangThai AS [Trạng thái]
                FROM GIUONG g LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();
            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND g.MaGiuong LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }
            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND g.TenGiuong LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }
            string maPhong = GetValue(cmbTimPhong);
            if (!string.IsNullOrEmpty(maPhong))
            {
                sql += " AND g.MaPhong = @timPhong ";
                ps.Add(DatabaseHelper.Param("@timPhong", maPhong));
            }
            if (cmbTimTT.SelectedIndex > 0 && cmbTimTT.Text != "-- Tất cả --")
            {
                sql += " AND g.TrangThai = @timTT ";
                ps.Add(DatabaseHelper.Param("@timTT", cmbTimTT.Text));
            }

            sql += " ORDER BY g.MaGiuong";

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
            txtMa.Text = GetText(r, "Mã giường");
            txtTen.Text = GetText(r, "Tên giường");
            if (GetText(r, "Mã phòng") != "") cmbPhong.SelectedValue = GetText(r, "Mã phòng");
            txtDonGia.Text = GetText(r, "Đơn giá");
        }

        /// <summary>
        /// clear
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtDonGia.Text = "";
        }

        /// <summary>
        /// reset form: xóa ô tìm kiếm, tải lại bảng và bỏ chọn.
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimPhong.Items.Count > 0) cmbTimPhong.SelectedIndex = 0;
            if (cmbTimTT.Items.Count > 0) cmbTimTT.SelectedIndex = 0;
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        /// <summary>
        /// refresh btn, call Reset().
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e) { Reset(); }

        /// <summary>
        /// validity check
        /// - can ten
        /// - phai chon giuong
        /// - don gia >= 0.
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") { ShowWarning("Vui lòng nhập tên/số giường."); txtTen.Focus(); return false; }
            if (GetValue(cmbPhong) == null) { ShowWarning("Vui lòng chọn phòng."); return false; }
            decimal dg;
            if (!decimal.TryParse(txtDonGia.Text, out dg) || dg < 0)
            { ShowWarning("Đơn giá phải là số >= 0."); txtDonGia.Focus(); return false; }
            return true;
        }

        /// <summary>
        /// sql param for whatever
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@p", GetValue(cmbPhong)),
                DatabaseHelper.Param("@gia", decimal.Parse(txtDonGia.Text))
            };
        }

        /// <summary>
        /// them giuong
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            if (!Valid()) return;
            try
            {
                string ma = DatabaseHelper.NextId("GIUONG", "MaGiuong", "G");
                DatabaseHelper.ExecuteNonQuery("INSERT INTO GIUONG(MaGiuong, TenGiuong, MaPhong, DonGia, TrangThai) VALUES(@ma, @ten, @p, @gia, N'Trống')", Ps(ma));
                Reset();
                ShowInfo("Đã thêm giường " + ma + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// update
        /// block neu bi su dun
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn giường cần sửa."); return; }
            if (!Valid()) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("UPDATE GIUONG SET TenGiuong=@ten, MaPhong=@p, DonGia=@gia WHERE MaGiuong=@ma", Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật giường " + txtMa.Text + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// xoa giuong, block neu bi su dung
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn giường cần xóa."); return; }
            /* if (!Ask("Xóa giường " + txtMa.Text + " - " + txtTen.Text + "?")) return;*/
            if (MessageBox.Show("Xóa giường " + txtMa.Text + " - " + txtTen.Text + "?","Xác nhận xóa",MessageBoxButtons.YesNo,MessageBoxIcon.Question
        ) != DialogResult.Yes)
                return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM GIUONG WHERE MaGiuong=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Giường này đang có bệnh nhân nằm hoặc đã có lịch sử nhập viện, không thể xóa.");
            }
        }
    }
}



/*add
string ma = DatabaseHelper.NextId("GIUONG", "MaGiuong", "G");

string sql = @"INSERT INTO GIUONG
(MaGiuong, TenGiuong, MaPhong, DonGia, TrangThai)
VALUES (@ma, @ten, @p, @gia, N'Trống')";

SqlParameter[] parameters =
{
DatabaseHelper.Param("@ma", ma),
DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
DatabaseHelper.Param("@p", GetValue(cmbPhong)),
DatabaseHelper.Param("@gia", decimal.Parse(txtDonGia.Text))
};

DatabaseHelper.ExecuteNonQuery(sql, parameters);



mod
 */

/*string sql = @"UPDATE GIUONG
               SET TenGiuong = @ten,
                   MaPhong = @p,
                   DonGia = @gia
               WHERE MaGiuong = @ma";

SqlParameter[] parameters =
{
    DatabaseHelper.Param("@ma", txtMa.Text.Trim()),
    DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
    DatabaseHelper.Param("@p", GetValue(cmbPhong)),
    DatabaseHelper.Param("@gia", decimal.Parse(txtDonGia.Text))
};

DatabaseHelper.ExecuteNonQuery(sql, parameters);*/



/*
 private void btnXoa_Click(object sender, EventArgs e)
{
    if (string.IsNullOrWhiteSpace(txtMa.Text))
    {
        MessageBox.Show("Vui lòng chọn giường cần xóa!");
        return;
    }

    DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn xóa giường này không?",
        "Xác nhận xóa",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

    if (result == DialogResult.No)
        return;

    string sql = "DELETE FROM GIUONG WHERE MaGiuong = @ma";

    SqlParameter[] parameters =
    {
        DatabaseHelper.Param("@ma", txtMa.Text.Trim())
    };

    try
    {
        DatabaseHelper.ExecuteNonQuery(sql, parameters);
        MessageBox.Show("Xóa giường thành công!");
        LoadData();
    }
    catch (SqlException)
    {
        MessageBox.Show(
            "Không thể xóa giường này vì đang được sử dụng hoặc có dữ liệu liên quan!"
        );
    }
}
 */