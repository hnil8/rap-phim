namespace CinemaManagement.BLL.DTOs;

// DTO login request từ Web/WinForms
public class LoginRequestDto
{
    public string TenDangNhap { get; set; } = string.Empty;
    public string MatKhau { get; set; } = string.Empty;
}

// DTO user sau khi đăng nhập thành công
public class UserDto
{
    public int TaiKhoanId { get; set; }
    public string TenDangNhap { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;
    public string VaiTro { get; set; } = string.Empty;
    public int NhanVienId { get; set; }
}
