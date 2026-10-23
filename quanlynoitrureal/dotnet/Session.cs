using System;
using System.Globalization;
using System.Text;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Lớp Session: Lưu thông tin người dùng đang đăng nhập trong suốt phiên làm việc.
    /// Là một lớp static (tĩnh) để bất kỳ Form hay lớp nào trong phần mềm cũng có thể truy xuất
    /// trực tiếp thông tin người dùng hiện tại mà không cần truyền tham số qua lại.
    /// </summary>
    public static class Session
    {
        /// <summary>Tên đăng nhập (username) hiện tại (ví dụ: "admin", "bacsi01")</summary>
        public static string User;

        /// <summary>Họ tên đầy đủ của người đăng nhập (ví dụ: "BS. Nguyễn Văn A")</summary>
        public static string FullName;

        /// <summary>Vai trò trong hệ thống (ví dụ: "Quản trị", "Bác sĩ", "Thu ngân")</summary>
        public static string Role;

        /// <summary>
        /// Hàm tiện ích kiểm tra xem vai trò hiện tại của người dùng có khớp với một trong các vai trò được phép hay không.
        /// Sử dụng chuẩn hóa FormC để so sánh chuỗi tiếng Việt có dấu chuẩn xác (không bị lệch Unicode dựng sẵn/tổ hợp).
        /// </summary>
        /// <param name="roles">Danh sách các vai trò được phép (ví dụ: "Quản trị", "Bác sĩ")</param>
        /// <returns>True nếu người dùng có một trong các vai trò này, ngược lại False</returns>
        public static bool Is(params string[] roles)
        {
            string me = (Role ?? "").Normalize(NormalizationForm.FormC);
            foreach (var r in roles)
            {
                if (string.Equals(r.Normalize(NormalizationForm.FormC), me, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Lớp Fmt (Format): Định dạng hiển thị các loại dữ liệu đặc thù (tiền tệ, ngày tháng...).
    /// Giúp giao diện hiển thị đồng bộ và thống nhất theo định dạng chuẩn Việt Nam.
    /// </summary>
    public static class Fmt
    {
        // Văn hóa Việt Nam (vi-VN): dùng dấu chấm "." phân cách hàng nghìn (ví dụ: 1.500.000)
        static readonly CultureInfo Vn = new CultureInfo("vi-VN");

        /// <summary>
        /// Định dạng số tiền kiểu decimal sang chuỗi có dấu phân cách hàng nghìn và ký hiệu tiền tệ "đ".
        /// Ví dụ: 1500000 -> "1.500.000 đ"
        /// </summary>
        /// <param name="d">Số tiền dạng số thực (decimal)</param>
        /// <returns>Chuỗi tiền tệ đã được định dạng</returns>
        public static string Money(decimal d) 
        { 
            return d.ToString("N0", Vn) + " đ"; 
        }
    }
}
