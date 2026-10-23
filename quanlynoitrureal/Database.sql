USE master;
GO

-- 1. Nếu Database đã tồn tại: đóng mọi kết nối và XÓA HẲN Database cũ để làm sạch hoàn toàn
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Quanlybenhnhan123456')
BEGIN
    ALTER DATABASE Quanlybenhnhan123456 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Quanlybenhnhan123456;
END
GO

-- 2. TẠO MỚI DATABASE SẠCH SẼ
CREATE DATABASE Quanlybenhnhan123456;
GO

USE Quanlybenhnhan123456;
GO

-- ----------------------------------------------------------------------------
-- 3. TẠO CÁC BẢNG DANH MỤC CƠ BẢN
-- ----------------------------------------------------------------------------

-- 3.1 Bảng TÀI KHOẢN (Đăng nhập)
CREATE TABLE TAIKHOAN (
    TenDangNhap  VARCHAR(50) PRIMARY KEY,
    MatKhau      VARCHAR(100) NOT NULL,
    HoTen        NVARCHAR(100) NOT NULL,
    VaiTro       NVARCHAR(50) NOT NULL DEFAULT N'Quản trị',
    MaKhoa       CHAR(5) NULL,
    TrangThai    BIT NOT NULL DEFAULT 1
);
GO

-- 3.2 Bảng KHOA
CREATE TABLE KHOA (
    MaKhoa    CHAR(5) PRIMARY KEY,
    TenKhoa   NVARCHAR(100) NOT NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    GhiChu    NVARCHAR(200) NULL
);
GO

-- 3.3 Bảng PHÒNG
CREATE TABLE PHONG (
    MaPhong   CHAR(5) PRIMARY KEY,
    TenPhong  NVARCHAR(100) NOT NULL,
    MaKhoa    CHAR(5) NOT NULL,
    DonGia    MONEY NOT NULL DEFAULT 150000,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_PHONG_KHOA FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);
GO

-- 3.4 Bảng GIƯỜNG (Khớp cả NhapVienForm: SoGiuong + TrangThai chữ 'Trống'/'Đang sử dụng')
CREATE TABLE GIUONG (
    MaGiuong  CHAR(5) PRIMARY KEY,
    TenGiuong NVARCHAR(100) NULL,
    SoGiuong  INT NOT NULL DEFAULT 1,
    MaPhong   CHAR(5) NOT NULL,
    DonGia    MONEY NOT NULL DEFAULT 0,
    TrangThai NVARCHAR(30) NOT NULL DEFAULT N'Trống',
    CONSTRAINT FK_GIUONG_PHONG FOREIGN KEY (MaPhong) REFERENCES PHONG(MaPhong)
);
GO

-- 3.5 Bảng BÁC SĨ (Khớp BacSiForm)
CREATE TABLE BACSI (
    MaBS        CHAR(5) PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    NgaySinh    DATE NULL,
    GioiTinh    NVARCHAR(10) NULL,
    ChuyenKhoa  NVARCHAR(100) NULL,
    SoDienThoai VARCHAR(20) NULL,
    Email       VARCHAR(100) NULL,
    CCCD        VARCHAR(20) NULL,
    ChucDanh    NVARCHAR(50) NULL,
    BangCap     NVARCHAR(100) NULL,
    VaiTro      NVARCHAR(50) DEFAULT N'Bác sĩ',
    MaKhoa      CHAR(5) NULL,
    TrangThai   BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_BACSI_KHOA FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);
GO

-- 3.6 Bảng BỆNH NHÂN (Khớp đầy đủ BenhNhanForm + PRD)
CREATE TABLE BENHNHAN (
    MaBN            CHAR(5) PRIMARY KEY,
    HoTen           NVARCHAR(100) NOT NULL,
    NgaySinh        DATE NOT NULL,
    GioiTinh        NVARCHAR(10) NOT NULL,
    CCCD            VARCHAR(20) NULL,
    SoDienThoai     VARCHAR(20) NULL,
    DiaChi          NVARCHAR(200) NULL,
    NgheNghiep      NVARCHAR(100) NULL,
    NguoiThan       NVARCHAR(100) NULL,
    SDTNguoiThan    VARCHAR(20) NULL,
    TinhTrang       NVARCHAR(200) NULL,
    BHYT            VARCHAR(30) NULL,
    TrangThai       NVARCHAR(50) DEFAULT N'Chờ'
);
GO

-- 3.7 Bảng THUỐC (Khớp ThuocForm + DonThuocForm)
CREATE TABLE THUOC (
    MaThuoc     CHAR(5) PRIMARY KEY,
    TenThuoc    NVARCHAR(100) NOT NULL,
    DonViTinh   NVARCHAR(20) NOT NULL DEFAULT N'Viên',
    DonGia      MONEY NOT NULL,
    SoLuongTon  INT NOT NULL DEFAULT 0,
    PT_BaoHiem  FLOAT DEFAULT 0,
    TrangThai   BIT NOT NULL DEFAULT 1,
    CONSTRAINT CK_THUOC_SL CHECK (SoLuongTon >= 0)
);
GO

-- 3.8 Bảng DỊCH VỤ (Khớp DichVuForm)
CREATE TABLE DICHVU (
    MaDV        CHAR(5) PRIMARY KEY,
    TenDV       NVARCHAR(100) NOT NULL,
    DonGia      MONEY NOT NULL,
    PT_BaoHiem  FLOAT DEFAULT 0,
    GhiChu      NVARCHAR(200) NULL,
    TrangThai   BIT NOT NULL DEFAULT 1
);
GO

-- 3.9 Bảng NHÂN VIÊN
CREATE TABLE NHANVIEN (
    MaNV        CHAR(5) PRIMARY KEY,
    HoTen       NVARCHAR(100) NOT NULL,
    ChucVu      NVARCHAR(50) NULL,
    VaiTro      NVARCHAR(50) NOT NULL DEFAULT N'Nhân viên',
    MaKhoa      CHAR(5) NULL,
    TrangThai   BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_NV_KHOA FOREIGN KEY (MaKhoa) REFERENCES KHOA(MaKhoa)
);
GO

-- ----------------------------------------------------------------------------
-- 4. CÁC BẢNG NGHIỆP VỤ NỘI TRÚ
-- ----------------------------------------------------------------------------

-- 4.1 Bảng NHẬP VIỆN (Có cột NgayRaVien)
CREATE TABLE NHAPVIEN (
    MaNhapVien   CHAR(5) PRIMARY KEY,
    MaBN         CHAR(5) NOT NULL,
    MaBS         CHAR(5) NOT NULL,
    MaGiuong     CHAR(5) NOT NULL,
    NgayNhapVien DATETIME NOT NULL DEFAULT GETDATE(),
    NgayRaVien   DATETIME NULL,
    LyDoNhapVien NVARCHAR(200) NULL,
    TrangThai    NVARCHAR(50) NOT NULL DEFAULT N'Đang điều trị',
    GhiChu       NVARCHAR(200) NULL,
    CONSTRAINT FK_NV_BN FOREIGN KEY (MaBN) REFERENCES BENHNHAN(MaBN),
    CONSTRAINT FK_NV_BS FOREIGN KEY (MaBS) REFERENCES BACSI(MaBS),
    CONSTRAINT FK_NV_GIUONG FOREIGN KEY (MaGiuong) REFERENCES GIUONG(MaGiuong)
);
GO

-- 4.2 Bảng BỆNH ÁN (Khớp BenhAnForm)
CREATE TABLE BENHAN (
    MaBA          CHAR(5) PRIMARY KEY,
    MaNhapVien    CHAR(5) NOT NULL,
    NgayLap       DATETIME NOT NULL DEFAULT GETDATE(),
    TrieuChung    NVARCHAR(200) NULL,
    ChanDoan      NVARCHAR(200) NULL,
    KetQuaKham    NVARCHAR(500) NULL,
    HuongDieuTri  NVARCHAR(500) NULL,
    CONSTRAINT FK_BA_NV FOREIGN KEY (MaNhapVien) REFERENCES NHAPVIEN(MaNhapVien)
);
GO

-- 4.3 Bảng ĐƠN THUỐC (Khớp DonThuocForm)
CREATE TABLE DONTHUOC (
    MaDon       CHAR(5) PRIMARY KEY,
    MaBA        CHAR(5) NOT NULL,
    NgayKeDon   DATETIME NOT NULL DEFAULT GETDATE(),
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT FK_DT_BA FOREIGN KEY (MaBA) REFERENCES BENHAN(MaBA)
);
GO

-- 4.4 Bảng CHI TIẾT ĐƠN THUỐC (Khớp CT_DONTHUOC)
CREATE TABLE CT_DONTHUOC (
    MaDon       CHAR(5) NOT NULL,
    MaThuoc     CHAR(5) NOT NULL,
    SoLuong     INT NOT NULL,
    LieuDung    NVARCHAR(100) NULL,
    CachDung    NVARCHAR(200) NULL,
    DonGia      MONEY NOT NULL,
    ThanhTien   MONEY NOT NULL,
    PRIMARY KEY (MaDon, MaThuoc),
    CONSTRAINT FK_CTDT_DT FOREIGN KEY (MaDon) REFERENCES DONTHUOC(MaDon),
    CONSTRAINT FK_CTDT_THUOC FOREIGN KEY (MaThuoc) REFERENCES THUOC(MaThuoc)
);
GO

-- 4.5 Bảng LỊCH ĐIỀU TRỊ / DỊCH VỤ SỬ DỤNG
CREATE TABLE LICH_DIEU_TRI (
    MaLDT       CHAR(5) PRIMARY KEY,
    MaNhapVien  CHAR(5) NOT NULL,
    MaDV        CHAR(5) NOT NULL,
    NgayDieuTri DATETIME NOT NULL DEFAULT GETDATE(),
    SoLuong     INT NOT NULL DEFAULT 1,
    DonGia      MONEY NOT NULL,
    PT_BaoHiem  FLOAT DEFAULT 0,
    GhiChu      NVARCHAR(200) NULL,
    CONSTRAINT FK_LDT_NV FOREIGN KEY (MaNhapVien) REFERENCES NHAPVIEN(MaNhapVien),
    CONSTRAINT FK_LDT_DV FOREIGN KEY (MaDV) REFERENCES DICHVU(MaDV)
);
GO

-- 4.6 Bảng VIỆN PHÍ (Khớp VienPhiForm + XuatVienForm)
CREATE TABLE VIENPHI (
    MaVP          CHAR(5) PRIMARY KEY,
    MaNhapVien    CHAR(5) NOT NULL UNIQUE,
    NgayLap       DATETIME NOT NULL DEFAULT GETDATE(),
    SoNgay        INT NOT NULL DEFAULT 1,
    TienPhong     MONEY NOT NULL DEFAULT 0,
    TienThuoc     MONEY NOT NULL DEFAULT 0,
    TienKham      MONEY NOT NULL DEFAULT 0,
    TienDichVu    MONEY NOT NULL DEFAULT 0,
    TongTien      MONEY NOT NULL DEFAULT 0,
    BHTra         MONEY NOT NULL DEFAULT 0,
    BNTra         MONEY NOT NULL DEFAULT 0,
    TrangThai     NVARCHAR(50) NOT NULL DEFAULT N'Chưa thanh toán',
    PhuongThucTT  NVARCHAR(50) NULL,
    NgayThanhToan DATETIME NULL,
    CONSTRAINT FK_VP_NV FOREIGN KEY (MaNhapVien) REFERENCES NHAPVIEN(MaNhapVien)
);
GO

-- ----------------------------------------------------------------------------
-- 5. CHÈN DỮ LIỆU MẪU ĐẦY ĐỦ
-- ----------------------------------------------------------------------------

-- Tài khoản đăng nhập (mật khẩu 123456)
INSERT INTO TAIKHOAN(TenDangNhap, MatKhau, HoTen, VaiTro, TrangThai) VALUES
('1', '1', N'Quản trị hệ thống', N'Quản trị', 1);

-- Khoa
INSERT INTO KHOA(MaKhoa, TenKhoa, TrangThai, GhiChu) VALUES
('KH01', N'Khoa Cấp cứu', 1, N'Khu nhà A tầng 1'),
('KH02', N'Khoa Nội tim mạch', 1, N'Khu nhà B tầng 2'),
('KH03', N'Khoa Ngoại chấn thương', 1, N'Khu nhà C tầng 3'),
('KH04', N'Khoa Hồi sức tích cực', 1, N'Khu nhà A tầng 2');

-- Phòng
INSERT INTO PHONG(MaPhong, TenPhong, MaKhoa, DonGia, TrangThai) VALUES
('PH01', N'Phòng Cấp cứu 101', 'KH01', 200000, 1),
('PH02', N'Phòng Tim mạch 201', 'KH02', 150000, 1),
('PH03', N'Phòng Ngoại 301', 'KH03', 180000, 1);

-- Giường
INSERT INTO GIUONG(MaGiuong, TenGiuong, SoGiuong, MaPhong, DonGia, TrangThai) VALUES
('GI01', N'Giường 1', 1, 'PH01', 200000, N'Trống'),
('GI02', N'Giường 2', 2, 'PH01', 200000, N'Trống'),
('GI03', N'Giường 1', 1, 'PH02', 150000, N'Trống'),
('GI04', N'Giường 2', 2, 'PH02', 150000, N'Trống'),
('GI05', N'Giường 1', 1, 'PH03', 180000, N'Trống');

-- Bác sĩ
INSERT INTO BACSI(MaBS, HoTen, NgaySinh, GioiTinh, ChuyenKhoa, SoDienThoai, Email, MaKhoa, TrangThai) VALUES
('BS01', N'BS. Nguyễn Văn A', '1980-05-12', N'Nam', N'Hồi sức cấp cứu', '0912345678', 'bsa@bv.vn', 'KH01', 1),
('BS02', N'BS. Trần Thị Mai', '1985-08-20', N'Nữ', N'Tim mạch can thiệp', '0987654321', 'bsmai@bv.vn', 'KH02', 1),
('BS03', N'BS. Lê Hoàng Nam', '1979-11-03', N'Nam', N'Phẫu thuật chấn thương', '0903112233', 'bsnam@bv.vn', 'KH03', 1);

-- Bệnh nhân
INSERT INTO BENHNHAN(MaBN, HoTen, NgaySinh, GioiTinh, CCCD, SoDienThoai, DiaChi, NgheNghiep, NguoiThan, SDTNguoiThan, TinhTrang, BHYT, TrangThai) VALUES
('BN01', N'Trần Văn Bình', '1990-02-15', N'Nam', '001090012345', '0978111222', N'Hà Nội', N'Kỹ sư', N'Vợ: Lê Thị Hoa', '0978999888', N'Tỉnh táo, khó thở nhẹ', 'DN4010123456789', N'Chờ'),
('BN02', N'Lê Thị Hương', '1982-10-25', N'Nữ', '001182098765', '0912333444', N'Bắc Ninh', N'Giáo viên', N'Chồng: Vũ Văn Toàn', '0912555666', N'Đau ngực từng cơn', 'GD4010987654321', N'Chờ'),
('BN03', N'Phạm Quang Huy', '1995-07-08', N'Nam', '001095033445', '0934555777', N'Hải Phòng', N'Tự do', N'Mẹ: Đỗ Thị Lan', '0934888999', N'Chấn thương phần mềm', '', N'Chờ');

-- Thuốc
INSERT INTO THUOC(MaThuoc, TenThuoc, DonViTinh, DonGia, SoLuongTon, PT_BaoHiem, TrangThai) VALUES
('TH01', N'Paracetamol 500mg', N'Viên', 2500, 1000, 80, 1),
('TH02', N'Amoxicillin 500mg', N'Viên', 5000, 500, 80, 1),
('TH03', N'Cefuroxim 500mg', N'Viên', 12000, 300, 50, 1),
('TH04', N'Omeprazol 20mg', N'Viên', 4000, 600, 80, 1),
('TH05', N'Dung dịch NaCl 0.9% 500ml', N'Chai', 18000, 200, 80, 1);

-- Dịch vụ
INSERT INTO DICHVU(MaDV, TenDV, DonGia, PT_BaoHiem, GhiChu, TrangThai) VALUES
('DV01', N'Chụp X-quang tim phổi thẳng', 120000, 80, N'Khoa Chẩn đoán hình ảnh', 1),
('DV02', N'Siêu âm tim Doppler màu', 250000, 80, N'Khoa Thăm dò chức năng', 1),
('DV03', N'Xét nghiệm công thức máu', 85000, 80, N'Khoa Xét nghiệm', 1),
('DV04', N'Điện tâm đồ (ECG)', 50000, 80, N'Khoa Cấp cứu', 1);

-- Ca nhập viện mẫu đang điều trị
INSERT INTO NHAPVIEN(MaNhapVien, MaBN, MaBS, MaGiuong, NgayNhapVien, NgayRaVien, LyDoNhapVien, TrangThai, GhiChu) VALUES
('NV01', 'BN01', 'BS01', 'GI01', DATEADD(day, -3, GETDATE()), NULL, N'Đau ngực, sốt cao', N'Đang điều trị', N'Theo dõi phản ứng thuốc');

UPDATE GIUONG SET TrangThai = N'Đang sử dụng' WHERE MaGiuong = 'GI01';
UPDATE BENHNHAN SET TrangThai = N'Đang nằm viện' WHERE MaBN = 'BN01';

-- Bệnh án mẫu
INSERT INTO BENHAN(MaBA, MaNhapVien, NgayLap, TrieuChung, ChanDoan, KetQuaKham, HuongDieuTri) VALUES
('BA01', 'NV01', DATEADD(day, -3, GETDATE()), N'Sốt 39 độ, khó thở nhẹ', N'Viêm phổi thùy cấp', N'Phổi ran ẩm rải rác', N'Kháng sinh, hạ sốt, truyền dịch');

-- Đơn thuốc mẫu
INSERT INTO DONTHUOC(MaDon, MaBA, NgayKeDon, GhiChu) VALUES
('DT01', 'BA01', DATEADD(day, -3, GETDATE()), N'Uống sau ăn');

INSERT INTO CT_DONTHUOC(MaDon, MaThuoc, SoLuong, LieuDung, CachDung, DonGia, ThanhTien) VALUES
('DT01', 'TH01', 10, N'2 viên/ngày', N'Uống khi sốt > 38.5 độ', 2500, 25000),
('DT01', 'TH03', 6, N'2 viên/ngày', N'Uống sáng - chiều', 12000, 72000);

-- Viện phí mẫu
INSERT INTO VIENPHI(MaVP, MaNhapVien, NgayLap, SoNgay, TienPhong, TienThuoc, TienKham, TienDichVu, TongTien, BHTra, BNTra, TrangThai) VALUES
('VP01', 'NV01', GETDATE(), 3, 600000, 97000, 50000, 120000, 867000, 520000, 347000, N'Chưa thanh toán');
GO