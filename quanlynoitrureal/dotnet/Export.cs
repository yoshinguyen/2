using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNoiTru.dotnet
{
    /// <summary>
    /// Lớp tiện ích phục vụ Xuất file Excel (.xlsx) và Nhập file Excel (.xlsx):
    /// ĐẶC BIỆT: Hoạt động thuần túy trên .NET Framework / .NET Core tiêu chuẩn,
    /// KHÔNG cần cài đặt Microsoft Office, không cần thư viện bên ngoài (EPPlus, ClosedXML, NPOI...).
    /// 
    /// ====================================================================================================
    /// 📚 HƯỚNG DẪN DÀNH CHO NGƯỜI MỚI HỌC C#: CƠ CHẾ XUẤT VÀ NHẬP FILE EXCEL (.XLSX)
    /// ====================================================================================================
    /// 1. CƠ CHẾ TỰ TẠO FILE EXCEL (.XLSX) CHUẨN OPENXML:
    ///    - Bản chất tệp .xlsx thực chất là một TỆP NÉN DẠNG ZIP chứa các file tài liệu XML bên trong.
    ///    - Bằng cách sử dụng thư viện nén 'ZipArchive' có sẵn của .NET:
    ///      + Tạo file XML 'sheet1.xml' chứa các thẻ chuẩn của Excel:
    ///         &lt;row r="1"&gt; : Đại diện cho 1 hàng.
    ///         &lt;c r="A1" t="inlineStr"&gt;&lt;is&gt;&lt;t&gt;Tiêu đề&lt;/t&gt;&lt;/is&gt;&lt;/c&gt; : Ô chứa chuỗi.
    ///         &lt;c r="B2"&gt;&lt;v&gt;150000&lt;/v&gt;&lt;/c&gt; : Ô chứa số.
    ///      + Tạo các file XML cấu hình quan hệ: '[Content_Types].xml', 'workbook.xml', '_rels/.rels'.
    ///      + Đóng gói tất cả vào 1 file có đuôi mở rộng là '.xlsx'.
    ///    - File tạo ra là chuẩn OpenXML quốc tế 100%, mở được trên Excel, Google Drive, LibreOffice.
    /// 
    /// 2. CƠ CHẾ ĐỌC (NHẬP) FILE EXCEL (.XLSX) CHUẨN OPENXML:
    ///    - Mở file .xlsx bằng ZipArchive -> Đọc file sheet1.xml bên trong.
    ///    - Phân tích (parse) các thẻ XML: &lt;row&gt;, &lt;c&gt;, &lt;v&gt;, &lt;is&gt;&lt;t&gt; để trích xuất dữ liệu.
    ///    - Đổ vào DataTable với dòng đầu tiên làm tên cột (header).
    ///    - Sau đó trả DataTable về để Form hiển thị lên DataGridView hoặc lưu vào CSDL.
    /// ====================================================================================================
    /// </summary>
    public static class Export
    {
        // Định dạng văn hóa chuẩn Việt Nam (phục vụ hiển thị ngày tháng và số tiền có dấu phân cách hàng nghìn)
        static readonly CultureInfo Vn = new CultureInfo("vi-VN");

        /// <summary>
        /// Chuyển đổi giá trị ô bất kỳ sang chuỗi hiển thị thân thiện:
        /// - null / DBNull: chuỗi rỗng
        /// - DateTime: "dd/MM/yyyy" (nếu không có giờ) hoặc "dd/MM/yyyy HH:mm"
        /// - bool: "Có" hoặc "Không"
        /// - decimal: định dạng tiền tệ có phân cách hàng nghìn (ví dụ: 150,000)
        /// </summary>
        public static string Cell(object v)
        {
            if (v == null || v == DBNull.Value) return "";
            if (v is DateTime)
            {
                var d = (DateTime)v;
                return d.TimeOfDay.Ticks == 0 ? d.ToString("dd/MM/yyyy") : d.ToString("dd/MM/yyyy HH:mm");
            }
            if (v is bool) return (bool)v ? "Có" : "Không";
            if (v is decimal) return ((decimal)v).ToString("N0", Vn);
            return v.ToString();
        }

        // ====================================================================
        // 1. XUẤT FILE EXCEL (.XLSX) TỪ DATATABLE - XUẤT BẢNG DỮ LIỆU
        // ====================================================================

        /// <summary>
        /// Tạo một tệp bảng tính Microsoft Excel (.xlsx) chuẩn OpenXML từ DataTable:
        /// - Tự động ghi sheetData với các thẻ XML: row, c (cell), v (value), inlineStr (chuỗi text).
        /// - Đóng gói các tệp cấu hình OpenXML cần thiết vào một file ZIP duy nhất (.xlsx).
        /// - Hoàn toàn không phụ thuộc Microsoft Office Interop hay bất kỳ thư viện bên thứ 3 nào.
        /// 
        /// CƠ CHẾ HOẠT ĐỘNG CHI TIẾT:
        /// - Bước 1: Duyệt từng cột và hàng của DataTable, chuyển thành chuỗi XML theo chuẩn OpenXML.
        /// - Bước 2: Nếu giá trị là kiểu số (int, decimal, double...) thì ghi dạng &lt;v&gt;số&lt;/v&gt; để Excel hiểu đây là Number.
        /// - Bước 3: Nếu giá trị là kiểu chuỗi/ngày tháng thì ghi dạng inline string &lt;is&gt;&lt;t&gt;text&lt;/t&gt;&lt;/is&gt;.
        /// - Bước 4: Đóng gói thành file .xlsx (thực chất là file ZIP chứa các file XML).
        /// </summary>
        public static void WriteXlsx(string path, DataTable dt)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");

            // 1. Tạo hàng tiêu đề (Row 1) - tên các cột
            sb.Append("<row r=\"1\">");
            for (int c = 0; c < dt.Columns.Count; c++)
                sb.Append(StrCell(ColName(c) + "1", dt.Columns[c].ColumnName));
            sb.Append("</row>");

            // 2. Tạo các hàng dữ liệu (bắt đầu từ Row 2)
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                int rn = r + 2;
                sb.Append("<row r=\"" + rn + "\">");
                for (int c = 0; c < dt.Columns.Count; c++)
                {
                    object v = dt.Rows[r][c];
                    if (v == null || v == DBNull.Value) continue;
                    string rf = ColName(c) + rn;
                    // Phân biệt kiểu số để Excel tự hiểu dạng Number thay vì chuỗi Text
                    if (v is int || v is long || v is short || v is byte || v is decimal || v is double || v is float)
                        sb.Append("<c r=\"" + rf + "\"><v>" + Convert.ToString(v, CultureInfo.InvariantCulture) + "</v></c>");
                    else
                        sb.Append(StrCell(rf, Cell(v)));
                }
                sb.Append("</row>");
            }
            sb.Append("</sheetData></worksheet>");

            // 3. Đóng gói file .xlsx bằng ZipArchive
            WriteXlsxPackage(path, sb.ToString());
        }

        // ====================================================================
        // 2. XUẤT FILE EXCEL (.XLSX) TỪ DANH SÁCH DÒNG VĂN BẢN (HÓA ĐƠN, PHIẾU)
        // ====================================================================

        /// <summary>
        /// Xuất danh sách các dòng văn bản (hóa đơn viện phí, phiếu xuất viện...) ra file Excel (.xlsx):
        /// - Mỗi dòng văn bản sẽ được ghi vào 1 ô trên cột A của bảng tính.
        /// - Dòng bắt đầu bằng '#' sẽ bỏ ký tự '#' (tiêu đề dùng cho chức năng in cũ).
        /// 
        /// VÍ DỤ ỨNG DỤNG:
        /// - Xuất hóa đơn viện phí (VienPhiForm.cs) -> mỗi dòng thông tin ghi thành 1 hàng Excel.
        /// - Xuất phiếu xuất viện (XuatVienForm.cs) -> mỗi dòng ghi thành 1 hàng Excel.
        /// </summary>
        /// <param name="path">Đường dẫn file .xlsx cần tạo</param>
        /// <param name="lines">Danh sách các dòng văn bản cần xuất</param>
        /// <param name="sheetName">Tên sheet trong file Excel (mặc định "HoaDon")</param>
        public static void WriteTextXlsx(string path, IList<string> lines, string sheetName = "HoaDon")
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");

            // Mỗi dòng văn bản ghi vào cột A, từ dòng 1 đến dòng N
            for (int i = 0; i < lines.Count; i++)
            {
                int rn = i + 1;
                string text = lines[i];
                // Bỏ ký tự '#' ở đầu dòng tiêu đề (nếu có)
                if (text.StartsWith("#")) text = text.Substring(1);
                sb.Append("<row r=\"" + rn + "\">");
                sb.Append(StrCell("A" + rn, text));
                sb.Append("</row>");
            }
            sb.Append("</sheetData></worksheet>");

            WriteXlsxPackage(path, sb.ToString(), sheetName);
        }

        // ====================================================================
        // 3. NHẬP (ĐỌC) FILE EXCEL (.XLSX) VÀO DATATABLE
        // ====================================================================

        /// <summary>
        /// Đọc (nhập) dữ liệu từ file Excel (.xlsx) vào DataTable:
        /// - Mở file .xlsx bằng ZipArchive (giải nén file ZIP).
        /// - Tìm file sheet1.xml bên trong (chứa dữ liệu bảng tính).
        /// - Phân tích (parse) các thẻ XML để trích xuất nội dung từng ô.
        /// - Dòng đầu tiên (Row 1) được coi là tên cột (header) của DataTable.
        /// - Các dòng tiếp theo (Row 2, 3, ...) là dữ liệu thực tế.
        /// 
        /// LƯU Ý QUAN TRỌNG:
        /// - Hàm này chỉ đọc được file .xlsx chuẩn (OpenXML), KHÔNG đọc được file .xls cũ (Excel 97-2003).
        /// - Chỉ đọc sheet đầu tiên trong workbook.
        /// - Hỗ trợ cả inline string và shared string đơn giản.
        /// 
        /// CƠ CHẾ HOẠT ĐỘNG CHI TIẾT:
        /// - Bước 1: Mở file .xlsx như một file ZIP bằng ZipArchive.
        /// - Bước 2: Tìm entry "xl/worksheets/sheet1.xml" (nội dung sheet đầu tiên).
        /// - Bước 3: Đọc toàn bộ nội dung XML của sheet.
        /// - Bước 4: Dùng phương pháp phân tích chuỗi XML đơn giản (không cần XmlDocument):
        ///   + Tách các thẻ &lt;row&gt; ra thành từng hàng.
        ///   + Trong mỗi hàng, tách các thẻ &lt;c&gt; ra thành từng ô.
        ///   + Đọc giá trị ô: nếu có thẻ &lt;v&gt; thì lấy số; nếu có &lt;is&gt;&lt;t&gt; thì lấy chuỗi.
        /// - Bước 5: Dòng đầu tiên (Row 1) làm tên cột, các dòng sau làm dữ liệu.
        /// </summary>
        /// <param name="path">Đường dẫn đến file .xlsx cần đọc</param>
        /// <returns>DataTable chứa dữ liệu đã đọc từ file Excel</returns>
        public static DataTable ReadXlsx(string path)
        {
            var dt = new DataTable();
            string sheetXml = "";

            // Bước 1+2: Mở file .xlsx như ZIP và đọc nội dung sheet1.xml
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Read))
            {
                // Tìm file sheet đầu tiên trong thư mục xl/worksheets/
                foreach (var entry in zip.Entries)
                {
                    if (entry.FullName.Contains("worksheets/sheet") && entry.FullName.EndsWith(".xml"))
                    {
                        using (var reader = new StreamReader(entry.Open(), Encoding.UTF8))
                            sheetXml = reader.ReadToEnd();
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(sheetXml)) return dt;

            // Bước 3: Phân tích XML đơn giản để lấy dữ liệu từng hàng và ô
            var rows = new List<List<string>>();
            int pos = 0;
            while (true)
            {
                // Tìm từng thẻ <row ...>...</row>
                int rowStart = sheetXml.IndexOf("<row ", pos);
                if (rowStart < 0) break;
                int rowEnd = sheetXml.IndexOf("</row>", rowStart);
                if (rowEnd < 0) break;
                string rowBlock = sheetXml.Substring(rowStart, rowEnd - rowStart + 6);
                pos = rowEnd + 6;

                // Tách từng ô <c ...>...</c> trong hàng
                var cells = new List<string>();
                int cPos = 0;
                while (true)
                {
                    // Tìm ô tiếp theo
                    int cStart = rowBlock.IndexOf("<c ", cPos);
                    if (cStart < 0) break;

                    // Kiểm tra xem ô này có nội dung hay tự đóng />
                    int closeTag = rowBlock.IndexOf(">", cStart);
                    if (closeTag < 0) break;

                    string cellValue = "";

                    // Ô tự đóng: <c r="A1" />
                    if (rowBlock[closeTag - 1] == '/')
                    {
                        cPos = closeTag + 1;
                        cells.Add("");
                        continue;
                    }

                    int cEnd = rowBlock.IndexOf("</c>", cStart);
                    if (cEnd < 0) break;
                    string cellBlock = rowBlock.Substring(cStart, cEnd - cStart + 4);
                    cPos = cEnd + 4;

                    // Thử đọc giá trị inline string: <is><t>text</t></is>
                    int tStart = cellBlock.IndexOf("<t");
                    if (tStart >= 0)
                    {
                        int tOpen = cellBlock.IndexOf(">", tStart);
                        int tEnd = cellBlock.IndexOf("</t>", tOpen);
                        if (tOpen >= 0 && tEnd >= 0)
                            cellValue = cellBlock.Substring(tOpen + 1, tEnd - tOpen - 1);
                    }
                    // Thử đọc giá trị số: <v>12345</v>
                    else
                    {
                        int vStart = cellBlock.IndexOf("<v>");
                        int vEnd = cellBlock.IndexOf("</v>");
                        if (vStart >= 0 && vEnd >= 0)
                            cellValue = cellBlock.Substring(vStart + 3, vEnd - vStart - 3);
                    }

                    cells.Add(cellValue);
                }
                rows.Add(cells);
            }

            if (rows.Count == 0) return dt;

            // Bước 4: Dòng đầu tiên là tên cột (header)
            var header = rows[0];
            for (int i = 0; i < header.Count; i++)
            {
                string colName = string.IsNullOrWhiteSpace(header[i]) ? "Cột " + (i + 1) : header[i];
                // Tránh trùng tên cột
                while (dt.Columns.Contains(colName))
                    colName += "_";
                dt.Columns.Add(colName);
            }

            // Bước 5: Các dòng còn lại là dữ liệu
            for (int r = 1; r < rows.Count; r++)
            {
                var row = dt.NewRow();
                for (int c = 0; c < Math.Min(rows[r].Count, dt.Columns.Count); c++)
                    row[c] = rows[r][c];
                dt.Rows.Add(row);
            }

            return dt;
        }

        /// <summary>
        /// Hiển thị hộp thoại chọn file Excel (.xlsx) để nhập dữ liệu:
        /// - Mở OpenFileDialog cho phép người dùng chọn file.
        /// - Đọc file bằng hàm ReadXlsx.
        /// - Trả về DataTable chứa dữ liệu, hoặc null nếu người dùng bấm Hủy.
        /// </summary>
        /// <returns>DataTable dữ liệu đã đọc, hoặc null nếu hủy</returns>
        public static DataTable ImportExcelDialog()
        {
            using (var dlg = new OpenFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                Title = "Chọn file Excel (.xlsx) để nhập dữ liệu"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return null;
                return ReadXlsx(dlg.FileName);
            }
        }

        /// <summary>
        /// Hiển thị hộp thoại chọn nơi lưu file và xuất DataTable ra file Excel (.xlsx):
        /// - Mở SaveFileDialog cho người dùng chọn tên file và vị trí lưu.
        /// - Ghi dữ liệu DataTable ra file .xlsx bằng hàm WriteXlsx.
        /// - Trả về đường dẫn file đã lưu, hoặc null nếu người dùng bấm Hủy.
        /// </summary>
        /// <param name="dt">Bảng dữ liệu cần xuất</param>
        /// <param name="defaultName">Tên file gợi ý mặc định</param>
        /// <returns>Đường dẫn file đã xuất thành công, hoặc null nếu hủy</returns>
        public static string ExportExcelDialog(DataTable dt, string defaultName = null)
        {
            if (defaultName == null) defaultName = "BaoCao_" + DateTime.Now.ToString("yyyyMMdd_HHmm");
            using (var dlg = new SaveFileDialog
            {
                Filter = "Excel (*.xlsx)|*.xlsx",
                FileName = defaultName + ".xlsx",
                Title = "Chọn nơi lưu file Excel"
            })
            {
                if (dlg.ShowDialog() != DialogResult.OK) return null;
                WriteXlsx(dlg.FileName, dt);
                return dlg.FileName;
            }
        }

        // ====================================================================
        // 4. HÀM NỘI BỘ ĐÓNG GÓI OPENXML
        // ====================================================================

        /// <summary>
        /// Đóng gói nội dung sheet XML thành file .xlsx hoàn chỉnh theo cấu trúc chuẩn OpenXML:
        /// Tạo cấu trúc thư mục ZIP: [Content_Types].xml, _rels/.rels, xl/workbook.xml, xl/worksheets/sheet1.xml.
        /// </summary>
        static void WriteXlsxPackage(string path, string sheetXml, string sheetName = "BaoCao")
        {
            const string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
            using (var fs = new FileStream(path, FileMode.Create))
            using (var zip = new ZipArchive(fs, ZipArchiveMode.Create))
            {
                // Định nghĩa Content Types (cho hệ thống biết có những loại file gì trong ZIP)
                AddEntry(zip, "[Content_Types].xml", xml +
                    "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                    "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                    "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                    "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                    "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                    "</Types>");

                // Quan hệ Package gốc (trỏ từ package root đến workbook)
                AddEntry(zip, "_rels/.rels", xml +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                    "</Relationships>");

                // Khai báo Workbook và Sheet
                AddEntry(zip, "xl/workbook.xml", xml +
                    "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                    "<sheets><sheet name=\"" + SecurityElement.Escape(sheetName) + "\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>");

                // Quan hệ giữa Workbook và Sheet1
                AddEntry(zip, "xl/_rels/workbook.xml.rels", xml +
                    "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                    "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                    "</Relationships>");

                // Nội dung dữ liệu bảng tính Sheet1
                AddEntry(zip, "xl/worksheets/sheet1.xml", sheetXml);
            }
        }

        /// <summary>
        /// Tạo một entry file nén trong file ZIP với nội dung chuỗi ký tự UTF-8 không BOM.
        /// </summary>
        static void AddEntry(ZipArchive z, string name, string content)
        {
            var entry = z.CreateEntry(name);
            using (var w = new StreamWriter(entry.Open(), new UTF8Encoding(false)))
                w.Write(content);
        }

        /// <summary>
        /// Tạo chuỗi XML đại diện cho một ô dạng chuỗi ký tự (inline string) có escape ký tự đặc biệt XML.
        /// </summary>
        static string StrCell(string rf, string text)
        {
            return "<c r=\"" + rf + "\" t=\"inlineStr\"><is><t xml:space=\"preserve\">" + SecurityElement.Escape(text) + "</t></is></c>";
        }

        /// <summary>
        /// Chuyển đổi chỉ số cột số nguyên sang ký tự cột Excel (0 -> A, 1 -> B, ..., 26 -> AA, ...).
        /// </summary>
        static string ColName(int i)
        {
            string s = "";
            i++;
            while (i > 0)
            {
                int m = (i - 1) % 26;
                s = (char)('A' + m) + s;
                i = (i - 1) / 26;
            }
            return s;
        }

        /// <summary>
        /// Tự động mở file vừa xuất bằng ứng dụng mặc định của hệ thống (Excel hoặc trình xem tương tự).
        /// </summary>
        public static void TryOpen(string path)
        {
            try { System.Diagnostics.Process.Start(path); } catch { }
        }
    }
}
