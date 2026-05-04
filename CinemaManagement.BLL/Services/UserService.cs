using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class UserService : IUserService
{
    private readonly CinemaDbContext _db;

    public UserService(CinemaDbContext db) => _db = db;

    public async Task<List<UserDto>> GetAllAsync()
    {
        return await _db.TaiKhoans
            .AsNoTracking()
            .Include(t => t.NhanVien)
            .Include(t => t.VaiTro)
            .Where(t => !t.IsDeleted)
            .OrderBy(t => t.TenDangNhap)
            .Select(t => new UserDto
            {
                TaiKhoanId  = t.TaiKhoanId,
                TenDangNhap = t.TenDangNhap,
                HoTen       = t.NhanVien.HoTen,
                VaiTro      = t.VaiTro.TenVaiTro,
                NhanVienId  = t.NhanVienId
            })
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int taiKhoanId)
    {
        return await _db.TaiKhoans
            .AsNoTracking()
            .Include(t => t.NhanVien)
            .Include(t => t.VaiTro)
            .Where(t => t.TaiKhoanId == taiKhoanId && !t.IsDeleted)
            .Select(t => new UserDto
            {
                TaiKhoanId  = t.TaiKhoanId,
                TenDangNhap = t.TenDangNhap,
                HoTen       = t.NhanVien.HoTen,
                VaiTro      = t.VaiTro.TenVaiTro,
                NhanVienId  = t.NhanVienId
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceResult> CreateAsync(string hoTen, string tenDangNhap, string matKhau, string tenVaiTro)
    {
        if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == tenDangNhap && !t.IsDeleted))
            return ServiceResult.Fail("Tên đăng nhập đã tồn tại");

        // Tìm VaiTroId theo tên
        var vaiTro = await _db.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == tenVaiTro);
        if (vaiTro == null) return ServiceResult.Fail($"Không tìm thấy vai trò: {tenVaiTro}");

        var nhanVien = new NhanVien
        {
            HoTen      = hoTen,
            NgayVaoLam = DateOnly.FromDateTime(DateTime.Today)
        };
        _db.NhanViens.Add(nhanVien);
        await _db.SaveChangesAsync();

        var taiKhoan = new TaiKhoan
        {
            TenDangNhap = tenDangNhap,
            MatKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhau),
            VaiTroId    = vaiTro.VaiTroId,
            IsActive    = true,
            NhanVienId  = nhanVien.NhanVienId,
            NgayTao     = DateTime.Now
        };
        _db.TaiKhoans.Add(taiKhoan);
        await _db.SaveChangesAsync();

        return ServiceResult.Success("Tạo tài khoản nhân viên thành công");
    }

    public async Task<ServiceResult> UpdateAsync(int taiKhoanId, string hoTen, string tenVaiTro)
    {
        var tk = await _db.TaiKhoans
            .Include(t => t.NhanVien)
            .FirstOrDefaultAsync(t => t.TaiKhoanId == taiKhoanId);
        if (tk == null) return ServiceResult.Fail("Không tìm thấy tài khoản");

        var vaiTro = await _db.VaiTros.FirstOrDefaultAsync(v => v.TenVaiTro == tenVaiTro);
        if (vaiTro != null) tk.VaiTroId = vaiTro.VaiTroId;

        tk.NhanVien.HoTen = hoTen;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cập nhật tài khoản thành công");
    }

    public async Task<ServiceResult> DeleteAsync(int taiKhoanId)
    {
        var tk = await _db.TaiKhoans.FindAsync(taiKhoanId);
        if (tk == null) return ServiceResult.Fail("Không tìm thấy tài khoản");
        tk.IsDeleted = true;
        tk.IsActive  = false;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Xóa tài khoản thành công");
    }

    public async Task<ServiceResult> ResetPasswordAsync(int taiKhoanId, string matKhauMoi)
    {
        var tk = await _db.TaiKhoans.FindAsync(taiKhoanId);
        if (tk == null) return ServiceResult.Fail("Không tìm thấy tài khoản");
        tk.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Reset mật khẩu thành công");
    }
}
