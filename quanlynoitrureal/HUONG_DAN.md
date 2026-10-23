# Quản lý hồ sơ bệnh nhân nội trú (WinForms C# + SQL Server)

## Cách chạy
1. Mở **SQL Server Management Studio**, chạy file `Database.sql` (tạo database + dữ liệu mẫu).
2. Mở `QuanLyNoiTru.csproj` bằng **Visual Studio 2022** (workload ".NET desktop development", .NET Framework 4.8).
3. Mở `DatabaseHelper.cs`, sửa `ConnectionString` cho đúng tên SQL Server và tên database của máy bạn.
4. Bấm **F5**.

## Tài khoản mẫu (mật khẩu đều là `123456`)
| Tài khoản | Vai trò | Được dùng |
|---|---|---|
| admin | Admin | Tất cả chức năng |
| bacsi01 | Bác sĩ | Bệnh nhân, Nhập viện, Bệnh án, Đơn thuốc, Báo cáo |
| nhanvien01 | Nhân viên | Bệnh nhân, Nhập viện, Báo cáo |
| ketoan01 | Kế toán | Viện phí, Báo cáo |

## Cấu trúc: mỗi form có 2 file
| File | Nội dung | Ai sửa |
|---|---|---|
| `XxxForm.cs` | Code xử lý (nạp dữ liệu, thêm/sửa/xóa...) | Bạn |
| `XxxForm.Designer.cs` | Vị trí, kích thước, màu của control | Visual Studio |

**Sửa giao diện:** trong Solution Explorer bấm đúp `BenhNhanForm.cs` (hoặc form khác) -> mở Designer -> kéo thả,
đổi thuộc tính ở cửa sổ Properties. Visual Studio tự ghi lại vào `.Designer.cs`.
Muốn gắn code cho nút: bấm đúp vào nút trong Designer.

Nếu Designer báo lỗi khi mở lần đầu: **Build > Rebuild Solution** rồi mở lại.

## Danh sách file
| File | Vai trò |
|---|---|
| `DatabaseHelper.cs` | Kết nối SQL Server + các hàm GetData, ExecuteNonQuery, ExecuteScalar, ExecuteTransaction, Param, NextId, Exists, TestConnection |
| `Session.cs` | Người đăng nhập (`Session`) và định dạng tiền (`Fmt.Money`) |
| `BaseForm.cs` | Hàm dùng chung cho form: ShowInfo, ShowWarning, Ask, ShowError, GetText, GetValue, FillCombo, FormatColumn |
| `Program.cs` | Điểm bắt đầu: Đăng nhập -> Form chính |
| `LoginForm` | Đăng nhập |
| `MainForm` | Menu trái + vùng nội dung, phân quyền bằng cách ẩn nút |
| `BenhNhanForm`, `BacSiForm` | Quản lý bệnh nhân, bác sĩ |
| `NhapVienForm` | Nhập viện: Khoa -> Phòng -> Giường + Bác sĩ |
| `BenhAnForm`, `DonThuocForm` | Bệnh án, đơn thuốc (trừ/cộng tồn kho) |
| `VienPhiForm` | Viện phí, thanh toán, xuất hóa đơn |
| `BaoCaoForm`, `Export.cs` | Báo cáo dạng bảng: xem, in, xuất PDF, xuất Excel |

## DatabaseHelper - nơi duy nhất làm việc với SQL Server
| Hàm | Dùng để | Ví dụ |
|---|---|---|
| `GetData(sql, params)` | SELECT -> `DataTable` | `dgv.DataSource = DatabaseHelper.GetData("SELECT * FROM KHOA");` |
| `ExecuteNonQuery(sql, params)` | INSERT / UPDATE / DELETE | `DatabaseHelper.ExecuteNonQuery("DELETE FROM BACSI WHERE MaBS=@ma", DatabaseHelper.Param("@ma", ma));` |
| `ExecuteScalar(sql, params)` | Lấy 1 giá trị (COUNT, MAX...) | `DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM BENHNHAN");` |
| `ExecuteTransaction(...)` | Nhiều lệnh: lưu hết hoặc hủy hết | xem `NhapVienForm.btnTaoHoSo_Click` |
| `Param(name, value)` | Tạo tham số (chống SQL Injection), chuỗi rỗng -> NULL | `DatabaseHelper.Param("@ten", txtTen.Text)` |
| `NextId(table, col, prefix)` | Sinh mã tiếp theo (BN05, BS03...) | `DatabaseHelper.NextId("BENHNHAN","MaBN","BN")` |
| `Exists(sql, params)` | Kiểm tra có dòng nào không | kiểm tra trùng |
| `TestConnection()` | Thử kết nối, true/false | màn hình đăng nhập |

## Quy tắc nghiệp vụ đã cài
- Chỉ chọn được giường **Trống**; tạo hồ sơ -> giường thành **Đang sử dụng**; ra viện / hủy -> giường về **Trống**.
- Một bệnh nhân chỉ có 1 hồ sơ **Đang điều trị** tại một thời điểm.
- Tiền phòng = số ngày nằm x đơn giá phòng (tính cả ngày vào và ra, tối thiểu 1 ngày).
- Tiền thuốc = tổng thành tiền các đơn thuốc của lần nhập viện. Hóa đơn đã thanh toán thì không sửa được.
- Thêm/sửa/xóa thuốc trong đơn tự trừ/cộng lại tồn kho.

## Lưu ý
- Mật khẩu đang lưu chữ thường để khớp dữ liệu mẫu; sản phẩm thật nên băm (SHA-256/bcrypt).
- Xuất PDF dùng máy in ảo "Microsoft Print to PDF" (có sẵn trên Windows 10/11).
