using System;
using System.Data;
using System.Data.SqlClient;

namespace QuanLyNoiTru.dotnet
{
    /// <summary>
    /// Lớp biểu diễn một câu lệnh SQL kèm theo danh sách tham số (Parameters).
    /// Được dùng trong các giao dịch (Transaction) khi cần thực thi nhiều thao tác SQL liên tiếp
    /// (ví dụ: vừa thêm hồ sơ nhập viện vừa chuyển trạng thái giường sang "Đang sử dụng").
    /// </summary>
    public class SqlStatement
    {
        /// <summary>Chuỗi truy vấn SQL (INSERT, UPDATE, DELETE...)</summary>
        public string Sql;

        /// <summary>Mảng các tham số an toàn đi kèm câu truy vấn</summary>
        public SqlParameter[] Parameters;

        /// <summary>
        /// Khởi tạo một đối tượng câu lệnh SQL kèm tham số
        /// </summary>
        public SqlStatement(string sql, params SqlParameter[] parameters)
        {
            Sql = sql;
            Parameters = parameters;
        }
    }

    /// <summary>
    /// Lớp DatabaseHelper: Tầng thao tác dữ liệu (Data Access Layer) duy nhất của dự án.
    /// Toàn bộ các Form trong ứng dụng KHÔNG trực tiếp tạo hay mở kết nối SQL,
    /// mà chỉ gọi thông qua các hàm tiện ích trong lớp này.
    /// 
    /// LỢI ÍCH:
    /// 1. Tập trung chuỗi kết nối (ConnectionString) tại 1 nơi duy nhất, dễ dàng thay đổi khi đổi máy.
    /// 2. Tự động mở và đóng kết nối an toàn bằng cấu trúc 'using', chống rò rỉ bộ nhớ/kết nối.
    /// 3. Sử dụng Parameter chuẩn để ngăn chặn 100% nguy cơ tấn công SQL Injection.
    /// 4. Hỗ trợ giao dịch (Transaction) đảm bảo tính toàn vẹn dữ liệu chuẩn ngân hàng/bệnh viện.
    /// </summary>
    public static class DatabaseHelper
    {
        // =========================================================================
        // CHUỖI KẾT NỐI (CONNECTION STRING) TỚI CƠ SỞ DỮ LIỆU SQL SERVER
        // - Server=.\SQLEXPRESS : Kết nối tới bản SQL Server Express trên máy cục bộ
        // - Database=Quanlybenhnhan123456 : Tên cơ sở dữ liệu của dự án
        // - Integrated Security=True : Sử dụng xác thực bằng tài khoản Windows hiện tại
        // =========================================================================
        public static string ConnectionString =
            @"Server=.\DBCUATAO;Database=Quanlybenhnhan123456;Integrated Security=True;TrustServerCertificate=True";

        // -------------------------------------------------------------------------
        // 1. QUẢN LÝ KẾT NỐI
        // -------------------------------------------------------------------------

        /// <summary>
        /// Tạo một đối tượng kết nối SqlConnection mới (chưa mở).
        /// Luôn sử dụng trong khối 'using' để kết nối tự động đóng sau khi chạy xong.
        /// </summary>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Thử mở kết nối tới SQL Server để kiểm tra xem server/database có hoạt động không.
        /// Thường gọi ở màn hình Đăng nhập để báo lỗi sớm nếu người dùng chưa bật SQL Server.
        /// </summary>
        /// <returns>True nếu kết nối thành công, False nếu thất bại</returns>
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open(); // Mở thử kết nối
                    return true;
                }
            }
            catch
            {
                return false; // Bắt ngoại lệ và trả về false, không làm chương trình bị văng lỗi crash
            }
        }

        // -------------------------------------------------------------------------
        // 2. THAM SỐ TRUY VẤN (SQL PARAMETER - CHỐNG SQL INJECTION)
        // -------------------------------------------------------------------------

        /// <summary>
        /// Tạo một tham số SqlParameter an toàn cho câu truy vấn SQL.
        /// Quy tắc xử lý: Nếu giá trị truyền vào là null hoặc chuỗi rỗng (""),
        /// hàm sẽ tự động đổi thành DBNull.Value để lưu giá trị NULL vào database một cách chuẩn xác.
        /// </summary>
        /// <param name="name">Tên tham số trong câu SQL (ví dụ: "@ten", "@ma")</param>
        /// <param name="value">Giá trị thực tế cần gán</param>
        public static SqlParameter Param(string name, object value)
        {
            var text = value as string;
            if (value == null || text != null && text.Trim().Length == 0)
                value = DBNull.Value;
            return new SqlParameter(name, value);
        }

        // -------------------------------------------------------------------------
        // 3. ĐỌC DỮ LIỆU TỪ CƠ SỞ DỮ LIỆU (SELECT)
        // -------------------------------------------------------------------------

        /// <summary>
        /// Thực thi câu lệnh SELECT và đổ dữ liệu vào một DataTable.
        /// DataTable này sau đó được gán trực tiếp làm nguồn dữ liệu (DataSource) cho DataGridView hoặc ComboBox.
        /// </summary>
        /// <param name="sql">Câu lệnh SELECT (ví dụ: "SELECT * FROM KHOA WHERE TrangThai=1")</param>
        /// <param name="parameters">Danh sách tham số (nếu có)</param>
        public static DataTable GetData(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var adapter = new SqlDataAdapter(sql, conn))
            {
                // Nếu có tham số truyền vào thì gắn tham số vào lệnh SELECT của adapter
                if (parameters != null)
                    adapter.SelectCommand.Parameters.AddRange(parameters);

                var table = new DataTable();
                // Phương thức Fill sẽ tự động mở kết nối, đọc dữ liệu đổ vào DataTable, rồi tự động đóng kết nối
                adapter.Fill(table);
                return table;
            }
        }

        /// <summary>
        /// Thực thi câu lệnh SQL trả về đúng 1 giá trị duy nhất (ô đầu tiên ở dòng đầu tiên của kết quả).
        /// Thường dùng cho các câu lệnh đếm: COUNT(*), tính tổng: SUM(), tìm giá trị lớn nhất: MAX()...
        /// </summary>
        /// <param name="sql">Câu lệnh SQL (ví dụ: "SELECT COUNT(*) FROM BENHNHAN")</param>
        /// <param name="parameters">Danh sách tham số</param>
        /// <returns>Đối tượng kết quả (cần ép kiểu về int, string... khi sử dụng)</returns>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteScalar(); // Trả về 1 ô giá trị
            }
        }

        // -------------------------------------------------------------------------
        // 4. THÊM / SỬA / XÓA DỮ LIỆU (INSERT / UPDATE / DELETE)
        // -------------------------------------------------------------------------

        /// <summary>
        /// Thực thi câu lệnh thay đổi dữ liệu: INSERT, UPDATE, hoặc DELETE.
        /// </summary>
        /// <param name="sql">Câu lệnh SQL thực thi</param>
        /// <param name="parameters">Danh sách tham số</param>
        /// <returns>Số dòng trong bảng bị ảnh hưởng bởi câu lệnh</returns>
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (var conn = GetConnection())
            using (var cmd = new SqlCommand(sql, conn))
            {
                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                conn.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Thực thi NHIỀU câu lệnh SQL trong cùng một GIAO DỊCH (Transaction).
        /// NGUYÊN TẮC: Hoặc là tất cả cùng thành công (Commit), hoặc nếu 1 câu lệnh bị lỗi thì
        /// toàn bộ các câu lệnh trước đó đều bị hủy bỏ (Rollback), bảo vệ dữ liệu không bao giờ bị sai lệch.
        /// 
        /// VÍ DỤ THỰC TẾ:
        /// - Khi bệnh nhân Nhập viện: Tạo hồ sơ nhập viện (INSERT) + Chuyển trạng thái giường thành 'Đang sử dụng' (UPDATE).
        ///   Nếu cập nhật giường lỗi thì hồ sơ nhập viện cũng tự động bị hủy, không để xảy ra ca nhập viện "ảo".
        /// </summary>
        /// <param name="statements">Danh sách các câu lệnh SqlStatement cần chạy đồng thời</param>
        public static void ExecuteTransaction(params SqlStatement[] statements)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                // Bắt đầu một Transaction mới
                using (var tran = conn.BeginTransaction())
                {
                    try
                    {
                        // Chạy lần lượt từng câu lệnh trong transaction
                        foreach (var st in statements)
                        {
                            using (var cmd = new SqlCommand(st.Sql, conn, tran))
                            {
                                if (st.Parameters != null)
                                    cmd.Parameters.AddRange(st.Parameters);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        // Nếu tất cả các lệnh đều chạy êm xuôi -> Xác nhận lưu vĩnh viễn vào CSDL
                        tran.Commit();
                    }
                    catch
                    {
                        // Nếu có bất kỳ lỗi nào xảy ra -> Thu hồi lại toàn bộ, trả về trạng thái cũ
                        tran.Rollback();
                        throw; // Ném lỗi ra ngoài để Form bắt được và hiển thị thông báo thân thiện cho người dùng
                    }
                }
            }
        }

        // -------------------------------------------------------------------------
        // 5. CÁC HÀM TIỆN ÍCH NGHIỆP VỤ
        // -------------------------------------------------------------------------

        /// <summary>
        /// Tự động sinh mã tiếp theo theo tiền tố.
        /// Ví dụ:
        ///   NextId("BENHNHAN", "MaBN", "BN") -> Kiểm tra các mã hiện có như "BN01", "BN02"...
        ///   Tìm số lớn nhất (2), cộng thêm 1 (3), và trả về chuỗi "BN03".
        /// </summary>
        /// <param name="table">Tên bảng cần sinh mã (ví dụ: "BENHNHAN", "BACSI")</param>
        /// <param name="column">Tên cột khóa chính chứa mã (ví dụ: "MaBN", "MaBS")</param>
        /// <param name="prefix">Tiền tố của mã (ví dụ: "BN", "BS", "KH", "PH")</param>
        /// <returns>Mã mới được định dạng chuẩn (ví dụ: "BN01", "BN02"...)</returns>
        public static string NextId(string table, string column, string prefix)
        {
            // Cắt phần số phía sau tiền tố, chuyển sang kiểu INT để tìm số lớn nhất MAX()
            string sql = "SELECT ISNULL(MAX(CAST(SUBSTRING(" + column + ", " + (prefix.Length + 1) + ", 10) AS INT)), 0) + 1 " +
                         "FROM " + table + " WHERE " + column + " LIKE '" + prefix + "[0-9]%'";
            int next = Convert.ToInt32(ExecuteScalar(sql));
            // Định dạng số thành tối thiểu 2 chữ số (ví dụ: 1 -> "01", 12 -> "12")
            return prefix + next.ToString("D2");
        }

        /// <summary>
        /// Kiểm tra nhanh xem một câu truy vấn có trả về bản ghi nào hay không.
        /// Thường dùng để kiểm tra trùng mã, trùng CCCD, hoặc kiểm tra xem bản ghi đã tồn tại hay chưa.
        /// </summary>
        /// <returns>True nếu có ít nhất 1 dòng thỏa mãn điều kiện, False nếu không có</returns>
        public static bool Exists(string sql, params SqlParameter[] parameters)
        {
            return GetData(sql, parameters).Rows.Count > 0;
        }
    }
}
