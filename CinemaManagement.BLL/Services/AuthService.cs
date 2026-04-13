using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class AuthService : IAuthService
{
    private readonly CinemaDbContext _db;

    public AuthService(CinemaDbContext db) => _db = db;

    public async Task<ServiceResult<UserDto>> LoginAsync(LoginRequestDto request)
    {
        var tk = await _db.TaiKhoans
            .AsNoTracking()
            .Include(t => t.NhanVien)
            .Include(t => t.VaiTro)
            .FirstOrDefaultAsync(t => t.TenDangNhap == request.TenDangNhap && !t.IsDeleted);

        if (tk == null)
            return ServiceResult<UserDto>.Fail("Tên đăng nhập không tồn tại");

        if (!BCrypt.Net.BCrypt.Verify(request.MatKhau, tk.MatKhauHash))
            return ServiceResult<UserDto>.Fail("Mật khẩu không chính xác");

        if (!tk.IsActive)
            return ServiceResult<UserDto>.Fail("Tài khoản đã bị vô hiệu hóa");

        var dto = new UserDto
        {
            TaiKhoanId  = tk.TaiKhoanId,
            TenDangNhap = tk.TenDangNhap,
            HoTen       = tk.NhanVien?.HoTen ?? tk.TenDangNhap,
            VaiTro      = tk.VaiTro?.TenVaiTro ?? string.Empty,
            NhanVienId  = tk.NhanVienId
        };

        return ServiceResult<UserDto>.Success(dto, "Đăng nhập thành công");
    }

    public async Task<ServiceResult> ChangePasswordAsync(int taiKhoanId, string matKhauCu, string matKhauMoi)
    {
        var tk = await _db.TaiKhoans.FindAsync(taiKhoanId);
        if (tk == null) return ServiceResult.Fail("Không tìm thấy tài khoản");

        if (!BCrypt.Net.BCrypt.Verify(matKhauCu, tk.MatKhauHash))
            return ServiceResult.Fail("Mật khẩu cũ không đúng");

        tk.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Đổi mật khẩu thành công");
    }
}
