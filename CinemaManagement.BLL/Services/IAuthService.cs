using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IAuthService
{
    Task<ServiceResult<UserDto>> LoginAsync(LoginRequestDto request);
    Task<ServiceResult> ChangePasswordAsync(int taiKhoanId, string matKhauCu, string matKhauMoi);
}
