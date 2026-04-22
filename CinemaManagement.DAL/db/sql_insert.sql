-- ============================================================
-- SEED DATA: Dữ liệu người dùng hệ thống CinemaDB
-- Thứ tự INSERT: VaiTro → NhanVien → TaiKhoan → HangThanhVien → KhachHang
-- Lưu ý: MatKhauHash đang lưu plain text (theo yêu cầu hiện tại)
--        → Cần chuyển sang BCrypt khi đưa lên production
-- ============================================================

USE CinemaDB;
GO

-- ============================================================
-- BƯỚC 1: INSERT VaiTro
-- Tạo 3 vai trò: Admin, QuanLy, NhanVien
-- ============================================================
PRINT N'>> Dang them du lieu VaiTro...';

INSERT INTO dbo.VaiTro (TenVaiTro, MoTa, IsDeleted)
VALUES
    (N'Admin',     N'Quản trị viên hệ thống, có toàn quyền truy cập', 0),
    (N'QuanLy',    N'Quản lý rạp, có quyền xem báo cáo và quản lý nhân viên', 0),
    (N'NhanVien',  N'Nhân viên quầy, chỉ truy cập POS và quản lý đơn hàng', 0);

PRINT N'>> Da them 3 vai tro';
GO

-- ============================================================
-- BƯỚC 2: INSERT NhanVien
-- Tạo nhân viên trước, sau đó liên kết với TaiKhoan
-- ============================================================
PRINT N'>> Dang them du lieu NhanVien...';

INSERT INTO dbo.NhanVien (HoTen, SoDienThoai, Email, GioiTinh, NgaySinh, DiaChi, NgayVaoLam, IsDeleted)
VALUES
    -- Admin
    (N'Hoàng Linh ',   '0901000001', 'admin@cinema.com',    N'Nam', '1990-01-15', N'123 Đường Lê Lợi, Q.1, TP.HCM',        '2022-01-01', 0),

    -- Quản lý
    (N'Trần Anh',   '0901000002', 'quanly@cinema.com',   N'Nữ', '1992-05-20', N'456 Đường Nguyễn Huệ, Q.1, TP.HCM',    '2022-03-15', 0),

    -- Nhân viên 1
    (N'Lê Văn Thiêm',   '0901000003', 'nv01@cinema.com',     N'Nam', '1998-08-10', N'789 Đường Trần Hưng Đạo, Q.5, TP.HCM', '2023-01-10', 0),

    -- Nhân viên 2
    (N'Phạm Thị Thu Hà',    '0901000004', 'nv02@cinema.com',     N'Nữ', '1999-11-25', N'321 Đường Nguyễn Trãi, Q.5, TP.HCM',   '2023-06-01', 0),

    -- Nhân viên 3
    (N'Hoàng Minh Tuấn',    '0901000005', 'nv03@cinema.com',     N'Nam', '2000-03-07', N'654 Đường CMT8, Q.3, TP.HCM',          '2024-01-15', 0);

PRINT N'>> Da them 5 nhan vien';
GO

-- ============================================================
-- BƯỚC 3: INSERT TaiKhoan
-- Liên kết NhanVienId và VaiTroId vừa tạo
-- Dùng subquery để lấy Id động → không bị lỗi khi IDENTITY thay đổi
-- ============================================================
PRINT N'>> Dang them du lieu TaiKhoan...';

INSERT INTO dbo.TaiKhoan (TenDangNhap, MatKhauHash, VaiTroId, NhanVienId, IsActive, IsDeleted)
VALUES
    -- Tài khoản Admin (VaiTroId=1, NhanVienId=1)
    (
        'admin',
        '123456',   -- TODO: Thay bằng BCrypt hash trước khi lên production
        (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'Admin'),
        (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'admin@cinema.com'),
        1, 0
    ),

    -- Tài khoản Quản Lý (VaiTroId=2, NhanVienId=2)
    (
        'quanly',
        '123456',
        (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'QuanLy'),
        (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'quanly@cinema.com'),
        1, 0
    ),

    -- Tài khoản Nhân Viên 1
    (
        'nhanvien01',
        '123456',
        (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'),
        (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv01@cinema.com'),
        1, 0
    ),

    -- Tài khoản Nhân Viên 2
    (
        'nhanvien02',
        '123456',
        (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'),
        (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv02@cinema.com'),
        1, 0
    ),

    -- Tài khoản Nhân Viên 3 (IsActive=0: tài khoản bị khóa, dùng để test)
    (
        'nhanvien03',
        '123456',
        (SELECT VaiTroId FROM dbo.VaiTro WHERE TenVaiTro = N'NhanVien'),
        (SELECT NhanVienId FROM dbo.NhanVien WHERE Email = 'nv03@cinema.com'),
        0, 0    -- IsActive = 0: tài khoản bị khóa
    );

PRINT N'>> Da them 5 tai khoan (1 Admin, 1 QuanLy, 3 NhanVien)';
GO

-- ============================================================
-- BƯỚC 4: INSERT HangThanhVien
-- Tạo trước KhachHang vì KhachHang.HangId FK đến bảng này
-- ============================================================
PRINT N'>> Dang them du lieu HangThanhVien...';

INSERT INTO dbo.HangThanhVien (TenHang, DiemToiThieu, PhanTramUuDai, MoTa, MauSac, IsDeleted)
VALUES
    (N'Thành Viên',  0,    0.00,  N'Hạng mặc định khi đăng ký',              '#C0C0C0', 0),  -- Bạc
    (N'Bạc',         500,  5.00,  N'Tích lũy từ 500 điểm, giảm thêm 5%',     '#A8A9AD', 0),
    (N'Vàng',        2000, 10.00, N'Tích lũy từ 2000 điểm, giảm thêm 10%',   '#FFD700', 0),
    (N'VIP',         5000, 15.00, N'Tích lũy từ 5000 điểm, giảm thêm 15%',   '#FF6347', 0),
    (N'VVIP',        10000,20.00, N'Tích lũy từ 10000 điểm, giảm thêm 20%',  '#9B59B6', 0);

PRINT N'>> Da them 5 hang thanh vien';
GO

-- ============================================================
-- BƯỚC 5: INSERT KhachHang
-- Tạo 5 khách hàng mẫu với các hạng thành viên khác nhau
-- MatKhauHash: dùng cho đăng nhập nền tảng Web
-- ============================================================
PRINT N'>> Dang them du lieu KhachHang...';

INSERT INTO dbo.KhachHang (HoTen, SoDienThoai, Email, MatKhauHash, NgaySinh, GioiTinh, DiemTichLuy, HangId, IsDeleted)
VALUES
    -- Khách hàng VVIP (nhiều điểm nhất)
    (
        N'Nguyễn Thị Lan',
        '0912345601',
        'lan.nguyen@gmail.com',
        '123456',   -- TODO: Thay bằng BCrypt hash
        '1995-04-12',
        N'Nữ',
        12500,
        (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'VVIP'),
        0
    ),

    -- Khách hàng VIP
    (
        N'Trần Văn Bình',
        '0912345602',
        'binh.tran@gmail.com',
        '123456',
        '1993-09-30',
        N'Nam',
        6200,
        (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'VIP'),
        0
    ),

    -- Khách hàng hạng Vàng
    (
        N'Lê Thị Cẩm',
        '0912345603',
        'cam.le@gmail.com',
        '123456',
        '2000-07-18',
        N'Nữ',
        2300,
        (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Vàng'),
        0
    ),

    -- Khách hàng hạng Bạc
    (
        N'Phạm Quốc Dũng',
        '0912345604',
        'dung.pham@gmail.com',
        '123456',
        '1997-02-14',
        N'Nam',
        750,
        (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Bạc'),
        0
    ),

    -- Khách hàng mới (hạng Thành Viên, chưa có điểm)
    (
        N'Hoàng Thị Emm',
        '0912345605',
        'emm.hoang@gmail.com',
        '123456',
        '2002-12-05',
        N'Nữ',
        0,
        (SELECT HangId FROM dbo.HangThanhVien WHERE TenHang = N'Thành Viên'),
        0
    ),

    -- Khách vãng lai (HangId = NULL, chưa đăng ký tài khoản Web)
    (
        N'Khách Vãng Lai',
        '0000000000',
        NULL,
        NULL,
        NULL,
        NULL,
        0,
        NULL,
        0
    );

PRINT N'>> Da them 6 khach hang (1 VVIP, 1 VIP, 1 Vang, 1 Bac, 1 Thanh Vien, 1 Vang Lai)';
GO

-- ============================================================
-- KIỂM TRA: Xem lại toàn bộ dữ liệu vừa thêm
-- ============================================================

PRINT N'';
PRINT N'--- KIEM TRA DU LIEU ---';

-- Xem VaiTro
SELECT 'VaiTro' AS [Bang], VaiTroId, TenVaiTro, MoTa FROM dbo.VaiTro;

-- Xem TaiKhoan kèm thông tin NhanVien và VaiTro
SELECT
    tk.TaiKhoanId,
    tk.TenDangNhap,
    tk.MatKhauHash,
    vt.TenVaiTro    AS [Vai Tro],
    nv.HoTen        AS [Ho Ten],
    nv.Email,
    CASE tk.IsActive WHEN 1 THEN N'Hoạt động' ELSE N'Bị khóa' END AS [Trang Thai]
FROM dbo.TaiKhoan tk
JOIN dbo.VaiTro   vt ON tk.VaiTroId   = vt.VaiTroId
JOIN dbo.NhanVien nv ON tk.NhanVienId = nv.NhanVienId
ORDER BY tk.TaiKhoanId;

-- Xem KhachHang kèm HangThanhVien
SELECT
    kh.KhachHangId,
    kh.HoTen,
    kh.SoDienThoai,
    kh.Email,
    kh.DiemTichLuy,
    ISNULL(htv.TenHang, N'Vãng lai') AS [Hang Thanh Vien]
FROM dbo.KhachHang kh
LEFT JOIN dbo.HangThanhVien htv ON kh.HangId = htv.HangId
ORDER BY kh.KhachHangId;

GO

PRINT N'';
PRINT N'==============================================';
PRINT N'  SEED DATA HOAN THANH                       ';
PRINT N'  5 TaiKhoan: admin / quanly / nhanvien01-03 ';
PRINT N'  Mat khau mac dinh: 123456                  ';
PRINT N'  6 KhachHang mau voi cac hang khac nhau     ';
PRINT N'==============================================';
GO