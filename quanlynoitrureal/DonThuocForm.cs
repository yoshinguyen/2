using QuanLyNoiTru.dotnet;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// FORM KÊ ĐƠN THUỐC
    /// -----------------------------------------------------------------------
    /// Cách dùng: chọn Bệnh án -> chọn một Đơn thuốc (hoặc bấm [Tạo đơn mới]) -> thêm thuốc vào đơn.
    ///
    /// Dữ liệu chia làm 2 bảng "cha - con" (Master - Detail):
    ///   - DONTHUOC    (bảng cha): thông tin chung của đơn: mã đơn, mã bệnh án, ghi chú...
    ///   - CT_DONTHUOC (bảng con): từng loại thuốc trong đơn: mã thuốc, số lượng, đơn giá, thành tiền.
    ///   Một đơn có thể chứa nhiều dòng thuốc (quan hệ 1 - nhiều).
    ///
    /// Trên màn hình:
    ///   - cmbBA  : danh sách bệnh án.
    ///   - cmbDon : các đơn thuốc của bệnh án đang chọn.
    ///   - dgv    : các thuốc của đơn đang chọn.
    ///
    /// Kho thuốc luôn khớp với đơn nhờ "Transaction"
    /// (chạy nhiều lệnh SQL cùng lúc: hoặc thành công hết, hoặc hủy hết):
    ///   - Thêm thuốc   : thêm dòng vào CT_DONTHUOC  VÀ  trừ tồn kho.
    ///   - Sửa số lượng : tính chênh lệch (số mới - số cũ) rồi trừ / cộng phần chênh vào kho.
    ///   - Xóa thuốc    : xóa dòng khỏi CT_DONTHUOC  VÀ  cộng trả số lượng về kho.
    /// </summary>
    public partial class DonThuocForm : BaseForm
    {
        // Cờ (true/false) báo "đang nạp dữ liệu".
        // Khi ta tự nạp lại danh sách vào ComboBox, ComboBox sẽ tự phát ra sự kiện "đổi lựa chọn".
        // Cờ này giúp bỏ qua những sự kiện đó, tránh chạy lặp không cần thiết.
        bool dangNap = false;

        // =====================================================================
        // 1. KHỞI TẠO FORM
        // =====================================================================

        /// <summary>
        /// Hàm khởi tạo (constructor): chạy 1 lần khi form được mở.
        /// </summary>
        public DonThuocForm()
        {
            InitializeComponent();   // dựng giao diện (code nằm trong file DonThuocForm.Designer.cs)
            LoadThuoc();             // nạp danh sách thuốc vào ô chọn thuốc

            // Nạp danh sách bệnh án vào cmbBA.
            // FillCombo(ô chọn, bảng dữ liệu, "cột hiển thị", "cột giá trị")
            //   - cột hiển thị: chữ người dùng nhìn thấy (ở đây là "BA01 - Tên bệnh nhân - Chẩn đoán")
            //   - cột giá trị : mã dùng để xử lý trong code (ở đây là MaBA)
            FillCombo(cmbBA,
                DatabaseHelper.GetData(@"
                    SELECT ba.MaBA, ba.MaBA + N' - ' + bn.HoTen + N' - ' + ISNULL(ba.ChanDoan, N'') AS Hien
                    FROM BENHAN ba
                    JOIN NHAPVIEN nv ON nv.MaNhapVien = ba.MaNhapVien
                    JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                    ORDER BY ba.NgayLap DESC, ba.MaBA DESC"),
                "Hien", "MaBA");

            // Gắn các sự kiện bằng "lambda" (s, e) => ...
            // Hiểu đơn giản: "khi có chuyện gì xảy ra thì chạy đoạn code bên phải mũi tên".
            cmbBA.SelectedIndexChanged += (s, e) => { if (!dangNap) LoadDon(null); };   // đổi bệnh án -> nạp các đơn của nó
            cmbDon.SelectedIndexChanged += (s, e) => { if (!dangNap) LoadCT(); };       // đổi đơn -> nạp các thuốc của đơn
            cmbThuoc.SelectedIndexChanged += (s, e) => ShowGia();                       // đổi thuốc -> hiện đơn giá
            dgv.SelectionChanged += (s, e) => FillFromGrid();                           // chọn dòng thuốc -> đưa lên ô nhập

            LoadDon(null);   // nạp đơn thuốc của bệnh án đầu tiên
        }

        // =====================================================================
        // 2. NẠP DỮ LIỆU LÊN MÀN HÌNH
        // =====================================================================

        /// <summary>
        /// Nạp các loại thuốc đang kinh doanh (TrangThai = 1) vào cmbThuoc, kèm số tồn kho.
        /// Sau khi nạp lại, chọn lại đúng loại thuốc lúc nãy để người dùng khỏi bị nhảy lựa chọn.
        /// </summary>
        void LoadThuoc()
        {
            string dangChon = GetValue(cmbThuoc);   // nhớ loại thuốc đang chọn

            FillCombo(cmbThuoc,
                DatabaseHelper.GetData(@"
                    SELECT MaThuoc,
                           TenThuoc + N' (tồn: ' + CAST(SoLuongTon AS NVARCHAR(10)) + N')' AS Hien,
                           DonGia
                    FROM THUOC
                    WHERE TrangThai = 1
                    ORDER BY TenThuoc"),
                "Hien", "MaThuoc");

            if (dangChon != null) cmbThuoc.SelectedValue = dangChon;   // chọn lại loại thuốc cũ
            ShowGia();
        }

        /// <summary>
        /// Hiện đơn giá của thuốc đang chọn lên nhãn lblGia.
        /// </summary>
        void ShowGia()
        {
            // cmbThuoc.SelectedItem là "dòng dữ liệu" đang chọn.
            // "as DataRowView" = thử coi nó là một dòng dữ liệu; nếu không phải (ví dụ chưa chọn gì) thì được null.
            var dong = cmbThuoc.SelectedItem as DataRowView;

            if (dong == null) lblGia.Text = "0 đ";
            else lblGia.Text = Fmt.Money(Convert.ToDecimal(dong["DonGia"]));   // Fmt.Money: đổi số thành dạng tiền
        }

        /// <summary>
        /// Nạp các đơn thuốc của bệnh án đang chọn vào cmbDon, rồi nạp luôn các thuốc trong đơn.
        /// </summary>
        /// <param name="maDonCanChon">Mã đơn muốn chọn sẵn sau khi nạp (truyền null = chọn đơn đầu tiên).</param>
        void LoadDon(string maDonCanChon)
        {
            dangNap = true;   // bật cờ: đang nạp, các sự kiện đổi lựa chọn hãy bỏ qua

            FillCombo(cmbDon,
                DatabaseHelper.GetData(@"
                    SELECT MaDon, MaDon + N' (' + CONVERT(NVARCHAR(10), NgayKeDon, 103) + N')' AS Hien
                    FROM DONTHUOC
                    WHERE MaBA = @ba
                    ORDER BY NgayKeDon DESC, MaDon DESC",
                    DatabaseHelper.Param("@ba", GetValue(cmbBA))),   // @ba là "tham số": thay giá trị vào câu SQL an toàn
                "Hien", "MaDon");

            if (maDonCanChon != null) cmbDon.SelectedValue = maDonCanChon;

            dangNap = false;  // tắt cờ
            LoadCT();         // nạp các thuốc của đơn vừa chọn
        }

        /// <summary>
        /// Nạp các thuốc của đơn đang chọn lên bảng dgv và tính tổng tiền cả đơn.
        /// </summary>
        void LoadCT()
        {
            DataTable bang = DatabaseHelper.GetData(@"
                SELECT ct.MaThuoc AS [Mã thuốc], t.TenThuoc AS [Tên thuốc], ct.SoLuong AS [Số lượng],
                       ct.LieuDung AS [Liều dùng], ct.CachDung AS [Cách dùng],
                       ct.DonGia AS [Đơn giá], ct.ThanhTien AS [Thành tiền]
                FROM CT_DONTHUOC ct
                JOIN THUOC t ON t.MaThuoc = ct.MaThuoc
                WHERE ct.MaDon = @d
                ORDER BY t.TenThuoc",
                DatabaseHelper.Param("@d", GetValue(cmbDon)));

            dgv.DataSource = bang;                      // hiện dữ liệu lên bảng
            FormatColumn(dgv, "Đơn giá", "N0");         // "N0" = có dấu phân cách hàng nghìn, không lẻ
            FormatColumn(dgv, "Thành tiền", "N0");

            // Cộng "Thành tiền" của từng dòng để ra tổng tiền đơn.
            decimal tong = 0;
            foreach (DataRow dong in bang.Rows)         // duyệt qua từng dòng của bảng
            {
                tong += Convert.ToDecimal(dong["Thành tiền"]);   // tong = tong + thành tiền của dòng
            }
            lblTong.Text = "Tổng tiền đơn: " + Fmt.Money(tong);
        }

        /// <summary>
        /// Khi bấm chọn một dòng thuốc trong bảng: đưa thông tin của dòng đó lên các ô nhập
        /// (loại thuốc, số lượng, liều dùng, cách dùng) để sẵn sàng bấm [Cập nhật].
        /// </summary>
        void FillFromGrid()
        {
            var dong = dgv.CurrentRow;        // dòng đang chọn
            if (dong == null) return;         // chưa chọn dòng nào thì thôi

            // GetText(dòng, "tên cột") = lấy chữ trong một ô của dòng.
            string maThuoc = GetText(dong, "Mã thuốc");
            if (maThuoc != "") cmbThuoc.SelectedValue = maThuoc;

            // int.TryParse: thử đổi chữ thành số nguyên. Thành công thì số nằm trong biến soLuong
            // (từ khóa "out" giúp hàm trả kết quả ra ngoài qua biến này).
            int soLuong;
            if (int.TryParse(GetText(dong, "Số lượng"), out soLuong) && soLuong >= 1 && soLuong <= nudSL.Maximum)
            {
                nudSL.Value = soLuong;
            }

            txtLieu.Text = GetText(dong, "Liều dùng");
            txtCach.Text = GetText(dong, "Cách dùng");
        }

        /// <summary>
        /// Sau mỗi lần thêm / sửa / xóa thuốc: nạp lại bảng thuốc trong đơn và danh sách thuốc
        /// (để số tồn kho hiển thị trong cmbThuoc được cập nhật).
        /// </summary>
        void LamMoiSauKhiSua()
        {
            LoadCT();
            LoadThuoc();
        }

        /// <summary>
        /// Lấy số lượng tồn kho hiện tại của một loại thuốc từ database.
        /// </summary>
        int TonKho(string maThuoc)
        {
            object kq = DatabaseHelper.ExecuteScalar(
                "SELECT SoLuongTon FROM THUOC WHERE MaThuoc = @t",
                DatabaseHelper.Param("@t", maThuoc));
            return Convert.ToInt32(kq);
        }

        // =====================================================================
        // 3. CÁC NÚT BẤM
        // =====================================================================

        /// <summary>
        /// Nút [Tạo đơn mới]: tạo một đơn thuốc rỗng cho bệnh án đang chọn, mã tự sinh dạng DT01, DT02...
        /// </summary>
        void btnTaoDon_Click(object s, EventArgs e)
        {
            string maBA = GetValue(cmbBA);
            if (maBA == null) { ShowWarning("Hãy chọn bệnh án trước."); return; }

            // try/catch: thử chạy; nếu có lỗi (mất kết nối, dữ liệu sai...) thì nhảy xuống catch
            // để báo lỗi gọn gàng, thay vì làm chương trình bị sập.
            try
            {
                string maDon = DatabaseHelper.NextId("DONTHUOC", "MaDon", "DT");   // sinh mã mới

                DatabaseHelper.ExecuteNonQuery(
                    "INSERT INTO DONTHUOC(MaDon, MaBA, GhiChu) VALUES(@ma, @ba, @gc)",
                    DatabaseHelper.Param("@ma", maDon),
                    DatabaseHelper.Param("@ba", maBA),
                    DatabaseHelper.Param("@gc", txtGhiChuDon.Text.Trim()));   // Trim() bỏ khoảng trắng thừa 2 đầu

                txtGhiChuDon.Text = "";
                LoadDon(maDon);   // nạp lại danh sách đơn và chọn luôn đơn vừa tạo
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút [Thêm thuốc]: kê thêm một loại thuốc vào đơn đang chọn.
        /// Các bước:
        ///   1. Kiểm tra đã chọn đơn và thuốc chưa.
        ///   2. Kiểm tra kho còn đủ thuốc không.
        ///   3. Kiểm tra thuốc này đã có trong đơn chưa (không cho kê trùng 2 dòng).
        ///   4. Transaction gồm 2 lệnh: thêm dòng vào CT_DONTHUOC + trừ tồn kho.
        /// </summary>
        void btnThemThuoc_Click(object s, EventArgs e)
        {
            string maDon = GetValue(cmbDon);
            string maThuoc = GetValue(cmbThuoc);
            if (maDon == null) { ShowWarning("Hãy tạo đơn mới hoặc chọn một đơn thuốc trước."); return; }
            if (maThuoc == null) { ShowWarning("Hãy chọn thuốc."); return; }

            int soLuong = (int)nudSL.Value;   // (int) đổi số từ ô nhập sang số nguyên

            try
            {
                // Bước 2: kiểm tra kho
                int ton = TonKho(maThuoc);
                if (ton < soLuong)
                {
                    ShowWarning("Không đủ thuốc trong kho (còn " + ton + ").");
                    return;   // return = dừng hàm tại đây
                }

                // Bước 3: kiểm tra thuốc đã có trong đơn chưa (đếm số dòng trùng)
                int soDongTrung = Convert.ToInt32(DatabaseHelper.ExecuteScalar(
                    "SELECT COUNT(*) FROM CT_DONTHUOC WHERE MaDon = @d AND MaThuoc = @t",
                    DatabaseHelper.Param("@d", maDon),
                    DatabaseHelper.Param("@t", maThuoc)));
                if (soDongTrung > 0)
                {
                    ShowWarning("Thuốc này đã có trong đơn. Hãy chọn dòng đó rồi bấm Cập nhật.");
                    return;
                }

                // Bước 4: Transaction (2 lệnh phải cùng thành công)
                DatabaseHelper.ExecuteTransaction(
                    // Lệnh 1: thêm thuốc vào đơn. Đơn giá lấy từ bảng THUOC, thành tiền = đơn giá x số lượng.
                    new SqlStatement(@"
                        INSERT INTO CT_DONTHUOC(MaDon, MaThuoc, SoLuong, LieuDung, CachDung, DonGia, ThanhTien)
                        SELECT @d, @t, @sl, @lieu, @cach, DonGia, DonGia * @sl
                        FROM THUOC WHERE MaThuoc = @t",
                        DatabaseHelper.Param("@d", maDon),
                        DatabaseHelper.Param("@t", maThuoc),
                        DatabaseHelper.Param("@sl", soLuong),
                        DatabaseHelper.Param("@lieu", txtLieu.Text.Trim()),
                        DatabaseHelper.Param("@cach", txtCach.Text.Trim())),

                    // Lệnh 2: trừ số lượng thuốc trong kho.
                    new SqlStatement(
                        "UPDATE THUOC SET SoLuongTon = SoLuongTon - @sl WHERE MaThuoc = @t",
                        DatabaseHelper.Param("@sl", soLuong),
                        DatabaseHelper.Param("@t", maThuoc)));

                LamMoiSauKhiSua();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút [Cập nhật]: sửa số lượng / liều dùng / cách dùng của dòng thuốc đang chọn trong bảng.
        /// Kho được điều chỉnh theo PHẦN CHÊNH LỆCH:
        ///   chenhLech = số lượng mới - số lượng cũ
        ///   - chenhLech dương (kê thêm)  -> kho bị trừ thêm.
        ///   - chenhLech âm   (kê bớt)    -> trừ số âm = cộng trả lại kho.
        /// </summary>
        void btnCapNhat_Click(object s, EventArgs e)
        {
            var dong = dgv.CurrentRow;
            string maDon = GetValue(cmbDon);
            if (dong == null || maDon == null)
            {
                ShowWarning("Hãy chọn một dòng thuốc trong đơn để cập nhật.");
                return;
            }

            string maThuoc = GetText(dong, "Mã thuốc");         // thuốc của dòng đang chọn
            int soLuongCu = int.Parse(GetText(dong, "Số lượng")); // int.Parse: đổi chữ thành số nguyên
            int soLuongMoi = (int)nudSL.Value;
            int chenhLech = soLuongMoi - soLuongCu;

            try
            {
                // Nếu kê thêm mà kho không đủ phần chênh thì dừng.
                if (chenhLech > TonKho(maThuoc))
                {
                    ShowWarning("Không đủ thuốc trong kho để tăng số lượng.");
                    return;
                }

                DatabaseHelper.ExecuteTransaction(
                    // Lệnh 1: sửa dòng thuốc trong đơn (thành tiền tính lại theo số lượng mới).
                    new SqlStatement(@"
                        UPDATE CT_DONTHUOC
                        SET SoLuong = @sl, ThanhTien = DonGia * @sl, LieuDung = @lieu, CachDung = @cach
                        WHERE MaDon = @d AND MaThuoc = @t",
                        DatabaseHelper.Param("@sl", soLuongMoi),
                        DatabaseHelper.Param("@lieu", txtLieu.Text.Trim()),
                        DatabaseHelper.Param("@cach", txtCach.Text.Trim()),
                        DatabaseHelper.Param("@d", maDon),
                        DatabaseHelper.Param("@t", maThuoc)),

                    // Lệnh 2: điều chỉnh kho theo phần chênh lệch.
                    new SqlStatement(
                        "UPDATE THUOC SET SoLuongTon = SoLuongTon - @c WHERE MaThuoc = @t",
                        DatabaseHelper.Param("@c", chenhLech),
                        DatabaseHelper.Param("@t", maThuoc)));

                LamMoiSauKhiSua();
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút [Xóa thuốc]: xóa dòng thuốc đang chọn khỏi đơn và hoàn trả đủ số lượng về kho.
        /// </summary>
        void btnXoaThuoc_Click(object s, EventArgs e)
        {
            var dong = dgv.CurrentRow;
            string maDon = GetValue(cmbDon);
            if (dong == null || maDon == null)
            {
                ShowWarning("Hãy chọn một dòng thuốc trong đơn để xóa.");
                return;
            }

            // Ask(...) hiện hộp thoại Yes/No, trả về true nếu người dùng bấm Yes.
            if (!Ask("Xóa thuốc " + GetText(dong, "Tên thuốc") + " khỏi đơn? (số lượng sẽ được hoàn lại kho)"))
                return;

            string maThuoc = GetText(dong, "Mã thuốc");
            int soLuong = int.Parse(GetText(dong, "Số lượng"));   // số lượng cần hoàn lại kho

            try
            {
                DatabaseHelper.ExecuteTransaction(
                    // Lệnh 1: xóa dòng thuốc khỏi đơn.
                    new SqlStatement(
                        "DELETE FROM CT_DONTHUOC WHERE MaDon = @d AND MaThuoc = @t",
                        DatabaseHelper.Param("@d", maDon),
                        DatabaseHelper.Param("@t", maThuoc)),

                    // Lệnh 2: cộng số lượng trả lại kho.
                    new SqlStatement(
                        "UPDATE THUOC SET SoLuongTon = SoLuongTon + @sl WHERE MaThuoc = @t",
                        DatabaseHelper.Param("@sl", soLuong),
                        DatabaseHelper.Param("@t", maThuoc)));

                LamMoiSauKhiSua();
            }
            catch (Exception ex) { ShowError(ex); }
        }
    }
}