using System;
using System.Windows.Forms;

namespace QuanLyNoiTru.dotnet
{
    /// <summary>
    /// Lớp Program: Điểm khởi đầu (Entry Point) của toàn bộ ứng dụng Windows Forms.
    /// Mọi chương trình C# đều bắt đầu thực thi từ hàm Main() trong lớp này.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// [STAThread] (Single-Threaded Apartment): Thuộc tính bắt buộc cho ứng dụng WinForms,
        /// giúp các thành phần giao diện như hộp thoại mở file, kéo thả, clipboard hoạt động đúng cách.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Bật giao diện trực quan theo phong cách hiện đại của Windows (nút bấm bo tròn, màu sắc chuẩn)
            Application.EnableVisualStyles();
            // Đảm bảo văn bản trên các điều khiển hiển thị tương thích tốt
            Application.SetCompatibleTextRenderingDefault(false);

            // =========================================================================
            // VÒNG LẶP ĐĂNG NHẬP - ĐĂNG XUẤT CHÍNH CỦA CHƯƠNG TRÌNH:
            // Sử dụng vòng lặp while(true) để khi người dùng bấm "Đăng xuất" ở MainForm,
            // chương trình không bị tắt hẳn mà sẽ quay trở lại màn hình Đăng nhập (LoginForm).
            // =========================================================================
            while (true)
            {
                // BƯỚC 1: Hiển thị màn hình Đăng nhập (LoginForm)
                // Lệnh 'using' giúp tự động giải phóng bộ nhớ của form khi form đóng lại
                using (var login = new LoginForm())
                {
                    // Hiển thị hộp thoại LoginForm dưới dạng modal (chặn form khác đến khi đóng)
                    // Nếu người dùng bấm dấu [X] hoặc Hủy đăng nhập (khác OK) -> thoát vòng lặp, tắt app
                    if (login.ShowDialog() != DialogResult.OK) 
                        break;
                }

                // BƯỚC 2: Khi đăng nhập thành công (DialogResult == OK), mở Màn hình chính (MainForm)
                using (var main = new MainForm())
                {
                    // Chạy vòng lặp thông điệp Windows trên MainForm
                    Application.Run(main);

                    // Khi MainForm đóng lại:
                    // - Nếu người dùng bấm nút [Đăng xuất] -> thuộc tính LoggedOut = true -> tiếp tục vòng lặp while để mở lại LoginForm
                    // - Nếu người dùng bấm dấu [X] để thoát app -> LoggedOut = false -> break để thoát hoàn toàn
                    if (!main.LoggedOut) 
                        break;
                }
            }
        }
    }
}
