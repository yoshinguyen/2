using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Quản lý bác sĩ (BacSiForm):
    /// Quản lý danh sách bác sĩ điều trị trong bệnh viện (Thêm, Sửa, Xóa, Tìm kiếm).
    /// Bác sĩ được liên kết với một Khoa cụ thể thông qua mã khoa (MaKhoa).
    /// </summary>
    public partial class BacSiForm : BaseForm
    {
        public BacSiForm()
        {
            InitializeComponent(); // Dựng giao diện người dùng
            cmbGt.SelectedIndex = 0; // Mặc định giới tính "Nam"

            // Đổ danh sách Khoa từ CSDL vào ComboBox khoa để người dùng chọn khi thêm/sửa bác sĩ
            var dtKhoa = DatabaseHelper.GetData("SELECT MaKhoa, TenKhoa FROM KHOA ORDER BY MaKhoa");
            FillCombo(cmbKhoa, dtKhoa, "TenKhoa", "MaKhoa");

            // Đổ danh sách Khoa vào ComboBox tìm kiếm (kèm tùy chọn '-- Tất cả --' ở đầu)
            var dtTimKhoa = dtKhoa.Copy();
            var rAll = dtTimKhoa.NewRow();
            rAll["MaKhoa"] = "";
            rAll["TenKhoa"] = "-- Tất cả --";
            dtTimKhoa.Rows.InsertAt(rAll, 0);
            FillCombo(cmbTimKhoa, dtTimKhoa, "TenKhoa", "MaKhoa");

            LoadData(); // Nạp danh sách bác sĩ lên bảng DataGridView

            // Gắn sự kiện tương tác người dùng:
            // - Khi chọn một bác sĩ trên bảng -> Nạp thông tin lên các ô nhập
            dgv.SelectionChanged += (s, e) => FillInputs();

            // Nhấn phím Enter trong các ô tìm kiếm để thực hiện tìm kiếm
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
            txtTimSdt.KeyDown += onEnter;
        }

        /// <summary>
        /// Nút [🔍 Tìm kiếm]: Thực hiện lọc danh sách theo các điều kiện người dùng đã nhập.
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Nút [🔄 Làm mới] ở thanh tìm kiếm: Xóa trắng các ô điều kiện tìm kiếm và nạp lại toàn bộ danh sách.
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimKhoa.Items.Count > 0) cmbTimKhoa.SelectedIndex = 0;
            txtTimSdt.Text = "";
            LoadData();
        }

        /// <summary>
        /// Nút [Làm mới]: Làm trống ô tìm kiếm và ô nhập liệu, đưa form về trạng thái sẵn sàng nhập mới.
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// Tải dữ liệu từ bảng BACSI theo nhiều điều kiện tìm kiếm kết hợp (Mã BS, Họ tên, Khoa, SĐT).
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT b.MaBS AS [Mã BS], b.HoTen AS [Họ tên], b.NgaySinh AS [Ngày sinh], b.GioiTinh AS [Giới tính],
                       b.ChuyenKhoa AS [Chuyên khoa], b.SoDienThoai AS [SĐT], b.Email AS [Email],
                       b.MaKhoa AS [Mã khoa], k.TenKhoa AS [Khoa], b.TrangThai AS [Đang làm việc]
                FROM BACSI b 
                LEFT JOIN KHOA k ON k.MaKhoa = b.MaKhoa
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();

            // 1. Tìm theo Mã BS
            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND b.MaBS LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }

            // 2. Tìm theo Họ và tên
            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND b.HoTen LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }

            // 3. Lọc theo Khoa
            string maKhoa = GetValue(cmbTimKhoa);
            if (!string.IsNullOrEmpty(maKhoa))
            {
                sql += " AND b.MaKhoa = @timKhoa ";
                ps.Add(DatabaseHelper.Param("@timKhoa", maKhoa));
            }

            // 4. Tìm theo Số điện thoại
            string sdt = txtTimSdt.Text.Trim();
            if (!string.IsNullOrEmpty(sdt))
            {
                sql += " AND b.SoDienThoai LIKE @timSdt ";
                ps.Add(DatabaseHelper.Param("@timSdt", "%" + sdt + "%"));
            }

            sql += " ORDER BY b.MaBS";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            FormatColumn(dgv, "Ngày sinh", "dd/MM/yyyy");
        }

        /// <summary>
        /// Đọc thông tin của bác sĩ từ dòng đang chọn và hiển thị lên các ô nhập liệu.
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;

            txtMa.Text = GetText(r, "Mã BS");
            txtTen.Text = GetText(r, "Họ tên");
            if (r.Cells["Ngày sinh"].Value is DateTime) 
                dtpNs.Value = (DateTime)r.Cells["Ngày sinh"].Value;
            cmbGt.Text = GetText(r, "Giới tính");
            txtChuyenKhoa.Text = GetText(r, "Chuyên khoa");
            txtSdt.Text = GetText(r, "SĐT");
            txtEmail.Text = GetText(r, "Email");
            
            // Chọn đúng khoa của bác sĩ trong ComboBox
            if (GetText(r, "Mã khoa") != "") 
                cmbKhoa.SelectedValue = GetText(r, "Mã khoa");

            // Đánh dấu ô CheckBox "Đang làm việc"
            chkLam.Checked = r.Cells["Đang làm việc"].Value is bool && (bool)r.Cells["Đang làm việc"].Value;
        }

        /// <summary>
        /// Xóa trắng các ô nhập liệu.
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtChuyenKhoa.Text = txtSdt.Text = txtEmail.Text = "";
            dtpNs.Value = DateTime.Today;
            cmbGt.SelectedIndex = 0;
            chkLam.Checked = true;
        }

        /// <summary>
        /// Đặt lại form về trạng thái ban đầu: Xóa ô tìm kiếm, nạp lại bảng, xóa ô nhập liệu.
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            if (cmbTimKhoa.Items.Count > 0) cmbTimKhoa.SelectedIndex = 0;
            txtTimSdt.Text = "";
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu trước khi lưu vào CSDL.
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") 
            { 
                ShowWarning("Vui lòng nhập họ tên bác sĩ."); 
                txtTen.Focus(); 
                return false; 
            }
            if (GetValue(cmbKhoa) == null) 
            { 
                ShowWarning("Vui lòng chọn Khoa công tác cho bác sĩ."); 
                return false; 
            }
            return true;
        }

        /// <summary>
        /// Đóng gói các trường nhập liệu thành mảng tham số SQL.
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@ns", dtpNs.Value.Date),
                DatabaseHelper.Param("@gt", cmbGt.Text),
                DatabaseHelper.Param("@ck", txtChuyenKhoa.Text.Trim()),
                DatabaseHelper.Param("@sdt", txtSdt.Text.Trim()),
                DatabaseHelper.Param("@email", txtEmail.Text.Trim()),
                DatabaseHelper.Param("@khoa", GetValue(cmbKhoa)),
                DatabaseHelper.Param("@tt", chkLam.Checked)
            };
        }

        /// <summary>
        /// Xử lý nút [Thêm]: Tự động sinh mã BS (BS01, BS02...) và thêm vào CSDL.
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            if (!Valid()) return;
            try
            {
                string ma = DatabaseHelper.NextId("BACSI", "MaBS", "BS");
                string sql = @"
                    INSERT INTO BACSI(MaBS, HoTen, NgaySinh, GioiTinh, ChuyenKhoa, SoDienThoai, Email, MaKhoa, TrangThai)
                    VALUES(@ma, @ten, @ns, @gt, @ck, @sdt, @email, @khoa, @tt)";

                DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
                Reset();
                ShowInfo("Đã thêm mới thành công bác sĩ " + ma + ".");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        /// Xử lý nút [Sửa]: Cập nhật thông tin bác sĩ đang chọn.
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn bác sĩ cần sửa trong danh sách."); 
                return; 
            }
            if (!Valid()) return;

            try
            {
                string sql = @"
                    UPDATE BACSI 
                    SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, ChuyenKhoa=@ck, SoDienThoai=@sdt,
                        Email=@email, MaKhoa=@khoa, TrangThai=@tt
                    WHERE MaBS=@ma";

                DatabaseHelper.ExecuteNonQuery(sql, Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật thông tin bác sĩ " + txtMa.Text + " thành công.");
            }
            catch (Exception ex) 
            { 
                ShowError(ex); 
            }
        }

        /// <summary>
        /// Xử lý nút [Xóa]: Xóa bác sĩ. 
        /// Nếu bác sĩ này đã từng phụ trách hồ sơ nhập viện -> Không thể xóa (Khóa ngoại),
        /// hướng dẫn người dùng bỏ chọn 'Đang làm việc' thay vì xóa vật lý khỏi CSDL (Soft delete).
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") 
            { 
                ShowWarning("Vui lòng chọn bác sĩ cần xóa."); 
                return; 
            }
            if (!Ask("Bạn có chắc chắn muốn xóa bác sĩ " + txtMa.Text + " - " + txtTen.Text + "?")) 
                return;

            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BACSI WHERE MaBS=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
                ShowInfo("Đã xóa bác sĩ thành công.");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Bác sĩ này đã phụ trách hồ sơ bệnh nhân trong hệ thống nên không thể xóa.\n" +
                              "Gợi ý: Hãy bỏ chọn ô 'Đang làm việc' rồi bấm 'Sửa' để chuyển bác sĩ sang trạng thái ngừng công tác.");
            }
        }
    }
}
