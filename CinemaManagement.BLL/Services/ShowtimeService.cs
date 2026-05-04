using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services;

public class ShowtimeService : IShowtimeService
{
    private readonly CinemaDbContext _db;

    public ShowtimeService(CinemaDbContext db)
    {
        _db = db;
    }

    public async Task<List<LichChieuDto>> GetByPhimIdAsync(int phimId, DateTime? ngay = null)
    {
        var query = _db.LichChieus
            .AsNoTracking()
            .Include(lc => lc.Phim)
            .Include(lc => lc.Phong)
            .Where(lc => lc.PhimId == phimId && lc.GioBatDau >= DateTime.Now && !lc.IsDeleted);

        if (ngay.HasValue)
            query = query.Where(lc => lc.GioBatDau.Date == ngay.Value.Date);

        return await query
            .OrderBy(lc => lc.GioBatDau)
            .Select(lc => MapToDto(lc))
            .ToListAsync();
    }

    public async Task<List<LichChieuDto>> GetByNgayAsync(DateTime ngay)
    {
        return await _db.LichChieus
            .AsNoTracking()
            .Include(lc => lc.Phim)
            .Include(lc => lc.Phong)
            .Where(lc => !lc.IsDeleted && lc.GioBatDau.Date == ngay.Date && lc.TrangThai != "HuyChieu")
            .OrderBy(lc => lc.GioBatDau)
            .Select(lc => MapToDto(lc))
            .ToListAsync();
    }

    public async Task<LichChieuDto?> GetByIdAsync(int lichChieuId)
    {
        var lc = await _db.LichChieus
            .AsNoTracking()
            .Include(item => item.Phim)
            .Include(item => item.Phong)
            .FirstOrDefaultAsync(item => item.LichChieuId == lichChieuId && !item.IsDeleted);

        return lc == null ? null : MapToDto(lc);
    }

    public async Task<List<SeatDto>> GetSeatMapAsync(int lichChieuId)
    {
        var cutoff = DateTime.Now.AddMinutes(-10);

        var rows = await _db.LichChieuGhes
            .AsNoTracking()
            .Include(lcg => lcg.Ghe)
                .ThenInclude(g => g.LoaiGhe)
            .Include(lcg => lcg.LichChieu)
            .Where(lcg => lcg.LichChieuId == lichChieuId)
            .OrderBy(lcg => lcg.Ghe.DayGhe)
            .ThenBy(lcg => lcg.Ghe.CotGhe)
            .ToListAsync();

        var lichChieu = rows.FirstOrDefault()?.LichChieu;
        var giaVeCoBan = lichChieu?.GiaVeCoBan ?? 0m;

        return rows.Select(lcg =>
        {
            var trangThai = lcg.TrangThaiGhe;
            if (trangThai == "DangGiu" && lcg.ThoiGianGiu.HasValue && lcg.ThoiGianGiu.Value < cutoff)
                trangThai = "Trong";

            string tenLoaiGhe = lcg.Ghe.LoaiGhe.TenLoai;

            // ????????????????????????????????????????????????????????????
            // ÁP D?NG YÊU C?U L?P TRÌNH: Phân lo?i h? s? giá
            // ????????????????????????????????????????????????????????????
            decimal heSo = 1.0m; // M?c ??nh gh? th??ng

            if (tenLoaiGhe == "VIP")
            {
                heSo = 1.2m; // T?ng 20%
            }
            else if (tenLoaiGhe == "Sweetbox")
            {
                heSo = 1.5m; // T?ng 50%
            }
            // (N?u trong Database b?ng LoaiGhes c?a b?n ?ã c?u hình s?n c?t HeSoGia chu?n xác, 
            // b?n có th? xóa c?m if-else trên và gán tr?c ti?p: heSo = lcg.Ghe.LoaiGhe.HeSoGia;)

            return new SeatDto
            {
                LichChieuGheId = lcg.LichChieuGheId,
                GheId = lcg.GheId,
                TenGhe = lcg.Ghe.TenGhe ?? $"{lcg.Ghe.DayGhe}{lcg.Ghe.CotGhe}",
                DayGhe = lcg.Ghe.DayGhe.Trim(),
                CotGhe = lcg.Ghe.CotGhe,
                LoaiGhe = tenLoaiGhe,
                HeSoGia = heSo,
                TrangThai = trangThai,
                GiaGoc = giaVeCoBan,
                // Tính giá bán th?c t? và làm tròn
                GiaBan = Math.Round(giaVeCoBan * heSo, 0)
            };
        }).ToList();
    }

    public async Task<ServiceResult> CreateAsync(CreateLichChieuDto dto)
    {
        var phim = await _db.Phims.FindAsync(dto.PhimId);
        if (phim == null) return ServiceResult.Fail("Khong tim thay phim");

        var gioKetThuc = dto.GioBatDau.AddMinutes(phim.ThoiLuongPhut + 20);
        var conflict = await HasConflictAsync(dto.PhongId, dto.GioBatDau, gioKetThuc);
        if (conflict) return ServiceResult.Fail("Phong chieu da co lich trong khung gio nay");

        var lichChieu = new LichChieu
        {
            PhimId = dto.PhimId,
            PhongId = dto.PhongId,
            GioBatDau = dto.GioBatDau,
            GioKetThuc = gioKetThuc,
            GiaVeCoBan = dto.GiaVeCoBan,
            TrangThai = "ChuaChieu",
            NgayTao = DateTime.Now
        };

        _db.LichChieus.Add(lichChieu);
        await _db.SaveChangesAsync();

        var danhSachGhe = await _db.GheNgois
            .Where(g => g.PhongId == dto.PhongId && !g.IsDeleted)
            .ToListAsync();

        var lichChieuGhes = danhSachGhe.Select(g => new LichChieuGhe
        {
            LichChieuId = lichChieu.LichChieuId,
            GheId = g.GheId,
            TrangThaiGhe = "Trong"
        }).ToList();

        _db.LichChieuGhes.AddRange(lichChieuGhes);
        await _db.SaveChangesAsync();

        return ServiceResult.Success("Tao lich chieu thanh cong", lichChieu.LichChieuId);
    }

    public async Task<ServiceResult> UpdateAsync(int lichChieuId, CreateLichChieuDto dto)
    {
        var lichChieu = await _db.LichChieus
            .Include(lc => lc.LichChieuGhes)
                .ThenInclude(g => g.Ghe)
            .FirstOrDefaultAsync(lc => lc.LichChieuId == lichChieuId && !lc.IsDeleted);

        if (lichChieu == null) return ServiceResult.Fail("Khong tim thay lich chieu");

        var phim = await _db.Phims.FindAsync(dto.PhimId);
        if (phim == null) return ServiceResult.Fail("Khong tim thay phim");

        var gioKetThuc = dto.GioBatDau.AddMinutes(phim.ThoiLuongPhut + 20);
        var conflict = await HasConflictAsync(dto.PhongId, dto.GioBatDau, gioKetThuc, lichChieuId);
        if (conflict) return ServiceResult.Fail("Phong chieu da co lich trong khung gio nay");

        var hasBookedSeat = lichChieu.LichChieuGhes.Any(g => g.VePhimId.HasValue || g.TrangThaiGhe == "DaDat");
        if (hasBookedSeat && (lichChieu.PhongId != dto.PhongId || lichChieu.GioBatDau != dto.GioBatDau))
            return ServiceResult.Fail("Khong the doi phong hoac gio chieu khi da co ghe duoc dat");

        var needsRebuildSeatMap = !hasBookedSeat && lichChieu.LichChieuGhes.Any(g => g.Ghe.PhongId != dto.PhongId);

        lichChieu.PhimId = dto.PhimId;
        lichChieu.PhongId = dto.PhongId;
        lichChieu.GioBatDau = dto.GioBatDau;
        lichChieu.GioKetThuc = gioKetThuc;
        lichChieu.GiaVeCoBan = dto.GiaVeCoBan;

        if (needsRebuildSeatMap)
        {
            _db.LichChieuGhes.RemoveRange(lichChieu.LichChieuGhes);

            var danhSachGhe = await _db.GheNgois
                .Where(g => g.PhongId == dto.PhongId && !g.IsDeleted)
                .ToListAsync();

            var lichChieuGhes = danhSachGhe.Select(g => new LichChieuGhe
            {
                LichChieuId = lichChieu.LichChieuId,
                GheId = g.GheId,
                TrangThaiGhe = "Trong"
            }).ToList();

            _db.LichChieuGhes.AddRange(lichChieuGhes);
        }

        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cap nhat lich chieu thanh cong");
    }

    public async Task<ServiceResult> UpdateTrangThaiAsync(int lichChieuId, string trangThai)
    {
        var lc = await _db.LichChieus.FindAsync(lichChieuId);
        if (lc == null) return ServiceResult.Fail("Khong tim thay lich chieu");

        lc.TrangThai = trangThai;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cap nhat trang thai thanh cong");
    }

    private Task<bool> HasConflictAsync(int phongId, DateTime gioBatDau, DateTime gioKetThuc, int? excludeLichChieuId = null)
    {
        return _db.LichChieus.AnyAsync(lc =>
            lc.PhongId == phongId &&
            !lc.IsDeleted &&
            lc.TrangThai != "HuyChieu" &&
            (!excludeLichChieuId.HasValue || lc.LichChieuId != excludeLichChieuId.Value) &&
            lc.GioBatDau < gioKetThuc &&
            lc.GioKetThuc > gioBatDau);
    }

    private static LichChieuDto MapToDto(LichChieu lc) => new()
    {
        LichChieuId = lc.LichChieuId,
        PhimId = lc.PhimId,
        TenPhim = lc.Phim?.TenPhim ?? string.Empty,
        PosterUrl = lc.Phim?.PosterUrl,
        PhongId = lc.PhongId,
        TenPhong = lc.Phong?.TenPhong ?? string.Empty,
        LoaiPhong = lc.Phong?.LoaiPhong ?? "2D",
        GioBatDau = lc.GioBatDau,
        GioKetThuc = lc.GioKetThuc,
        GiaVeCoBan = lc.GiaVeCoBan,
        TrangThai = lc.TrangThai
    };
}