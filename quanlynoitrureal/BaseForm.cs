using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Lớp BaseForm: Form cơ sở (Base Class) kế thừa từ System.Windows.Forms.Form.
    /// 
    /// MỤC ĐÍCH & Ý NGHĨA CHO NGƯỜI MỚI HỌC C#:
    /// Thay vì ở mỗi màn hình (Bệnh nhân, Bác sĩ, Nhập viện...) chúng ta phải viết đi viết lại
    /// các đoạn code giống nhau như: hiển thị MessageBox, kiểm tra lỗi SQL ngoại lệ, đọc ô trong lưới,
    /// đổ dữ liệu danh mục vào ComboBox... 
    /// Ta gom tất cả các hàm tiện ích đó vào BaseForm. Các form khác chỉ cần khai báo:
    ///    public partial class BenhNhanForm : BaseForm
    /// là nghiễm nhiên sở hữu và sử dụng được toàn bộ các hàm tiện ích bên dưới.
    /// Đây chính là nguyên lý Kế thừa (Inheritance) và Tái sử dụng mã nguồn (DRY) trong Lập trình Hướng đối tượng (OOP).
    /// </summary>
    public class BaseForm : Form
    {
        // =========================================================================
        // 1. CÁC HÀM TIỆN ÍCH HIỂN THỊ HỘP THOẠI THÔNG BÁO (DIALOG)
        // =========================================================================

        /// <summary>
        /// Hiển thị hộp thoại thông tin (dấu chấm than xanh 'i').
        /// Dùng khi: Thao tác thành công, nhắc nhở nhẹ nhàng cho người dùng.
        /// </summary>
        /// <param name="message">Nội dung thông báo cần hiển thị</param>
        protected void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Hiển thị hộp thoại cảnh báo (tam giác vàng '!').
        /// Dùng khi: Người dùng nhập thiếu dữ liệu, chọn sai điều kiện, vi phạm quy tắc nghiệp vụ.
        /// </summary>
        /// <param name="message">Nội dung cảnh báo</param>
        protected void ShowWarning(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Hiển thị hộp thoại xác nhận có 2 nút [Có / Không] (Yes/No).
        /// Thường dùng trước các hành động quan trọng không thể hoàn tác như: Xóa dữ liệu, Ra viện, Hủy hồ sơ.
        /// </summary>
        /// <param name="message">Câu hỏi cần người dùng xác nhận</param>
        /// <returns>True nếu người dùng bấm Có (Yes), False nếu bấm Không (No)</returns>
        protected bool Ask(string message)
        {
            return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// Bắt và phân tích lỗi từ SQL Server để hiển thị thông báo tiếng Việt dễ hiểu cho người dùng,
        /// thay vì hiện chuỗi tiếng Anh kỹ thuật gây khó hiểu.
        /// 
        /// CÁC MÃ LỖI SQL PHỔ BIẾN:
        /// - Lỗi 547: Vi phạm khóa ngoại (Foreign Key Constraint). Ví dụ: Không thể xóa Khoa khi vẫn còn Phòng bên trong.
        /// - Lỗi 2627 hoặc 2601: Vi phạm khóa chính hoặc trùng lặp dữ liệu (Unique Constraint). Ví dụ: Trùng mã bệnh nhân, trùng CCCD.
        /// </summary>
        /// <param name="ex">Ngoại lệ Exception bắt được từ khối try/catch</param>
        /// <param name="foreignKeyMessage">Thông báo tùy chỉnh khi vướng khóa ngoại (nếu để null sẽ dùng câu mặc định)</param>
        protected void ShowError(Exception ex, string foreignKeyMessage = null)
        {
            var sqlEx = ex as SqlException;

            // Kiểm tra nếu là lỗi do vi phạm ràng buộc khóa ngoại trong CSDL (Mã 547)
            if (sqlEx != null && sqlEx.Number == 547)
            {
                ShowWarning(foreignKeyMessage ?? "Dữ liệu đang được liên kết sử dụng ở bảng khác nên không thể xóa/sửa.");
            }
            // Kiểm tra lỗi trùng lặp khóa chính / trùng dữ liệu duy nhất (Mã 2627, 2601)
            else if (sqlEx != null && (sqlEx.Number == 2627 || sqlEx.Number == 2601))
            {
                ShowWarning("Dữ liệu bị trùng lặp (Mã hoặc thông tin đã tồn tại trong hệ thống).");
            }
            // Các lỗi hệ thống hoặc ngoại lệ khác
            else
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================================
        // 2. CÁC HÀM TIỆN ÍCH THAO TÁC DỮ LIỆU GIAO DIỆN (UI CONTROLS)
        // =========================================================================

        /// <summary>
        /// Đọc an toàn giá trị văn bản của một ô trong DataGridView theo tên cột.
        /// Tự động xử lý trường hợp ô có giá trị NULL hoặc rỗng để không bị văng lỗi 'NullReferenceException'.
        /// </summary>
        /// <param name="row">Dòng hiện tại đang chọn (DataGridViewRow)</param>
        /// <param name="column">Tên cột cần lấy dữ liệu</param>
        /// <returns>Chuỗi giá trị bên trong ô, nếu ô trống thì trả về chuỗi rỗng ""</returns>
        protected static string GetText(DataGridViewRow row, string column)
        {
            if (row == null || !row.DataGridView.Columns.Contains(column)) 
                return "";

            object value = row.Cells[column].Value;
            return (value == null || value == DBNull.Value) ? "" : value.ToString();
        }

        /// <summary>
        /// Đặt định dạng hiển thị cho một cột cụ thể trong DataGridView.
        /// Ví dụ: 
        /// - Cột ngày tháng: FormatColumn(dgv, "Ngày sinh", "dd/MM/yyyy")
        /// - Cột tiền tệ: FormatColumn(dgv, "Đơn giá", "N0")
        /// </summary>
        /// <param name="grid">Bảng DataGridView</param>
        /// <param name="column">Tên cột cần định dạng</param>
        /// <param name="format">Mẫu định dạng chuỗi</param>
        protected static void FormatColumn(DataGridView grid, string column, string format)
        {
            if (grid.Columns.Contains(column)) 
                grid.Columns[column].DefaultCellStyle.Format = format;
        }

        /// <summary>
        /// Đổ dữ liệu từ DataTable vào ComboBox (hộp chọn thả xuống).
        /// </summary>
        /// <param name="combo">Điều khiển ComboBox cần nạp</param>
        /// <param name="table">Bảng dữ liệu DataTable chứa danh sách</param>
        /// <param name="display">Tên cột sẽ hiển thị cho người dùng nhìn thấy (ví dụ: "TenKhoa")</param>
        /// <param name="value">Tên cột giá trị ẩn bên dưới dùng cho lập trình (ví dụ: "MaKhoa")</param>
        protected void FillCombo(ComboBox combo, DataTable table, string display, string value)
        {
            combo.DataSource = null;           // Xóa nguồn cũ trước khi gán mới
            combo.DisplayMember = display;     // Cột hiển thị
            combo.ValueMember = value;         // Cột giá trị
            combo.DataSource = table;          // Gán nguồn dữ liệu
        }

        /// <summary>
        /// Lấy giá trị mã (ValueMember) đang được chọn trong ComboBox.
        /// </summary>
        /// <param name="combo">Điều khiển ComboBox</param>
        /// <returns>Mã đang chọn dạng chuỗi (string), nếu chưa chọn trả về null</returns>
        protected static string GetValue(ComboBox combo)
        {
            return combo.SelectedValue == null ? null : combo.SelectedValue.ToString();
        }
    }
}
