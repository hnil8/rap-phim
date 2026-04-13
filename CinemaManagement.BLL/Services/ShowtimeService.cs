using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

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
            .Where(lc => lc.PhimId == phimId && lc.GioBatDau >= DateTime.Now);

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
            .Where(lc => lc.GioBatDau.Date == ngay.Date && lc.TrangThai != "HuyChieu")
            .OrderBy(lc => lc.GioBatDau)
            .Select(lc => MapToDto(lc))
            .ToListAsync();
    }

    public async Task<LichChieuDto?> GetByIdAsync(int lichChieuId)
    {
        var lc = await _db.LichChieus
            .AsNoTracking()
            .Include(lc => lc.Phim)
            .Include(lc => lc.Phong)
            .FirstOrDefaultAsync(lc => lc.LichChieuId == lichChieuId);

        return lc == null ? null : MapToDto(lc);
    }

    public async Task<List<SeatDto>> GetSeatMapAsync(int lichChieuId)
    {
        // Lấy tất cả ghế của lịch chiếu kèm trạng thái realtime
        // Ghế hết timeout (DangGiu quá 10 phút) → coi là Trong
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

        // Lấy giá cơ bản của lịch chiếu để tính giá ghế
        var lichChieu = rows.FirstOrDefault()?.LichChieu;
        var giaVeCoBan = lichChieu?.GiaVeCoBan ?? 0m;

        return rows.Select(lcg =>
        {
            // Ghế DangGiu đã quá timeout → trả về Trong
            var trangThai = lcg.TrangThaiGhe;
            if (trangThai == "DangGiu" && lcg.ThoiGianGiu.HasValue && lcg.ThoiGianGiu.Value < cutoff)
                trangThai = "Trong";

            var heSo = lcg.Ghe.LoaiGhe.HeSoGia;
            return new SeatDto
            {
                LichChieuGheId = lcg.LichChieuGheId,
                GheId          = lcg.GheId,
                TenGhe         = lcg.Ghe.TenGhe ?? $"{lcg.Ghe.DayGhe}{lcg.Ghe.CotGhe}",
                DayGhe         = lcg.Ghe.DayGhe.Trim(),
                CotGhe         = lcg.Ghe.CotGhe,
                LoaiGhe        = lcg.Ghe.LoaiGhe.TenLoai,
                HeSoGia        = heSo,
                TrangThai      = trangThai,
                GiaGoc         = giaVeCoBan,
                GiaBan         = Math.Round(giaVeCoBan * heSo, 0)
            };
        }).ToList();
    }

    public async Task<ServiceResult> CreateAsync(CreateLichChieuDto dto)
    {
        // Kiểm tra xung đột phòng chiếu
        var phim = await _db.Phims.FindAsync(dto.PhimId);
        if (phim == null) return ServiceResult.Fail("Không tìm thấy phim");

        var gioKetThuc = dto.GioBatDau.AddMinutes(phim.ThoiLuongPhut + 20); // +20 phút dọn phòng

        var conflict = await _db.LichChieus.AnyAsync(lc =>
            lc.PhongId == dto.PhongId &&
            lc.TrangThai != "HuyChieu" &&
            lc.GioBatDau < gioKetThuc &&
            lc.GioKetThuc > dto.GioBatDau);

        if (conflict) return ServiceResult.Fail("Phòng chiếu đã có lịch trong khung giờ này");

        var lichChieu = new LichChieu
        {
            PhimId      = dto.PhimId,
            PhongId     = dto.PhongId,
            GioBatDau   = dto.GioBatDau,
            GioKetThuc  = gioKetThuc,
            GiaVeCoBan  = dto.GiaVeCoBan,
            TrangThai   = "ChuaChieu",
            NgayTao     = DateTime.Now
        };

        _db.LichChieus.Add(lichChieu);
        await _db.SaveChangesAsync();

        // Tạo bản ghi LichChieu_Ghe cho tất cả ghế trong phòng
        var danhSachGhe = await _db.GheNgois
            .Where(g => g.PhongId == dto.PhongId && !g.IsDeleted)
            .ToListAsync();

        var lichChieuGhes = danhSachGhe.Select(g => new LichChieuGhe
        {
            LichChieuId  = lichChieu.LichChieuId,
            GheId        = g.GheId,
            TrangThaiGhe = "Trong"
        }).ToList();

        _db.LichChieuGhes.AddRange(lichChieuGhes);
        await _db.SaveChangesAsync();

        return ServiceResult.Success("Tạo lịch chiếu thành công", lichChieu.LichChieuId);
    }

    public async Task<ServiceResult> UpdateTrangThaiAsync(int lichChieuId, string trangThai)
    {
        var lc = await _db.LichChieus.FindAsync(lichChieuId);
        if (lc == null) return ServiceResult.Fail("Không tìm thấy lịch chiếu");

        lc.TrangThai = trangThai;
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Cập nhật trạng thái thành công");
    }

    private static LichChieuDto MapToDto(LichChieu lc) => new()
    {
        LichChieuId = lc.LichChieuId,
        PhimId      = lc.PhimId,
        TenPhim     = lc.Phim?.TenPhim ?? string.Empty,
        PosterUrl   = lc.Phim?.PosterUrl,
        PhongId     = lc.PhongId,
        TenPhong    = lc.Phong?.TenPhong ?? string.Empty,
        LoaiPhong   = lc.Phong?.LoaiPhong ?? "2D",
        GioBatDau   = lc.GioBatDau,
        GioKetThuc  = lc.GioKetThuc,
        GiaVeCoBan  = lc.GiaVeCoBan,
        TrangThai   = lc.TrangThai
    };
}
