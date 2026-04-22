using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class CustomerService : ICustomerService
{
    private readonly CinemaDbContext _db;
    public CustomerService(CinemaDbContext db) => _db = db;

    public async Task<KhachHangDto?> GetByIdAsync(int khachHangId)
    {
        return await _db.KhachHangs
            .AsNoTracking()
            .Include(kh => kh.Hang)
            .Where(kh => kh.KhachHangId == khachHangId && !kh.IsDeleted)
            .Select(kh => MapToDto(kh))
            .FirstOrDefaultAsync();
    }

    public async Task<KhachHangDto?> GetBySoDienThoaiAsync(string soDienThoai)
    {
        return await _db.KhachHangs
            .AsNoTracking()
            .Include(kh => kh.Hang)
            .Where(kh => kh.SoDienThoai == soDienThoai && !kh.IsDeleted)
            .Select(kh => MapToDto(kh))
            .FirstOrDefaultAsync();
    }

    public async Task<ServiceResult<KhachHangDto>> CreateAsync(CreateCustomerDto dto)
    {
        // Kiểm tra SĐT đã tồn tại
        if (await _db.KhachHangs.AnyAsync(kh => kh.SoDienThoai == dto.SoDienThoai && !kh.IsDeleted))
            return ServiceResult<KhachHangDto>.Fail("Số điện thoại đã được đăng ký");

        var khachHang = new KhachHang
        {
            HoTen       = dto.HoTen,
            SoDienThoai = dto.SoDienThoai,
            Email       = dto.Email,
            GioiTinh    = dto.GioiTinh,
            NgaySinh    = dto.NgaySinh,
            DiemTichLuy = 0,
            NgayDangKy  = DateTime.Now
        };

        _db.KhachHangs.Add(khachHang);
        await _db.SaveChangesAsync();

        var result = await GetByIdAsync(khachHang.KhachHangId);
        return ServiceResult<KhachHangDto>.Success(result!, "Đăng ký thành công");
    }

    public async Task<ServiceResult> UpdateAsync(UpdateCustomerDto dto)
    {
        var kh = await _db.KhachHangs.FindAsync(dto.KhachHangId);
        if (kh == null || kh.IsDeleted) return ServiceResult.Fail("Không tìm thấy khách hàng");

        kh.HoTen   = dto.HoTen;
        kh.Email   = dto.Email;
        kh.GioiTinh = dto.GioiTinh;
        kh.NgaySinh = dto.NgaySinh;

        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cập nhật thông tin thành công");
    }

    public async Task<ServiceResult> DeleteAsync(int khachHangId)
    {
        var kh = await _db.KhachHangs.FindAsync(khachHangId);
        if (kh == null) return ServiceResult.Fail("Không tìm thấy khách hàng");
        kh.IsDeleted = true;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Xóa khách hàng thành công");
    }

    public async Task<List<KhachHangDto>> SearchAsync(string keyword)
    {
        return await _db.KhachHangs
            .AsNoTracking()
            .Include(kh => kh.Hang)
            .Where(kh => !kh.IsDeleted &&
                (kh.HoTen.Contains(keyword) || kh.SoDienThoai.Contains(keyword)))
            .OrderBy(kh => kh.HoTen)
            .Take(50)
            .Select(kh => MapToDto(kh))
            .ToListAsync();
    }

    private static KhachHangDto MapToDto(KhachHang kh) => new()
    {
        KhachHangId      = kh.KhachHangId,
        HoTen            = kh.HoTen,
        SoDienThoai      = kh.SoDienThoai,
        Email            = kh.Email,
        GioiTinh         = kh.GioiTinh,
        DiemTichLuy      = kh.DiemTichLuy,
        NgayDangKy       = kh.NgayDangKy,
        TenHangThanhVien = kh.Hang?.TenHang,
        MauHang          = kh.Hang?.MauSac
    };
}
