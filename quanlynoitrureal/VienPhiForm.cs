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
    /// Form quản lý Viện phí:
    /// - Tính toán chi phí điều trị bao gồm:
    ///   + Tiền phòng/giường = số ngày nằm x đơn giá (tính cả ngày vào và ngày ra, tối thiểu 1 ngày).
    ///   + Tiền thuốc = tổng thành tiền của tất cả các đơn thuốc thuộc hồ sơ bệnh án.
    ///   + Tiền khám và tiền dịch vụ kỹ thuật (có thể điều chỉnh thủ công hoặc nhập bổ sung).
    /// - Cho phép lưu hóa đơn, cập nhật trạng thái 'Đã thanh toán', xuất hóa đơn ra file Excel và nhập dữ liệu từ file Excel.
    /// </summary>
    public partial class VienPhiForm : BaseForm
    {
        // Các biến lưu thông tin tính toán chi phí hiện tại
        decimal tienPhong, tienThuoc;
        int soNgay;
        string maVP = "", trangThai = "Chưa lập";
        string maBN = "", tenBN = "", tenBS = "", tenPhong = "";
        DateTime vao;
        DateTime? ra;
        bool loading; // Cờ kiểm soát tránh trigger sự kiện lặp khi nạp dữ liệu

        /// <summary>
        /// Khởi tạo giao diện form Viện phí, nạp danh sách hồ sơ nhập viện và liên kết các sự kiện UI.
        /// </summary>
        public VienPhiForm()
        {
            InitializeComponent();   // dựng giao diện (code nằm trong VienPhiForm.Designer.cs)

            // Nạp danh sách các hồ sơ nhập viện (loại trừ các ca đã hủy) vào ComboBox
            FillCombo(cmbNV,
                DatabaseHelper.GetData(@"SELECT nv.MaNhapVien,
                                  nv.MaNhapVien + N' - ' + bn.HoTen + N' (' + nv.TrangThai + N')' AS Hien
                           FROM NHAPVIEN nv JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                           WHERE nv.TrangThai <> N'Hủy'
                           ORDER BY nv.NgayNhapVien DESC"),
                "Hien", "MaNhapVien");

            LoadData();

            // Đổi hồ sơ nhập viện -> tính toán lại viện phí và nạp số liệu đã lưu (nếu có)
            cmbNV.SelectedIndexChanged += (s, e) => Tinh(true);

            // Khi người dùng thay đổi giá trị tiền khám hoặc tiền dịch vụ -> cập nhật lại tổng cộng
            nudKham.ValueChanged += (s, e) => UpdateTong();
            nudDV.ValueChanged += (s, e) => UpdateTong();

            // Khi click chọn 1 dòng hóa đơn trên lưới -> đồng bộ chọn hồ sơ nhập viện tương ứng
            dgv.SelectionChanged += (s, e) =>
            {
                var r = dgv.CurrentRow;
                if (r != null && GetText(r, "Mã NV") != "" && GetValue(cmbNV) != GetText(r, "Mã NV"))
                    cmbNV.SelectedValue = GetText(r, "Mã NV");
            };

            Tinh(true);
        }

        /// <summary>
        /// Nút Tính tiền: tính lại theo dữ liệu thực tế hiện tại (không ghi đè số tiền khám/dịch vụ người dùng đang gõ).
        /// </summary>
        void btnTinhTien_Click(object s, EventArgs e)
        {
            Tinh(false);
        }

        /// <summary>
        /// Nút Làm mới: tải lại danh sách hóa đơn và tính lại thông tin cho hồ sơ đang chọn.
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e)
        {
            LoadData();
            Tinh(true);
        }

        /// <summary>
        /// Tải toàn bộ danh sách hóa đơn viện phí từ cơ sở dữ liệu lên DataGridView.
        /// Định dạng tiền tệ kiểu số phân cách hàng nghìn (N0) và ngày giờ Việt Nam.
        /// </summary>
        void LoadData()
        {
            dgv.DataSource = DatabaseHelper.GetData(
                @"SELECT vp.MaVP AS [Mã VP], vp.MaNhapVien AS [Mã NV], bn.HoTen AS [Bệnh nhân],
                         vp.TienPhong AS [Tiền phòng], vp.TienThuoc AS [Tiền thuốc], vp.TienKham AS [Tiền khám],
                         vp.TienDichVu AS [Tiền dịch vụ], vp.TongTien AS [Tổng tiền], vp.NgayLap AS [Ngày lập],
                         vp.TrangThai AS [Trạng thái]
                  FROM VIENPHI vp
                  JOIN NHAPVIEN nv ON nv.MaNhapVien = vp.MaNhapVien
                  JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                  ORDER BY vp.NgayLap DESC");
            foreach (var c in new[] { "Tiền phòng", "Tiền thuốc", "Tiền khám", "Tiền dịch vụ", "Tổng tiền" }) FormatColumn(dgv, c, "N0");
            FormatColumn(dgv, "Ngày lập", "dd/MM/yyyy HH:mm");
        }

        /// <summary>
        /// Tính toán tổng hợp tiền phòng, tiền thuốc và viện phí từ cơ sở dữ liệu.
        /// </summary>
        /// <param name="loadSaved">
        /// Nếu true: nạp lại số tiền khám và dịch vụ đã lưu từ hóa đơn trước đó (nếu đã lưu).
        /// Nếu false: giữ nguyên giá trị đang nhập trên giao diện.
        /// </param>
        void Tinh(bool loadSaved)
        {
            string nv = GetValue(cmbNV);
            if (nv == null) return;

            // Truy vấn thông tin ngày vào, ngày ra, đơn giá phòng và tổng tiền thuốc từ các đơn đã kê
            var dt = DatabaseHelper.GetData(
                @"SELECT nv.NgayNhapVien, nv.NgayRaVien, bn.MaBN, bn.HoTen AS TenBN,
                         ISNULL(bs.HoTen, N'') AS TenBS, ISNULL(p.TenPhong, N'') AS TenPhong, ISNULL(p.DonGia, 0) AS DonGiaPhong,
                         ISNULL((SELECT SUM(ct.ThanhTien)
                                 FROM CT_DONTHUOC ct
                                 JOIN DONTHUOC d ON d.MaDon = ct.MaDon
                                 JOIN BENHAN ba ON ba.MaBA = d.MaBA
                                 WHERE ba.MaNhapVien = nv.MaNhapVien), 0) AS TienThuoc
                  FROM NHAPVIEN nv
                  JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
                  LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS
                  LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
                  LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
                  WHERE nv.MaNhapVien = @nv", DatabaseHelper.Param("@nv", nv));
            if (dt.Rows.Count == 0) return;

            var r = dt.Rows[0];
            vao = (DateTime)r["NgayNhapVien"];
            ra = r["NgayRaVien"] == DBNull.Value ? (DateTime?)null : (DateTime)r["NgayRaVien"];
            maBN = r["MaBN"].ToString();
            tenBN = r["TenBN"].ToString();
            tenBS = r["TenBS"].ToString();
            tenPhong = r["TenPhong"].ToString();
            decimal donGia = Convert.ToDecimal(r["DonGiaPhong"]);

            // Tính số ngày nằm viện: tối thiểu 1 ngày
            DateTime den = ra ?? DateTime.Now;
            soNgay = Math.Max(1, (den.Date - vao.Date).Days + 1);
            tienPhong = soNgay * donGia;
            tienThuoc = Convert.ToDecimal(r["TienThuoc"]);

            // Kiểm tra xem hồ sơ này đã có hóa đơn VIENPHI trong CSDL chưa
            var vp = DatabaseHelper.GetData("SELECT MaVP, TienPhong, TienThuoc, TienKham, TienDichVu, TrangThai FROM VIENPHI WHERE MaNhapVien=@nv", DatabaseHelper.Param("@nv", nv));
            loading = true;
            if (vp.Rows.Count > 0)
            {
                maVP = vp.Rows[0]["MaVP"].ToString();
                trangThai = vp.Rows[0]["TrangThai"].ToString();
                if (trangThai == "Đã thanh toán")   // Nếu đã thanh toán: giữ nguyên số liệu đã chốt
                {
                    tienPhong = Convert.ToDecimal(vp.Rows[0]["TienPhong"]);
                    tienThuoc = Convert.ToDecimal(vp.Rows[0]["TienThuoc"]);
                }
                if (loadSaved || trangThai == "Đã thanh toán")
                {
                    nudKham.Value = Convert.ToDecimal(vp.Rows[0]["TienKham"]);
                    nudDV.Value = Convert.ToDecimal(vp.Rows[0]["TienDichVu"]);
                }
            }
            else
            {
                maVP = "";
                trangThai = "Chưa lập";
                if (loadSaved) { nudKham.Value = 0; nudDV.Value = 0; }
            }
            loading = false;

            // Hiển thị các thông tin lên màn hình
            txtMaVP.Text = maVP;
            txtTrangThai.Text = trangThai;
            txtSoNgay.Text = soNgay.ToString();
            txtDonGia.Text = Fmt.Money(donGia);
            txtPhong.Text = Fmt.Money(tienPhong);
            txtThuoc.Text = Fmt.Money(tienThuoc);
            UpdateTong();
        }

        /// <summary>
        /// Tính và hiển thị tổng số tiền phải trả = Tiền phòng + Tiền thuốc + Tiền khám + Tiền dịch vụ.
        /// </summary>
        void UpdateTong()
        {
            if (loading) return;
            txtTong.Text = Fmt.Money(tienPhong + tienThuoc + nudKham.Value + nudDV.Value);
        }

        /// <summary>
        /// Lưu hóa đơn viện phí:
        /// - Nếu chưa có hóa đơn: Sinh mã mới (VP...) và INSERT.
        /// - Nếu đã có hóa đơn: UPDATE lại các số tiền và ngày lập.
        /// Lưu ý: Hóa đơn đã thanh toán sẽ bị khóa không cho sửa đổi.
        /// </summary>
        void btnLuu_Click(object s, EventArgs e)
        {
            string nv = GetValue(cmbNV);
            if (nv == null) { ShowWarning("Hãy chọn hồ sơ nhập viện."); return; }
            try
            {
                Tinh(false);
                if (trangThai == "Đã thanh toán") { ShowWarning("Hóa đơn đã thanh toán, không thể sửa."); return; }

                if (maVP == "")
                {
                    string ma = DatabaseHelper.NextId("VIENPHI", "MaVP", "VP");
                    DatabaseHelper.ExecuteNonQuery(@"INSERT INTO VIENPHI(MaVP, MaNhapVien, TienPhong, TienThuoc, TienKham, TienDichVu, TrangThai)
                              VALUES(@ma, @nv, @phong, @thuoc, @kham, @dv, N'Chưa thanh toán')",
                        DatabaseHelper.Param("@ma", ma), DatabaseHelper.Param("@nv", nv), DatabaseHelper.Param("@phong", tienPhong), DatabaseHelper.Param("@thuoc", tienThuoc),
                        DatabaseHelper.Param("@kham", nudKham.Value), DatabaseHelper.Param("@dv", nudDV.Value));
                }
                else
                {
                    DatabaseHelper.ExecuteNonQuery(@"UPDATE VIENPHI SET TienPhong=@phong, TienThuoc=@thuoc, TienKham=@kham, TienDichVu=@dv, NgayLap=GETDATE()
                              WHERE MaVP=@ma",
                        DatabaseHelper.Param("@phong", tienPhong), DatabaseHelper.Param("@thuoc", tienThuoc),
                        DatabaseHelper.Param("@kham", nudKham.Value), DatabaseHelper.Param("@dv", nudDV.Value), DatabaseHelper.Param("@ma", maVP));
                }
                LoadData();
                Tinh(true);
                ShowInfo("Đã lưu hóa đơn viện phí.");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Xác nhận thu tiền viện phí: Cập nhật trạng thái hóa đơn thành 'Đã thanh toán'.
        /// </summary>
        void btnThanhToan_Click(object s, EventArgs e)
        {
            if (maVP == "") { ShowWarning("Chưa có hóa đơn. Hãy bấm \"Lưu hóa đơn\" trước."); return; }
            if (trangThai == "Đã thanh toán") { ShowInfo("Hóa đơn này đã thanh toán rồi."); return; }
            if (!Ask("Xác nhận bệnh nhân đã thanh toán hóa đơn " + maVP + "?")) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("UPDATE VIENPHI SET TrangThai = N'Đã thanh toán' WHERE MaVP = @ma", DatabaseHelper.Param("@ma", maVP));
                LoadData();
                Tinh(true);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Xuất hóa đơn viện phí ra file bảng tính Microsoft Excel (.xlsx):
        /// 
        /// NGUYÊN LÝ HOẠT ĐỘNG DÀNH CHO NGƯỜI MỚI HỌC C#:
        /// 1. Tinh(false) để đảm bảo toàn bộ số liệu viện phí (phòng, thuốc, khám, dịch vụ, tổng tiền) là số liệu mới nhất.
        /// 2. Tạo một danh sách các chuỗi văn bản (List&lt;string&gt; L) định dạng từng dòng của hóa đơn viện phí:
        ///    - Thông tin đơn vị, mã hóa đơn, ngày giờ lập hóa đơn.
        ///    - Thông tin bệnh nhân: họ tên, mã BN, mã hồ sơ nhập viện, khoa phòng.
        ///    - Bảng chi tiết: tiền phòng, tiền thuốc, tiền khám, tiền dịch vụ.
        ///    - Tổng cộng và phần chữ ký xác nhận của các bên.
        /// 3. Mở SaveFileDialog cho phép người dùng chọn thư mục và đặt tên file .xlsx cần lưu.
        /// 4. Gọi Export.WriteTextXlsx(...) để đóng gói thành file Excel chuẩn OpenXML (file zip chứa XML).
        /// 5. Sau khi xuất thành công, thông báo và tự động mở file bằng Microsoft Excel hoặc ứng dụng xem mặc định.
        /// </summary>
        void btnInHoaDon_Click(object s, EventArgs e)
        {
            if (GetValue(cmbNV) == null) { ShowWarning("Hãy chọn hồ sơ nhập viện."); return; }
            try
            {
                Tinh(false);
                decimal tong = tienPhong + tienThuoc + nudKham.Value + nudDV.Value;
                var L = new List<string>();
                L.Add("#HÓA ĐƠN VIỆN PHÍ");
                L.Add("");
                L.Add("Mã hóa đơn : " + (maVP == "" ? "(chưa lưu)" : maVP));
                L.Add("Ngày xuất  : " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                L.Add("Người lập  : " + Session.FullName);
                L.Add("Bệnh nhân  : " + tenBN + " (" + maBN + ")");
                L.Add("Mã nhập viện: " + GetValue(cmbNV));
                L.Add("Bác sĩ     : " + tenBS);
                L.Add("Phòng      : " + tenPhong);
                L.Add("Vào viện   : " + vao.ToString("dd/MM/yyyy HH:mm"));
                L.Add("Ra viện    : " + (ra.HasValue ? ra.Value.ToString("dd/MM/yyyy HH:mm") : "(đang điều trị)"));
                L.Add(new string('-', 60));
                L.Add(Row("Tiền phòng (" + soNgay + " ngày)", tienPhong));
                L.Add(Row("Tiền thuốc", tienThuoc));
                L.Add(Row("Tiền khám", nudKham.Value));
                L.Add(Row("Tiền dịch vụ", nudDV.Value));
                L.Add(new string('-', 60));
                L.Add(Row("TỔNG CỘNG", tong));
                L.Add("");
                L.Add("Trạng thái: " + trangThai);
                L.Add("");
                L.Add("      Người nộp tiền                     Người lập hóa đơn");
                L.Add("      (ký, ghi rõ họ tên)                (ký, ghi rõ họ tên)");

                using (var dlg = new SaveFileDialog
                {
                    Filter = "Excel (*.xlsx)|*.xlsx",
                    FileName = "HoaDon_" + (maVP == "" ? GetValue(cmbNV) : maVP) + "_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx",
                    Title = "Chọn nơi lưu hóa đơn Excel"
                })
                {
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    Export.WriteTextXlsx(dlg.FileName, L, "HoaDonVienPhi");
                    ShowInfo("Đã xuất hóa đơn ra file Excel thành công:\n" + dlg.FileName);
                    Export.TryOpen(dlg.FileName);
                }
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút Nhập Excel: Mở hộp thoại chọn tệp bảng tính Excel (.xlsx) từ máy tính và nạp dữ liệu lên bảng DataGridView.
        /// 
        /// NGUYÊN LÝ HOẠT ĐỘNG DÀNH CHO NGƯỜI MỚI HỌC C#:
        /// 1. Mở OpenFileDialog qua hàm Export.ImportExcelDialog() để người dùng chọn tệp .xlsx cần nhập.
        /// 2. Hệ thống giải nén tệp zip .xlsx, trích xuất dữ liệu XML trong 'sheet1.xml' và đưa vào một DataTable.
        /// 3. Nếu đọc thành công (DataTable khác null):
        ///    - Gán DataTable làm DataSource cho DataGridView (dgv.DataSource = dt).
        ///    - Thông báo số dòng đã nhập thành công để người dùng kiểm tra số lượng bản ghi.
        /// </summary>
        void btnNhapExcel_Click(object s, EventArgs e)
        {
            try
            {
                var dt = Export.ImportExcelDialog();
                if (dt != null)
                {
                    dgv.DataSource = dt;
                    ShowInfo("Đã nhập thành công " + dt.Rows.Count + " dòng từ file Excel!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Hàm tiện ích căn lề chuỗi văn bản cho dòng hóa đơn (cột tên căn trái, số tiền căn phải).
        /// </summary>
        static string Row(string label, decimal amount)
        {
            return label.PadRight(32) + Fmt.Money(amount).PadLeft(28);
        }
    }
}
