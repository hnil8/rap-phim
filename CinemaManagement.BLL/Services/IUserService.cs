using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IUserService
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto?> GetByIdAsync(int taiKhoanId);
    Task<ServiceResult> CreateAsync(string hoTen, string tenDangNhap, string matKhau, string vaiTro);
    Task<ServiceResult> UpdateAsync(int taiKhoanId, string hoTen, string vaiTro);
    Task<ServiceResult> DeleteAsync(int taiKhoanId);
    Task<ServiceResult> ResetPasswordAsync(int taiKhoanId, string matKhauMoi);
}
