namespace CinemaManagement.BLL.DTOs;

// DTO khách hàng (đọc)
public class KhachHangDto
{
    public int KhachHangId { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string? Email { get; set; }
    public DateOnly? NgaySinh { get; set; }
    public string? GioiTinh { get; set; }
    public int DiemTichLuy { get; set; }
    public string? TenHangThanhVien { get; set; }
    public string? MauHang { get; set; }
    public DateTime NgayDangKy { get; set; }
}

// DTO tạo mới khách hàng
public class CreateCustomerDto
{
    public string HoTen { get; set; } = string.Empty;
    public string SoDienThoai { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? GioiTinh { get; set; }
    public DateOnly? NgaySinh { get; set; }
}

// DTO cập nhật thông tin cá nhân
public class UpdateCustomerDto
{
    public int KhachHangId { get; set; }
    public string HoTen { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? GioiTinh { get; set; }
    public DateOnly? NgaySinh { get; set; }
}