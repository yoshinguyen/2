using QuanLyNoiTru.dotnet;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Lớp MainForm: Màn hình điều khiển chính (Dashboard) của hệ thống.
    /// 
    /// KIẾN TRÚC GIAO DIỆN HIỆN ĐẠI (Panel Container Architecture):
    /// - Bên trái (pnlMenu): Thanh điều hướng chứa danh sách các nút chức năng (Bệnh nhân, Nhập viện, Viện phí, Xuất viện...).
    /// - Phía trên (pnlHeader): Hiển thị tiêu đề màn hình hiện tại và thông tin người dùng đang đăng nhập.
    /// - Trung tâm (pnlContent): Vùng chứa chính. Khi người dùng bấm nút nào, Form chức năng tương ứng
    ///   sẽ được nhúng trực tiếp vào pnlContent này như một điều khiển con (không bật cửa sổ rời rạc).
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Cờ đánh dấu người dùng bấm nút "Đăng xuất" (true) hay bấm nút [X] để thoát hẳn phần mềm (false).
        /// Biến này được Program.cs kiểm tra để quyết định có mở lại màn hình đăng nhập hay không.
        /// </summary>
        public bool LoggedOut;

        // Màu nền mặc định của các nút trên thanh menu bên trái (Xanh đậm)
        static readonly Color MenuColor = Color.FromArgb(30, 58, 95);

        // Màu nổi bật của nút menu đang được chọn/đang hoạt động (Xanh sáng)
        static readonly Color MenuActive = Color.FromArgb(52, 101, 164);

        // Mảng chứa toàn bộ các nút chức năng để dễ dàng duyệt qua và quản lý trạng thái
        readonly Button[] menuButtons;

        public MainForm()
        {
            InitializeComponent(); // Dựng giao diện từ MainForm.Designer.cs

            // Hiển thị lời chào và tên người đăng nhập trên góc phải thanh Header
            lblUser.Text = "Xin chào, " + Session.FullName;

            // Khởi tạo danh sách các nút menu chức năng
            menuButtons = new[] { 
                btnBenhNhan, btnNhapVien, btnBenhAn, btnDonThuoc, btnVienPhi, btnXuatVien,
                btnKhoa, btnPhong, btnGiuong, btnThuoc, btnDichVu, btnBacSi, btnBaoCao 
            };

            // Đảm bảo toàn bộ các nút chức năng luôn hiển thị đầy đủ cho quyền Quản trị viên
            foreach (var btn in menuButtons)
            {
                btn.Visible = true;
            }

            // Sự kiện Shown: Chạy ngay sau khi form chính xuất hiện lần đầu trên màn hình.
            // Tự động kích hoạt nút đầu tiên (Bệnh nhân) để người dùng có sẵn màn hình làm việc ngay lập tức.
            Shown += (s, e) =>
            {
                foreach (var b in menuButtons)
                {
                    if (b.Visible)
                    {
                        b.PerformClick(); // Giả lập hành động bấm chuột vào nút
                        break;
                    }
                }
            };
        }

        // =========================================================================
        // SỰ KIỆN CLICK CỦA CÁC NÚT MENU (GỌI HÀM OpenChild ĐỂ MỞ FORM TƯƠNG ỨNG)
        // =========================================================================
        void btnBenhNhan_Click(object sender, EventArgs e) { OpenChild(() => new BenhNhanForm(), btnBenhNhan, "Quản lý hồ sơ bệnh nhân"); }
        void btnNhapVien_Click(object sender, EventArgs e) { OpenChild(() => new NhapVienForm(), btnNhapVien, "Hồ sơ nhập viện nội trú"); }
        void btnBenhAn_Click(object sender, EventArgs e)   { OpenChild(() => new BenhAnForm(), btnBenhAn, "Bệnh án & Lịch sử khám"); }
        void btnDonThuoc_Click(object sender, EventArgs e) { OpenChild(() => new DonThuocForm(), btnDonThuoc, "Kê đơn thuốc"); }
        void btnVienPhi_Click(object sender, EventArgs e)  { OpenChild(() => new VienPhiForm(), btnVienPhi, "Quản lý viện phí"); }
        void btnXuatVien_Click(object sender, EventArgs e) { OpenChild(() => new XuatVienForm(), btnXuatVien, "Thủ tục xuất viện & Tổng hợp chi phí"); }
        void btnKhoa_Click(object sender, EventArgs e)     { OpenChild(() => new KhoaForm(), btnKhoa, "Danh mục Khoa"); }
        void btnPhong_Click(object sender, EventArgs e)    { OpenChild(() => new PhongForm(), btnPhong, "Danh mục Phòng bệnh"); }
        void btnGiuong_Click(object sender, EventArgs e)   { OpenChild(() => new GiuongForm(), btnGiuong, "Danh mục Giường bệnh"); }
        void btnThuoc_Click(object sender, EventArgs e)    { OpenChild(() => new ThuocForm(), btnThuoc, "Danh mục Thuốc & Tồn kho"); }
        void btnDichVu_Click(object sender, EventArgs e)   { OpenChild(() => new DichVuForm(), btnDichVu, "Danh mục Dịch vụ y tế"); }
        void btnBacSi_Click(object sender, EventArgs e)    { OpenChild(() => new BacSiForm(), btnBacSi, "Danh mục Bác sĩ điều trị"); }
        void btnBaoCao_Click(object sender, EventArgs e)   { OpenChild(() => new BaoCaoForm(), btnBaoCao, "Báo cáo & Thống kê"); }

        /// <summary>
        /// Xử lý sự kiện khi người dùng bấm nút [Đăng xuất] ở góc dưới cùng menu bên trái.
        /// </summary>
        void btnDangXuat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?", 
                "Xác nhận đăng xuất", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LoggedOut = true; // Bật cờ đánh dấu đăng xuất
                Close();          // Đóng MainForm, điều khiển sẽ trả về hàm Main() trong Program.cs
            }
        }

        /// <summary>
        /// KỸ THUẬT NHÚNG FORM CON VÀO PANEL (Panel Embedding / Sub-form Container):
        /// Thay vì mở cửa sổ con mới trôi nổi ngoài màn hình, hàm này chuyển Form con thành một 
        /// điều khiển đồ họa và gắn chặt nó vào pnlContent.
        /// </summary>
        /// <param name="create">Một hàm ẩn (Delegate Func) trả về đối tượng Form cần mở</param>
        /// <param name="clicked">Nút menu vừa được bấm (để đổi màu sáng Active)</param>
        /// <param name="title">Tiêu đề nghiệp vụ hiển thị lên thanh Header</param>
        void OpenChild(Func<Form> create, Button clicked, string title)
        {
            try
            {
                // BƯỚC 1: Khởi tạo Form con. Nếu việc kết nối CSDL trong Constructor của Form con bị lỗi,
                // ngoại lệ sẽ nhảy thẳng xuống catch, giữ nguyên màn hình cũ không bị đơ hoặc trắng xóa.
                Form child = create();

                // BƯỚC 2: Cập nhật màu sắc nút bấm trên Menu:
                // Trả toàn bộ nút về màu nền tối MenuColor, sau đó đổi riêng nút vừa bấm sang màu sáng MenuActive
                foreach (var b in menuButtons) 
                    b.BackColor = MenuColor;
                clicked.BackColor = MenuActive;

                // BƯỚC 3: Dọn dẹp Form con cũ đang nằm trong pnlContent (giải phóng RAM để tránh nặng máy)
                while (pnlContent.Controls.Count > 0)
                {
                    Control old = pnlContent.Controls[0];
                    pnlContent.Controls.Remove(old);
                    old.Dispose(); // Hủy và giải phóng tài nguyên đồ họa của form cũ
                }

                // BƯỚC 4: Cấu hình Form con để biến thành một thành phần nhúng bên trong Panel:
                // - TopLevel = false: Cho phép Form này đóng vai trò như một UserControl con (không có thanh tiêu đề riêng ngoài Windows)
                // - FormBorderStyle = None: Bỏ viền khung cửa sổ và các nút thu nhỏ/phóng to/[X]
                // - Dock = Fill: Tự động kéo giãn kích thước Form con vừa khít toàn bộ diện tích của pnlContent
                child.TopLevel = false;
                child.FormBorderStyle = FormBorderStyle.None;
                child.Dock = DockStyle.Fill;

                // BƯỚC 5: Thêm form con vào Panel và hiển thị
                pnlContent.Controls.Add(child);
                lblTitle.Text = title; // Đặt tiêu đề nghiệp vụ trên thanh Header
                child.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở chức năng này:\n" + ex.Message, "Lỗi nạp màn hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
