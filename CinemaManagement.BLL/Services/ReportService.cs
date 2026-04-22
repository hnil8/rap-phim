using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class ReportService : IReportService
{
    private readonly CinemaDbContext _db;
    public ReportService(CinemaDbContext db) => _db = db;

    public async Task<DoanhThuDto> GetDoanhThuTheoNgayAsync(DateTime tuNgay, DateTime denNgay)
    {
        var hoaDons = await _db.HoaDons
            .AsNoTracking()
            .Include(hd => hd.VePhims)
            .Where(hd => hd.TrangThai == "HoanThanh"
                      && hd.ThoiGianTao >= tuNgay
                      && hd.ThoiGianTao <= denNgay.AddDays(1).AddTicks(-1))
            .ToListAsync();

        var chiTietNgay = hoaDons
            .GroupBy(hd => DateOnly.FromDateTime(hd.ThoiGianTao))
            .Select(g => new DoanhThuNgayDto
            {
                Ngay      = g.Key,
                DoanhThu  = g.Sum(hd => hd.ThanhTien),
                SoVe      = g.Sum(hd => hd.VePhims.Count)
            })
            .OrderBy(x => x.Ngay)
            .ToList();

        return new DoanhThuDto
        {
            TongHoaDon  = hoaDons.Count,
            TongDoanhThu = hoaDons.Sum(hd => hd.ThanhTien),
            DoanhThuVe  = hoaDons.Sum(hd => hd.TongTienVe),
            DoanhThuFnB = hoaDons.Sum(hd => hd.TongTienFnB),
            TongVeBan   = hoaDons.Sum(hd => hd.VePhims.Count),
            ChiTietNgay = chiTietNgay
        };
    }

    public async Task<List<TopPhimDto>> GetTopPhimAsync(DateTime tuNgay, DateTime denNgay, int top = 10)
    {
        var data = await _db.VePhims
            .AsNoTracking()
            .Include(v => v.HoaDon)
            .Include(v => v.LichChieuGhe).ThenInclude(lcg => lcg.LichChieu).ThenInclude(lc => lc.Phim)
            .Where(v => v.TrangThai == "DaBan"
                     && v.HoaDon.ThoiGianTao >= tuNgay
                     && v.HoaDon.ThoiGianTao <= denNgay.AddDays(1).AddTicks(-1))
            .GroupBy(v => new
            {
                v.LichChieuGhe.LichChieu.PhimId,
                v.LichChieuGhe.LichChieu.Phim.TenPhim,
                v.LichChieuGhe.LichChieu.Phim.PosterUrl
            })
            .Select(g => new TopPhimDto
            {
                PhimId       = g.Key.PhimId,
                TenPhim      = g.Key.TenPhim,
                PosterUrl    = g.Key.PosterUrl,
                TongVe       = g.Count(),
                TongDoanhThu = g.Sum(v => v.GiaBan)
            })
            .OrderByDescending(x => x.TongDoanhThu)
            .Take(top)
            .ToListAsync();

        return data;
    }
}
