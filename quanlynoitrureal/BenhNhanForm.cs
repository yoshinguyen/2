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
    /// Form Quản lý bệnh nhân (BenhNhanForm): 
    /// 3. THÊM MỚI (Insert):
    ///    - Bước 3.1: Gọi hàm 'Valid()' để kiểm tra người dùng có nhập đủ và đúng định dạng không (VD: Tên không rỗng, CCCD đủ 12 số).
    ///    - Bước 3.2: Tự động sinh mã tiếp theo qua 'DatabaseHelper.NextId("BENHNHAN", "MaBN", "BN")' -> ra "BN01", "BN02"...
    ///    - Bước 3.3: Đóng gói các giá trị từ giao diện thành mảng tham số 'Ps(ma)' để chống hack SQL Injection.
    ///    - Bước 3.4: Gọi 'DatabaseHelper.ExecuteNonQuery(sql, Ps(ma))' để ghi vào CSDL SQL Server.
    ///    - Bước 3.5: Gọi 'Reset()' để tải lại bảng và xóa trắng ô nhập liệu, thông báo thành công.
    /// 
    /// 4. CHỈNH SỬA (Update):
    ///    - Bước 4.1: Kiểm tra ô 'txtMa.Text' xem người dùng đã nhấp chọn bệnh nhân cần sửa trên bảng chưa.
    ///    - Bước 4.2: Kiểm tra tính hợp lệ dữ liệu qua 'Valid()'.
    ///    - Bước 4.3: Gọi câu lệnh SQL: UPDATE BENHNHAN SET HoTen=@ten, ... WHERE MaBN=@ma.
    ///    - Bước 4.4: Gọi 'LoadData()' để làm mới lại lưới với thông tin mới nhất.
    /// 
    /// 5. XÓA (Delete):
    ///    - Bước 5.1: Kiểm tra đã chọn bệnh nhân cần xóa chưa.
    ///    - Bước 5.2: Hiển thị hộp thoại hỏi lại 'Ask("Bạn có chắc chắn muốn xóa...?")' để chống bấm nhầm.
    ///    - Bước 5.3: Thực thi 'DELETE FROM BENHNHAN WHERE MaBN=@ma'.
    ///    - Bước 5.4: Nếu bệnh nhân này đã có dữ liệu ở bảng khác (nhập viện, viện phí...) -> CSDL sẽ chặn lại
    ///      bằng ràng buộc Khóa Ngoại (Foreign Key - Mã lỗi 547). Hàm 'ShowError(ex, "...")' sẽ tự động bắt
    ///      lỗi này và hiện câu nhắc tiếng Việt rõ ràng, ngăn chương trình không bị văng lỗi (crash).
    /// </summary>
    public partial class BenhNhanForm : BaseForm
    {
        public BenhNhanForm()
        {
            InitializeComponent();
            cmbGt.SelectedIndex = 0; // default nam
            cmbTimGt.SelectedIndex = 0; // default"-- Tất cả --"

            // load from db
            LoadData();

            //dgv click
            dgv.SelectionChanged += (s, e) => FillInputs();

            // enter press
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
            txtTimSdt.KeyDown += onEnter;
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
            cmbTimGt.SelectedIndex = 0;
            txtTimSdt.Text = "";
            LoadData();
        }

        /// <summary>
        /// clear all
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e)
        {
            Reset();
        }

        /// <summary>
        /// 1. Dùng mệnh đề khởi tạo "WHERE 1=1" - vì 1=1 luôn ĐÚNG, giúp việc nối thêm các điều kiện "AND ..."
        ///    sau đó trở nên cực kỳ linh hoạt mà không cần phải kiểm tra xem trước đó đã có chữ "WHERE" hay chưa.
        /// 2. Với mỗi thành phần tìm kiếm:
        ///    - Nếu ô Mã BN có nhập: nối thêm " AND MaBN LIKE @timMa " (tìm kiếm tương đối chứa chuỗi).
        ///    - Nếu ô Họ tên có nhập: nối thêm " AND HoTen LIKE @timTen ".
        ///    - Nếu Giới tính khác '-- Tất cả --': nối thêm " AND GioiTinh = @timGt " (lọc chính xác).
        ///    - Nếu ô SĐT có nhập: nối thêm " AND (SoDienThoai LIKE @timSdt OR SDTNguoiThan LIKE @timSdt) ".
        /// 3. Sử dụng List&lt;SqlParameter&gt; ps để truyền tham số, ngăn ngừa triệt để lỗi SQL Injection.
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT MaBN AS [Mã BN], HoTen AS [Họ tên], NgaySinh AS [Ngày sinh], GioiTinh AS [Giới tính],
                       CCCD AS [CCCD], SoDienThoai AS [SĐT], DiaChi AS [Địa chỉ], NgheNghiep AS [Nghề nghiệp],
                       NguoiThan AS [Người thân], SDTNguoiThan AS [SĐT người thân], TinhTrang AS [Tình trạng bệnh]
                FROM BENHNHAN
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();

            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND MaBN LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }

            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND HoTen LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }

            if (cmbTimGt.SelectedIndex > 0 && cmbTimGt.Text != "-- Tất cả --")
            {
                sql += " AND GioiTinh = @timGt ";
                ps.Add(DatabaseHelper.Param("@timGt", cmbTimGt.Text));
            }

            string sdt = txtTimSdt.Text.Trim();
            if (!string.IsNullOrEmpty(sdt))
            {
                sql += " AND (SoDienThoai LIKE @timSdt OR SDTNguoiThan LIKE @timSdt) ";
                ps.Add(DatabaseHelper.Param("@timSdt", "%" + sdt + "%"));
            }

            sql += " ORDER BY MaBN";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            // date format
            FormatColumn(dgv, "Ngày sinh", "dd/MM/yyyy");
        }

        /// <summary>
        ///dgv click function
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;

            txtMa.Text = GetText(r, "Mã BN");
            txtTen.Text = GetText(r, "Họ tên");
            if (r.Cells["Ngày sinh"].Value is DateTime) 
                dtpNs.Value = (DateTime)r.Cells["Ngày sinh"].Value;
            cmbGt.Text = GetText(r, "Giới tính");
            txtCccd.Text = GetText(r, "CCCD");
            txtSdt.Text = GetText(r, "SĐT");
            txtDiaChi.Text = GetText(r, "Địa chỉ");
            txtNghe.Text = GetText(r, "Nghề nghiệp");
            txtNguoiThan.Text = GetText(r, "Người thân");
            txtSdtNT.Text = GetText(r, "SĐT người thân");
            txtTinhTrang.Text = GetText(r, "Tình trạng bệnh");
        }

        /// <summary>
        /// clear
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtCccd.Text = txtSdt.Text = txtDiaChi.Text = "";
            txtNghe.Text = txtNguoiThan.Text = txtSdtNT.Text = txtTinhTrang.Text = "";
            dtpNs.Value = DateTime.Today;
            cmbGt.SelectedIndex = 0;
        }

        /// <summary>
        /// clear all
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            cmbTimGt.SelectedIndex = 0;
            txtTimSdt.Text = "";
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        /// <summary>
        /// Validation
        /// </summary>
        /// <returns>True nếu dữ liệu hợp lệ, False nếu có lỗi</returns>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") 
            { 
                ShowWarning("Vui lòng nhập họ tên bệnh nhân."); 
                txtTen.Focus(); 
                return false; 
            }

            if (dtpNs.Value.Date > DateTime.Today) 
            { 
                ShowWarning("Ngày sinh không thể lớn hơn ngày hiện tại."); 
                return false; 
            }

            string c = txtCccd.Text.Trim();
            // cccd check
            if (c != "" && (c.Length != 12 || !c.All(char.IsDigit))) 
            { 
                ShowWarning("Số CCCD phải gồm đúng 12 chữ số."); 
                txtCccd.Focus(); 
                return false; 
            }

            return true;
        }

        /// <summary>
        /// sql param
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@ns", dtpNs.Value.Date),
                DatabaseHelper.Param("@gt", cmbGt.Text),
                DatabaseHelper.Param("@cccd", txtCccd.Text.Trim()),
                DatabaseHelper.Param("@sdt", txtSdt.Text.Trim()),
                DatabaseHelper.Param("@dc", txtDiaChi.Text.Trim()),
                DatabaseHelper.Param("@nghe", txtNghe.Text.Trim()),
                DatabaseHelper.Param("@nt", txtNguoiThan.Text.Trim()),
                DatabaseHelper.Param("@sdtnt", txtSdtNT.Text.Trim()),
                DatabaseHelper.Param("@tt", txtTinhTrang.Text.Trim())
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
                string ma = DatabaseHelper.NextId("BENHNHAN", "MaBN", "BN");

                string sql = @"
                    INSERT INTO BENHNHAN(MaBN, HoTen, NgaySinh, GioiTinh, CCCD, SoDienThoai, DiaChi, NgheNghiep, NguoiThan, SDTNguoiThan, TinhTrang)
                    VALUES(@ma, @ten, @ns, @gt, @cccd, @sdt, @dc, @nghe, @nt, @sdtnt, @tt)";

                DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
                Reset();
                ShowInfo("Đã thêm mới thành công bệnh nhân " + ma + ".");
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
                ShowWarning("Vui lòng chọn bệnh nhân cần chỉnh sửa trong danh sách phía dưới."); 
                return; 
            }
            if (!Valid()) return;

            try
            {
                string ma = txtMa.Text;
                string sql = @"
                    UPDATE BENHNHAN 
                    SET HoTen=@ten, NgaySinh=@ns, GioiTinh=@gt, CCCD=@cccd, SoDienThoai=@sdt,
                        DiaChi=@dc, NgheNghiep=@nghe, NguoiThan=@nt, SDTNguoiThan=@sdtnt, TinhTrang=@tt
                    WHERE MaBN=@ma";

                DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
                LoadData();
                ShowInfo("Đã cập nhật thông tin bệnh nhân " + ma + " thành công.");
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
                ShowWarning("Vui lòng chọn bệnh nhân cần xóa trong danh sách."); 
                return; 
            }

            /* if (!Ask("Bạn có chắc chắn muốn xóa bệnh nhân " + txtMa.Text + " - " + txtTen.Text + "?")) 
                 return;*/
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa bệnh nhân " + txtMa.Text + " - " + txtTen.Text + "?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question
) != DialogResult.Yes)
                return;
                try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM BENHNHAN WHERE MaBN=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
                ShowInfo("Đã xóa bệnh nhân thành công.");
            }
            catch (Exception ex) 
            { 
                // Nếu vướng khóa ngoại với bảng NHAPVIEN -> Thông báo thay vì văng lỗi kỹ thuật
                ShowError(ex, "Bệnh nhân này đã có hồ sơ nhập viện trong hệ thống nên không thể xóa."); 
            }
        }
    }
}
