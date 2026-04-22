namespace CinemaManagement.BLL.Common;

// --- Trạng thái Ghế ---
public enum TrangThaiGhe
{
    Trong,      // Có thể chọn
    DangGiu,    // Đang được giữ (trong 10 phút)
    DaBan,      // Đã thanh toán thành công
    Blocked     // Hỏng/bị chặn bởi quản trị
}

// --- Trạng thái Phim ---
public enum TrangThaiPhim
{
    SapChieu,
    DangChieu,
    NgungChieu
}

// --- Vai trò hệ thống ---
public enum LoaiVaiTro
{
    QuanTriVien = 1,
    NhanVien    = 2,
    ThuNgan     = 3
}

// --- Phương thức thanh toán ---
public enum PhuongThucThanhToan
{
    TienMat,
    ChuyenKhoan,
    VNPAY,
    MoMo
}

// --- Trạng thái Hóa đơn ---
public enum TrangThaiHoaDon
{
    DangXuLy,
    HoanThanh,
    DaHuy,
    HoanTien
}

// --- Loại giảm giá khuyến mãi ---
public enum LoaiGiamGia
{
    PhanTram,
    TienMat
}

// --- Loại đối tượng vé ---
public enum DoiTuongKhach
{
    NguoiLon,
    SinhVien,
    TreEm,
    NguoiCaoTuoi
}
