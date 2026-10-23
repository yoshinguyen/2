using QuanLyNoiTru.dotnet;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QuanLyNoiTru
{
    /// <summary>
    /// Form quản lý danh mục Thuốc và Kho dược:
    /// - Quản lý thông tin: Mã thuốc, Tên thuốc, Số lượng tồn kho, Đơn giá và Tỷ lệ Bảo hiểm Y tế (% BHYT) thanh toán.
    /// - Hỗ trợ các thao tác Thêm, Sửa, Xóa và Tìm kiếm thuốc nhiều điều kiện độc lập (Mã thuốc, Tên thuốc).
    /// - Kết nối trực tiếp với chức năng kê đơn nội trú (kê đơn sẽ tự động trừ kho, hủy đơn sẽ hoàn lại kho).
    /// </summary>
    public partial class ThuocForm : BaseForm
    {
        /// <summary>
        /// Khởi tạo form danh mục thuốc, tải dữ liệu ban đầu và gán các sự kiện giao diện.
        /// </summary>
        public ThuocForm()
        {
            InitializeComponent();
            LoadData();

            // Khi click chọn dòng thuốc trên lưới -> hiển thị chi tiết lên các ô nhập
            dgv.SelectionChanged += (s, e) => FillInputs();

            // Nhấn phím Enter trong ô tìm kiếm để thực hiện tìm kiếm
            KeyEventHandler onEnter = (s, e) => { if (e.KeyCode == Keys.Enter) LoadData(); };
            txtTimMa.KeyDown += onEnter;
            txtTimTen.KeyDown += onEnter;
        }

        /// <summary>
        /// Nút [🔍 Tìm kiếm]: Thực hiện lọc danh sách thuốc theo điều kiện đã nhập.
        /// </summary>
        void btnTimKiem_Click(object s, EventArgs e)
        {
            LoadData();
        }

        /// <summary>
        /// Nút [🔄 Làm mới] ở thanh tìm kiếm: Xóa trắng các ô tìm kiếm và nạp lại toàn bộ danh mục thuốc.
        /// </summary>
        void btnLamMoiTim_Click(object s, EventArgs e)
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            LoadData();
        }

        /// <summary>
        /// Tải danh sách thuốc từ bảng THUOC theo nhiều điều kiện kết hợp (Mã thuốc và Tên thuốc).
        /// </summary>
        void LoadData()
        {
            string sql = @"
                SELECT MaThuoc AS [Mã thuốc], TenThuoc AS [Tên thuốc],
                       SoLuongTon AS [Tồn kho], DonGia AS [Đơn giá],
                       PT_BaoHiem AS [% BH]
                FROM THUOC
                WHERE 1=1 ";

            var ps = new List<SqlParameter>();

            string ma = txtTimMa.Text.Trim();
            if (!string.IsNullOrEmpty(ma))
            {
                sql += " AND MaThuoc LIKE @timMa ";
                ps.Add(DatabaseHelper.Param("@timMa", "%" + ma + "%"));
            }

            string ten = txtTimTen.Text.Trim();
            if (!string.IsNullOrEmpty(ten))
            {
                sql += " AND TenThuoc LIKE @timTen ";
                ps.Add(DatabaseHelper.Param("@timTen", "%" + ten + "%"));
            }

            sql += " ORDER BY MaThuoc";

            dgv.DataSource = DatabaseHelper.GetData(sql, ps.ToArray());
            FormatColumn(dgv, "Đơn giá", "N0");
        }

        /// <summary>
        /// Đưa thông tin từ dòng thuốc được chọn trên DataGridView lên các ô TextBox tương ứng.
        /// </summary>
        void FillInputs()
        {
            var r = dgv.CurrentRow;
            if (r == null) return;
            txtMa.Text = GetText(r, "Mã thuốc");
            txtTen.Text = GetText(r, "Tên thuốc");
            txtTonKho.Text = GetText(r, "Tồn kho");
            txtDonGia.Text = GetText(r, "Đơn giá");
            txtBH.Text = GetText(r, "% BH");
        }

        /// <summary>
        /// Xóa sạch dữ liệu trên các ô nhập liệu thuốc.
        /// </summary>
        void ClearInputs()
        {
            txtMa.Text = txtTen.Text = txtTonKho.Text = txtDonGia.Text = txtBH.Text = "";
        }

        /// <summary>
        /// Đặt lại form về trạng thái ban đầu: xóa ô tìm kiếm, tải lại bảng và bỏ chọn.
        /// </summary>
        void Reset()
        {
            txtTimMa.Text = "";
            txtTimTen.Text = "";
            LoadData();
            dgv.CurrentCell = null;
            ClearInputs();
        }

        /// <summary>
        /// Nút Làm mới: xóa dữ liệu đang nhập và làm mới danh sách.
        /// </summary>
        void btnLamMoi_Click(object s, EventArgs e) { Reset(); }

        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu thuốc:
        /// - Tên thuốc không được để trống.
        /// - Số lượng tồn kho phải là số nguyên >= 0.
        /// - Đơn giá phải là số >= 0.
        /// </summary>
        bool Valid()
        {
            if (txtTen.Text.Trim() == "") { ShowWarning("Vui lòng nhập tên thuốc."); txtTen.Focus(); return false; }
            int sl;
            if (!int.TryParse(txtTonKho.Text, out sl) || sl < 0)
            { ShowWarning("Tồn kho phải là số nguyên >= 0."); txtTonKho.Focus(); return false; }
            decimal dg;
            if (!decimal.TryParse(txtDonGia.Text, out dg) || dg < 0)
            { ShowWarning("Đơn giá phải là số >= 0."); txtDonGia.Focus(); return false; }
            return true;
        }

        /// <summary>
        /// Tạo mảng tham số SqlParameter cho các thao tác lưu dữ liệu thuốc.
        /// </summary>
        SqlParameter[] Ps(string ma)
        {
            double? bh = null;
            double bhVal;
            if (double.TryParse(txtBH.Text, out bhVal)) bh = bhVal;

            return new[]
            {
                DatabaseHelper.Param("@ma", ma),
                DatabaseHelper.Param("@ten", txtTen.Text.Trim()),
                DatabaseHelper.Param("@sl", int.Parse(txtTonKho.Text)),
                DatabaseHelper.Param("@gia", decimal.Parse(txtDonGia.Text)),
                DatabaseHelper.Param("@bh", (object)bh ?? DBNull.Value)
            };
        }

        /// <summary>
        /// Thêm thuốc mới vào danh mục với mã sinh tự động (TH...).
        /// </summary>
        void btnThem_Click(object s, EventArgs e)
        {
            if (!Valid()) return;
            try
            {
                string ma = DatabaseHelper.NextId("THUOC", "MaThuoc", "TH");
                DatabaseHelper.ExecuteNonQuery(
                    "INSERT INTO THUOC(MaThuoc, TenThuoc, SoLuongTon, DonGia, PT_BaoHiem) VALUES(@ma, @ten, @sl, @gia, @bh)", Ps(ma));
                Reset();
                ShowInfo("Đã thêm thuốc " + ma + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Cập nhật thông tin thuốc (tên, tồn kho, đơn giá, % BHYT).
        /// </summary>
        void btnSua_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn thuốc cần sửa."); return; }
            if (!Valid()) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE THUOC SET TenThuoc=@ten, SoLuongTon=@sl, DonGia=@gia, PT_BaoHiem=@bh WHERE MaThuoc=@ma", Ps(txtMa.Text));
                LoadData();
                ShowInfo("Đã cập nhật thuốc " + txtMa.Text + ".");
            }
            catch (Exception ex) { ShowError(ex); }
        }

        /// <summary>
        /// Xóa thuốc khỏi danh mục. Nếu thuốc đã được kê trong bất kỳ đơn thuốc nào thì hệ thống sẽ chặn xóa.
        /// </summary>
        void btnXoa_Click(object s, EventArgs e)
        {
            if (txtMa.Text == "") { ShowWarning("Hãy chọn thuốc cần xóa."); return; }
            if (!Ask("Xóa thuốc " + txtMa.Text + " - " + txtTen.Text + "?")) return;
            try
            {
                DatabaseHelper.ExecuteNonQuery("DELETE FROM THUOC WHERE MaThuoc=@ma", DatabaseHelper.Param("@ma", txtMa.Text));
                Reset();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Thuốc này đang được sử dụng trong đơn thuốc nên không thể xóa.");
            }
        }
    }
}
