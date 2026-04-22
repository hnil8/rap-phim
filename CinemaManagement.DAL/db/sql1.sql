-- ============================================================
-- HỆ THỐNG QUẢN LÝ RẠP CHIẾU PHIM
-- Các bảng Độc lập (Independent Tables)
-- Database: SQL Server
-- ============================================================

USE master;
GO

-- Tạo database nếu chưa tồn tại
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'CinemaDB')
BEGIN
    CREATE DATABASE CinemaDB COLLATE Vietnamese_CI_AS;
    PRINT N'>> Đã tạo database CinemaDB';
END
GO

USE CinemaDB;
GO

-- ============================================================
-- NHÓM 1: HỆ THỐNG & NHÂN SỰ
-- Thứ tự tạo: VaiTro → NhanVien → TaiKhoan
-- ============================================================

-- ------------------------------------------------------------
-- 1. BẢNG: VaiTro
-- Mục đích: Danh mục vai trò trong hệ thống
-- Không có FK → Tạo đầu tiên
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.VaiTro', 'U') IS NOT NULL DROP TABLE dbo.VaiTro;

CREATE TABLE dbo.VaiTro (
    VaiTroId    INT            IDENTITY(1,1)    NOT NULL,
    TenVaiTro   NVARCHAR(50)                    NOT NULL,  -- 'Admin', 'Quan ly', 'Nhan vien'
    MoTa        NVARCHAR(200)                   NULL,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,  -- Soft Delete: 0=Dùng, 1=Đã xóa

    CONSTRAINT PK_VaiTro              PRIMARY KEY (VaiTroId),
    CONSTRAINT UQ_VaiTro_TenVaiTro    UNIQUE (TenVaiTro)
);
PRINT N'>> Da tao bang VaiTro';
GO

-- ------------------------------------------------------------
-- 2. BẢNG: NhanVien
-- Mục đích: Thông tin nhân viên
-- Không có FK → Tạo trước TaiKhoan
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.NhanVien', 'U') IS NOT NULL DROP TABLE dbo.NhanVien;

CREATE TABLE dbo.NhanVien (
    NhanVienId      INT            IDENTITY(1,1)    NOT NULL,
    HoTen           NVARCHAR(100)                   NOT NULL,
    SoDienThoai     NVARCHAR(15)                    NULL,
    Email           NVARCHAR(100)                   NULL,
    GioiTinh        NVARCHAR(10)                    NULL,    -- 'Nam', 'Nu', 'Khac'
    NgaySinh        DATE                            NULL,
    DiaChi          NVARCHAR(250)                   NULL,
    NgayVaoLam      DATE           DEFAULT GETDATE() NOT NULL,
    IsDeleted       BIT            DEFAULT 0        NOT NULL, -- Soft Delete
    NgayXoa         DATETIME                        NULL,    -- Ghi lại thời điểm xóa mềm

    CONSTRAINT PK_NhanVien PRIMARY KEY (NhanVienId)
);

-- Index thường xuyên tìm kiếm theo tên và SĐT
CREATE INDEX IX_NhanVien_HoTen       ON dbo.NhanVien (HoTen);
CREATE INDEX IX_NhanVien_SoDienThoai ON dbo.NhanVien (SoDienThoai);
PRINT N'>> Da tao bang NhanVien';
GO

-- ------------------------------------------------------------
-- 3. BẢNG: TaiKhoan
-- Mục đích: Tài khoản đăng nhập, liên kết NhanVien và VaiTro
-- FK đến: VaiTro, NhanVien
-- QUAN TRỌNG: Không lưu mật khẩu plain text - chỉ lưu hash (SHA256/BCrypt)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.TaiKhoan', 'U') IS NOT NULL DROP TABLE dbo.TaiKhoan;

CREATE TABLE dbo.TaiKhoan (
    TaiKhoanId       INT            IDENTITY(1,1)    NOT NULL,
    TenDangNhap      NVARCHAR(50)                    NOT NULL,
    MatKhauHash      NVARCHAR(256)                   NOT NULL,  -- Lưu BCrypt hash, KHÔNG plain text
    VaiTroId         INT                             NOT NULL,
    NhanVienId       INT                             NOT NULL,
    IsActive         BIT            DEFAULT 1        NOT NULL,  -- 1=Hoạt động, 0=Bị khóa
    NgayTao          DATETIME       DEFAULT GETDATE() NOT NULL,
    LanDangNhapCuoi  DATETIME                        NULL,      -- Ghi lại lần đăng nhập gần nhất
    IsDeleted        BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_TaiKhoan               PRIMARY KEY (TaiKhoanId),
    CONSTRAINT UQ_TaiKhoan_TenDangNhap   UNIQUE (TenDangNhap),
    CONSTRAINT UQ_TaiKhoan_NhanVienId    UNIQUE (NhanVienId), -- 1 NV chỉ có 1 tài khoản

    CONSTRAINT FK_TaiKhoan_VaiTro        FOREIGN KEY (VaiTroId)
        REFERENCES dbo.VaiTro (VaiTroId),
    CONSTRAINT FK_TaiKhoan_NhanVien      FOREIGN KEY (NhanVienId)
        REFERENCES dbo.NhanVien (NhanVienId)
);

CREATE INDEX IX_TaiKhoan_VaiTroId ON dbo.TaiKhoan (VaiTroId);
PRINT N'>> Da tao bang TaiKhoan';
GO


-- ============================================================
-- NHÓM 2: CƠ SỞ VẬT CHẤT
-- Thứ tự: LoaiGhe → PhongChieu → GheNgoi
-- ============================================================

-- ------------------------------------------------------------
-- 4. BẢNG: LoaiGhe
-- Mục đích: Loại ghế và hệ số nhân giá
-- Không có FK → Tạo trước GheNgoi
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.LoaiGhe', 'U') IS NOT NULL DROP TABLE dbo.LoaiGhe;

CREATE TABLE dbo.LoaiGhe (
    LoaiGheId   INT            IDENTITY(1,1)    NOT NULL,
    TenLoai     NVARCHAR(50)                    NOT NULL,   -- 'Thuong', 'VIP', 'Sweetbox'
    HeSoGia     DECIMAL(5,2)   DEFAULT 1.00    NOT NULL,   -- 1.00=Thường, 1.5=VIP, 2.0=Sweetbox
    MoTa        NVARCHAR(200)                   NULL,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_LoaiGhe        PRIMARY KEY (LoaiGheId),
    CONSTRAINT UQ_LoaiGhe_Ten    UNIQUE (TenLoai),
    CONSTRAINT CK_LoaiGhe_HeSo   CHECK (HeSoGia > 0)
);
PRINT N'>> Da tao bang LoaiGhe';
GO

-- ------------------------------------------------------------
-- 5. BẢNG: PhongChieu
-- Mục đích: Phòng chiếu phim
-- Không có FK → Tạo trước GheNgoi và LichChieu
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.PhongChieu', 'U') IS NOT NULL DROP TABLE dbo.PhongChieu;

CREATE TABLE dbo.PhongChieu (
    PhongId     INT            IDENTITY(1,1)    NOT NULL,
    TenPhong    NVARCHAR(100)                   NOT NULL,   -- 'Phong 1', 'Phong IMAX'
    SucChua     INT                             NOT NULL,   -- Tổng số ghế trong phòng
    LoaiPhong   NVARCHAR(50)   DEFAULT N'2D'   NULL,       -- '2D', '3D', 'IMAX', '4DX'
    TrangThai   NVARCHAR(20)   DEFAULT N'HoatDong' NOT NULL, -- 'HoatDong', 'BaoTri', 'DongCua'
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_PhongChieu        PRIMARY KEY (PhongId),
    CONSTRAINT UQ_PhongChieu_Ten    UNIQUE (TenPhong),
    CONSTRAINT CK_PhongChieu_TT     CHECK (TrangThai IN (N'HoatDong', N'BaoTri', N'DongCua')),
    CONSTRAINT CK_PhongChieu_SC     CHECK (SucChua > 0)
);
PRINT N'>> Da tao bang PhongChieu';
GO

-- ------------------------------------------------------------
-- 6. BẢNG: GheNgoi
-- Mục đích: Ghế vật lý trong phòng, tổ chức theo ma trận Dãy/Cột
-- FK đến: PhongChieu, LoaiGhe
-- Tính năng đặc biệt: Cột TenGhe tự động tính từ DayGhe + CotGhe
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.GheNgoi', 'U') IS NOT NULL DROP TABLE dbo.GheNgoi;

CREATE TABLE dbo.GheNgoi (
    GheId       INT            IDENTITY(1,1)    NOT NULL,
    PhongId     INT                             NOT NULL,
    LoaiGheId   INT                             NOT NULL,
    DayGhe      CHAR(2)                         NOT NULL,   -- 'A', 'B', ..., 'J'
    CotGhe      INT                             NOT NULL,   -- 1, 2, ..., 15
    TenGhe      AS (DayGhe + CAST(CotGhe AS VARCHAR(3))) PERSISTED,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_GheNgoi            PRIMARY KEY (GheId),
    CONSTRAINT UQ_GheNgoi_ViTri      UNIQUE (PhongId, DayGhe, CotGhe),

    CONSTRAINT FK_GheNgoi_Phong      FOREIGN KEY (PhongId)
        REFERENCES dbo.PhongChieu (PhongId),
    CONSTRAINT FK_GheNgoi_LoaiGhe    FOREIGN KEY (LoaiGheId)
        REFERENCES dbo.LoaiGhe (LoaiGheId)
);

CREATE INDEX IX_GheNgoi_PhongId  ON dbo.GheNgoi (PhongId);
PRINT N'>> Da tao bang GheNgoi';
GO


-- ============================================================
-- NHÓM 3: THỂ LOẠI PHIM & PHIM
-- ============================================================

-- ------------------------------------------------------------
-- 7. BẢNG: TheLoaiPhim
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.TheLoaiPhim', 'U') IS NOT NULL DROP TABLE dbo.TheLoaiPhim;

CREATE TABLE dbo.TheLoaiPhim (
    TheLoaiId   INT            IDENTITY(1,1)    NOT NULL,
    TenTheLoai  NVARCHAR(100)                   NOT NULL,   -- 'Hanh dong', 'Hai', 'Kinh di'
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_TheLoaiPhim       PRIMARY KEY (TheLoaiId),
    CONSTRAINT UQ_TheLoaiPhim_Ten   UNIQUE (TenTheLoai)
);
PRINT N'>> Da tao bang TheLoaiPhim';
GO

-- ------------------------------------------------------------
-- 8. BẢNG: Phim
-- Mục đích: Thông tin chi tiết phim
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.Phim', 'U') IS NOT NULL DROP TABLE dbo.Phim;

CREATE TABLE dbo.Phim (
    PhimId            INT            IDENTITY(1,1)    NOT NULL,
    TenPhim           NVARCHAR(200)                   NOT NULL,
    TenGoc            NVARCHAR(200)                   NULL,       -- Tên tiếng Anh / tên gốc
    DaoDien           NVARCHAR(200)                   NULL,
    DienVienChinh     NVARCHAR(500)                   NULL,       -- Phân cách bởi dấu phẩy
    ThoiLuongPhut     INT                             NOT NULL,   -- Thời lượng tính bằng phút
    NuocSanXuat       NVARCHAR(100)                   NULL,
    NamPhatHanh       INT                             NULL,
    GioiHanDoTuoi     NVARCHAR(10)   DEFAULT N'P'    NOT NULL,   -- 'P', 'C13', 'C16', 'C18'
    NgonNgu           NVARCHAR(50)   DEFAULT N'VietSub' NULL,    -- 'VietSub', 'ThuyetMinh', 'Goc'
    MoTa              NVARCHAR(2000)                  NULL,       -- Nội dung / synopsis
    PosterUrl         NVARCHAR(500)                   NULL,
    TrailerUrl        NVARCHAR(500)                   NULL,
    TrangThai         NVARCHAR(20)   DEFAULT N'SapChieu' NOT NULL,
    NgayKhoiChieu     DATE                            NULL,
    IsDeleted         BIT            DEFAULT 0        NOT NULL,
    NgayTao           DATETIME       DEFAULT GETDATE() NOT NULL,

    CONSTRAINT PK_Phim            PRIMARY KEY (PhimId),
    CONSTRAINT CK_Phim_ThoiLuong  CHECK (ThoiLuongPhut > 0),
    CONSTRAINT CK_Phim_DoTuoi     CHECK (GioiHanDoTuoi IN (N'P', N'C13', N'C16', N'C18')),
    CONSTRAINT CK_Phim_TrangThai  CHECK (TrangThai IN (N'SapChieu', N'DangChieu', N'NgungChieu'))
);

CREATE INDEX IX_Phim_TenPhim   ON dbo.Phim (TenPhim);
CREATE INDEX IX_Phim_TrangThai ON dbo.Phim (TrangThai);
PRINT N'>> Da tao bang Phim';
GO


-- ============================================================
-- NHÓM 5: GIÁ VÉ & KHUYẾN MÃI
-- ============================================================

-- ------------------------------------------------------------
-- 9. BẢNG: QuyTacGia (Dynamic Pricing)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.QuyTacGia', 'U') IS NOT NULL DROP TABLE dbo.QuyTacGia;

CREATE TABLE dbo.QuyTacGia (
    QuyTacId     INT            IDENTITY(1,1)    NOT NULL,
    TenQuyTac    NVARCHAR(100)                   NOT NULL,   -- 'Cuoi tuan - Toi - Nguoi lon'
    LoaiNgay     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  -- 'TatCa', 'NgayThuong', 'CuoiTuan'
    KhungGio     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  -- 'TatCa', 'SangTrua', 'ChieuToi'
    GioTu        TIME                            NULL,       -- VD: '17:00:00'
    GioDen       TIME                            NULL,       -- VD: '23:59:00'
    DoiTuong     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  -- 'TatCa', 'NguoiLon', 'SinhVien', 'TreEm'
    GiaCoBan     DECIMAL(18,0)                   NOT NULL,
    IsActive     BIT            DEFAULT 1        NOT NULL,
    IsDeleted    BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_QuyTacGia           PRIMARY KEY (QuyTacId),
    CONSTRAINT CK_QuyTacGia_GiaCoBan  CHECK (GiaCoBan > 0),
    CONSTRAINT CK_QuyTacGia_LoaiNgay  CHECK (LoaiNgay IN (N'TatCa', N'NgayThuong', N'CuoiTuan')),
    CONSTRAINT CK_QuyTacGia_DoiTuong  CHECK (DoiTuong IN (N'TatCa', N'NguoiLon', N'SinhVien', N'TreEm'))
);
PRINT N'>> Da tao bang QuyTacGia';
GO

-- ------------------------------------------------------------
-- 10. BẢNG: KhuyenMai (Voucher / Coupon)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.KhuyenMai', 'U') IS NOT NULL DROP TABLE dbo.KhuyenMai;

CREATE TABLE dbo.KhuyenMai (
    KhuyenMaiId      INT            IDENTITY(1,1)    NOT NULL,
    TenChuongTrinh   NVARCHAR(200)                   NOT NULL,
    MaCode           NVARCHAR(50)                    NOT NULL,
    LoaiGiam         NVARCHAR(20)                    NOT NULL,   -- 'PhanTram' hoac 'TienMat'
    GiaTriGiam       DECIMAL(18,0)                   NOT NULL,
    GiaTriGiamToiDa  DECIMAL(18,0)                   NULL,
    SoLuongPhatHanh  INT                             NULL,
    SoLuongDaDung    INT            DEFAULT 0        NOT NULL,
    NgayBatDau       DATETIME                        NOT NULL,
    NgayHetHan       DATETIME                        NOT NULL,
    IsActive         BIT            DEFAULT 1        NOT NULL,
    IsDeleted        BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_KhuyenMai           PRIMARY KEY (KhuyenMaiId),
    CONSTRAINT UQ_KhuyenMai_MaCode    UNIQUE (MaCode),
    CONSTRAINT CK_KhuyenMai_LoaiGiam  CHECK (LoaiGiam IN (N'PhanTram', N'TienMat')),
    CONSTRAINT CK_KhuyenMai_GiaTri    CHECK (GiaTriGiam > 0),
    CONSTRAINT CK_KhuyenMai_ThoiGian  CHECK (NgayHetHan > NgayBatDau)
);

CREATE INDEX IX_KhuyenMai_MaCode ON dbo.KhuyenMai (MaCode);
PRINT N'>> Da tao bang KhuyenMai';
GO


-- ============================================================
-- NHÓM 6: F&B - ĐỒ ĂN THỨC UỐNG
-- ============================================================

-- ------------------------------------------------------------
-- 11. BẢNG: NhomFnB
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.NhomFnB', 'U') IS NOT NULL DROP TABLE dbo.NhomFnB;

CREATE TABLE dbo.NhomFnB (
    NhomId      INT            IDENTITY(1,1)    NOT NULL,
    TenNhom     NVARCHAR(100)                   NOT NULL,
    BieuTuong   NVARCHAR(100)                   NULL,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_NhomFnB       PRIMARY KEY (NhomId),
    CONSTRAINT UQ_NhomFnB_Ten   UNIQUE (TenNhom)
);
PRINT N'>> Da tao bang NhomFnB';
GO

-- ------------------------------------------------------------
-- 12. BẢNG: SanPhamFnB
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.SanPhamFnB', 'U') IS NOT NULL DROP TABLE dbo.SanPhamFnB;

CREATE TABLE dbo.SanPhamFnB (
    SanPhamId   INT            IDENTITY(1,1)    NOT NULL,
    NhomId      INT                             NOT NULL,
    TenSanPham  NVARCHAR(200)                   NOT NULL,
    MoTa        NVARCHAR(500)                   NULL,
    GiaBan      DECIMAL(18,0)                   NOT NULL,
    TonKho      INT            DEFAULT 0        NOT NULL,
    HinhAnhUrl  NVARCHAR(500)                   NULL,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_SanPhamFnB          PRIMARY KEY (SanPhamId),
    CONSTRAINT CK_SanPhamFnB_Gia      CHECK (GiaBan >= 0),
    CONSTRAINT CK_SanPhamFnB_TonKho   CHECK (TonKho >= 0),

    CONSTRAINT FK_SanPhamFnB_Nhom     FOREIGN KEY (NhomId)
        REFERENCES dbo.NhomFnB (NhomId)
);

CREATE INDEX IX_SanPhamFnB_NhomId ON dbo.SanPhamFnB (NhomId);
PRINT N'>> Da tao bang SanPhamFnB';
GO

-- ------------------------------------------------------------
-- 13. BẢNG: Combo
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.Combo', 'U') IS NOT NULL DROP TABLE dbo.Combo;

CREATE TABLE dbo.Combo (
    ComboId     INT            IDENTITY(1,1)    NOT NULL,
    TenCombo    NVARCHAR(200)                   NOT NULL,
    MoTa        NVARCHAR(500)                   NULL,
    GiaCombo    DECIMAL(18,0)                   NOT NULL,
    HinhAnhUrl  NVARCHAR(500)                   NULL,
    IsActive    BIT            DEFAULT 1        NOT NULL,
    IsDeleted   BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_Combo     PRIMARY KEY (ComboId),
    CONSTRAINT CK_Combo_Gia CHECK (GiaCombo > 0)
);
PRINT N'>> Da tao bang Combo';
GO

-- ------------------------------------------------------------
-- 14. BẢNG: ChiTietCombo
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.ChiTietCombo', 'U') IS NOT NULL DROP TABLE dbo.ChiTietCombo;

CREATE TABLE dbo.ChiTietCombo (
    ChiTietComboId  INT    IDENTITY(1,1)  NOT NULL,
    ComboId         INT                   NOT NULL,
    SanPhamId       INT                   NOT NULL,
    SoLuong         INT    DEFAULT 1      NOT NULL,

    CONSTRAINT PK_ChiTietCombo           PRIMARY KEY (ChiTietComboId),
    CONSTRAINT UQ_ChiTietCombo_Item      UNIQUE (ComboId, SanPhamId),
    CONSTRAINT CK_ChiTietCombo_SL        CHECK (SoLuong > 0),

    CONSTRAINT FK_ChiTietCombo_Combo     FOREIGN KEY (ComboId)
        REFERENCES dbo.Combo (ComboId),
    CONSTRAINT FK_ChiTietCombo_SanPham   FOREIGN KEY (SanPhamId)
        REFERENCES dbo.SanPhamFnB (SanPhamId)
);
PRINT N'>> Da tao bang ChiTietCombo';
GO


-- ============================================================
-- NHÓM 7: KHÁCH HÀNG & THÀNH VIÊN
-- ============================================================

-- ------------------------------------------------------------
-- 15. BẢNG: HangThanhVien
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.HangThanhVien', 'U') IS NOT NULL DROP TABLE dbo.HangThanhVien;

CREATE TABLE dbo.HangThanhVien (
    HangId           INT            IDENTITY(1,1)    NOT NULL,
    TenHang          NVARCHAR(50)                    NOT NULL,   -- 'Thanh vien', 'VIP', 'VVIP'
    DiemToiThieu     INT            DEFAULT 0        NOT NULL,  -- Mốc điểm tối thiểu để đạt hạng
    PhanTramUuDai    DECIMAL(5,2)   DEFAULT 0        NOT NULL,  -- % giảm thêm khi mua vé
    MoTa             NVARCHAR(300)                   NULL,
    MauSac           NVARCHAR(20)                    NULL,       -- Màu badge VD: '#FFD700' (vàng)
    IsDeleted        BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_HangThanhVien           PRIMARY KEY (HangId),
    CONSTRAINT UQ_HangThanhVien_Ten       UNIQUE (TenHang),
    CONSTRAINT CK_HangThanhVien_Diem      CHECK (DiemToiThieu >= 0),
    CONSTRAINT CK_HangThanhVien_UuDai     CHECK (PhanTramUuDai >= 0 AND PhanTramUuDai <= 100)
);
PRINT N'>> Da tao bang HangThanhVien';
GO

-- ------------------------------------------------------------
-- 16. BẢNG: KhachHang (Đã tích hợp MatKhauHash và AvatarUrl cho Web)
-- Mục đích: Thông tin khách hàng, tài khoản Web và tích điểm
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.KhachHang', 'U') IS NOT NULL DROP TABLE dbo.KhachHang;

CREATE TABLE dbo.KhachHang (
    KhachHangId     INT            IDENTITY(1,1)    NOT NULL,
    HoTen           NVARCHAR(100)                   NOT NULL,
    SoDienThoai     NVARCHAR(15)                    NOT NULL,   -- Dùng tra cứu nhanh tại quầy POS
    Email           NVARCHAR(100)                   NULL,
    MatKhauHash     NVARCHAR(256)                   NULL,       -- Phục vụ đăng nhập nền tảng Web
    AvatarUrl       NVARCHAR(500)                   NULL,       -- Ảnh đại diện trải nghiệm Web
    NgaySinh        DATE                            NULL,
    GioiTinh        NVARCHAR(10)                    NULL,
    DiemTichLuy     INT            DEFAULT 0        NOT NULL,
    HangId          INT                             NULL,       -- NULL = Khách vãng lai, chưa có thẻ
    NgayDangKy      DATETIME       DEFAULT GETDATE() NOT NULL,
    IsDeleted       BIT            DEFAULT 0        NOT NULL,

    CONSTRAINT PK_KhachHang          PRIMARY KEY (KhachHangId),
    CONSTRAINT UQ_KhachHang_SoDT     UNIQUE (SoDienThoai),
    CONSTRAINT CK_KhachHang_Diem     CHECK (DiemTichLuy >= 0),

    CONSTRAINT FK_KhachHang_Hang     FOREIGN KEY (HangId)
        REFERENCES dbo.HangThanhVien (HangId)
);

CREATE INDEX IX_KhachHang_SoDienThoai ON dbo.KhachHang (SoDienThoai);
CREATE INDEX IX_KhachHang_HoTen       ON dbo.KhachHang (HoTen);
PRINT N'>> Da tao bang KhachHang (Bao gom cot Web)';
GO


PRINT N'';
PRINT N'============================================';
PRINT N'  PHAN 1 HOAN THANH: 16 bang da duoc tao  ';
PRINT N'  VaiTro, NhanVien, TaiKhoan               ';
PRINT N'  LoaiGhe, PhongChieu, GheNgoi             ';
PRINT N'  TheLoaiPhim, Phim                        ';
PRINT N'  QuyTacGia, KhuyenMai                     ';
PRINT N'  NhomFnB, SanPhamFnB, Combo, ChiTietCombo ';
PRINT N'  HangThanhVien, KhachHang                 ';
PRINT N'============================================';
GO