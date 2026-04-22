-- ============================================================
-- HỆ THỐNG QUẢN LÝ RẠP CHIẾU PHIM
-- Các bảng Phụ thuộc (Dependent Tables)
-- Database: CinemaDB (SQL Server)
-- ============================================================

USE CinemaDB;
GO

-- ============================================================
-- NHÓM 3 (Phần còn lại): BẢNG TRUNG GIAN PHIM - THỂ LOẠI
-- ============================================================

-- ------------------------------------------------------------
-- 17. BẢNG: Phim_TheLoai
-- Mục đích: Bảng trung gian (Many-to-Many) giữa Phim và TheLoaiPhim
-- Ví dụ: "Avengers" thuộc cả "Hành động" lẫn "Khoa học viễn tưởng"
-- FK đến: Phim (Phần 1), TheLoaiPhim (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.Phim_TheLoai', 'U') IS NOT NULL DROP TABLE dbo.Phim_TheLoai;

CREATE TABLE dbo.Phim_TheLoai (
    -- Bảng trung gian thuần túy: Khóa chính là composite của 2 FK
    PhimId      INT     NOT NULL,
    TheLoaiId   INT     NOT NULL,

    CONSTRAINT PK_Phim_TheLoai  PRIMARY KEY (PhimId, TheLoaiId),

    CONSTRAINT FK_PhimTheLoai_Phim      FOREIGN KEY (PhimId)
        REFERENCES dbo.Phim (PhimId)
        ON DELETE CASCADE,  -- Xóa phim → tự xóa các liên kết thể loại
    CONSTRAINT FK_PhimTheLoai_TheLoai   FOREIGN KEY (TheLoaiId)
        REFERENCES dbo.TheLoaiPhim (TheLoaiId)
);

CREATE INDEX IX_PhimTheLoai_TheLoaiId ON dbo.Phim_TheLoai (TheLoaiId);
PRINT N'>> Da tao bang Phim_TheLoai';
GO


-- ============================================================
-- NHÓM 9: CA LÀM VIỆC
-- Phụ thuộc NhanVien (Phần 1) → Tạo trước HoaDon
-- ============================================================

-- ------------------------------------------------------------
-- 18. BẢNG: CaLamViec
-- Mục đích: Quản lý ca làm việc của nhân viên quầy POS
-- Luồng: Mở ca (ghi ThoiGianMoCa, TienDauCa) → Bán hàng (HoaDon liên kết CaId)
--        → Chốt ca (ghi ThoiGianChotCa, tổng kết tiền)
-- FK đến: NhanVien (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.CaLamViec', 'U') IS NOT NULL DROP TABLE dbo.CaLamViec;

CREATE TABLE dbo.CaLamViec (
    CaId                INT             IDENTITY(1,1)   NOT NULL,
    NhanVienId          INT                             NOT NULL,
    ThoiGianMoCa        DATETIME        DEFAULT GETDATE() NOT NULL,
    ThoiGianChotCa      DATETIME                        NULL,    -- NULL = Ca đang mở (chưa chốt)
    -- Tiền nhận đầu ca (tiền lẻ bàn giao từ quản lý)
    TienDauCa           DECIMAL(18,0)   DEFAULT 0       NOT NULL,
    -- Tổng tiền thu được trong ca (tự động cộng dồn qua HoaDon, hoặc chốt thủ công)
    TongThuTienMat      DECIMAL(18,0)   DEFAULT 0       NOT NULL,
    TongThuChuyenKhoan  DECIMAL(18,0)   DEFAULT 0       NOT NULL,
    TongThuThe          DECIMAL(18,0)   DEFAULT 0       NOT NULL,
    -- Ghi chú khi chốt ca (bàn giao, sự cố...)
    GhiChuChotCa        NVARCHAR(500)                   NULL,
    -- TrangThai ca: DangMo → DaChotCa
    TrangThai           NVARCHAR(20)    DEFAULT N'DangMo' NOT NULL,

    CONSTRAINT PK_CaLamViec         PRIMARY KEY (CaId),
    CONSTRAINT CK_CaLamViec_TT      CHECK (TrangThai IN (N'DangMo', N'DaChotCa')),
    CONSTRAINT CK_CaLamViec_TienDau CHECK (TienDauCa >= 0),
    -- Đảm bảo giờ chốt ca phải sau giờ mở ca
    CONSTRAINT CK_CaLamViec_ThoiGian
        CHECK (ThoiGianChotCa IS NULL OR ThoiGianChotCa >= ThoiGianMoCa),

    CONSTRAINT FK_CaLamViec_NhanVien FOREIGN KEY (NhanVienId)
        REFERENCES dbo.NhanVien (NhanVienId)
);

CREATE INDEX IX_CaLamViec_NhanVienId ON dbo.CaLamViec (NhanVienId);
CREATE INDEX IX_CaLamViec_TrangThai  ON dbo.CaLamViec (TrangThai);  -- Query nhanh ca đang mở
PRINT N'>> Da tao bang CaLamViec';
GO


-- ============================================================
-- NHÓM 3 (Lịch chiếu): LICHCHIEU & LICHCHIEU_GHE
-- LichChieu phụ thuộc Phim, PhongChieu (Phần 1)
-- LichChieu_Ghe phụ thuộc LichChieu + GheNgoi (Phần 1)
-- ============================================================

-- ------------------------------------------------------------
-- 19. BẢNG: LichChieu
-- Mục đích: Mỗi suất chiếu = 1 Phim + 1 Phòng + Khung giờ cụ thể
-- Logic chống trùng lịch sẽ được xử lý ở tầng BLL (C#),
-- không dùng trigger để giữ DB đơn giản, dễ maintain
-- FK đến: Phim (Phần 1), PhongChieu (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.LichChieu', 'U') IS NOT NULL DROP TABLE dbo.LichChieu;

CREATE TABLE dbo.LichChieu (
    LichChieuId     INT             IDENTITY(1,1)   NOT NULL,
    PhimId          INT                             NOT NULL,
    PhongId         INT                             NOT NULL,
    GioBatDau       DATETIME                        NOT NULL,
    -- GioKetThuc = GioBatDau + ThoiLuongPhim + 15p dọn phòng (tính ở BLL khi INSERT)
    GioKetThuc      DATETIME                        NOT NULL,
    -- GiaVeCoBan của suất chiếu này (snapshot tại thời điểm tạo lịch)
    -- BLL sẽ đọc QuyTacGia để tính, sau đó lưu vào đây
    GiaVeCoBan      DECIMAL(18,0)                   NOT NULL,
    -- Trạng thái suất chiếu
    TrangThai       NVARCHAR(20)    DEFAULT N'ChuaChieu' NOT NULL, -- 'ChuaChieu','DangChieu','DaKetThuc','HuyChieu'
    IsDeleted       BIT             DEFAULT 0       NOT NULL,
    NgayTao         DATETIME        DEFAULT GETDATE() NOT NULL,

    CONSTRAINT PK_LichChieu             PRIMARY KEY (LichChieuId),
    CONSTRAINT CK_LichChieu_TrangThai   CHECK (TrangThai IN (N'ChuaChieu', N'DangChieu', N'DaKetThuc', N'HuyChieu')),
    -- Đảm bảo giờ kết thúc phải sau giờ bắt đầu
    CONSTRAINT CK_LichChieu_ThoiGian    CHECK (GioKetThuc > GioBatDau),
    CONSTRAINT CK_LichChieu_GiaVe       CHECK (GiaVeCoBan > 0),

    CONSTRAINT FK_LichChieu_Phim        FOREIGN KEY (PhimId)
        REFERENCES dbo.Phim (PhimId),
    CONSTRAINT FK_LichChieu_Phong       FOREIGN KEY (PhongId)
        REFERENCES dbo.PhongChieu (PhongId)
);

-- Index kép (PhongId + GioBatDau): BLL query kiểm tra trùng lịch phòng rất nhanh
CREATE INDEX IX_LichChieu_PhongId_GioBatDau ON dbo.LichChieu (PhongId, GioBatDau);
CREATE INDEX IX_LichChieu_PhimId            ON dbo.LichChieu (PhimId);
CREATE INDEX IX_LichChieu_TrangThai         ON dbo.LichChieu (TrangThai);
PRINT N'>> Da tao bang LichChieu';
GO

-- ------------------------------------------------------------
-- 20. BẢNG: LichChieu_Ghe  *** BẢNG TRUNG TÂM CỦA MODULE POS ***
-- Mục đích: Theo dõi trạng thái từng ghế trong từng suất chiếu
--
-- Nguyên tắc hoạt động (Lock Ticket):
--   Bước 1: NV click chọn ghế → UPDATE TrangThaiGhe = 'DangGiu', ghi ThoiGianGiu
--   Bước 2: Thanh toán thành công → UPDATE TrangThaiGhe = 'DaBan', ghi VePhimId
--   Bước 3: Hủy / timeout (BLL kiểm tra ThoiGianGiu > 10 phút) → SET TrangThaiGhe = 'Trong'
--
-- FK đến: LichChieu (trên), GheNgoi (Phần 1), VePhim (sẽ tạo bên dưới - dùng nullable)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.LichChieu_Ghe', 'U') IS NOT NULL DROP TABLE dbo.LichChieu_Ghe;

CREATE TABLE dbo.LichChieu_Ghe (
    LichChieuGheId  INT             IDENTITY(1,1)   NOT NULL,
    LichChieuId     INT                             NOT NULL,
    GheId           INT                             NOT NULL,
    -- 3 trạng thái chính của sơ đồ ghế tại POS
    -- 'Trong'   → Màu TRẮNG (có thể chọn)
    -- 'DangGiu' → Màu XANH LÁ (đang được NV tại quầy khác giữ, tạm thời không chọn được)
    -- 'DaBan'   → Màu ĐỎ (đã có người mua, không thể chọn)
    TrangThaiGhe    NVARCHAR(20)    DEFAULT N'Trong' NOT NULL,
    -- Ghi lại thời điểm ghế bị "giữ" để BLL kiểm tra timeout và tự động giải phóng
    ThoiGianGiu     DATETIME                        NULL,
    -- FK ngược lại VePhim (nullable, chỉ có giá trị khi TrangThaiGhe = 'DaBan')
    VePhimId        INT                             NULL,

    CONSTRAINT PK_LichChieu_Ghe         PRIMARY KEY (LichChieuGheId),
    -- Mỗi ghế chỉ có 1 trạng thái trong 1 suất chiếu
    CONSTRAINT UQ_LichChieu_Ghe_ViTri   UNIQUE (LichChieuId, GheId),
    CONSTRAINT CK_LichChieu_Ghe_TrangThai
        CHECK (TrangThaiGhe IN (N'Trong', N'DangGiu', N'DaBan')),

    CONSTRAINT FK_LichChieuGhe_LichChieu FOREIGN KEY (LichChieuId)
        REFERENCES dbo.LichChieu (LichChieuId),
    CONSTRAINT FK_LichChieuGhe_Ghe      FOREIGN KEY (GheId)
        REFERENCES dbo.GheNgoi (GheId)
    -- FK_LichChieuGhe_VePhim sẽ được thêm sau khi tạo bảng VePhim (ALTER TABLE ở cuối file)
);

-- Index tìm kiếm nhanh khi vẽ sơ đồ ghế: truy vấn theo LichChieuId là thao tác chính
CREATE INDEX IX_LichChieuGhe_LichChieuId ON dbo.LichChieu_Ghe (LichChieuId);
CREATE INDEX IX_LichChieuGhe_TrangThai   ON dbo.LichChieu_Ghe (LichChieuId, TrangThaiGhe);
PRINT N'>> Da tao bang LichChieu_Ghe';
GO


-- ============================================================
-- NHÓM 8: GIAO DỊCH BÁN HÀNG
-- Thứ tự: HoaDon → VePhim → HoaDon_FnB
-- (Sau đó ALTER TABLE LichChieu_Ghe để thêm FK đến VePhim)
-- ============================================================

-- ------------------------------------------------------------
-- 21. BẢNG: HoaDon
-- Mục đích: Lưu tổng quan 1 giao dịch bán hàng tại quầy POS
-- 1 HoaDon gồm: nhiều VePhim + nhiều HoaDon_FnB
-- FK đến: CaLamViec (trên), KhachHang (Phần 1), KhuyenMai (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.HoaDon', 'U') IS NOT NULL DROP TABLE dbo.HoaDon;

CREATE TABLE dbo.HoaDon (
    HoaDonId            INT             IDENTITY(1,1)   NOT NULL,
    CaId                INT                             NOT NULL,   -- Thuộc ca làm việc nào
    KhachHangId         INT                             NULL,       -- NULL = Khách vãng lai
    KhuyenMaiId         INT                             NULL,       -- NULL = Không dùng mã KM

    -- *** PHÂN RÃ TIỀN THEO YÊU CẦU ***
    TongTienVe          DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- Tổng tiền vé (trước giảm giá)
    TongTienFnB         DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- Tổng tiền đồ ăn uống
    TongTienGoc         DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- = TongTienVe + TongTienFnB
    TienGiamKM          DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- Tiền giảm từ mã KhuyenMai
    TienGiamDiem        DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- Tiền giảm từ điểm tích lũy KH
    TienGiamThanhVien   DECIMAL(18,0)   DEFAULT 0       NOT NULL,   -- Tiền giảm từ hạng thành viên
    TongTienGiam        AS (TienGiamKM + TienGiamDiem + TienGiamThanhVien) PERSISTED, -- Tổng giảm (computed)
    ThanhTien           DECIMAL(18,0)                   NOT NULL,   -- Số tiền KH thực trả (cuối cùng)

    -- Thanh toán
    PhuongThucTT        NVARCHAR(30)                    NOT NULL,   -- 'TienMat', 'ChuyenKhoan', 'The', 'KetHop'
    TienKhachDua        DECIMAL(18,0)                   NULL,       -- Chỉ dùng khi PTTT = 'TienMat'
    TienThoiLai         DECIMAL(18,0)                   NULL,       -- Tiền thối lại cho khách

    -- Điểm tích lũy
    DiemTichDuoc        INT             DEFAULT 0       NOT NULL,   -- Điểm được cộng từ giao dịch này
    DiemSuDung          INT             DEFAULT 0       NOT NULL,   -- Điểm đã dùng để đổi giảm giá

    -- Trạng thái hóa đơn
    TrangThai           NVARCHAR(20)    DEFAULT N'HoanThanh' NOT NULL, -- 'HoanThanh', 'DaHuy'
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

-- Index thống kê doanh thu theo thời gian (dùng trong Module Báo cáo)
CREATE INDEX IX_HoaDon_ThoiGianTao  ON dbo.HoaDon (ThoiGianTao);
CREATE INDEX IX_HoaDon_CaId         ON dbo.HoaDon (CaId);
CREATE INDEX IX_HoaDon_KhachHangId  ON dbo.HoaDon (KhachHangId);
PRINT N'>> Da tao bang HoaDon';
GO

-- ------------------------------------------------------------
-- 22. BẢNG: VePhim
-- Mục đích: Chi tiết từng vé trong hóa đơn
-- Nguyên tắc: Snapshot giá tại thời điểm mua (GiaBan) - không phụ thuộc thay đổi QuyTacGia sau này
-- FK đến: HoaDon (trên), LichChieu_Ghe (trên), QuyTacGia (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.VePhim', 'U') IS NOT NULL DROP TABLE dbo.VePhim;

CREATE TABLE dbo.VePhim (
    VeId                INT             IDENTITY(1,1)   NOT NULL,
    HoaDonId            INT                             NOT NULL,
    LichChieuGheId      INT                             NOT NULL,   -- Xác định đúng: suất chiếu + ghế nào
    QuyTacId            INT                             NULL,       -- Quy tắc giá đã áp dụng (có thể NULL nếu giá cố định)

    -- Snapshot giá để đảm bảo tính toàn vẹn lịch sử
    GiaGoc              DECIMAL(18,0)                   NOT NULL,   -- Giá trước khi giảm theo hạng TV
    GiaBan              DECIMAL(18,0)                   NOT NULL,   -- Giá thực tế đã bán (snapshot)

    -- Thông tin đối tượng khách (ảnh hưởng đến giá)
    DoiTuongKhach       NVARCHAR(20)    DEFAULT N'NguoiLon' NOT NULL, -- 'NguoiLon', 'SinhVien', 'TreEm'

    -- Trạng thái vé
    TrangThai           NVARCHAR(20)    DEFAULT N'DaBan' NOT NULL,  -- 'DaBan', 'DaHuy'
    ThoiGianIn          DATETIME                        NULL,       -- Thời điểm in vé / xuất PDF
    MaVach              NVARCHAR(100)                   NULL,       -- Barcode/QR code nếu cần

    CONSTRAINT PK_VePhim                PRIMARY KEY (VeId),
    CONSTRAINT UQ_VePhim_GheSuat        UNIQUE (LichChieuGheId),    -- 1 chỗ ngồi/suất chỉ bán 1 vé
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

-- ------------------------------------------------------------
-- 23. BẢNG: HoaDon_FnB
-- Mục đích: Chi tiết đồ ăn uống trong hóa đơn
-- Lưu ý: Cho phép bán cả sản phẩm đơn lẻ (SanPhamId) lẫn combo (ComboId)
--        CHECK constraint đảm bảo chỉ 1 trong 2 có giá trị, không được cả 2 cùng NULL
-- FK đến: HoaDon (trên), SanPhamFnB (Phần 1), Combo (Phần 1)
-- ------------------------------------------------------------
IF OBJECT_ID('dbo.HoaDon_FnB', 'U') IS NOT NULL DROP TABLE dbo.HoaDon_FnB;

CREATE TABLE dbo.HoaDon_FnB (
    HoaDonFnBId     INT             IDENTITY(1,1)   NOT NULL,
    HoaDonId        INT                             NOT NULL,
    -- Hoặc bán sản phẩm đơn, hoặc bán combo - chỉ 1 trong 2 được có giá trị
    SanPhamId       INT                             NULL,
    ComboId         INT                             NULL,
    SoLuong         INT             DEFAULT 1       NOT NULL,
    -- Snapshot giá tại thời điểm bán (tránh bị ảnh hưởng khi admin thay đổi giá sau này)
    DonGia          DECIMAL(18,0)                   NOT NULL,
    ThanhTien       AS (SoLuong * DonGia) PERSISTED, -- Cột tính toán: tiền từng dòng

    CONSTRAINT PK_HoaDon_FnB            PRIMARY KEY (HoaDonFnBId),
    CONSTRAINT CK_HoaDon_FnB_SoLuong    CHECK (SoLuong > 0),
    CONSTRAINT CK_HoaDon_FnB_DonGia     CHECK (DonGia >= 0),
    -- Logic nghiệp vụ: Phải là sản phẩm ĐƠN HOẶC combo, không được cả 2 đều NULL
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
-- BƯỚC CUỐI: THÊM FK VÒNG (Circular FK)
-- Vấn đề kỹ thuật:
--   LichChieu_Ghe.VePhimId → VePhim (VePhim chưa tồn tại khi tạo LichChieu_Ghe)
--   VePhim.LichChieuGheId  → LichChieu_Ghe (tạo trước)
-- Giải pháp: Tạo 2 bảng trước, sau đó ALTER để thêm FK còn thiếu
-- ============================================================
ALTER TABLE dbo.LichChieu_Ghe
    ADD CONSTRAINT FK_LichChieuGhe_VePhim
        FOREIGN KEY (VePhimId)
        REFERENCES dbo.VePhim (VeId);

PRINT N'>> Da them FK vong: LichChieu_Ghe.VePhimId → VePhim';
GO


-- ============================================================
-- KIỂM TRA: Liệt kê tất cả bảng và số cột trong CinemaDB
-- Chạy đoạn này để xác nhận 23 bảng đã được tạo đủ
-- ============================================================
SELECT
    t.name          AS [Ten Bang],
    s.name          AS [Schema],
    COUNT(c.column_id) AS [So Cot],
    t.create_date   AS [Ngay Tao]
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.columns c ON t.object_id = c.object_id
WHERE s.name = 'dbo'
GROUP BY t.name, s.name, t.create_date
ORDER BY t.create_date, t.name;
GO

-- ============================================================
-- KIỂM TRA: Liệt kê tất cả Foreign Key trong database
-- Dùng để verify các liên kết giữa bảng
-- ============================================================
SELECT
    fk.name                     AS [Ten FK],
    tp.name                     AS [Bang Cha],
    cp.name                     AS [Cot Cha],
    tr.name                     AS [Bang Con],
    cr.name                     AS [Cot Con]
FROM sys.foreign_keys fk
JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
JOIN sys.tables tp   ON fkc.referenced_object_id = tp.object_id
JOIN sys.columns cp  ON fkc.referenced_object_id = cp.object_id AND fkc.referenced_column_id = cp.column_id
JOIN sys.tables tr   ON fkc.parent_object_id = tr.object_id
JOIN sys.columns cr  ON fkc.parent_object_id = cr.object_id AND fkc.parent_column_id = cr.column_id
ORDER BY tp.name, tr.name;
GO

PRINT N'';
PRINT N'==============================================';
PRINT N'  7 bang da duoc tao     ';
PRINT N'  Phim_TheLoai                               ';
PRINT N'  CaLamViec                                  ';
PRINT N'  LichChieu, LichChieu_Ghe                   ';
PRINT N'  HoaDon, VePhim, HoaDon_FnB                 ';
PRINT N'  + FK vong LichChieu_Ghe <-> VePhim         ';
PRINT N'  Tong cong: 23 bang, san sang cho du lieu   ';
PRINT N'==============================================';
GO