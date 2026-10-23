# CẨM NANG HƯỚNG DẪN LẬP TRÌNH CHO NGƯỜI MỚI HỌC C#
## DỰ ÁN QUẢN LÝ BỆNH NHÂN NỘI TRÚ (QuanLyNoiTru)

Tài liệu này được biên soạn đặc biệt dành cho các bạn mới làm quen với ngôn ngữ C# và Windows Forms, giải thích chi tiết cơ chế hoạt động của toàn bộ các chức năng cốt lõi trong dự án: **Thêm, Sửa, Xóa, Tìm kiếm, Xuất file, và Liên kết dữ liệu đa bảng**.

---

## MỤC LỤC
1. [Kiến Trúc Tổng Thể Của Dự Án](#1-kiến-trúc-tổng-thể-của-dự-án)
2. [Cơ Chế Hoạt Động Của Các Chức Năng Cốt Lõi (CRUD & Search)](#2-cơ-chế-hoạt-động-của-các-chức-năng-cốt-lõi-crud--search)
   - [2.1 Tìm kiếm tức thời (Search)](#21-tìm-kiếm-tức-thời-search)
   - [2.2 Chọn dòng và đổ dữ liệu lên ô nhập (Select & Fill)](#22-chọn-dòng-và-đổ-dữ-liệu-lên-ô-nhập-select--fill)
   - [2.3 Thêm mới dữ liệu (Insert)](#23-thêm-mới-dữ-liệu-insert)
   - [2.4 Cập nhật dữ liệu (Update)](#24-cập-nhật-dữ-liệu-update)
   - [2.5 Xóa dữ liệu và bảo vệ khóa ngoại (Delete)](#25-xóa-dữ-liệu-và-bảo-vệ-khóa-ngoại-delete)
3. [Hướng Dẫn: Muốn Thêm Hoặc Bớt 1 Thành Phần (Trường/Cột) Thì Làm Như Thế Nào?](#3-hướng-dẫn-muốn-thêm-hoặc-bớt-1-thành-phần-trườngcột-thì-làm-như-thế-nào)
4. [Cơ Chế Liên Kết Dữ Liệu Giữa Các Bảng (Multi-Table Linking)](#4-cơ-chế-liên-kết-dữ-liệu-giữa-các-bảng-multi-table-linking)
   - [4.1 Quan hệ 1 - Nhiều (1 - N) và kỹ thuật SQL JOIN](#41-quan-hệ-1---nhiều-1---n-và-kỹ-thuật-sql-join)
   - [4.2 ComboBox phân cấp bậc (Cascading Dropdown: Khoa -> Phòng -> Giường)](#42-combobox-phân-cấp-bậc-cascading-dropdown-khoa---phòng---giường)
   - [4.3 Cơ chế Giao dịch (Transaction) đồng bộ nhiều bảng](#43-cơ-chế-giao-dịch-transaction-đồng-bộ-nhiều-bảng)
5. [Cơ Chế Hoạt Động Của Tính Năng Xuất File (In Ấn, PDF, Excel)](#5-cơ-chế-hoạt-động-của-tính-năng-xuất-file-in-ấn-pdf-excel)
   - [5.1 In ấn và Xem trước (Print & Print Preview)](#51-in-ấn-và-xem-trước-print--print-preview)
   - [5.2 Xuất file PDF tự nhiên](#52-xuất-file-pdf-tự-nhiên)
   - [5.3 Tự tạo file Excel (.xlsx) chuẩn OpenXML không cần Office](#53-tự-tạo-file-excel-xlsx-chuẩn-openxml-không-cần-office)
6. [Quy Trình Tạo Một Form Quản Lý Mới Kế Thừa BaseForm](#6-quy-trình-tạo-một-form-quản-lý-mới-kế-thừa-baseform)

---

## 1. KIẾN TRÚC TỔNG THỂ CỦA DỰ ÁN

Dự án được phân chia thành 3 tầng rõ ràng:
```
┌─────────────────────────────────────────────────────────────────┐
│                     TẦNG GIAO DIỆN (UI LAYER)                   │
│   Các Form kế thừa BaseForm: BenhNhanForm, NhapVienForm, ...   │
│   (Người dùng nhập liệu, bấm nút, xem bảng DataGridView)        │
└─────────────────────────────────┬───────────────────────────────┘
                                  │ Gọi các hàm tiện ích
┌─────────────────────────────────▼───────────────────────────────┐
│                  TẦNG HỖ TRỢ & TRUY CẬP CSDL                    │
│   - BaseForm.cs: Cung cấp ShowInfo, ShowWarning, FillCombo...   │
│   - DatabaseHelper.cs: Quản lý kết nối, tham số chống hack,     │
│     thực thi GetData, ExecuteNonQuery, ExecuteTransaction...    │
│   - Export.cs: In ấn, tạo PDF, tạo Excel .xlsx                  │
└─────────────────────────────────┬───────────────────────────────┘
                                  │ Gửi câu lệnh SQL qua ADO.NET
┌─────────────────────────────────▼───────────────────────────────┐
│               CƠ SỞ DỮ LIỆU (SQL SERVER DATABASE)               │
│   Database: Quanlybenhnhan123456                                │
│   Các bảng: BENHNHAN, BACSI, KHOA, PHONG, GIUONG, NHAPVIEN...   │
└─────────────────────────────────────────────────────────────────┘
```

---

## 2. CƠ CHẾ HOẠT ĐỘNG CỦA CÁC CHỨC NĂNG CỐT LÕI (CRUD & SEARCH)

### 2.1 Tìm kiếm tức thời (Search)
- **Cách hoạt động**: Khi bạn gõ vào ô `txtTim`, sự kiện `txtTim.TextChanged` lập tức kích hoạt và gọi hàm `LoadData()`.
- **Đoạn code trong form**:
  ```csharp
  txtTim.TextChanged += (s, e) => LoadData();
  ```
- **Cách truy vấn trong `LoadData()`**:
  ```csharp
  string sql = @"SELECT MaBN AS [Mã BN], HoTen AS [Họ tên], ...
                 FROM BENHNHAN
                 WHERE MaBN LIKE @k OR HoTen LIKE @k OR CCCD LIKE @k
                 ORDER BY MaBN";
  dgv.DataSource = DatabaseHelper.GetData(sql, DatabaseHelper.Param("@k", "%" + txtTim.Text.Trim() + "%"));
  ```
- **Giải thích**: Ký tự `%` ở hai đầu từ khóa biến câu lệnh SQL thành tìm kiếm "chứa từ khóa" (chỉ cần họ tên hoặc mã có chứa cụm từ bạn gõ là sẽ hiện ra). Hàm `DatabaseHelper.Param` tự động bảo vệ không bị lỗi cú pháp hay bị tấn công SQL Injection.

---

### 2.2 Chọn dòng và đổ dữ liệu lên ô nhập (Select & Fill)
- **Cách hoạt động**: Khi người dùng dùng chuột nhấp vào 1 dòng trên DataGridView, sự kiện `dgv.SelectionChanged` sẽ kích hoạt hàm `FillInputs()`.
- **Đoạn code**:
  ```csharp
  void FillInputs()
  {
      var r = dgv.CurrentRow; // Lấy dòng hiện tại đang được chọn
      if (r == null) return;

      txtMa.Text = GetText(r, "Mã BN");
      txtTen.Text = GetText(r, "Họ tên");
      if (r.Cells["Ngày sinh"].Value is DateTime) 
          dtpNs.Value = (DateTime)r.Cells["Ngày sinh"].Value;
      cmbGt.Text = GetText(r, "Giới tính");
      // ...
  }
  ```
- **Hàm `GetText(r, "Cột")` trong BaseForm**: Giúp bạn đọc an toàn dữ liệu từ ô của bảng. Nếu ô đó bị `NULL`, hàm tự trả về chuỗi rỗng `""` thay vì làm chương trình bị lỗi `NullReferenceException`.

---

### 2.3 Thêm mới dữ liệu (Insert)
Quy trình thêm mới chuẩn gồm 5 bước:
1. **Kiểm tra hợp lệ (`Valid()`)**:
   ```csharp
   if (txtTen.Text.Trim() == "") { ShowWarning("Vui lòng nhập họ tên."); return false; }
   ```
2. **Tự động sinh mã tiếp theo**:
   ```csharp
   string ma = DatabaseHelper.NextId("BENHNHAN", "MaBN", "BN"); 
   // Ví dụ trong CSDL đã có BN01, BN02 -> hàm tự trả về BN03
   ```
3. **Đóng gói tham số an toàn (`Ps(ma)`)**:
   ```csharp
   SqlParameter[] Ps(string ma) {
       return new[] {
           DatabaseHelper.Param("@ma", ma),
           DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
           // ...
       };
   }
   ```
4. **Thực thi SQL qua DatabaseHelper**:
   ```csharp
   string sql = "INSERT INTO BENHNHAN(MaBN, HoTen, ...) VALUES(@ma, @ten, ...)";
   DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
   ```
5. **Làm mới giao diện và thông báo**:
   ```csharp
   Reset(); // Nạp lại bảng và xóa trắng ô nhập
   ShowInfo("Đã thêm mới thành công bệnh nhân " + ma + ".");
   ```

---

### 2.4 Cập nhật dữ liệu (Update)
- **Cách hoạt động**: Người dùng nhấp chọn dòng trên bảng -> Sửa thông tin trên các ô TextBox -> Nhấn nút [Sửa/Cập nhật].
- **Code mẫu**:
  ```csharp
  void btnSua_Click(object s, EventArgs e)
  {
      if (txtMa.Text == "") { ShowWarning("Hãy chọn bệnh nhân cần sửa."); return; }
      if (!Valid()) return;

      string ma = txtMa.Text;
      string sql = "UPDATE BENHNHAN SET HoTen=@ten, NgaySinh=@ns ... WHERE MaBN=@ma";
      DatabaseHelper.ExecuteNonQuery(sql, Ps(ma));
      LoadData(); // Nạp lại bảng để người dùng thấy thông tin mới
      ShowInfo("Đã cập nhật bệnh nhân thành công.");
  }
  ```

---

### 2.5 Xóa dữ liệu và bảo vệ khóa ngoại (Delete)
- **Cách hoạt động**:
  1. Kiểm tra người dùng đã chọn dòng chưa.
  2. Bật hộp thoại hỏi xác nhận `Ask("...")` để tránh xóa nhầm.
  3. Thực thi câu lệnh `DELETE FROM BENHNHAN WHERE MaBN=@ma`.
  4. **Cực kỳ quan trọng**: Nếu bệnh nhân này đã từng nhập viện (bảng `NHAPVIEN` đang giữ khóa ngoại tham chiếu đến bệnh nhân này), SQL Server sẽ trả về mã lỗi `547`. Hàm `ShowError(ex, "...")` trong `BaseForm` sẽ bắt mã lỗi này và hiện câu nhắc thân thiện:
     ```csharp
     ShowError(ex, "Bệnh nhân này đã có hồ sơ nhập viện nên không thể xóa.");
     ```

---

## 3. HƯỚNG DẪN: MUỐN THÊM HOẶC BỚT 1 THÀNH PHẦN (TRƯỜNG/CỘT) THÌ LÀM NHƯ THẾ NÀO?

Giả sử bạn muốn **THÊM** trường **"Email"** vào bảng Bệnh nhân (`BENHNHAN`):

Bạn chỉ cần thực hiện đúng **8 bước** tuần tự sau:

1. **Bước 1 (Cơ sở dữ liệu)**: Mở SQL Server Management Studio (SSMS), chạy lệnh:
   ```sql
   ALTER TABLE BENHNHAN ADD Email NVARCHAR(100) NULL;
   ```
2. **Bước 2 (Giao diện Form)**: Mở `BenhNhanForm.cs` ở chế độ Design (hoặc code `Designer.cs`), kéo thêm 1 Label `"Email:"` và 1 TextBox đặt tên là `txtEmail`.
3. **Bước 3 (Hiển thị bảng `LoadData`)**: Trong câu lệnh SQL SELECT của hàm `LoadData()`, thêm cột mới:
   ```sql
   SELECT MaBN AS [Mã BN], HoTen AS [Họ tên], Email AS [Email], ...
   ```
4. **Bước 4 (Đổ dữ liệu lên ô `FillInputs`)**: Trong hàm `FillInputs()`, thêm dòng:
   ```csharp
   txtEmail.Text = GetText(r, "Email");
   ```
5. **Bước 5 (Xóa trắng ô `ClearInputs`)**: Trong hàm `ClearInputs()`, thêm dòng:
   ```csharp
   txtEmail.Text = "";
   ```
6. **Bước 6 (Kiểm tra hợp lệ `Valid`)**: Nếu muốn email bắt buộc phải có chữ `@`:
   ```csharp
   if (txtEmail.Text.Trim() != "" && !txtEmail.Text.Contains("@")) {
       ShowWarning("Địa chỉ Email không hợp lệ.");
       txtEmail.Focus();
       return false;
   }
   ```
7. **Bước 7 (Đóng gói tham số `Ps`)**: Trong hàm `Ps(string ma)`, thêm tham số:
   ```csharp
   DatabaseHelper.Param("@email", txtEmail.Text.Trim())
   ```
8. **Bước 8 (Thực thi Thêm & Sửa)**:
   - Trong `btnThem_Click`: Thêm cột `Email` và `@email` vào câu lệnh INSERT:
     ```sql
     INSERT INTO BENHNHAN(MaBN, HoTen, Email, ...) VALUES(@ma, @ten, @email, ...)
     ```
   - Trong `btnSua_Click`: Thêm `Email=@email` vào câu lệnh UPDATE:
     ```sql
     UPDATE BENHNHAN SET HoTen=@ten, Email=@email, ... WHERE MaBN=@ma
     ```

> **Ghi chú**: Nếu bạn muốn **BỚT (XÓA)** một trường (ví dụ bỏ trường "Nghề nghiệp"), bạn chỉ cần làm ngược lại toàn bộ 8 bước trên (Xóa ô ở Form, xóa khỏi SELECT, FillInputs, ClearInputs, Ps, INSERT và UPDATE).

---

## 4. CƠ CHẾ LIÊN KẾT DỮ LIỆU GIỮA CÁC BẢNG (MULTI-TABLE LINKING)

### 4.1 Quan hệ 1 - Nhiều (1 - N) và kỹ thuật SQL JOIN
Trong CSDL, để tránh trùng lặp dữ liệu, ta dùng các mã khóa ngoại (Foreign Key). Ví dụ:
- Bảng `PHONG` chỉ lưu `MaKhoa = 'K01'`
- Bảng `GIUONG` chỉ lưu `MaPhong = 'P101'`
- Bảng `NHAPVIEN` chỉ lưu `MaBN = 'BN01'`, `MaBS = 'BS02'`, `MaGiuong = 'GI05'`

Để hiển thị tên đầy đủ ra màn hình cho người dùng xem, ta dùng lệnh `JOIN`:
```sql
SELECT nv.MaNhapVien AS [Mã NV], 
       bn.HoTen AS [Bệnh nhân], 
       bs.HoTen AS [Bác sĩ], 
       p.TenPhong AS [Phòng], 
       g.TenGiuong AS [Giường]
FROM NHAPVIEN nv
JOIN BENHNHAN bn ON bn.MaBN = nv.MaBN
LEFT JOIN BACSI bs ON bs.MaBS = nv.MaBS
LEFT JOIN GIUONG g ON g.MaGiuong = nv.MaGiuong
LEFT JOIN PHONG p ON p.MaPhong = g.MaPhong
```
- Dùng `LEFT JOIN` để ngay cả khi hồ sơ chưa kịp gán bác sĩ hay giường thì dòng đó vẫn hiển thị trên bảng chứ không bị biến mất.

---

### 4.2 ComboBox phân cấp bậc (Cascading Dropdown: Khoa -> Phòng -> Giường)
Trong `NhapVienForm`, khi người dùng chọn Khoa thì danh sách Phòng tự đổi, chọn Phòng thì danh sách Giường tự đổi. Cơ chế được lập trình như sau:

```mermaid
flowchart LR
    A["Chọn Khoa (cmbKhoa)"] -->|"SelectedIndexChanged"| B["LoadPhongBS()<br/>SELECT FROM PHONG WHERE MaKhoa=@k"]
    B --> C["Đổ vào cmbPhong"]
    C -->|"SelectedIndexChanged"| D["LoadGiuong()<br/>SELECT FROM GIUONG WHERE MaPhong=@p"]
    D --> E["Đổ vào cmbGiuong<br/>(Chỉ lấy giường Trống)"]
```

- **Lưu ý biến `bool loading`**: Khi nạp dữ liệu vào ComboBox bằng lệnh `combo.DataSource = table`, hệ thống sẽ tự động kích hoạt sự kiện `SelectedIndexChanged`. Biến cờ `loading = true` trước khi nạp và `loading = false` sau khi nạp giúp chặn các sự kiện lặp không mong muốn.

---

### 4.3 Cơ chế Giao dịch (Transaction) đồng bộ nhiều bảng
Khi một nghiệp vụ y tế đòi hỏi cập nhật nhiều bảng cùng lúc, ta **bắt buộc** phải dùng `DatabaseHelper.ExecuteTransaction`.

#### Ví dụ 1: Bệnh nhân Nhập viện
- **Vấn đề**: Cần vừa thêm hồ sơ nhập viện (`INSERT NHAPVIEN`), vừa đổi trạng thái giường bệnh thành "Đang sử dụng" (`UPDATE GIUONG`).
- **Giải pháp**:
  ```csharp
  DatabaseHelper.ExecuteTransaction(
      new SqlStatement("INSERT INTO NHAPVIEN(...) VALUES(...)", ...),
      new SqlStatement("UPDATE GIUONG SET TrangThai = N'Đang sử dụng' WHERE MaGiuong = @g", ...)
  );
  ```
  Nếu lệnh cập nhật giường bị lỗi thì lệnh thêm hồ sơ nhập viện cũng tự động bị hủy bỏ (Rollback), bảo đảm giường và hồ sơ luôn khớp 100%.

#### Ví dụ 2: Kê đơn thuốc và Kho Dược (DonThuocForm)
- **Thêm thuốc vào đơn**:
  ```csharp
  DatabaseHelper.ExecuteTransaction(
      new SqlStatement("INSERT INTO CT_DONTHUOC(...) VALUES(...)", ...),
      new SqlStatement("UPDATE THUOC SET SoLuongTon = SoLuongTon - @sl WHERE MaThuoc = @t", ...)
  );
  ```
- **Xóa thuốc khỏi đơn**:
  ```csharp
  DatabaseHelper.ExecuteTransaction(
      new SqlStatement("DELETE FROM CT_DONTHUOC WHERE MaDon=@d AND MaThuoc=@t", ...),
      new SqlStatement("UPDATE THUOC SET SoLuongTon = SoLuongTon + @sl WHERE MaThuoc = @t", ...) // Hoàn lại kho
  );
  ```

#### Ví dụ 3: Hoàn tất Xuất viện (XuatVienForm - Quy tắc BR07)
- Hệ thống kiểm tra bệnh nhân đã thanh toán viện phí 100% chưa:
  ```csharp
  if (trangThaiVP != "Đã thanh toán") {
      ShowWarning("Không thể xuất viện! Bệnh nhân bắt buộc phải thanh toán 100% viện phí theo quy định BR07.");
      return;
  }
  ```
- Sau đó chạy Transaction 3 bảng đồng thời:
  ```csharp
  DatabaseHelper.ExecuteTransaction(
      new SqlStatement("UPDATE NHAPVIEN SET TrangThai = N'Đã xuất viện', NgayRaVien = @ra WHERE MaNhapVien = @nv", ...),
      new SqlStatement("UPDATE GIUONG SET TrangThai = N'Trống' WHERE MaGiuong = @g", ...), // Trả giường tự do
      new SqlStatement("UPDATE BENHNHAN SET TrangThai = N'Đã xuất viện' WHERE MaBN = @bn", ...)
  );
  ```

---

## 5. CƠ CHẾ HOẠT ĐỘNG CỦA TÍNH NĂNG XUẤT FILE (IN ẤN, PDF, EXCEL)

Tất cả các chức năng xuất file đều nằm tập trung trong file `Export.cs`, được viết bằng C# thuần túy, **không phụ thuộc bất kỳ thư viện bên ngoài hay cài đặt Microsoft Office nào**.

### 5.1 In ấn và Xem trước (Print & Print Preview)
- Sử dụng đối tượng chuẩn `PrintDocument` của thư viện `System.Drawing.Printing`.
- Trong hàm `TableDoc(title, dt)`:
  - Sự kiện `PrintPage` cung cấp đối tượng `Graphics` (như bảng vẽ).
  - Tự động đo độ dài dữ liệu để chia tỷ lệ độ rộng các cột (`weights`).
  - Dùng `g.DrawString()` để vẽ từng tiêu đề và ô dữ liệu.
  - **Tự động ngắt trang**: Khi dòng vẽ chạm đến đáy trang (`y + rh > b.Bottom`), gán `e.HasMorePages = true`. Hệ thống máy in sẽ tự động tạo trang 2, trang 3 cho đến khi in hết bảng dữ liệu.

---

### 5.2 Xuất file PDF tự nhiên
- Windows 10 và Windows 11 đều tích hợp sẵn máy in ảo tên là `"Microsoft Print to PDF"`.
- Trong hàm `Export.ExportPdf(doc, path)`:
  ```csharp
  doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";
  doc.PrinterSettings.PrintToFile = true;
  doc.PrinterSettings.PrintFileName = path;
  doc.Print();
  ```
- Không cần dùng iTextSharp hay Acrobat, tài liệu được in thẳng vào file PDF với chất lượng đồ họa vector sắc nét.

---

### 5.3 Tự tạo file Excel (.xlsx) chuẩn OpenXML không cần Office
- Một file Excel `.xlsx` thực chất là một file nén dạng `.zip` chứa các file tài liệu `.xml` theo chuẩn quốc tế Office Open XML.
- Trong hàm `Export.WriteXlsx(path, dt)`:
  1. Dùng `StringBuilder` để sinh nội dung file `sheet1.xml` với các thẻ:
     - `<row r="1">`: Đại diện cho 1 hàng
     - `<c r="A1" t="inlineStr"><is><t>Tiêu đề</t></is></c>`: Ô chứa văn bản
     - `<c r="B2"><v>150000</v></c>`: Ô chứa số (để người dùng có thể dùng hàm tính toán trong Excel)
  2. Dùng thư viện `ZipArchive` có sẵn của .NET để nén các file cấu hình `workbook.xml`, `[Content_Types].xml`, `_rels` và `sheet1.xml`.
  3. Kết quả tạo ra file `.xlsx` nhẹ, mở được trên mọi phiên bản Excel, Google Sheets mà máy tính không cần cài Microsoft Office.

---

## 6. QUY TRÌNH TẠO MỘT FORM QUẢN LÝ MỚI KẾ THỪA BASEFORM

Nếu bạn muốn tạo thêm một màn hình quản lý mới (ví dụ: Quản lý Vật tư y tế `VatTuForm`):

1. **Tạo Form mới**: Chuột phải vào Project -> Add -> Windows Form -> Đặt tên `VatTuForm.cs`.
2. **Kế thừa BaseForm**: Mở file code `VatTuForm.cs`, sửa khai báo:
   ```csharp
   public partial class VatTuForm : BaseForm // Thay vì : Form
   ```
3. **Khai báo các hàm chuẩn**:
   - `LoadData()`: Gọi `DatabaseHelper.GetData(...)` gán cho `dgv.DataSource`.
   - `FillInputs()`: Đọc từ dòng chọn qua `GetText(r, "Cột")` gán vào TextBox.
   - `ClearInputs()`: Xóa sạch các ô nhập.
   - `Reset()`: Gọi xóa ô tìm kiếm, nạp lại bảng và gọi `ClearInputs()`.
   - `Valid()`: Kiểm tra các điều kiện bắt buộc nhập.
   - `Ps(string ma)`: Tạo mảng `SqlParameter` qua `DatabaseHelper.Param(...)`.
   - `btnThem_Click`: Dùng `DatabaseHelper.NextId(...)` và `DatabaseHelper.ExecuteNonQuery(...)`.
   - `btnSua_Click`: Dùng `UPDATE ... WHERE Ma = @ma`.
   - `btnXoa_Click`: Dùng `DELETE ... WHERE Ma = @ma`, bọc trong `try-catch` gọi `ShowError(ex)`.
   - Gán sự kiện tìm kiếm: `txtTim.TextChanged += (s, e) => LoadData();`.

---
*Chúc các bạn học tập và phát triển ứng dụng C# Windows Forms thành công!*
