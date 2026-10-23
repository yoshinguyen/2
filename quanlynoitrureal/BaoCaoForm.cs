using System;
using System.Diagnostics;
using System.Data;
using System.Windows.Forms;
using QuanLyNoiTru.dotnet;
using ClosedXML.Excel;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Báo cáo thống kê:
    /// Cung cấp 4 loại báo cáo chuyên dụng cho bệnh viện:
    /// 1. Danh sách bệnh nhân đang điều trị (theo khoa, phòng, giường, bác sĩ).
    /// 2. Danh sách tất cả bệnh nhân trong hệ thống.
    /// 3. Danh sách hồ sơ nhập viện (lịch sử vào/ra viện).
    /// 4. Danh sách viện phí và trạng thái thanh toán.
    /// Hỗ trợ các chức năng: xem trực tiếp, xuất dữ liệu ra file Excel (.xlsx) và nhập dữ liệu từ file Excel (.xlsx).
    /// </summary>
    public partial class BaoCaoForm : BaseForm
    {
        DataTable current;   // Biến lưu trữ bảng dữ liệu của báo cáo đang được chọn hiển thị

        // Tên hiển thị của các loại báo cáo trong ComboBox
        static readonly string[] Names =
        {
            "Danh sách bệnh nhân đang điều trị",
            "Danh sách tất cả bệnh nhân",
            "Danh sách hồ sơ nhập viện",
            "Danh sách viện phí"
        };

        // Các câu lệnh SQL truy vấn tương ứng với từng loại báo cáo
        static readonly string[] Queries =
        {
            // Báo cáo 1: Bệnh nhân đang điều trị nội trú
            @"SELECT bn.MaBN AS [Mã BN], bn.HoTen AS [Họ tên], bn.NgaySinh AS [Ngày sinh], bn.GioiTinh AS [Giới tính],
                     k.TenKhoa AS [Khoa], p.TenPhong AS [Phòng], g.SoGiuong AS [Giường], bs.HoTen AS [Bác sĩ],
                     nv.NgayNhapVien AS [Ngày vào viện], bn.TinhTrang AS [Tình trạng bệnh]
              FROM NHAPVIEN nv
              JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
              LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS
              LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
              LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
              LEFT JOIN KHOA k ON k.MaKhoa = p.MaKhoa
              WHERE nv.TrangThai = N'Đang điều trị'
              ORDER BY nv.NgayNhapVien",

            // Báo cáo 2: Toàn bộ danh mục bệnh nhân
            @"SELECT MaBN AS [Mã BN], HoTen AS [Họ tên], NgaySinh AS [Ngày sinh], GioiTinh AS [Giới tính], CCCD AS [CCCD],
                     SoDienThoai AS [SĐT], DiaChi AS [Địa chỉ], NgheNghiep AS [Nghề nghiệp], TinhTrang AS [Tình trạng bệnh]
              FROM BENHNHAN ORDER BY MaBN",

            // Báo cáo 3: Toàn bộ lịch sử hồ sơ nhập viện
            @"SELECT nv.MaNhapVien AS [Mã NV], bn.HoTen AS [Bệnh nhân], bs.HoTen AS [Bác sĩ], p.TenPhong AS [Phòng],
                     nv.MaGiuong AS [Giường], nv.NgayNhapVien AS [Ngày vào viện], nv.NgayRaVien AS [Ngày ra viện],
                     nv.LyDoNhapVien AS [Lý do], nv.TrangThai AS [Trạng thái]
              FROM NHAPVIEN nv
              JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
              LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS
              LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
              LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
              ORDER BY nv.NgayNhapVien DESC",

            // Báo cáo 4: Danh sách hóa đơn và tình hình thu viện phí
            @"SELECT vp.MaVP AS [Mã VP], bn.HoTen AS [Bệnh nhân], vp.TienPhong AS [Tiền phòng], vp.TienThuoc AS [Tiền thuốc],
                     vp.TienKham AS [Tiền khám], vp.TienDichVu AS [Tiền dịch vụ], vp.TongTien AS [Tổng tiền],
                     vp.NgayLap AS [Ngày lập], vp.TrangThai AS [Trạng thái]
              FROM VIENPHI vp
              JOIN NHAPVIEN nv ON nv.MaNhapVien = vp.MaNhapVien
              JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
              ORDER BY vp.NgayLap DESC"
        };

        /// <summary>
        /// Khởi tạo form báo cáo, đổ danh sách loại báo cáo vào ComboBox và tải báo cáo mặc định đầu tiên.
        /// </summary>
        public BaoCaoForm()
        {
            InitializeComponent();   // dựng giao diện (code nằm trong BaoCaoForm.Designer.cs)
            cmbBC.Items.AddRange(Names);
            cmbBC.SelectedIndex = 0;
            LoadReport();
            // Khi chọn loại báo cáo khác -> tải dữ liệu mới tương ứng
            cmbBC.SelectedIndexChanged += (s, e) => LoadReport();
        }

        /// <summary>
        /// Thực thi câu lệnh SQL để lấy dữ liệu báo cáo tương ứng với loại đang chọn,
        /// sau đó hiển thị lên DataGridView và định dạng cột (tiền tệ, ngày tháng).
        /// </summary>
        void LoadReport()
        {
            try
            {
                current = DatabaseHelper.GetData(Queries[cmbBC.SelectedIndex]);
                dgv.DataSource = current;
                // Định dạng hiển thị các cột kiểu số tiền
                foreach (DataGridViewColumn c in dgv.Columns)
                    if (c.ValueType == typeof(decimal)) c.DefaultCellStyle.Format = "N0";
                // Định dạng hiển thị ngày giờ chuẩn Việt Nam
                FormatColumn(dgv, "Ngày sinh", "dd/MM/yyyy");
                FormatColumn(dgv, "Ngày vào viện", "dd/MM/yyyy HH:mm");
                FormatColumn(dgv, "Ngày ra viện", "dd/MM/yyyy HH:mm");
                FormatColumn(dgv, "Ngày lập", "dd/MM/yyyy HH:mm");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Nút Xem báo cáo: tải lại dữ liệu mới nhất từ CSDL.
        /// </summary>
        void btnXem_Click(object s, EventArgs e)
        {
            LoadReport();
        }

        /// <summary>
        /// Nút Nhập Excel: Cho phép người dùng chọn tệp bảng tính Excel (.xlsx) từ máy tính
        /// và nạp dữ liệu vào bảng hiển thị DataGridView.
        /// 
        /// NGUYÊN LÝ HOẠT ĐỘNG DÀNH CHO NGƯỜI MỚI HỌC C#:
        /// 1. Gọi hàm tiện ích Export.ImportExcelDialog() để mở hộp thoại OpenFileDialog cho người dùng chọn file .xlsx.
        /// 2. Bên trong, hàm sẽ giải nén file zip .xlsx, đọc file XML 'sheet1.xml' và chuyển thành một DataTable.
        /// 3. Nếu đọc thành công (kết quả khác null):
        ///    - Gán DataTable vào biến 'current' và đặt làm DataSource cho lưới DataGridView 'dgv'.
        ///    - DataGridView tự động vẽ lại các cột và hàng tương ứng với dữ liệu trong file Excel.
        ///    - Hiển thị thông báo thành công cho người dùng biết có bao nhiêu dòng dữ liệu đã được nhập.
        /// </summary>
        void btnNhapExcel_Click(object s, EventArgs e)
        {
            try
            {
                var dt = Export.ImportExcelDialog();
                if (dt != null)
                {
                    current = dt;
                    dgv.DataSource = current;
                    ShowInfo("Đã nhập thành công " + dt.Rows.Count + " dòng từ file Excel!");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        /// <summary>
        /// Nút Xuất Excel: xuất toàn bộ dữ liệu báo cáo hiện tại ra file bảng tính Microsoft Excel (.xlsx).
        /// 
        /// NGUYÊN LÝ HOẠT ĐỘNG DÀNH CHO NGƯỜI MỚI HỌC C#:
        /// 1. Kiểm tra xem bảng 'current' có dữ liệu không. Nếu null thì dừng lại.
        /// 2. Mở SaveFileDialog với đuôi mở rộng mặc định là .xlsx và tên file chứa ngày giờ hiện tại.
        /// 3. Khi người dùng chọn đường dẫn lưu và bấm Save:
        ///    - Gọi hàm tiện ích Export.WriteXlsx(dlg.FileName, current).
        ///    - Hàm này tự động tạo các file XML theo cấu trúc OpenXML và nén lại thành file .xlsx.
        ///    - Hoàn toàn KHÔNG cần máy tính cài sẵn Microsoft Office hay thư viện bên ngoài.
        /// 4. Thông báo xuất thành công và tự động mở file bằng ứng dụng bảng tính mặc định qua hàm TryOpen.
        /// </summary>
        /// 

        void btnExcel_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx";
                saveFileDialog.Title = "xuất file excel";
                saveFileDialog.FileName = "filexuat.xlsx";

                if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    return;

                try
                {
                    using (XLWorkbook workbook = new XLWorkbook())
                    {
                        IXLWorksheet worksheet = workbook.Worksheets.Add("Data");

                        // =========================
                        // COLUMN HEADERS
                        // =========================
                        for (int col = 0; col < dgv.Columns.Count; col++)
                        {
                            worksheet.Cell(1, col + 1).Value =
                                dgv.Columns[col].HeaderText;
                        }

                        // =========================
                        // DATA ROWS
                        // =========================
                        for (int row = 0; row < dgv.Rows.Count; row++)
                        {
                            // Skip the empty row at the bottom
                            if (dgv.Rows[row].IsNewRow)
                                continue;

                            for (int col = 0; col < dgv.Columns.Count; col++)
                            {
                                object value =
                                    dgv.Rows[row].Cells[col].Value;

                                var cell = worksheet.Cell(row + 2, col + 1);

                                // =========================
                                // DATE
                                // =========================
                                if (value is DateTime dateValue)
                                {
                                    cell.Value = dateValue.Date;

                                    // Only show the date, NOT the time
                                    cell.Style.DateFormat.Format = "d/M/yyyy";
                                }

                                // =========================
                                // PHONE NUMBER
                                // =========================
                                else if (dgv.Columns[col].HeaderText
               .Equals("Điện thoại",
                       StringComparison.OrdinalIgnoreCase))
                                {
                                    // Store phone number as text
                                    cell.SetValue(value?.ToString() ?? "");
                                }

                                // =========================
                                // EVERYTHING ELSE
                                // =========================
                                else
                                {
                                    cell.Value = value?.ToString() ?? "";
                                }
                            }
                        }

                        // =========================
                        // FORMATTING
                        // =========================

                        // Make headers bold
                        worksheet.Row(1).Style.Font.Bold = true;

                        // Adjust column widths
                        worksheet.Columns().AdjustToContents();

                        // Save Excel file
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    MessageBox.Show(
                        "Xuất thành công!",
                        "Xuất xong",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        "lỗi:\n" + ex.Message,
                        "ko xuất đc",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }

        }
        /*   void btnExcel_Click(object s, EventArgs e)
           {
               if (current == null) return;
               using (var dlg = new SaveFileDialog { Filter = "Excel (*.xlsx)|*.xlsx", FileName = "BaoCao_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx" })
               {
                   if (dlg.ShowDialog() != DialogResult.OK) return;
                   try
                   {
                       Export.WriteXlsx(dlg.FileName, current);
                       ShowInfo("Đã xuất Excel thành công:\n" + dlg.FileName);
                       TryOpen(dlg.FileName);
                   }
                   catch (Exception ex) { ShowError(ex); }
               }
           }*/

        /// <summary>
        /// Tự động mở file vừa xuất bằng ứng dụng mặc định của hệ thống (PDF Viewer hoặc Excel).
        /// </summary>
        static void TryOpen(string path)
        {
            try { Process.Start(path); } catch { }
        }
    }
}
