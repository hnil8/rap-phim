-- ============================================================

-- HỆ THỐNG QUẢN LÝ RẠP CHIẾU PHIM (GỘP TỪ 3 FILE)

-- Database: RapphimDB (SQL Server)

-- Bao gồm: Tạo Database -> Tạo Bảng -> Thêm Khóa Ngoại Vòng -> Thêm Dữ Liệu Mẫu

-- ============================================================



USE master;

GO



-- ============================================================

-- BƯỚC 1: TẠO DATABASE

-- ============================================================

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'RapphimDB')

BEGIN

    CREATE DATABASE RapphimDB COLLATE Vietnamese_CI_AS;

    PRINT N'>> Đã tạo database RapphimDB';

END

GO



USE RapphimDB;

GO



-- ============================================================

-- BƯỚC 2: TẠO CÁC BẢNG ĐỘC LẬP VÀ PHỤ THUỘC TẦNG 1

-- ============================================================



-- 1. BẢNG: VaiTro

IF OBJECT_ID('dbo.VaiTro', 'U') IS NOT NULL DROP TABLE dbo.VaiTro;

CREATE TABLE dbo.VaiTro (

    VaiTroId    INT            IDENTITY(1,1)    NOT NULL,

    TenVaiTro   NVARCHAR(50)                    NOT NULL,  

    MoTa        NVARCHAR(200)                   NULL,

    IsDeleted   BIT            DEFAULT 0        NOT NULL,  



    CONSTRAINT PK_VaiTro              PRIMARY KEY (VaiTroId),

    CONSTRAINT UQ_VaiTro_TenVaiTro    UNIQUE (TenVaiTro)

);

PRINT N'>> Da tao bang VaiTro';

GO



-- 2. BẢNG: NhanVien

IF OBJECT_ID('dbo.NhanVien', 'U') IS NOT NULL DROP TABLE dbo.NhanVien;

CREATE TABLE dbo.NhanVien (

    NhanVienId      INT            IDENTITY(1,1)    NOT NULL,

    HoTen           NVARCHAR(100)                   NOT NULL,

    SoDienThoai     NVARCHAR(15)                    NULL,

    Email           NVARCHAR(100)                   NULL,

    GioiTinh        NVARCHAR(10)                    NULL,    

    NgaySinh        DATE                            NULL,

    DiaChi          NVARCHAR(250)                   NULL,

    NgayVaoLam      DATE           DEFAULT GETDATE() NOT NULL,

    IsDeleted       BIT            DEFAULT 0        NOT NULL, 

    NgayXoa         DATETIME                        NULL,    



    CONSTRAINT PK_NhanVien PRIMARY KEY (NhanVienId)

);

CREATE INDEX IX_NhanVien_HoTen       ON dbo.NhanVien (HoTen);

CREATE INDEX IX_NhanVien_SoDienThoai ON dbo.NhanVien (SoDienThoai);

PRINT N'>> Da tao bang NhanVien';

GO



-- 3. BẢNG: TaiKhoan

IF OBJECT_ID('dbo.TaiKhoan', 'U') IS NOT NULL DROP TABLE dbo.TaiKhoan;

CREATE TABLE dbo.TaiKhoan (

    TaiKhoanId       INT            IDENTITY(1,1)    NOT NULL,

    TenDangNhap      NVARCHAR(50)                    NOT NULL,

    MatKhauHash      NVARCHAR(256)                   NOT NULL,  

    VaiTroId         INT                             NOT NULL,

    NhanVienId       INT                             NOT NULL,

    IsActive         BIT            DEFAULT 1        NOT NULL,  

    NgayTao          DATETIME       DEFAULT GETDATE() NOT NULL,

    LanDangNhapCuoi  DATETIME                        NULL,      

    IsDeleted        BIT            DEFAULT 0        NOT NULL,



    CONSTRAINT PK_TaiKhoan               PRIMARY KEY (TaiKhoanId),

    CONSTRAINT UQ_TaiKhoan_TenDangNhap   UNIQUE (TenDangNhap),

    CONSTRAINT UQ_TaiKhoan_NhanVienId    UNIQUE (NhanVienId), 



    CONSTRAINT FK_TaiKhoan_VaiTro        FOREIGN KEY (VaiTroId)

        REFERENCES dbo.VaiTro (VaiTroId),

    CONSTRAINT FK_TaiKhoan_NhanVien      FOREIGN KEY (NhanVienId)

        REFERENCES dbo.NhanVien (NhanVienId)

);

CREATE INDEX IX_TaiKhoan_VaiTroId ON dbo.TaiKhoan (VaiTroId);

PRINT N'>> Da tao bang TaiKhoan';

GO



-- 4. BẢNG: LoaiGhe

IF OBJECT_ID('dbo.LoaiGhe', 'U') IS NOT NULL DROP TABLE dbo.LoaiGhe;

CREATE TABLE dbo.LoaiGhe (

    LoaiGheId   INT            IDENTITY(1,1)    NOT NULL,

    TenLoai     NVARCHAR(50)                    NOT NULL,   

    HeSoGia     DECIMAL(5,2)   DEFAULT 1.00    NOT NULL,   

    MoTa        NVARCHAR(200)                   NULL,

    IsDeleted   BIT            DEFAULT 0        NOT NULL,



    CONSTRAINT PK_LoaiGhe        PRIMARY KEY (LoaiGheId),

    CONSTRAINT UQ_LoaiGhe_Ten    UNIQUE (TenLoai),

    CONSTRAINT CK_LoaiGhe_HeSo   CHECK (HeSoGia > 0)

);

PRINT N'>> Da tao bang LoaiGhe';

GO



-- 5. BẢNG: PhongChieu

IF OBJECT_ID('dbo.PhongChieu', 'U') IS NOT NULL DROP TABLE dbo.PhongChieu;

CREATE TABLE dbo.PhongChieu (

    PhongId     INT            IDENTITY(1,1)    NOT NULL,

    TenPhong    NVARCHAR(100)                   NOT NULL,   

    SucChua     INT                             NOT NULL,   

    LoaiPhong   NVARCHAR(50)   DEFAULT N'2D'   NULL,       

    TrangThai   NVARCHAR(20)   DEFAULT N'HoatDong' NOT NULL, 

    IsDeleted   BIT            DEFAULT 0        NOT NULL,



    CONSTRAINT PK_PhongChieu        PRIMARY KEY (PhongId),

    CONSTRAINT UQ_PhongChieu_Ten    UNIQUE (TenPhong),

    CONSTRAINT CK_PhongChieu_TT     CHECK (TrangThai IN (N'HoatDong', N'BaoTri', N'DongCua')),

    CONSTRAINT CK_PhongChieu_SC     CHECK (SucChua > 0)

);

PRINT N'>> Da tao bang PhongChieu';

GO



-- 6. BẢNG: GheNgoi

IF OBJECT_ID('dbo.GheNgoi', 'U') IS NOT NULL DROP TABLE dbo.GheNgoi;

CREATE TABLE dbo.GheNgoi (

    GheId       INT            IDENTITY(1,1)    NOT NULL,

    PhongId     INT                             NOT NULL,

    LoaiGheId   INT                             NOT NULL,

    DayGhe      CHAR(2)                         NOT NULL,   

    CotGhe      INT                             NOT NULL,   

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



-- 7. BẢNG: TheLoaiPhim

IF OBJECT_ID('dbo.TheLoaiPhim', 'U') IS NOT NULL DROP TABLE dbo.TheLoaiPhim;

CREATE TABLE dbo.TheLoaiPhim (

    TheLoaiId   INT            IDENTITY(1,1)    NOT NULL,

    TenTheLoai  NVARCHAR(100)                   NOT NULL,   

    IsDeleted   BIT            DEFAULT 0        NOT NULL,



    CONSTRAINT PK_TheLoaiPhim       PRIMARY KEY (TheLoaiId),

    CONSTRAINT UQ_TheLoaiPhim_Ten   UNIQUE (TenTheLoai)

);

PRINT N'>> Da tao bang TheLoaiPhim';

GO



-- 8. BẢNG: Phim

IF OBJECT_ID('dbo.Phim', 'U') IS NOT NULL DROP TABLE dbo.Phim;

CREATE TABLE dbo.Phim (

    PhimId            INT            IDENTITY(1,1)    NOT NULL,

    TenPhim           NVARCHAR(200)                   NOT NULL,

    TenGoc            NVARCHAR(200)                   NULL,       

    DaoDien           NVARCHAR(200)                   NULL,

    DienVienChinh     NVARCHAR(500)                   NULL,       

    ThoiLuongPhut     INT                             NOT NULL,   

    NuocSanXuat       NVARCHAR(100)                   NULL,

    NamPhatHanh       INT                             NULL,

    GioiHanDoTuoi     NVARCHAR(10)   DEFAULT N'P'    NOT NULL,   

    NgonNgu           NVARCHAR(50)   DEFAULT N'VietSub' NULL,    

    MoTa              NVARCHAR(2000)                  NULL,       

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



-- 9. BẢNG: QuyTacGia

IF OBJECT_ID('dbo.QuyTacGia', 'U') IS NOT NULL DROP TABLE dbo.QuyTacGia;

CREATE TABLE dbo.QuyTacGia (

    QuyTacId     INT            IDENTITY(1,1)    NOT NULL,

    TenQuyTac    NVARCHAR(100)                   NOT NULL,   

    LoaiNgay     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  

    KhungGio     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  

    GioTu        TIME                            NULL,       

    GioDen       TIME                            NULL,       

    DoiTuong     NVARCHAR(20)   DEFAULT N'TatCa' NOT NULL,  

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



-- 10. BẢNG: KhuyenMai

IF OBJECT_ID('dbo.KhuyenMai', 'U') IS NOT NULL DROP TABLE dbo.KhuyenMai;

CREATE TABLE dbo.KhuyenMai (

    KhuyenMaiId      INT            IDENTITY(1,1)    NOT NULL,

    TenChuongTrinh   NVARCHAR(200)                   NOT NULL,

    MaCode           NVARCHAR(50)                    NOT NULL,

    LoaiGiam         NVARCHAR(20)                    NOT NULL,   

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



-- 11. BẢNG: NhomFnB

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



-- 12. BẢNG: SanPhamFnB

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



-- 13. BẢNG: Combo

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



-- 14. BẢNG: ChiTietCombo

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



-- 15. BẢNG: HangThanhVien

IF OBJECT_ID('dbo.HangThanhVien', 'U') IS NOT NULL DROP TABLE dbo.HangThanhVien;

CREATE TABLE dbo.HangThanhVien (

    HangId           INT            IDENTITY(1,1)    NOT NULL,

    TenHang          NVARCHAR(50)                    NOT NULL,   

    DiemToiThieu     INT            DEFAULT 0        NOT NULL,  

    PhanTramUuDai    DECIMAL(5,2)   DEFAULT 0        NOT NULL,  

    MoTa             NVARCHAR(300)                   NULL,

    MauSac           NVARCHAR(20)                    NULL,       

    IsDeleted        BIT            DEFAULT 0        NOT NULL,



    CONSTRAINT PK_HangThanhVien           PRIMARY KEY (HangId),

    CONSTRAINT UQ_HangThanhVien_Ten       UNIQUE (TenHang),

    CONSTRAINT CK_HangThanhVien_Diem      CHECK (DiemToiThieu >= 0),

    CONSTRAINT CK_HangThanhVien_UuDai     CHECK (PhanTramUuDai >= 0 AND PhanTramUuDai <= 100)

);

PRINT N'>> Da tao bang HangThanhVien';

GO



-- 16. BẢNG: KhachHang

IF OBJECT_ID('dbo.KhachHang', 'U') IS NOT NULL DROP TABLE dbo.KhachHang;

CREATE TABLE dbo.KhachHang (

    KhachHangId     INT            IDENTITY(1,1)    NOT NULL,

    HoTen           NVARCHAR(100)                   NOT NULL,

    SoDienThoai     NVARCHAR(15)                    NOT NULL,   

    Email           NVARCHAR(100)                   NULL,

    MatKhauHash     NVARCHAR(256)                   NULL,       

    AvatarUrl       NVARCHAR(500)                   NULL,       

    NgaySinh        DATE                            NULL,

    GioiTinh        NVARCHAR(10)                    NULL,

    DiemTichLuy     INT            DEFAULT 0        NOT NULL,

    HangId          INT                             NULL,       

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

PRINT N'>> Da tao bang KhachHang';

GO





-- ============================================================

-- BƯỚC 3: TẠO CÁC BẢNG TRUNG GIAN & PHỤ THUỘC TẦNG 2

-- ============================================================



-- 17. BẢNG: Phim_TheLoai

IF OBJECT_ID('dbo.Phim_TheLoai', 'U') IS NOT NULL DROP TABLE dbo.Phim_TheLoai;

CREATE TABLE dbo.Phim_TheLoai (

    PhimId      INT     NOT NULL,

    TheLoaiId   INT     NOT NULL,



    CONSTRAINT PK_Phim_TheLoai  PRIMARY KEY (PhimId, TheLoaiId),



    CONSTRAINT FK_PhimTheLoai_Phim      FOREIGN KEY (PhimId)

        REFERENCES dbo.Phim (PhimId)

        ON DELETE CASCADE, 

    CONSTRAINT FK_PhimTheLoai_TheLoai   FOREIGN KEY (TheLoaiId)

        REFERENCES dbo.TheLoaiPhim (TheLoaiId)

);

CREATE INDEX IX_PhimTheLoai_TheLoaiId ON dbo.Phim_TheLoai (TheLoaiId);

PRINT N'>> Da tao bang Phim_TheLoai';

GO



-- 18. BẢNG: CaLamViec

IF OBJECT_ID('dbo.CaLamViec', 'U') IS NOT NULL DROP TABLE dbo.CaLamViec;

CREATE TABLE dbo.CaLamViec (

    CaId                INT             IDENTITY(1,1)   NOT NULL,

    NhanVienId          INT                             NOT NULL,

    ThoiGianMoCa        DATETIME        DEFAULT GETDATE() NOT NULL,

    ThoiGianChotCa      DATETIME                        NULL,    

    TienDauCa           DECIMAL(18,0)   DEFAULT 0       NOT NULL,

    TongThuTienMat      DECIMAL(18,0)   DEFAULT 0       NOT NULL,

    TongThuChuyenKhoan  DECIMAL(18,0)   DEFAULT 0       NOT NULL,

    TongThuThe          DECIMAL(18,0)   DEFAULT 0       NOT NULL,

    GhiChuChotCa        NVARCHAR(500)                   NULL,

    TrangThai           NVARCHAR(20)    DEFAULT N'DangMo' NOT NULL,



    CONSTRAINT PK_CaLamViec         PRIMARY KEY (CaId),

    CONSTRAINT CK_CaLamViec_TT      CHECK (TrangThai IN (N'DangMo', N'DaChotCa')),

    CONSTRAINT CK_CaLamViec_TienDau CHECK (TienDauCa >= 0),

    CONSTRAINT CK_CaLamViec_ThoiGian CHECK (ThoiGianChotCa IS NULL OR ThoiGianChotCa >= ThoiGianMoCa),



    CONSTRAINT FK_CaLamViec_NhanVien FOREIGN KEY (NhanVienId)

        REFERENCES dbo.NhanVien (NhanVienId)

);

CREATE INDEX IX_CaLamViec_NhanVienId ON dbo.CaLamViec (NhanVienId);

CREATE INDEX IX_CaLamViec_TrangThai  ON dbo.CaLamViec (TrangThai); 

PRINT N'>> Da tao bang CaLamViec';

GO



-- 19. BẢNG: LichChieu

IF OBJECT_ID('dbo.LichChieu', 'U') IS NOT NULL DROP TABLE dbo.LichChieu;

CREATE TABLE dbo.LichChieu (

    LichChieuId     INT             IDENTITY(1,1)   NOT NULL,

    PhimId          INT                             NOT NULL,

    PhongId         INT                             NOT NULL,

    GioBatDau       DATETIME                        NOT NULL,

    GioKetThuc      DATETIME                        NOT NULL,

    GiaVeCoBan      DECIMAL(18,0)                   NOT NULL,

    TrangThai       NVARCHAR(20)    DEFAULT N'ChuaChieu' NOT NULL, 

    IsDeleted       BIT             DEFAULT 0       NOT NULL,

    NgayTao         DATETIME        DEFAULT GETDATE() NOT NULL,



    CONSTRAINT PK_LichChieu             PRIMARY KEY (LichChieuId),

    CONSTRAINT CK_LichChieu_TrangThai   CHECK (TrangThai IN (N'ChuaChieu', N'DangChieu', N'DaKetThuc', N'HuyChieu')),

    CONSTRAINT CK_LichChieu_ThoiGian    CHECK (GioKetThuc > GioBatDau),

    CONSTRAINT CK_LichChieu_GiaVe       CHECK (GiaVeCoBan > 0),



    CONSTRAINT FK_LichChieu_Phim        FOREIGN KEY (PhimId)

        REFERENCES dbo.Phim (PhimId),

    CONSTRAINT FK_LichChieu_Phong       FOREIGN KEY (PhongId)

        REFERENCES dbo.PhongChieu (PhongId)

);

CREATE INDEX IX_LichChieu_PhongId_GioBatDau ON dbo.LichChieu (PhongId, GioBatDau);

CREATE INDEX IX_LichChieu_PhimId            ON dbo.LichChieu (PhimId);

CREATE INDEX IX_LichChieu_TrangThai         ON dbo.LichChieu (TrangThai);

PRINT N'>> Da tao bang LichChieu';

GO



-- 20. BẢNG: LichChieu_Ghe

IF OBJECT_ID('dbo.LichChieu_Ghe', 'U') IS NOT NULL DROP TABLE dbo.LichChieu_Ghe;

CREATE TABLE dbo.LichChieu_Ghe (

    LichChieuGheId  INT             IDENTITY(1,1)   NOT NULL,

    LichChieuId     INT                             NOT NULL,

    GheId           INT                             NOT NULL,

    TrangThaiGhe    NVARCHAR(20)    DEFAULT N'Trong' NOT NULL,

    ThoiGianGiu     DATETIME                        NULL,

    VePhimId        INT                             NULL,



    CONSTRAINT PK_LichChieu_Ghe         PRIMARY KEY (LichChieuGheId),

    CONSTRAINT UQ_LichChieu_Ghe_ViTri   UNIQUE (LichChieuId, GheId),

    CONSTRAINT CK_LichChieu_Ghe_TrangThai CHECK (TrangThaiGhe IN (N'Trong', N'DangGiu', N'DaBan')),



    CONSTRAINT FK_LichChieuGhe_LichChieu FOREIGN KEY (LichChieuId)

        REFERENCES dbo.LichChieu (LichChieuId),

    CONSTRAINT FK_LichChieuGhe_Ghe      FOREIGN KEY (GheId)

        REFERENCES dbo.GheNgoi (GheId)

);

CREATE INDEX IX_LichChieuGhe_LichChieuId ON dbo.LichChieu_Ghe (LichChieuId);

CREATE INDEX IX_LichChieuGhe_TrangThai   ON dbo.LichChieu_Ghe (LichChieuId, TrangThaiGhe);

PRINT N'>> Da tao bang LichChieu_Ghe';

GO



-- 21. BẢNG: HoaDon

IF OBJECT_ID('dbo.HoaDon', 'U') IS NOT NULL DROP TABLE dbo.HoaDon;

CREATE TABLE dbo.HoaDon (

    HoaDonId            INT             IDENTITY(1,1)   NOT NULL,

    CaId                INT                             NOT NULL,   

    KhachHangId         INT                             NULL,       

    KhuyenMaiId         INT                             NULL,       



    TongTienVe          DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TongTienFnB         DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TongTienGoc         DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TienGiamKM          DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TienGiamDiem        DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TienGiamThanhVien   DECIMAL(18,0)   DEFAULT 0       NOT NULL,   

    TongTienGiam        AS (TienGiamKM + TienGiamDiem + TienGiamThanhVien) PERSISTED, 

    ThanhTien           DECIMAL(18,0)                   NOT NULL,   



    PhuongThucTT        NVARCHAR(30)                    NOT NULL,   

    TienKhachDua        DECIMAL(18,0)                   NULL,       

    TienThoiLai         DECIMAL(18,0)                   NULL,       



    DiemTichDuoc        INT             DEFAULT 0       NOT NULL,   

    DiemSuDung          INT             DEFAULT 0       NOT NULL,   



    TrangThai           NVARCHAR(20)    DEFAULT N'HoanThanh' NOT NULL, 

    LyDoHuy             NVARCHAR(300)                   NULL,

    ThoiGianTao         DATETIME        DEFAULT GETDATE() NOT NULL,

    GhiChu              NVARCHAR(500)                   NULL,



    CONSTRAINT PK_HoaDon                PRIMARY KEY (HoaDonId),

    CONSTRAINT CK_HoaDon_PTTT           CHECK (PhuongThucTT IN (N'TienMat', N'ChuyenKhoan', N'The', N'KetHop')),

    CONSTRAINT CK_HoaDon_TrangThai      CHECK (TrangThai IN (N'HoanThanh', N'DaHuy')),

    CONSTRAINT CK_HoaDon_ThanhTien      CHECK (ThanhTien >= 0),

    CONSTRAINT CK_HoaDon_TongTienGoc    CHECK (TongTienGoc >= 0),



    CONSTRAINT FK_HoaDon_Ca             FOREIGN KEY (CaId)

        REFERENCES dbo.CaLamViec (CaId),

    CONSTRAINT FK_HoaDon_KhachHang      FOREIGN KEY (KhachHangId)

        REFERENCES dbo.KhachHang (KhachHangId),

    CONSTRAINT FK_HoaDon_KhuyenMai      FOREIGN KEY (KhuyenMaiId)

        REFERENCES dbo.KhuyenMai (KhuyenMaiId)

);

CREATE INDEX IX_HoaDon_ThoiGianTao  ON dbo.HoaDon (ThoiGianTao);

CREATE INDEX IX_HoaDon_CaId         ON dbo.HoaDon (CaId);

CREATE INDEX IX_HoaDon_KhachHangId  ON dbo.HoaDon (KhachHangId);

PRINT N'>> Da tao bang HoaDon';

GO



-- 22. BẢNG: VePhim

IF OBJECT_ID('dbo.VePhim', 'U') IS NOT NULL DROP TABLE dbo.VePhim;

CREATE TABLE dbo.VePhim (

    VeId                INT             IDENTITY(1,1)   NOT NULL,

    HoaDonId            INT                             NOT NULL,

    LichChieuGheId      INT                             NOT NULL,   

    QuyTacId            INT                             NULL,       



    GiaGoc              DECIMAL(18,0)                   NOT NULL,   

    GiaBan              DECIMAL(18,0)                   NOT NULL,   



    DoiTuongKhach       NVARCHAR(20)    DEFAULT N'NguoiLon' NOT NULL, 



    TrangThai           NVARCHAR(20)    DEFAULT N'DaBan' NOT NULL,  

    ThoiGianIn          DATETIME                        NULL,       

    MaVach              NVARCHAR(100)                   NULL,       



    CONSTRAINT PK_VePhim                PRIMARY KEY (VeId),

    CONSTRAINT UQ_VePhim_GheSuat        UNIQUE (LichChieuGheId),    

    CONSTRAINT CK_VePhim_GiaBan         CHECK (GiaBan >= 0),

    CONSTRAINT CK_VePhim_DoiTuong       CHECK (DoiTuongKhach IN (N'NguoiLon', N'SinhVien', N'TreEm')),

    CONSTRAINT CK_VePhim_TrangThai      CHECK (TrangThai IN (N'DaBan', N'DaHuy')),



    CONSTRAINT FK_VePhim_HoaDon         FOREIGN KEY (HoaDonId)

        REFERENCES dbo.HoaDon (HoaDonId),

    CONSTRAINT FK_VePhim_LichChieuGhe   FOREIGN KEY (LichChieuGheId)

        REFERENCES dbo.LichChieu_Ghe (LichChieuGheId),

    CONSTRAINT FK_VePhim_QuyTacGia      FOREIGN KEY (QuyTacId)

        REFERENCES dbo.QuyTacGia (QuyTacId)

);

CREATE INDEX IX_VePhim_HoaDonId         ON dbo.VePhim (HoaDonId);

CREATE INDEX IX_VePhim_LichChieuGheId   ON dbo.VePhim (LichChieuGheId);

PRINT N'>> Da tao bang VePhim';

GO



-- 23. BẢNG: HoaDon_FnB

IF OBJECT_ID('dbo.HoaDon_FnB', 'U') IS NOT NULL DROP TABLE dbo.HoaDon_FnB;

CREATE TABLE dbo.HoaDon_FnB (

    HoaDonFnBId     INT             IDENTITY(1,1)   NOT NULL,

    HoaDonId        INT                             NOT NULL,

    SanPhamId       INT                             NULL,

    ComboId         INT                             NULL,

    SoLuong         INT             DEFAULT 1       NOT NULL,

    DonGia          DECIMAL(18,0)                   NOT NULL,

    ThanhTien       AS (SoLuong * DonGia) PERSISTED, 



    CONSTRAINT PK_HoaDon_FnB            PRIMARY KEY (HoaDonFnBId),

    CONSTRAINT CK_HoaDon_FnB_SoLuong    CHECK (SoLuong > 0),

    CONSTRAINT CK_HoaDon_FnB_DonGia     CHECK (DonGia >= 0),

    CONSTRAINT CK_HoaDon_FnB_Item

        CHECK (

            (SanPhamId IS NOT NULL AND ComboId IS NULL) OR

            (SanPhamId IS NULL AND ComboId IS NOT NULL)

        ),



    CONSTRAINT FK_HoaDonFnB_HoaDon      FOREIGN KEY (HoaDonId)

        REFERENCES dbo.HoaDon (HoaDonId),

    CONSTRAINT FK_HoaDonFnB_SanPham     FOREIGN KEY (SanPhamId)

        REFERENCES dbo.SanPhamFnB (SanPhamId),

    CONSTRAINT FK_HoaDonFnB_Combo       FOREIGN KEY (ComboId)

        REFERENCES dbo.Combo (ComboId)

);

CREATE INDEX IX_HoaDonFnB_HoaDonId ON dbo.HoaDon_FnB (HoaDonId);

PRINT N'>> Da tao bang HoaDon_FnB';

GO





-- ============================================================

-- BƯỚC 4: THÊM KHÓA NGOẠI VÒNG (Circular FK)

-- ============================================================

ALTER TABLE dbo.LichChieu_Ghe

    ADD CONSTRAINT FK_LichChieuGhe_VePhim

        FOREIGN KEY (VePhimId)

        REFERENCES dbo.VePhim (VeId);



PRINT N'>> Da them FK vong: LichChieu_Ghe.VePhimId → VePhim';

GO





-- ============================================================

-- BƯỚC 5: THÊM DỮ LIỆU MẪU (SEED DATA)

-- ============================================================



PRINT N'>> Dang them du lieu VaiTro...';

INSERT INTO dbo.VaiTro (TenVaiTro, MoTa, IsDeleted)

VALUES

    (N'Admin',     N'Quản trị viên hệ thống, có toàn quyền truy cập', 0),

    (N'QuanLy',    N'Quản lý rạp, có quyền xem báo cáo và quản lý nhân viên', 0),

    (N'NhanVien',  N'Nhân viên quầy, chỉ truy cập POS và quản lý đơn hàng', 0);

PRINT N'>> Da them 3 vai tro';

GO



PRINT N'>> Dang them du lieu NhanVien...';

INSERT INTO dbo.NhanVien (HoTen, SoDienThoai, Email, GioiTinh, NgaySinh, DiaChi, NgayVaoLam, IsDeleted)

VALUES

    (N'Hoàng Linh',         '0901000001', 'admin@cinema.com',    N'Nam', '1990-01-15', N'123 Đường Lê Lợi, Q.1, TP.HCM',        '2022-01-01', 0),

    (N'Trần Anh',           '0901000002', 'quanly@cinema.com',   N'Nữ',  '1992-05-20', N'456 Đường Nguyễn Huệ, Q.1, TP.HCM',    '2022-03-15', 0),

    (N'Lê Văn Thiêm',       '0901000003', 'nv01@cinema.com',     N'Nam', '1998-08-10', N'789 Đường Trần Hưng Đạo, Q.5, TP.HCM', '2023-01-10', 0),

    (N'Phạm Thị Thu Hà',    '0901000004', 'nv02@cinema.com',     N'Nữ',  '1999-11-25', N'321 Đường Nguyễn Trãi, Q.5, TP.HCM',   '2023-06-01', 0),

    (N'Hoàng Minh Tuấn',    '0901000005', 'nv03@cinema.com',     N'Nam', '2000-03-07', N'654 Đường CMT8, Q.3, TP.HCM',          '2024-01-15', 0);

PRINT N'>> Da them 5 nhan vien';

GO



PRINT N'>> Dang them du lieu TaiKhoan...';

INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, VaiTroId, NhanVienId, IsActive, IsDeleted)

VALUES

    ('admin',      '123456', (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'Admin'),    (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'admin@cinema.com'), 1, 0),

    ('quanly',     '123456', (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'QuanLy'),   (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'quanly@cinema.com'), 1, 0),

    ('nhanvien01', '123456', (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'), (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv01@cinema.com'), 1, 0),

    ('nhanvien02', '123456', (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'), (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv02@cinema.com'), 1, 0),

    ('nhanvien03', '123456', (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'), (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv03@cinema.com'), 0, 0);

PRINT N'>> Da them 5 tai khoan (1 Admin, 1 QuanLy, 3 NhanVien)';

GO



PRINT N'>> Dang them du lieu HangThanhVien...';

INSERT INTO dbo.HangThanhVien (TenHang, DiemToiThieu, PhanTramUuDai, MoTa, MauSac, IsDeleted)

VALUES

    (N'Thành Viên',  0,    0.00,  N'Hạng mặc định khi đăng ký',              '#C0C0C0', 0),

    (N'Bạc',         500,  5.00,  N'Tích lũy từ 500 điểm, giảm thêm 5%',     '#A8A9AD', 0),

    (N'Vàng',        2000, 10.00, N'Tích lũy từ 2000 điểm, giảm thêm 10%',   '#FFD700', 0),

    (N'VIP',         5000, 15.00, N'Tích lũy từ 5000 điểm, giảm thêm 15%',   '#FF6347', 0),

    (N'VVIP',        10000,20.00, N'Tích lũy từ 10000 điểm, giảm thêm 20%',  '#9B59B6', 0);

PRINT N'>> Da them 5 hang thanh vien';

GO



PRINT N'>> Dang them du lieu KhachHang...';

INSERT INTO dbo.KhachHang (HoTen, SoDienThoai, Email, MatKhauHash, NgaySinh, GioiTinh, DiemTichLuy, HangId, IsDeleted)

VALUES

    (N'Nguyễn Thị Lan', '0912345601', 'lan.nguyen@gmail.com', '123456', '1995-04-12', N'Nữ',  12500, (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'VVIP'), 0),

    (N'Trần Văn Bình',  '0912345602', 'binh.tran@gmail.com',  '123456', '1993-09-30', N'Nam', 6200,  (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'VIP'),  0),

    (N'Lê Thị Cẩm',     '0912345603', 'cam.le@gmail.com',     '123456', '2000-07-18', N'Nữ',  2300,  (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Vàng'), 0),

    (N'Phạm Quốc Dũng', '0912345604', 'dung.pham@gmail.com',  '123456', '1997-02-14', N'Nam', 750,   (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Bạc'),  0),

    (N'Hoàng Thị Emm',  '0912345605', 'emm.hoang@gmail.com',  '123456', '2002-12-05', N'Nữ',  0,     (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Thành Viên'), 0),

    (N'Khách Vãng Lai', '0000000000', NULL,                   NULL,     NULL,         NULL,   0,     NULL, 0);

PRINT N'>> Da them 6 khach hang';

GO



PRINT N'';

PRINT N'=================================================';

PRINT N'  HOÀN TẤT TẠO DATABASE: RapphimDB               ';

PRINT N'  Đã khởi tạo 23 Bảng và dữ liệu mẫu thành công. ';

PRINT N'=================================================';

GO
 -- 1. Tạo bảng Phiếu Nhập (Master)
CREATE TABLE PhieuNhap (
    PhieuNhapId INT IDENTITY(1,1) PRIMARY KEY,
    NguoiNhap NVARCHAR(100) NOT NULL,
    NgayNhap DATETIME DEFAULT GETDATE(),
    GhiChu NVARCHAR(500),
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0
);
GO

-- 2. Tạo bảng Chi Tiết Phiếu Nhập (Detail)
CREATE TABLE ChiTietPhieuNhap (
    PhieuNhapId INT NOT NULL,
    SanPhamId INT NOT NULL, -- Đảm bảo tên cột này khớp với bảng Sản phẩm của bạn
    SoLuong INT NOT NULL,
    GiaNhap DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,
    
    -- Thiết lập Khóa chính kép
    PRIMARY KEY (PhieuNhapId, SanPhamId),
    
    -- Thiết lập Khóa ngoại
    FOREIGN KEY (PhieuNhapId) REFERENCES PhieuNhap(PhieuNhapId),
    -- LƯU Ý: Thay 'SanPhamFnB' bằng đúng tên bảng chứa Bắp/Nước của bạn nếu nó tên khác
    FOREIGN KEY (SanPhamId) REFERENCES SanPhamFnB(SanPhamId) 
);
GO
