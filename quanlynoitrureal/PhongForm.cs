using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Quản lý Phòng bệnh (PhongForm):
    /// Quản lý danh mục các phòng bệnh trực thuộc từng Khoa (Phòng cấp cứu 101, Phòng tim mạch 201...).
    /// Phòng bệnh là nơi chứa các Giường bệnh để bệnh nhân nằm điều trị nội trú.
    /// Kế thừa BaseForm để dùng các hàm tiện ích.
    /// </summary>
    public partial class PhongForm : BaseForm
    {
        public PhongForm()
        {
            InitializeComponent(); // Dựng giao diện

            // Đổ danh sách Khoa đang hoạt động (TrangThai = 1) vào ComboBox để người dùng gán phòng vào khoa
            var dtKhoa = DatabaseHelper.GetData("SELECT MaKhoa, TenKhoa FROM KHOA WHERE TrangThai=1 ORDER BY MaKhoa");
            FillCombo(cmbKhoa, dtKhoa, "TenKhoa", "MaKhoa");

            // Đổ danh sách Khoa vào ComboBox tìm kiếm kèm '-- Tất cả --'
            var dtTimKhoa = dtKhoa.Copy();
            var rAll = dtTimKhoa.NewRow();
            rAll["MaKhoa"] = "";
            rAll["TenKhoa"] = "-- Tất cả --";
            dtTimKhoa.Rows.InsertAt(rAll, 0);
            FillCombo(cmbTimKhoa, dtTimKhoa, "TenKhoa", "MaKhoa");

            LoadData(); // Nạp dữ liệu phòng lên bảng DataGridView

            // Gắn sự kiện:
            // - Bấm chọn dòng trên bảng -> Nạp dữ liệu lên ô nhập
            dgv.SelectionChanged += (s, e) => FillInputs();

            // Nhấn phím Enter trong các ô tìm kiếm để thực hiện tìm kiếm
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
        }

        /// <summary>
        /// Nút [🔍 Tìm kiếm]: Thực hiện lọc danh sách phòng theo điều kiện đã nhập.
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Nút [🔄 Làm mới] ở thanh tìm kiếm: Xóa trắng các ô tìm kiếm và nạp lại toàn bộ danh sách phòng.
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimKhoa.Items.Count > 0) cmbTimKhoa.SelectedIndex = 0;
            LoadData();
        }

        /// <summary>
        /// Nạp danh sách phòng từ bảng PHONG theo nhiều điều kiện kết hợp (Mã phòng, Tên phòng, Khoa).
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT p.MaPhong AS [Mã phòng], p.TenPhong AS [Tên phòng],
                       p.MaKhoa AS [Mã khoa], k.TenKhoa AS [Khoa]
                FROM PHONG p 
                LEFT JOIN KHOA k ON k.MaKhoa = p.MaKhoa
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();
            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND p.MaPhong LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }
            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND p.TenPhong LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }
            string maKhoa = GetValue(cmbTimKhoa);
            if (!string.IsNullOrEmpty(maKhoa))
            {
                sql += " AND p.MaKhoa = @timKhoa ";
                ps.Add(DatabaseHelper.Param("@timKhoa", maKhoa));
            }
            sql += " ORDER BY p.MaPhong";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
        }

        /// <summary>
        /// Đọc thông tin phòng từ dòng đang chọn trong DataGridView và hiển thị lên giao diện.
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;

            txtMa.Text = GetText(r, "Mã phòng");
            txtTen.Text = GetText(r, "Tên phòng");
            // Chọn đúng khoa của phòng trong ComboBox
            if (GetText(r, "Mã khoa") != "") 
                cmbKhoa.SelectedValue = GetText(r, "Mã khoa");
        }

        /// <summary>
        /// Xóa trắng các ô nhập liệu.
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = "";
        }

        /// <summary>
        /// Đặt lại form: xóa từ khóa tìm kiếm, tải lại danh sách.
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimKhoa.Items.Count > 0) cmbTimKhoa.SelectedIndex = 0;
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        void btnLamMoi_Click(object s, EventArgs e) 
        { 
            Reset(); 
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu trước khi lưu: Tên phòng không rỗng và phải chọn Khoa.
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") 
            { 
                ShowWarning("Vui lòng nhập tên phòng bệnh."); 
                txtTen.Focus(); 
                return false; 
            }
            if (GetValue(cmbKhoa) == null) 
            { 
                ShowWarning("Vui lòng chọn Khoa quản lý phòng này."); 
                return false; 
            }
            return true;
        }

        /// <summary>
        /// Đóng gói dữ liệu thành mảng tham số SQL.
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@khoa", GetValue(cmbKhoa))
            };
        }

        /// <summary>
        /// Xử lý nút [Thêm]: Tự động sinh mã PHxx (PH01, PH02...) và lưu phòng mới vào CSDL.
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            if (!Valid()) return;
            try
            {
                string ma = DatabaseHelper.NextId("PHONG", "MaPhong", "PH");
                string sql = "INSERT INTO PHONG(MaPhong, TenPhong, MaKhoa) VALUES(@ma, @ten, @khoa)";
                DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
                Reset();
                ShowInfo("Đã thêm mới thành công phòng " + ma + ".");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        /// Xử lý nút [Sửa]: Cập nhật tên phòng hoặc chuyển phòng sang khoa khác.
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn phòng cần sửa trong danh sách."); 
                return; 
            }
            if (!Valid()) return;

            try
            {
                string sql = "UPDATE PHONG SET TenPhong=@ten, MaKhoa=@khoa WHERE MaPhong=@ma";
                DatabaseHelper.ExecuteNonQuery(sql, Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật thông tin phòng " + txtMa.Text + " thành công.");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        /// Xử lý nút [Xóa]: Xóa phòng bệnh.
        /// Ràng buộc toàn vẹn: Không cho xóa nếu trong phòng này vẫn còn giường bệnh.
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn phòng cần xóa."); 
                return; 
            }
            if (!Ask("Bạn có chắc chắn muốn xóa phòng " + txtMa.Text + " - " + txtTen.Text + "?")) 
                return;

            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM PHONG WHERE MaPhong=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
                ShowInfo("Đã xóa phòng thành công.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Phòng này hiện đang có giường bệnh bên trong nên không thể xóa.\n" +
                              "Vui lòng vào danh mục 'Giường' xóa các giường thuộc phòng này trước.");
            }
        }
    }
}
