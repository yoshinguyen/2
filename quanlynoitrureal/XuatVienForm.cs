using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// FORM XUẤT VIỆN
    /// -----------------------------------------------------------------------
    /// Quy trình của form này gồm 3 bước, tương ứng với 3 nút bấm:
    ///   Bước 1. [Lưu viện phí]       : tính tiền giường + thuốc + dịch vụ rồi lưu vào bảng VIENPHI.
    ///   Bước 2. [Thanh toán]         : đánh dấu viện phí là "Đã thanh toán".
    ///   Bước 3. [Hoàn tất xuất viện] : chỉ cho phép khi đã thanh toán đủ; sau đó
    ///                                  trả giường về "Trống" và đổi trạng thái bệnh nhân.
    ///
    /// Công thức tính tiền:
    ///   - Số ngày nằm = Ngày ra - Ngày vào (tối thiểu 1 ngày).
    ///   - Tiền giường = Số ngày nằm x Đơn giá giường (nếu giường chưa có giá thì lấy giá phòng).
    ///   - Tiền thuốc  = tổng thành tiền của mọi đơn thuốc trong đợt điều trị.
    ///   - Tiền dịch vụ = tổng (số lượng x đơn giá) trong lịch điều trị.
    ///   - Bảo hiểm trả = 80% tiền giường (nếu có thẻ BHYT) + phần % BH của từng thuốc + từng dịch vụ.
    ///   - Bệnh nhân trả = Tổng tiền - Bảo hiểm trả.
    /// </summary>
    public partial class XuatVienForm : BaseForm
    {
        // =====================================================================
        // 1. HẰNG SỐ VÀ BIẾN DÙNG CHUNG
        // =====================================================================

        // "const" = hằng số, giá trị không bao giờ thay đổi.
        // Đặt tên cho các chuỗi trạng thái để khỏi gõ sai chính tả nhiều lần trong code.
        const string DANG_DIEU_TRI = "Đang điều trị";
        const string CHO_THANH_TOAN = "Chờ thanh toán";
        const string DA_XUAT_VIEN = "Đã xuất viện";
        const string DA_THANH_TOAN = "Đã thanh toán";
        const string CHUA_THANH_TOAN = "Chưa thanh toán";

        // Các "biến thành viên" (field): sống suốt thời gian form mở,
        // nên mọi hàm trong form đều đọc / ghi được.
        // Chúng lưu thông tin của hồ sơ nhập viện đang được chọn trong bảng.
        string maNV = "";        // mã hồ sơ nhập viện (khóa chính bảng NHAPVIEN)
        string maBN = "";        // mã bệnh nhân
        string tenBN = "";       // tên bệnh nhân
        string maGiuong = "";    // mã giường (dùng để trả giường về Trống)
        string tenGiuong = "";
        string tenPhong = "";
        string tenKhoa = "";
        string trangThaiNV = ""; // trạng thái hồ sơ nhập viện
        string trangThaiVP = CHUA_THANH_TOAN; // trạng thái viện phí
        DateTime ngayVao;        // ngày giờ nhập viện
        int soNgayNam = 1;       // số ngày nằm viện

        // Các khoản tiền. "decimal" là kiểu số dành cho tiền (chính xác, không sai số lẻ).
        decimal tienGiuong = 0;
        decimal tienThuoc = 0;
        decimal tienDichVu = 0;
        decimal tienKham = 0;    // hiện chưa dùng nên luôn bằng 0, nhưng bảng VIENPHI có cột này
        decimal tongTien = 0;    // tổng chi phí
        decimal tongBHTra = 0;   // phần bảo hiểm chi trả
        decimal tongBNTra = 0;   // phần bệnh nhân phải trả

        // Cờ (true/false) báo "đang nạp dữ liệu lên màn hình".
        // Dùng để các sự kiện không chạy lặp lại không cần thiết.
        bool dangNap = false;

        // =====================================================================
        // 2. KHỞI TẠO FORM
        // =====================================================================

        /// <summary>
        /// Hàm khởi tạo (constructor): chạy 1 lần khi form được mở.
        /// </summary>
        public XuatVienForm()
        {
            InitializeComponent();           // dựng giao diện (code nằm trong file Designer)
            dtpNgayRa.Value = DateTime.Now;  // ngày ra viện mặc định là hôm nay
            LoadDanhSach();                  // nạp danh sách hồ sơ lên bảng

            // Gắn sự kiện bằng "lambda" (s, e) => ...
            // Hiểu đơn giản: "khi có chuyện gì xảy ra thì chạy đoạn code bên phải mũi tên".
            dgv.SelectionChanged += (s, e) => ChonDong();                      // đổi dòng chọn
            dtpNgayRa.ValueChanged += (s, e) => { if (!dangNap) TinhChiPhi(); }; // đổi ngày ra

            // Nhấn phím Enter trong ô tìm kiếm thì tìm luôn, khỏi bấm nút.
            KeyEventHandler khiNhanPhim = (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) LoadDanhSach();
            };
            txtTimMa.KeyDown += khiNhanPhim;
            txtTimBN.KeyDown += khiNhanPhim;
        }

        /// <summary>
        /// Designer đang gắn sự kiện này vào giao diện nên phải GIỮ LẠI (nếu xóa sẽ báo lỗi).
        /// Hàm để trống vì không cần làm gì.
        /// </summary>
        private void pnlTopList_Paint(object sender, PaintEventArgs e)
        {
        }

        // =====================================================================
        // 3. NÚT TÌM KIẾM
        // =====================================================================

        /// <summary>Nút [Tìm kiếm]: nạp lại danh sách theo điều kiện đã nhập.</summary>
        void btnTimKiem_Click(object sender, EventArgs e)
        {
            LoadDanhSach();
        }

        /// <summary>Nút [Làm mới]: xóa chữ trong 2 ô tìm kiếm rồi nạp lại toàn bộ danh sách.</summary>
        void btnLamMoiTim_Click(object sender, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimBN.Text = "";
            LoadDanhSach();
        }

        // =====================================================================
        // 4. NẠP DANH SÁCH VÀ CHỌN DÒNG
        // =====================================================================

        /// <summary>
        /// Lấy danh sách hồ sơ nhập viện (kèm thông tin viện phí) từ SQL và hiện lên bảng dgv.
        /// Có lọc theo ô "mã hồ sơ" và ô "tên / mã bệnh nhân" nếu người dùng có nhập.
        /// </summary>
        void LoadDanhSach()
        {
            // Nhớ hồ sơ đang chọn. Sau khi nạp lại bảng, ta chọn lại đúng dòng này.
            // (Nếu không, bảng tự nhảy về dòng đầu tiên và có thể thao tác nhầm sang bệnh nhân khác!)
            string maDangChon = maNV;

            string sql = @"
                SELECT nv.MaNhapVien AS [Mã NV], bn.MaBN AS [Mã BN], bn.HoTen AS [Bệnh nhân],
                       bn.BHYT AS [Số thẻ BHYT], k.TenKhoa AS [Khoa], p.TenPhong AS [Phòng],
                       g.TenGiuong AS [Giường], g.MaGiuong,
                       nv.NgayNhapVien AS [Ngày vào], nv.NgayRaVien AS [Ngày ra],
                       nv.TrangThai AS [Trạng thái NV],
                       ISNULL(vp.TrangThai, N'Chưa lập VP') AS [Trạng thái viện phí],
                       ISNULL(vp.TongTien, 0) AS [Tổng tiền]
                FROM NHAPVIEN nv
                JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
                LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
                LEFT JOIN KHOA k ON k.MaKhoa = p.MaKhoa
                LEFT JOIN VIENPHI vp ON vp.MaNhapVien = nv.MaNhapVien
                WHERE 1=1 ";   // "1=1" luôn đúng, để phía dưới nối thêm " AND ..." cho dễ

            // List<SqlParameter> = danh sách tham số của câu SQL (truyền tham số giúp tránh lỗi bảo mật SQL Injection).
            var ps = new List<SqlParameter>();

            // Nếu ô "mã hồ sơ" có chữ thì nối thêm điều kiện lọc.
            if (!string.IsNullOrWhiteSpace(txtTimMa.Text))
            {
                sql += " AND nv.MaNhapVien LIKE @maNV ";
                ps.Add(DatabaseHelper.Param("@maNV", "%" + txtTimMa.Text.Trim() + "%"));
            }

            // Nếu ô "bệnh nhân" có chữ thì lọc theo tên hoặc mã bệnh nhân.
            if (!string.IsNullOrWhiteSpace(txtTimBN.Text))
            {
                sql += " AND (bn.HoTen LIKE @bn OR bn.MaBN LIKE @bn) ";
                ps.Add(DatabaseHelper.Param("@bn", "%" + txtTimBN.Text.Trim() + "%"));
            }

            // Sắp xếp: đang điều trị lên đầu, rồi chờ thanh toán, rồi các hồ sơ còn lại; mới nhất trước.
            sql += @" ORDER BY CASE WHEN nv.TrangThai = N'Đang điều trị' THEN 1
                                    WHEN nv.TrangThai = N'Chờ thanh toán' THEN 2
                                    ELSE 3 END,
                               nv.NgayNhapVien DESC";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            FormatColumn(dgv, "Ngày vào", "dd/MM/yyyy HH:mm");
            FormatColumn(dgv, "Ngày ra", "dd/MM/yyyy HH:mm");
            FormatColumn(dgv, "Tổng tiền", "N0");

            // Chọn lại đúng dòng lúc nãy (nếu dòng đó vẫn còn trong kết quả).
            if (maDangChon != "")
            {
                foreach (DataGridViewRow dong in dgv.Rows)
                {
                    if (GetText(dong, "Mã NV") == maDangChon)
                    {
                        dgv.CurrentCell = dong.Cells[0];
                        break;   // tìm thấy rồi thì thoát vòng lặp
                    }
                }
            }
        }

        /// <summary>
        /// Chạy mỗi khi người dùng bấm chọn một dòng trong bảng:
        /// lấy thông tin của dòng đó đưa vào các ô bên trên, rồi tính lại chi phí.
        /// </summary>
        void ChonDong()
        {
            var r = dgv.CurrentRow;      // dòng đang chọn
            if (r == null) return;       // chưa chọn dòng nào thì thôi

            dangNap = true;              // bật cờ: đang nạp, đừng tính toán lặp

            // GetText(dòng, "tên cột") = lấy chữ trong một ô của dòng.
            maNV = GetText(r, "Mã NV");
            maBN = GetText(r, "Mã BN");
            tenBN = GetText(r, "Bệnh nhân");
            maGiuong = GetText(r, "MaGiuong");
            tenGiuong = GetText(r, "Giường");
            tenPhong = GetText(r, "Phòng");
            tenKhoa = GetText(r, "Khoa");
            trangThaiNV = GetText(r, "Trạng thái NV");

            // Đưa lên các ô trên màn hình.
            txtMaNV.Text = maNV;
            txtTenBN.Text = tenBN + " (" + maBN + ")";
            txtBHYT.Text = GetText(r, "Số thẻ BHYT");
            txtKhoaPhong.Text = tenKhoa + " - " + tenPhong + " (" + tenGiuong + ")";
            lblTrangThaiNV.Text = trangThaiNV;

            // Ngày vào: kiểm tra ô có phải kiểu ngày giờ không rồi mới lấy ("is" = kiểm tra kiểu dữ liệu).
            object giaTriVao = r.Cells["Ngày vào"].Value;
            if (giaTriVao is DateTime) ngayVao = (DateTime)giaTriVao;
            else ngayVao = DateTime.Today;
            txtNgayVao.Text = ngayVao.ToString("dd/MM/yyyy HH:mm");

            // Ngày ra: nếu hồ sơ đã có ngày ra thì dùng, chưa có thì lấy giờ hiện tại.
            object giaTriRa = r.Cells["Ngày ra"].Value;
            if (giaTriRa is DateTime) dtpNgayRa.Value = (DateTime)giaTriRa;
            else dtpNgayRa.Value = DateTime.Now;

            dangNap = false;             // tắt cờ
            TinhChiPhi();
        }

        // =====================================================================
        // 5. TÍNH CHI PHÍ
        // =====================================================================

        /// <summary>
        /// Tính tiền giường, thuốc, dịch vụ, bảo hiểm và hiện kết quả lên màn hình.
        /// </summary>
        void TinhChiPhi()
        {
            if (maNV == "") return;   // chưa chọn hồ sơ thì chưa tính

            // --- a) Số ngày nằm viện ---
            DateTime ngayRa = dtpNgayRa.Value;
            if (ngayRa < ngayVao) ngayRa = ngayVao;           // ngày ra không được trước ngày vào
            int soNgayChenh = (ngayRa.Date - ngayVao.Date).Days;
            soNgayNam = Math.Max(1, soNgayChenh);             // Math.Max lấy số lớn hơn => tối thiểu 1 ngày
            txtSoNgayNam.Text = soNgayNam + " ngày";

            // --- b) Tiền giường ---
            tienGiuong = soNgayNam * LayDonGiaGiuong();

            // Có số thẻ BHYT (ô không rỗng) thì bảo hiểm trả 80% tiền giường.
            // "a ? b : c" là phép chọn nhanh: nếu a đúng thì lấy b, ngược lại lấy c.
            bool coBHYT = !string.IsNullOrWhiteSpace(txtBHYT.Text);
            decimal bhGiuong = coBHYT ? tienGiuong * 0.8m : 0;   // chữ "m" báo đây là số kiểu decimal

            // --- c) Tiền thuốc: cộng thành tiền mọi dòng thuốc trong các đơn của hồ sơ này ---
            decimal bhThuoc;
            DocTong(@"SELECT ISNULL(SUM(ct.ThanhTien), 0) AS Tong,
                             ISNULL(SUM(ct.ThanhTien * ISNULL(t.PT_BaoHiem, 0) / 100.0), 0) AS BH
                      FROM CT_DONTHUOC ct
                      JOIN THUOC t ON t.MaThuoc = ct.MaThuoc
                      JOIN DONTHUOC dt ON dt.MaDon = ct.MaDon
                      JOIN BENHAN ba ON ba.MaBA = dt.MaBA
                      WHERE ba.MaNhapVien = @nv",
                    out tienThuoc, out bhThuoc);

            // --- d) Tiền dịch vụ: cộng trong lịch điều trị ---
            decimal bhDichVu;
            DocTong(@"SELECT ISNULL(SUM(ldt.SoLuong * ldt.DonGia), 0) AS Tong,
                             ISNULL(SUM(ldt.SoLuong * ldt.DonGia * ISNULL(ldt.PT_BaoHiem, 0) / 100.0), 0) AS BH
                      FROM LICH_DIEU_TRI ldt
                      WHERE ldt.MaNhapVien = @nv",
                    out tienDichVu, out bhDichVu);

            // --- e) Tổng hợp ---
            tongTien = tienGiuong + tienThuoc + tienDichVu + tienKham;
            tongBHTra = bhGiuong + bhThuoc + bhDichVu;
            tongBNTra = Math.Max(0, tongTien - tongBHTra);   // không để số âm

            // --- f) Hiện lên màn hình (Fmt.Money đổi số thành dạng tiền, ví dụ 1.500.000 đ) ---
            txtTienGiuong.Text = Fmt.Money(tienGiuong);
            txtTienThuoc.Text = Fmt.Money(tienThuoc);
            txtTienDichVu.Text = Fmt.Money(tienDichVu);
            txtTongTien.Text = Fmt.Money(tongTien);
            txtBHTra.Text = Fmt.Money(tongBHTra);
            txtBNTra.Text = Fmt.Money(tongBNTra);

            // --- g) Trạng thái viện phí lấy từ database (chưa có hóa đơn thì coi là chưa thanh toán) ---
            DataTable dtVP = DatabaseHelper.GetData(
                "SELECT TrangThai FROM VIENPHI WHERE MaNhapVien = @nv",
                DatabaseHelper.Param("@nv", maNV));
            if (dtVP.Rows.Count > 0) trangThaiVP = dtVP.Rows[0]["TrangThai"].ToString();
            else trangThaiVP = CHUA_THANH_TOAN;

            lblTrangThaiVP.Text = trangThaiVP;
            lblTrangThaiVP.ForeColor = (trangThaiVP == DA_THANH_TOAN) ? Color.Green : Color.Red;
        }

        /// <summary>
        /// Lấy đơn giá giường / ngày: dùng giá của giường, nếu giường chưa có giá (bằng 0) thì dùng giá của phòng.
        /// </summary>
        decimal LayDonGiaGiuong()
        {
            DataTable dt = DatabaseHelper.GetData(@"
                SELECT CASE WHEN ISNULL(g.DonGia, 0) > 0 THEN g.DonGia ELSE ISNULL(p.DonGia, 0) END AS DonGiaApDung
                FROM NHAPVIEN nv
                LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
                LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
                WHERE nv.MaNhapVien = @nv", DatabaseHelper.Param("@nv", maNV));

            if (dt.Rows.Count > 0 && dt.Rows[0]["DonGiaApDung"] != DBNull.Value)
                return Convert.ToDecimal(dt.Rows[0]["DonGiaApDung"]);
            return 0;
        }

        /// <summary>
        /// Chạy một câu SQL trả về 2 cột: "Tong" (tổng tiền) và "BH" (phần bảo hiểm trả).
        /// Dùng từ khóa "out" để hàm trả về cùng lúc 2 giá trị: tong và bh.
        /// (Biến "out" bắt buộc phải được gán giá trị trước khi hàm kết thúc.)
        /// </summary>
        void DocTong(string sql, out decimal tong, out decimal bh)
        {
            tong = 0;
            bh = 0;
            DataTable dt = DatabaseHelper.GetData(sql, DatabaseHelper.Param("@nv", maNV));
            if (dt.Rows.Count > 0)
            {
                tong = Convert.ToDecimal(dt.Rows[0]["Tong"]);
                bh = Convert.ToDecimal(dt.Rows[0]["BH"]);
            }
        }

        // =====================================================================
        // 6. BƯỚC 1: LƯU VIỆN PHÍ
        // =====================================================================

        /// <summary>
        /// Tính lại chi phí rồi ghi vào bảng VIENPHI (chưa có thì INSERT, có rồi thì UPDATE).
        /// Nếu hồ sơ đang "Đang điều trị" thì chuyển sang "Chờ thanh toán".
        /// Hàm này không hiện thông báo; các nút bấm bên dưới sẽ tự hiện.
        /// </summary>
        void LuuVienPhi()
        {
            TinhChiPhi();   // luôn tính lại cho số liệu mới nhất

            // Kiểm tra hồ sơ này đã có dòng trong bảng VIENPHI chưa.
            bool daCo = DatabaseHelper.Exists("SELECT 1 FROM VIENPHI WHERE MaNhapVien = @nv",
                                              DatabaseHelper.Param("@nv", maNV));
            if (!daCo)
            {
                // Chưa có: thêm mới với mã tự sinh (VP01, VP02...).
                string maVP = DatabaseHelper.NextId("VIENPHI", "MaVP", "VP");
                DatabaseHelper.ExecuteNonQuery(@"
                    INSERT INTO VIENPHI(MaVP, MaNhapVien, NgayLap, SoNgay, TienPhong, TienThuoc, TienKham,
                                        TienDichVu, TongTien, BHTra, BNTra, TrangThai)
                    VALUES(@mvp, @nv, GETDATE(), @sn, @tp, @tt, @tk, @tdv, @tong, @bh, @bn, N'Chưa thanh toán')",
                    DatabaseHelper.Param("@mvp", maVP), DatabaseHelper.Param("@nv", maNV),
                    DatabaseHelper.Param("@sn", soNgayNam), DatabaseHelper.Param("@tp", tienGiuong),
                    DatabaseHelper.Param("@tt", tienThuoc), DatabaseHelper.Param("@tk", tienKham),
                    DatabaseHelper.Param("@tdv", tienDichVu), DatabaseHelper.Param("@tong", tongTien),
                    DatabaseHelper.Param("@bh", tongBHTra), DatabaseHelper.Param("@bn", tongBNTra));
            }
            else
            {
                // Có rồi: cập nhật số liệu mới.
                DatabaseHelper.ExecuteNonQuery(@"
                    UPDATE VIENPHI
                    SET SoNgay = @sn, TienPhong = @tp, TienThuoc = @tt, TienKham = @tk, TienDichVu = @tdv,
                        TongTien = @tong, BHTra = @bh, BNTra = @bn, NgayLap = GETDATE()
                    WHERE MaNhapVien = @nv",
                    DatabaseHelper.Param("@sn", soNgayNam), DatabaseHelper.Param("@tp", tienGiuong),
                    DatabaseHelper.Param("@tt", tienThuoc), DatabaseHelper.Param("@tk", tienKham),
                    DatabaseHelper.Param("@tdv", tienDichVu), DatabaseHelper.Param("@tong", tongTien),
                    DatabaseHelper.Param("@bh", tongBHTra), DatabaseHelper.Param("@bn", tongBNTra),
                    DatabaseHelper.Param("@nv", maNV));
            }

            // Đang điều trị => chuyển sang chờ thanh toán.
            if (trangThaiNV == DANG_DIEU_TRI)
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE NHAPVIEN SET TrangThai = N'Chờ thanh toán' WHERE MaNhapVien = @nv",
                    DatabaseHelper.Param("@nv", maNV));
            }

            LoadDanhSach();   // nạp lại bảng (code bên trong tự chọn lại đúng dòng cũ)
        }

        /// <summary>Nút [Lưu viện phí].</summary>
        void btnLuuVienPhi_Click(object sender, EventArgs e)
        {
            if (maNV == "") { ShowWarning("Hãy chọn hồ sơ nhập viện cần tính viện phí."); return; }
            if (trangThaiVP == DA_THANH_TOAN) { ShowWarning("Viện phí đã thanh toán, không cần lưu lại."); return; }

            // try/catch: thử chạy; nếu có lỗi (mất kết nối, sai dữ liệu...) thì nhảy xuống catch
            // để báo lỗi gọn gàng thay vì làm chương trình sập.
            try
            {
                LuuVienPhi();
                ShowInfo("Đã lưu bảng tổng hợp chi phí thành công.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // =====================================================================
        // 7. BƯỚC 2: THANH TOÁN
        // =====================================================================

        /// <summary>Nút [Thanh toán]: thu tiền viện phí (bắt buộc làm trước khi xuất viện).</summary>
        void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (maNV == "") { ShowWarning("Hãy chọn hồ sơ cần thanh toán."); return; }
            if (trangThaiVP == DA_THANH_TOAN) { ShowInfo("Hồ sơ này đã thanh toán 100% rồi."); return; }

            // Hỏi xác nhận TRƯỚC khi làm bất cứ thay đổi nào. Bấm "No" thì thoát luôn.
            // Số tiền hỏi là số của bảng kê hiện tại; LuuVienPhi() bên dưới sẽ tính lại cho chắc.
            TinhChiPhi();
            if (!Ask("Xác nhận thu tiền viện phí: " + Fmt.Money(tongBNTra) + " từ bệnh nhân " + tenBN + "?"))
                return;

            try
            {
                LuuVienPhi();   // đảm bảo bảng VIENPHI đã có số liệu mới nhất

                DatabaseHelper.ExecuteNonQuery(@"
                    UPDATE VIENPHI
                    SET TrangThai = N'Đã thanh toán', PhuongThucTT = N'Tiền mặt', NgayThanhToan = GETDATE()
                    WHERE MaNhapVien = @nv", DatabaseHelper.Param("@nv", maNV));

                LoadDanhSach();
                TinhChiPhi();
                ShowInfo("Thanh toán thành công! Bệnh nhân đã đủ điều kiện để Hoàn tất xuất viện.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // =====================================================================
        // 8. BƯỚC 3: HOÀN TẤT XUẤT VIỆN
        // =====================================================================

        /// <summary>
        /// Nút [Hoàn tất xuất viện]. Quy tắc BR07: phải thanh toán đủ 100% mới được xuất viện.
        /// Khi xuất viện thành công, 3 việc sau được làm cùng lúc trong một "Transaction":
        ///   1) Hồ sơ nhập viện -> "Đã xuất viện" và lưu ngày ra.
        ///   2) Giường -> "Trống".
        ///   3) Bệnh nhân -> "Đã xuất viện".
        /// Transaction = "được ăn cả, ngã về không": cả 3 việc cùng thành công,
        /// hoặc nếu một việc lỗi thì cả 3 đều bị hủy => dữ liệu không bao giờ bị lệch.
        /// </summary>
        void btnHoanTatXuatVien_Click(object sender, EventArgs e)
        {
            if (maNV == "") { ShowWarning("Hãy chọn hồ sơ cần xuất viện."); return; }
            if (trangThaiNV == DA_XUAT_VIEN) { ShowInfo("Bệnh nhân này đã xuất viện rồi."); return; }

            // Chặn nếu chưa thanh toán đủ.
            if (trangThaiVP != DA_THANH_TOAN)
            {
                ShowWarning("KHÔNG THỂ XUẤT VIỆN!\n\n" +
                            "Bệnh nhân phải thanh toán đủ 100% viện phí trước khi xuất viện.\n" +
                            "Trạng thái hiện tại: [" + trangThaiVP + "]. Hãy bấm [Thanh toán] trước.");
                return;
            }

            // Ngày ra không được sớm hơn ngày vào (tránh lỗi dữ liệu).
            if (dtpNgayRa.Value < ngayVao)
            {
                ShowWarning("Ngày ra viện không được trước ngày nhập viện (" + ngayVao.ToString("dd/MM/yyyy HH:mm") + ").");
                return;
            }

            if (!Ask("Xác nhận HOÀN TẤT XUẤT VIỆN cho bệnh nhân " + tenBN + "?\n" +
                     "Giường " + tenGiuong + " sẽ được trả về trạng thái Trống."))
                return;

            try
            {
                DatabaseHelper.ExecuteTransaction(
                    // Việc 1: cập nhật hồ sơ nhập viện
                    new SqlStatement(
                        "UPDATE NHAPVIEN SET TrangThai = N'Đã xuất viện', NgayRaVien = @ra WHERE MaNhapVien = @nv",
                        DatabaseHelper.Param("@ra", dtpNgayRa.Value), DatabaseHelper.Param("@nv", maNV)),

                    // Việc 2: trả giường về Trống
                    new SqlStatement(
                        "UPDATE GIUONG SET TrangThai = N'Trống' WHERE MaGiuong = @g",
                        DatabaseHelper.Param("@g", maGiuong)),

                    // Việc 3: cập nhật trạng thái bệnh nhân
                    new SqlStatement(
                        "UPDATE BENHNHAN SET TrangThai = N'Đã xuất viện' WHERE MaBN = @bn",
                        DatabaseHelper.Param("@bn", maBN)));

                LoadDanhSach();
                ShowInfo("ĐÃ HOÀN TẤT XUẤT VIỆN!\n" +
                         "- Hồ sơ nhập viện: Đã xuất viện\n" +
                         "- Giường: đã trả về Trống\n" +
                         "- Bệnh nhân: Đã xuất viện");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        // =====================================================================
        // 9. XUẤT PHIẾU RA EXCEL VÀ NHẬP TỪ EXCEL
        // =====================================================================

        /// <summary>
        /// Nút [In phiếu]: tạo "Giấy xuất viện + bảng chi phí" rồi lưu ra file Excel (.xlsx).
        /// Cách làm: ghép từng dòng chữ vào một danh sách (List), mỗi phần tử là 1 dòng của phiếu,
        /// sau đó giao cho Export.WriteTextXlsx ghi ra file.
        /// </summary>
        void btnInPhieu_Click(object sender, EventArgs e)
        {
            if (maNV == "") { ShowWarning("Hãy chọn hồ sơ nhập viện để xuất phiếu."); return; }

            try
            {
                TinhChiPhi();   // lấy số liệu mới nhất

                var L = new List<string>();   // mỗi L.Add(...) = thêm 1 dòng vào phiếu
                L.Add("============================================================");
                L.Add("            BỆNH VIỆN ĐA KHOA NỘI TRÚ");
                L.Add("        GIẤY XUẤT VIỆN VÀ BẢNG TỔNG HỢP CHI PHÍ");
                L.Add("============================================================");
                L.Add("");
                L.Add("Mã hồ sơ NV : " + maNV);
                L.Add("Bệnh nhân   : " + tenBN + " (Mã: " + maBN + ")");
                L.Add("Số thẻ BHYT : " + (string.IsNullOrEmpty(txtBHYT.Text) ? "Không có" : txtBHYT.Text));
                L.Add("Khoa phòng  : " + tenKhoa + " - " + tenPhong + " (Giường: " + tenGiuong + ")");
                L.Add("Ngày vào    : " + ngayVao.ToString("dd/MM/yyyy HH:mm"));
                L.Add("Ngày ra     : " + dtpNgayRa.Value.ToString("dd/MM/yyyy HH:mm"));
                L.Add("Số ngày nằm : " + soNgayNam + " ngày");
                L.Add(new string('-', 60));   // new string('-', 60) = chuỗi gồm 60 dấu gạch ngang
                L.Add("CHI TIẾT CÁC KHOẢN CHI PHÍ:");
                L.Add("1. Tiền giường/phòng : " + Fmt.Money(tienGiuong));
                L.Add("2. Tiền thuốc        : " + Fmt.Money(tienThuoc));
                L.Add("3. Tiền dịch vụ y tế : " + Fmt.Money(tienDichVu));
                L.Add(new string('-', 60));
                L.Add("TỔNG CHI PHÍ        : " + Fmt.Money(tongTien));
                L.Add("BẢO HIỂM CHI TRẢ    : " + Fmt.Money(tongBHTra));
                L.Add("BỆNH NHÂN TỰ TRẢ    : " + Fmt.Money(tongBNTra));
                L.Add(new string('-', 60));
                L.Add("Trạng thái viện phí : " + trangThaiVP);
                L.Add("Trạng thái ra viện  : " + trangThaiNV);
                L.Add("");
                L.Add("   Người nộp tiền                         Bác sĩ / Kế toán");
                L.Add(" (ký, ghi rõ họ tên)                     (ký, ghi rõ họ tên)");

                // "using" tự đóng và giải phóng hộp thoại khi dùng xong.
                using (var dlg = new SaveFileDialog())
                {
                    dlg.Filter = "Excel (*.xlsx)|*.xlsx";
                    dlg.FileName = "PhieuXuatVien_" + maNV + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
                    dlg.Title = "Chọn nơi lưu phiếu xuất viện Excel";

                    if (dlg.ShowDialog() != DialogResult.OK) return;   // người dùng bấm Cancel thì thôi

                    Export.WriteTextXlsx(dlg.FileName, L, "PhieuXuatVien");
                    ShowInfo("Đã xuất phiếu ra file Excel:\n" + dlg.FileName);
                    Export.TryOpen(dlg.FileName);   // mở file vừa lưu
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút [Nhập Excel]: chọn một file .xlsx từ máy tính và hiển thị nội dung lên bảng dgv.
        /// Export.ImportExcelDialog() mở hộp thoại chọn file, đọc file và trả về DataTable (bảng dữ liệu);
        /// nếu người dùng bấm Cancel thì trả về null (không có gì).
        /// </summary>
        void btnNhapExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = Export.ImportExcelDialog();
                if (dt != null)
                {
                    dgv.DataSource = dt;
                    ShowInfo("Đã nhập thành công " + dt.Rows.Count + " dòng từ file Excel!");
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }
    }
}