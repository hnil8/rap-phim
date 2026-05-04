using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class PromotionService : IPromotionService
{
    private readonly CinemaDbContext _db;
    public PromotionService(CinemaDbContext db) => _db = db;

    public async Task<List<KhuyenMaiDto>> GetAllAsync()
    {
        return await _db.KhuyenMais
            .AsNoTracking()
            .Where(km => !km.IsDeleted)
            .OrderByDescending(km => km.NgayBatDau)
            .Select(km => MapToDto(km))
            .ToListAsync();
    }

    public async Task<KhuyenMaiDto?> GetByMaCodeAsync(string maCode)
    {
        var km = await _db.KhuyenMais
            .AsNoTracking()
            .FirstOrDefaultAsync(k => k.MaCode == maCode && !k.IsDeleted && k.IsActive);
        return km == null ? null : MapToDto(km);
    }

    public async Task<ServiceResult> CreateAsync(CreateKhuyenMaiDto dto)
    {
        if (await _db.KhuyenMais.AnyAsync(k => k.MaCode == dto.MaCode && !k.IsDeleted))
            return ServiceResult.Fail("Mã code đã tồn tại");

        var km = new KhuyenMai
        {
            TenChuongTrinh  = dto.TenKhuyenMai,
            MaCode          = dto.MaCode.ToUpper(),
            LoaiGiam        = dto.LoaiGiam,
            GiaTriGiam      = dto.GiaTriGiam,
            GiaTriGiamToiDa = dto.GiaTriGiamToiDa,
            NgayBatDau      = dto.NgayBatDau,
            NgayHetHan      = dto.NgayHetHan,
            SoLuongPhatHanh = dto.SoLuongPhatHanh,
            SoLuongDaDung   = 0,
            IsActive        = dto.IsActive
        };
        _db.KhuyenMais.Add(km);
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Tạo khuyến mãi thành công", km.KhuyenMaiId);
    }

    public async Task<ServiceResult> UpdateAsync(int khuyenMaiId, CreateKhuyenMaiDto dto)
    {
        var km = await _db.KhuyenMais.FindAsync(khuyenMaiId);
        if (km == null || km.IsDeleted) return ServiceResult.Fail("Không tìm thấy khuyến mãi");

        km.TenChuongTrinh  = dto.TenKhuyenMai;
        km.LoaiGiam        = dto.LoaiGiam;
        km.GiaTriGiam      = dto.GiaTriGiam;
        km.GiaTriGiamToiDa = dto.GiaTriGiamToiDa;
        km.NgayBatDau      = dto.NgayBatDau;
        km.NgayHetHan      = dto.NgayHetHan;
        km.SoLuongPhatHanh = dto.SoLuongPhatHanh;
        km.IsActive        = dto.IsActive;

        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cập nhật khuyến mãi thành công");
    }

    public async Task<ServiceResult> DeleteAsync(int khuyenMaiId)
    {
        var km = await _db.KhuyenMais.FindAsync(khuyenMaiId);
        if (km == null) return ServiceResult.Fail("Không tìm thấy khuyến mãi");
        km.IsDeleted = true;
        km.IsActive  = false;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Xóa khuyến mãi thành công");
    }

    private static KhuyenMaiDto MapToDto(KhuyenMai km) => new()
    {
        KhuyenMaiId     = km.KhuyenMaiId,
        TenKhuyenMai    = km.TenChuongTrinh,   // TenChuongTrinh → TenKhuyenMai DTO
        MaCode          = km.MaCode,
        LoaiGiam        = km.LoaiGiam,
        GiaTriGiam      = km.GiaTriGiam,
        GiaTriGiamToiDa = km.GiaTriGiamToiDa,
        NgayBatDau      = km.NgayBatDau,
        NgayHetHan      = km.NgayHetHan,
        SoLuongPhatHanh = km.SoLuongPhatHanh,
        SoLuongDaDung   = km.SoLuongDaDung,
        IsActive        = km.IsActive
    };
}
