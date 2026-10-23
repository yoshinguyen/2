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
    /// Form quản lý Hồ sơ Nhập viện:
    /// - Tiếp nhận bệnh nhân vào điều trị nội trú.
    /// - Phân công: Chọn Khoa -> Chọn Phòng -> Chọn Giường bệnh còn Trống + Chọn Bác sĩ phụ trách.
    /// - Khi tạo hồ sơ: Tự động chuyển trạng thái giường sang 'Đang sử dụng'.
    /// - Khi chuyển giường: Tự động trả giường cũ về 'Trống' và gán giường mới sang 'Đang sử dụng'.
    /// - Khi hủy hồ sơ hoặc xuất viện: Trả giường về 'Trống'.
    /// 
    /// ====================================================================================================
    /// 📚 HƯỚNG DẪN DÀNH CHO NGƯỜI MỚI HỌC C#: CƠ CHẾ LIÊN KẾT DỮ LIỆU ĐA BẢNG VÀ TRANSACTION
    /// ====================================================================================================
    /// 1. CƠ CHẾ COMBOBOX PHÂN CẤP (Cascading Dropdown: Khoa -> Phòng -> Giường):
    ///    - Khi người dùng chọn một Khoa ở 'cmbKhoa':
    ///      -> Sự kiện 'cmbKhoa.SelectedIndexChanged' được kích hoạt.
    ///      -> Hàm 'LoadPhongBS()' chạy: Lấy mã khoa vừa chọn bằng 'GetValue(cmbKhoa)'.
    ///      -> Dùng câu lệnh SQL: SELECT ... FROM PHONG WHERE MaKhoa = @k để chỉ lấy các phòng thuộc khoa đó.
    ///      -> Đổ danh sách phòng vào 'cmbPhong' qua hàm 'FillCombo()'.
    ///    - Khi 'cmbPhong' có dữ liệu mới -> Sự kiện 'cmbPhong.SelectedIndexChanged' tiếp tục kích hoạt:
    ///      -> Hàm 'LoadGiuong()' chạy: Lấy mã phòng 'MaPhong'.
    ///      -> Dùng câu lệnh SQL: SELECT ... FROM GIUONG WHERE MaPhong = @p AND TrangThai = N'Trống'
    ///         để chỉ hiển thị các giường bệnh thực sự còn trống trong phòng đó.
    ///    - TẠI SAO CẦN BIẾN 'bool loading'?:
    ///      Khi nạp dữ liệu bằng code, mỗi lần gán DataSource thì ComboBox sẽ tự động kích hoạt sự kiện
    ///      SelectedIndexChanged. Nếu không có biến cờ 'loading' để chặn lại (nếu loading = true thì bỏ qua),
    ///      chương trình sẽ bị gọi đệ quy lặp vô tận hoặc truy vấn CSDL liên tục làm chậm giao diện.
    /// 
    /// 2. CƠ CHẾ CÂU LỆNH SQL JOIN ĐA BẢNG ĐỂ HIỂN THỊ DỮ LIỆU:
    ///    - Trong CSDL, bảng NHAPVIEN chỉ lưu các mã: MaBN, MaBS, MaGiuong.
    ///    - Nếu chỉ hiển thị mã ra bảng thì người dùng không thể biết đó là bệnh nhân hay bác sĩ nào.
    ///    - Do đó, trong hàm 'LoadData()', ta dùng kỹ thuật JOIN:
    ///         FROM NHAPVIEN nv
    ///         JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN         --> Để lấy tên bệnh nhân (bn.HoTen)
    ///         LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS       --> Để lấy tên bác sĩ phụ trách (bs.HoTen)
    ///         LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong --> Để lấy giường bệnh
    ///         LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong    --> Để lấy tên phòng và mã khoa
    ///    - 'LEFT JOIN' được sử dụng vì phòng trường hợp một số trường có thể chưa kịp gán (NULL)
    ///      thì dòng nhập viện đó vẫn được hiển thị chứ không bị biến mất như 'INNER JOIN'.
    /// 
    /// 3. CƠ CHẾ GIAO DỊCH (TRANSACTION) ĐẢM BẢO TÍNH ĐỒNG BỘ GIỮA CÁC BẢNG:
    ///    - Nghiệp vụ Tạo hồ sơ nhập viện:
    ///      Lệnh 1: INSERT INTO NHAPVIEN(...)  -> Lưu hồ sơ vào bảng NHAPVIEN.
    ///      Lệnh 2: UPDATE GIUONG SET TrangThai = N'Đang sử dụng' -> Khóa giường lại, không cho người khác chọn.
    ///      Hai lệnh này BẮT BUỘC phải cùng thành công hoặc cùng thất bại thông qua 'DatabaseHelper.ExecuteTransaction'.
    ///      Nếu không dùng Transaction, giả sử lệnh 1 thành công mà lệnh 2 bị lỗi mất mạng thì bệnh nhân
    ///      đã nhập viện nhưng giường vẫn báo 'Trống', dẫn tới hai bệnh nhân bị xếp trùng một giường!
    /// 
    ///    - Nghiệp vụ Chuyển giường (Bed Transfer):
    ///      Nếu bệnh nhân đổi từ Giường cũ (editingBed) sang Giường mới (g):
    ///      Transaction thực thi đồng thời:
    ///      Lệnh 1: UPDATE NHAPVIEN gán MaGiuong = g mới.
    ///      Lệnh 2: UPDATE GIUONG trả Giường cũ về TrangThai = N'Trống'.
    ///      Lệnh 3: UPDATE GIUONG đổi Giường mới sang TrangThai = N'Đang sử dụng'.
    /// ====================================================================================================
    /// </summary>
    public partial class NhapVienForm : BaseForm
    {
        bool loading;                // true = đang nạp dữ liệu, tạm thời bỏ qua các sự kiện đổi lựa chọn của ComboBox
        string editingBed = "";      // Mã giường hiện tại của hồ sơ đang chọn
        string editingStatus = "";   // Trạng thái hiện tại của hồ sơ đang chọn ('Đang điều trị', 'Đã xuất viện', 'Hủy')

        /// <summary>
        /// Khởi tạo giao diện form Nhập viện, nạp danh mục bệnh nhân, khoa phòng, bác sĩ và gán sự kiện.
        /// </summary>
        public NhapVienForm()
        {
            InitializeComponent();   // dựng giao diện (code nằm trong NhapVienForm.Designer.cs)
            cmbTimTT.SelectedIndex = 0; // Mặc định hiển thị "-- Tất cả --"

            // Nạp danh mục Bệnh nhân và Khoa vào ComboBox
            FillCombo(cmbBN, DatabaseHelper.GetData("SELECT MaBN, MaBN + N' - ' + HoTen AS Hien FROM BENHNHAN ORDER BY MaBN"), "Hien", "MaBN");
            FillCombo(cmbKhoa, DatabaseHelper.GetData("SELECT MaKhoa, TenKhoa FROM KHOA ORDER BY MaKhoa"), "TenKhoa", "MaKhoa");
            LoadPhongBS();
            LoadData();

            // Lắng nghe sự kiện người dùng tương tác
            cmbKhoa.SelectedIndexChanged += (s, e) => { if (!loading) LoadPhongBS(); };
            cmbPhong.SelectedIndexChanged += (s, e) => { if (!loading) LoadGiuong(); };
            dgv.SelectionChanged += (s, e) => SelectRow();

            // Nhấn Enter trong các ô tìm kiếm để thực hiện tìm kiếm
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimBN.KeyDown += onEnter;
        }

        /// <summary>
        /// Nút [🔍 Tìm kiếm]: Thực hiện lọc danh sách hồ sơ nhập viện theo các tiêu chí đã nhập.
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Nút [🔄 Làm mới] ở thanh tìm kiếm: Xóa các điều kiện tìm kiếm và nạp lại toàn bộ danh sách.
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimBN.Text = "";
            cmbTimTT.SelectedIndex = 0;
            LoadData();
        }

        /// <summary>
        /// Nút Làm mới: xóa trắng dữ liệu nhập liệu và nạp lại danh sách.
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e)
        {
            ResetForm();
        }

        // ====================================================================
        // NẠP DỮ LIỆU
        // ====================================================================

        /// <summary>
        /// Tải danh sách phòng và bác sĩ thuộc Khoa đang được chọn.
        /// Sau đó tự động tải danh sách giường của phòng đầu tiên.
        /// </summary>
        void LoadPhongBS()
        {
            string k = GetValue(cmbKhoa);
            bool old = loading; loading = true;
            FillCombo(cmbPhong,
                DatabaseHelper.GetData(@"SELECT MaPhong, TenPhong + N' - ' + CAST(CAST(DonGia AS INT) AS NVARCHAR(20)) + N'đ/ngày' AS Hien
                           FROM PHONG WHERE MaKhoa = @k AND TrangThai = 1 ORDER BY MaPhong", DatabaseHelper.Param("@k", k)),
                "Hien", "MaPhong");
            FillCombo(cmbBS,
                DatabaseHelper.GetData(@"SELECT MaBS, MaBS + N' - ' + HoTen AS Hien
                           FROM BACSI WHERE MaKhoa = @k AND TrangThai = 1 ORDER BY MaBS", DatabaseHelper.Param("@k", k)),
                "Hien", "MaBS");
            loading = old;
            LoadGiuong();
        }

        /// <summary>
        /// Tải danh sách giường của Phòng đang chọn:
        /// Chỉ hiển thị các giường có trạng thái 'Trống' hoặc chính là giường mà ca nhập viện hiện tại đang nằm.
        /// </summary>
        void LoadGiuong()
        {
            string ph = GetValue(cmbPhong);
            bool old = loading; loading = true;
            FillCombo(cmbGiuong,
                DatabaseHelper.GetData(@"SELECT MaGiuong, N'Giường ' + CAST(SoGiuong AS NVARCHAR(5)) + N' (' + MaGiuong + N')' AS Hien
                           FROM GIUONG
                           WHERE MaPhong = @p AND (TrangThai = N'Trống' OR MaGiuong = @cur)
                           ORDER BY SoGiuong", DatabaseHelper.Param("@p", ph), DatabaseHelper.Param("@cur", editingBed)),
                "Hien", "MaGiuong");
            loading = old;
        }

        /// <summary>
        /// Tải toàn bộ danh sách hồ sơ nhập viện lên DataGridView theo nhiều điều kiện kết hợp:
        /// Mã nhập viện, Bệnh nhân (Mã BN hoặc Tên BN), và Trạng thái.
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT nv.MaNhapVien AS [Mã NV], nv.MaBN AS [Mã BN], bn.HoTen AS [Bệnh nhân],
                       nv.MaBS AS [Mã BS], bs.HoTen AS [Bác sĩ],
                       p.MaKhoa AS [Mã khoa], p.MaPhong AS [Mã phòng], p.TenPhong AS [Phòng], nv.MaGiuong AS [Mã giường],
                       nv.NgayNhapVien AS [Ngày vào viện], nv.NgayRaVien AS [Ngày ra viện],
                       nv.LyDoNhapVien AS [Lý do], nv.TrangThai AS [Trạng thái], nv.GhiChu AS [Ghi chú]
                FROM NHAPVIEN nv
                JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS
                LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
                LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(txtTimMa.Text))
            {
                sql += " AND nv.MaNhapVien LIKE @maNV ";
                ps.Add(DatabaseHelper.Param("@maNV", "%" + txtTimMa.Text.Trim() + "%"));
            }

            if (!string.IsNullOrWhiteSpace(txtTimBN.Text))
            {
                sql += " AND (nv.MaBN LIKE @bn OR bn.HoTen LIKE @bn) ";
                ps.Add(DatabaseHelper.Param("@bn", "%" + txtTimBN.Text.Trim() + "%"));
            }

            if (cmbTimTT.SelectedIndex > 0)
            {
                sql += " AND nv.TrangThai = @tt ";
                ps.Add(DatabaseHelper.Param("@tt", cmbTimTT.Text));
            }

            sql += " ORDER BY nv.NgayNhapVien DESC ";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            FormatColumn(dgv, "Ngày vào viện", "dd/MM/yyyy HH:mm");
            FormatColumn(dgv, "Ngày ra viện", "dd/MM/yyyy HH:mm");
            // Ẩn bớt các cột mã phục vụ liên kết nội bộ
            foreach (var c in new[] { "Mã khoa", "Mã phòng" })
                if (dgv.Columns.Contains(c)) dgv.Columns[c].Visible = false;
        }

        // ====================================================================
        // CHỌN DÒNG / LÀM MỚI FORM
        // ====================================================================

        /// <summary>
        /// Khi người dùng chọn một dòng trên lưới, nạp toàn bộ thông tin chi tiết của hồ sơ nhập viện lên các điều khiển giao diện.
        /// </summary>
        void SelectRow()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;

            loading = true;
            editingBed = GetText(r, "Mã giường");
            editingStatus = GetText(r, "Trạng thái");
            txtMa.Text = GetText(r, "Mã NV");
            cmbBN.SelectedValue = GetText(r, "Mã BN");
            if (GetText(r, "Mã khoa") != "") cmbKhoa.SelectedValue = GetText(r, "Mã khoa");
            LoadPhongBS();
            if (GetText(r, "Mã phòng") != "") cmbPhong.SelectedValue = GetText(r, "Mã phòng");
            LoadGiuong();
            if (editingBed != "") cmbGiuong.SelectedValue = editingBed;
            if (GetText(r, "Mã BS") != "") cmbBS.SelectedValue = GetText(r, "Mã BS");

            if (r.Cells["Ngày vào viện"].Value is DateTime) dtpVao.Value = (DateTime)r.Cells["Ngày vào viện"].Value;
            if (r.Cells["Ngày ra viện"].Value is DateTime) dtpRa.Value = (DateTime)r.Cells["Ngày ra viện"].Value;
            else dtpRa.Value = DateTime.Now;
            txtLyDo.Text = GetText(r, "Lý do");
            txtGhiChu.Text = GetText(r, "Ghi chú");
            lblTT.Text = editingStatus;
            loading = false;
        }

        /// <summary>
        /// Xóa sạch thông tin trên form nhập liệu để chuẩn bị tiếp nhận bệnh nhân mới.
        /// </summary>
        void ResetForm()
        {
            txtTimMa.Text = "";
            txtTimBN.Text = "";
            cmbTimTT.SelectedIndex = 0;
            LoadData();
            dgv.CurrentCell = null;
            editingBed = "";
            editingStatus = "";
            txtMa.Text = "";
            txtLyDo.Text = "";
            txtGhiChu.Text = "";
            dtpVao.Value = DateTime.Now;
            dtpRa.Value = DateTime.Now;
            lblTT.Text = "Mới";
            LoadPhongBS();
        }

        // ====================================================================
        // NGHIỆP VỤ NHẬP VIỆN
        // ====================================================================

        /// <summary>
        /// Tiếp nhận bệnh nhân - Tạo hồ sơ nhập viện mới:
        /// 1. Kiểm tra bệnh nhân có đang điều trị ca nào chưa ra viện không (mỗi bệnh nhân chỉ có 1 ca điều trị tại một thời điểm).
        /// 2. Kiểm tra giường được chọn có thực sự còn Trống không.
        /// 3. Sinh mã nhập viện tự động (NV...).
        /// 4. Thực thi Transaction: INSERT hồ sơ nhập viện và UPDATE giường thành 'Đang sử dụng'.
        /// </summary>
        void btnTaoHoSo_Click(object s, EventArgs e)
        {
            string bn = GetValue(cmbBN), g = GetValue(cmbGiuong), bs = GetValue(cmbBS);
            if (bn == null) { ShowWarning("Hãy chọn bệnh nhân."); return; }
            if (g == null) { ShowWarning("Hãy chọn giường (phòng này có thể đã hết giường trống)."); return; }
            if (bs == null) { ShowWarning("Hãy chọn bác sĩ phụ trách."); return; }

            try
            {
                if (Convert.ToInt32(DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM NHAPVIEN WHERE MaBN=@bn AND TrangThai=N'Đang điều trị'", DatabaseHelper.Param("@bn", bn))) > 0)
                { ShowWarning("Bệnh nhân này đang điều trị nội trú, không thể tạo thêm hồ sơ nhập viện."); return; }

                string st = Convert.ToString(DatabaseHelper.ExecuteScalar("SELECT TrangThai FROM GIUONG WHERE MaGiuong=@g", DatabaseHelper.Param("@g", g)));
                if (st != "Trống") { ShowWarning("Giường này không còn trống."); return; }

                string ma = DatabaseHelper.NextId("NHAPVIEN", "MaNhapVien", "NV");
                DatabaseHelper.ExecuteTransaction(
                    new SqlStatement(@"INSERT INTO NHAPVIEN(MaNhapVien, MaBN, MaBS, MaGiuong, NgayNhapVien, LyDoNhapVien, TrangThai, GhiChu)
                               VALUES(@ma, @bn, @bs, @g, @vao, @ly, N'Đang điều trị', @gc)",
                        DatabaseHelper.Param("@ma", ma), DatabaseHelper.Param("@bn", bn), DatabaseHelper.Param("@bs", bs), DatabaseHelper.Param("@g", g),
                        DatabaseHelper.Param("@vao", dtpVao.Value), DatabaseHelper.Param("@ly", txtLyDo.Text.Trim()), DatabaseHelper.Param("@gc", txtGhiChu.Text.Trim())),
                    new SqlStatement("UPDATE GIUONG SET TrangThai = N'Đang sử dụng' WHERE MaGiuong = @g", DatabaseHelper.Param("@g", g)));

                ResetForm();
                ShowInfo("Đã tạo hồ sơ nhập viện " + ma + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Cập nhật thông tin hồ sơ nhập viện:
        /// Cho phép đổi bác sĩ, lý do, ngày vào, ghi chú và đặc biệt là chuyển giường (tự động giải phóng giường cũ về 'Trống' và chiếm giường mới).
        /// </summary>
        void btnCapNhat_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn hồ sơ cần cập nhật trong danh sách."); return; }
            if (editingStatus != "Đang điều trị") { ShowWarning("Chỉ cập nhật được hồ sơ đang điều trị."); return; }
            string g = GetValue(cmbGiuong), bs = GetValue(cmbBS);
            if (g == null || bs == null) { ShowWarning("Hãy chọn đủ giường và bác sĩ phụ trách."); return; }

            try
            {
                var list = new List<SqlStatement>();
                list.Add(new SqlStatement(
                    @"UPDATE NHAPVIEN SET MaBS=@bs, MaGiuong=@g, NgayNhapVien=@vao, LyDoNhapVien=@ly, GhiChu=@gc
                      WHERE MaNhapVien=@ma",
                    DatabaseHelper.Param("@bs", bs), DatabaseHelper.Param("@g", g), DatabaseHelper.Param("@vao", dtpVao.Value),
                    DatabaseHelper.Param("@ly", txtLyDo.Text.Trim()), DatabaseHelper.Param("@gc", txtGhiChu.Text.Trim()), DatabaseHelper.Param("@ma", txtMa.Text)));

                if (g != editingBed)   // Xử lý chuyển giường: trả giường cũ về Trống, chuyển giường mới sang Đang sử dụng
                {
                    if (editingBed != "")
                        list.Add(new SqlStatement("UPDATE GIUONG SET TrangThai = N'Trống' WHERE MaGiuong = @o", DatabaseHelper.Param("@o", editingBed)));
                    list.Add(new SqlStatement("UPDATE GIUONG SET TrangThai = N'Đang sử dụng' WHERE MaGiuong = @n", DatabaseHelper.Param("@n", g)));
                }
                DatabaseHelper.ExecuteTransaction(list.ToArray());
                string ma = txtMa.Text;
                ResetForm();
                ShowInfo("Đã cập nhật hồ sơ " + ma + " (bệnh nhân không đổi được; muốn đổi hãy hủy hồ sơ và tạo mới).");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Điều hướng sang giao diện Xuất viện chuyên biệt (tuân thủ quy tắc BR07: bắt buộc thanh toán 100% trước khi ra viện).
        /// </summary>
        void btnRaVien_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn hồ sơ cần cho ra viện."); return; }
            
            // Theo PRD BR07: Xuất viện phải qua chức năng Xuất viện riêng để tổng hợp chi phí và thanh toán 100%
            var frm = new XuatVienForm();
            frm.ShowDialog();
            ResetForm();
        }

        /// <summary>
        /// Hủy hồ sơ nhập viện (khi nhập nhầm ca điều trị):
        /// Cập nhật trạng thái hồ sơ sang 'Hủy' và tự động trả giường về 'Trống'.
        /// </summary>
        void btnHuy_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn hồ sơ cần hủy."); return; }
            if (editingStatus != "Đang điều trị") { ShowWarning("Chỉ hủy được hồ sơ đang điều trị."); return; }
            if (!Ask("Hủy hồ sơ nhập viện " + txtMa.Text + "?")) return;

            try
            {
                DatabaseHelper.ExecuteTransaction(
                    new SqlStatement("UPDATE NHAPVIEN SET TrangThai=N'Hủy' WHERE MaNhapVien=@ma", DatabaseHelper.Param("@ma", txtMa.Text)),
                    new SqlStatement("UPDATE GIUONG SET TrangThai=N'Trống' WHERE MaGiuong=@g", DatabaseHelper.Param("@g", editingBed)));
                ResetForm();
            }
            catch (Exception ex) { ShowError(ex); }
        }
    }
}
