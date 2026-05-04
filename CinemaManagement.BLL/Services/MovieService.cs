using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services;

public class MovieService : IMovieService
{
    private readonly CinemaDbContext _db;

    public MovieService(CinemaDbContext db)
    {
        _db = db;
    }

    // ────────────────────────────────────────────────────────────────────────
    //  HÀM MỚI BỔ SUNG CHO GIAO DIỆN (Load FlowLayoutPanel)
    // ────────────────────────────────────────────────────────────────────────
    public async Task<List<TheLoaiPhim>> GetAllTheLoaisAsync()
    {
        return await _db.TheLoaiPhims.AsNoTracking().ToListAsync();
    }

    // ────────────────────────────────────────────────────────────────────────
    //  CÁC HÀM GET DỮ LIỆU
    // ────────────────────────────────────────────────────────────────────────
    public async Task<List<PhimDto>> GetDangChieuAsync()
    {
        return await _db.Phims
            .AsNoTracking()
            .Where(p => p.TrangThai == "DangChieu" && !p.IsDeleted)
            .OrderByDescending(p => p.NgayKhoiChieu)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<List<PhimDto>> GetSapChieuAsync()
    {
        return await _db.Phims
            .AsNoTracking()
            .Where(p => p.TrangThai == "SapChieu" && !p.IsDeleted)
            .OrderBy(p => p.NgayKhoiChieu)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<List<PhimDto>> GetAllAsync(string? searchTerm = null, string? trangThai = null)
    {
        var query = _db.Phims.AsNoTracking().Where(p => !p.IsDeleted);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(p => p.TenPhim.Contains(searchTerm) || (p.TenGoc != null && p.TenGoc.Contains(searchTerm)));

        if (!string.IsNullOrWhiteSpace(trangThai))
            query = query.Where(p => p.TrangThai == trangThai);

        return await query
            .OrderByDescending(p => p.NgayTao)
            .Select(p => MapToDto(p))
            .ToListAsync();
    }

    public async Task<PhimDto?> GetByIdAsync(int phimId)
    {
        var phim = await _db.Phims
            .AsNoTracking()
            .Include(p => p.TheLoais)
            .FirstOrDefaultAsync(p => p.PhimId == phimId && !p.IsDeleted);

        if (phim == null) return null;

        var dto = MapToDto(phim);
        dto.TheLoais = phim.TheLoais.Select(t => t.TenTheLoai).ToList();
        return dto;
    }

    // ────────────────────────────────────────────────────────────────────────
    //  CÁC HÀM THAO TÁC DỮ LIỆU (CRUD)
    // ────────────────────────────────────────────────────────────────────────
    public async Task<ServiceResult> CreateAsync(CreatePhimDto dto)
    {
        var phim = new Phim
        {
            TenPhim = dto.TenPhim,
            TenGoc = dto.TenGoc,
            DaoDien = dto.DaoDien,
            DienVienChinh = dto.DienVienChinh,
            ThoiLuongPhut = dto.ThoiLuongPhut,
            NuocSanXuat = dto.NuocSanXuat,
            NamPhatHanh = dto.NamPhatHanh,
            GioiHanDoTuoi = dto.GioiHanDoTuoi,
            NgonNgu = dto.NgonNgu,
            MoTa = dto.MoTa,
            PosterUrl = dto.PosterUrl,
            TrailerUrl = dto.TrailerUrl,
            TrangThai = dto.TrangThai,
            NgayKhoiChieu = dto.NgayKhoiChieu,
            NgayTao = DateTime.Now
        };

        if (dto.TheLoaiIds.Any())
        {
            var theLoais = await _db.TheLoaiPhims
                .Where(t => dto.TheLoaiIds.Contains(t.TheLoaiId))
                .ToListAsync();
            phim.TheLoais = theLoais;
        }

        _db.Phims.Add(phim);
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Thêm phim thành công", phim.PhimId);
    }

    public async Task<ServiceResult> UpdateAsync(int phimId, CreatePhimDto dto)
    {
        var phim = await _db.Phims
            .Include(p => p.TheLoais)
            .FirstOrDefaultAsync(p => p.PhimId == phimId && !p.IsDeleted);

        if (phim == null) return ServiceResult.Fail("Không tìm thấy phim");

        phim.TenPhim = dto.TenPhim;
        phim.TenGoc = dto.TenGoc;
        phim.DaoDien = dto.DaoDien;
        phim.DienVienChinh = dto.DienVienChinh;
        phim.ThoiLuongPhut = dto.ThoiLuongPhut;
        phim.NuocSanXuat = dto.NuocSanXuat;
        phim.NamPhatHanh = dto.NamPhatHanh;
        phim.GioiHanDoTuoi = dto.GioiHanDoTuoi;
        phim.NgonNgu = dto.NgonNgu;
        phim.MoTa = dto.MoTa;
        phim.PosterUrl = dto.PosterUrl;
        phim.TrailerUrl = dto.TrailerUrl;
        phim.TrangThai = dto.TrangThai;
        phim.NgayKhoiChieu = dto.NgayKhoiChieu;

        // Cập nhật thể loại
        if (dto.TheLoaiIds.Any())
        {
            var theLoais = await _db.TheLoaiPhims
                .Where(t => dto.TheLoaiIds.Contains(t.TheLoaiId))
                .ToListAsync();
            phim.TheLoais.Clear();
            foreach (var t in theLoais) phim.TheLoais.Add(t);
        }

        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cập nhật phim thành công");
    }

    public async Task<ServiceResult> SoftDeleteAsync(int phimId)
    {
        var phim = await _db.Phims.FindAsync(phimId);
        if (phim == null) return ServiceResult.Fail("Không tìm thấy phim");

        phim.IsDeleted = true;
        phim.TrangThai = "NgungChieu"; // Đổi trạng thái khi bị xóa mềm
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Đã xóa phim thành công");
    }

    // ────────────────────────────────────────────────────────────────────────
    //  HÀM HỖ TRỢ (Helper)
    // ────────────────────────────────────────────────────────────────────────
    // Hàm projection nội bộ (không include TheLoais để tránh N+1)
    private static PhimDto MapToDto(Phim p) => new()
    {
        PhimId = p.PhimId,
        TenPhim = p.TenPhim,
        TenGoc = p.TenGoc,
        DaoDien = p.DaoDien,
        DienVienChinh = p.DienVienChinh,
        ThoiLuongPhut = p.ThoiLuongPhut,
        NuocSanXuat = p.NuocSanXuat,
        NamPhatHanh = p.NamPhatHanh,
        GioiHanDoTuoi = p.GioiHanDoTuoi,
        NgonNgu = p.NgonNgu,
        MoTa = p.MoTa,
        PosterUrl = p.PosterUrl,
        TrailerUrl = p.TrailerUrl,
        TrangThai = p.TrangThai,
        NgayKhoiChieu = p.NgayKhoiChieu
    };
}