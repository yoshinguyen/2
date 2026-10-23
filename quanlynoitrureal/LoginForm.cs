using QuanLyNoiTru.dotnet;
using System;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form Đăng nhập (LoginForm): Cửa sổ xác thực danh tính người dùng trước khi vào hệ thống.
    /// Kiểm tra tài khoản, mật khẩu từ bảng TAIKHOAN trong cơ sở dữ liệu SQL Server.
    /// </summary>
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent(); // Dựng giao diện người dùng (vị trí TextBox, nút bấm từ file LoginForm.Designer.cs)
        }

        /// <summary>
        /// Xử lý sự kiện khi người dùng bấm nút [Đăng nhập] hoặc nhấn Enter trên bàn phím.
        /// </summary>
        void btnLogin_Click(object sender, EventArgs e)
        {
            // BƯỚC 1: Kiểm tra kết nối tới cơ sở dữ liệu SQL Server
            // Nếu không kết nối được -> Hiện thông báo chỉ dẫn chi tiết để người dùng dễ khắc phục
            if (!DatabaseHelper.TestConnection())
            {
                MessageBox.Show(
                    "Không thể kết nối đến cơ sở dữ liệu SQL Server!\n\n" +
                    "Cách khắc phục:\n" +
                    "1. Kiểm tra dịch vụ SQL Server (SQLEXPRESS) trên máy đã bật chưa.\n" +
                    "2. Kiểm tra chuỗi kết nối ConnectionString trong file DatabaseHelper.cs.\n" +
                    "3. Đảm bảo bạn đã chạy file Database.sql trong SQL Server Management Studio.",
                    "Lỗi kết nối CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // BƯỚC 2: Kiểm tra tính hợp lệ của tài khoản và mật khẩu
                // Mệnh đề 'COLLATE Latin1_General_CS_AS' (Case-Sensitive): 
                // Bắt buộc SQL Server phải so sánh chính xác chữ HOA và chữ thường của mật khẩu (ví dụ: '123' khác '123A').
                // Điều kiện 'TrangThai = 1': Chỉ cho phép đăng nhập nếu tài khoản đang ở trạng thái Hoạt động.
                string sql = @"
                    SELECT TenDangNhap, HoTen, VaiTro 
                    FROM TAIKHOAN 
                    WHERE TenDangNhap = @u 
                      AND MatKhau COLLATE Latin1_General_CS_AS = @p 
                      AND TrangThai = 1";

                var table = DatabaseHelper.GetData(sql,
                    DatabaseHelper.Param("@u", txtUser.Text.Trim()),
                    DatabaseHelper.Param("@p", txtPass.Text));

                // Nếu không tìm thấy dòng nào khớp -> Báo sai tài khoản hoặc mật khẩu
                if (table.Rows.Count == 0)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác, hoặc tài khoản đã bị khóa.", 
                        "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPass.Focus();
                    txtPass.SelectAll();
                    return;
                }

                // BƯỚC 3: Đăng nhập thành công -> Lưu thông tin người dùng vào lớp Session
                // Dữ liệu này sẽ được dùng trên thanh tiêu đề và các hóa đơn, phiếu khám
                Session.User = table.Rows[0]["TenDangNhap"].ToString();
                Session.FullName = table.Rows[0]["HoTen"].ToString();
                Session.Role = table.Rows[0]["VaiTro"].ToString();

                // Gán DialogResult = OK: Báo hiệu cho vòng lặp trong Program.cs biết rằng 
                // form này đóng lại do đăng nhập THÀNH CÔNG, sẵn sàng mở MainForm
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi trong quá trình đăng nhập:\n" + ex.Message, 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
