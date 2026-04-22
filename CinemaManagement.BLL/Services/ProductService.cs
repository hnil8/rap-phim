using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;


public class ProductService : IProductService
{
    private readonly CinemaDbContext _db;
    public ProductService(CinemaDbContext db) => _db = db;

    public async Task<List<NhomFnBDto>> GetAllNhomWithSanPhamAsync()
    {
        var nhoms = await _db.NhomFnBs
            .AsNoTracking()
            .Include(n => n.SanPhamFnBs)
            .Where(n => !n.IsDeleted)
            .OrderBy(n => n.TenNhom)
            .ToListAsync();

        return nhoms.Select(n => new NhomFnBDto
        {
            NhomId   = n.NhomId,
            TenNhom  = n.TenNhom,
            BieuTuong = n.BieuTuong,
            SanPhams = n.SanPhamFnBs
                .Where(sp => !sp.IsDeleted && sp.TonKho > 0)
                .Select(sp => new SanPhamDto
                {
                    SanPhamId  = sp.SanPhamId,
                    TenSanPham = sp.TenSanPham,
                    MoTa       = sp.MoTa,
                    GiaBan     = sp.GiaBan,
                    TonKho     = sp.TonKho,
                    HinhAnhUrl = sp.HinhAnhUrl,
                    TenNhom    = n.TenNhom,
                    NhomId     = n.NhomId
                }).ToList()
        }).ToList();
    }

    public async Task<List<ComboDto>> GetAllCombosAsync()
    {
        var combos = await _db.Combos
            .AsNoTracking()
            .Include(c => c.ChiTietCombos).ThenInclude(ct => ct.SanPham)
            .Where(c => c.IsActive && !c.IsDeleted)
            .OrderBy(c => c.TenCombo)
            .ToListAsync();

        return combos.Select(c => new ComboDto
        {
            ComboId   = c.ComboId,
            TenCombo  = c.TenCombo,
            MoTa      = c.MoTa,
            GiaCombo  = c.GiaCombo,
            HinhAnhUrl = c.HinhAnhUrl,
            Items = c.ChiTietCombos.Select(ct => new ComboItemDto
            {
                TenSanPham = ct.SanPham.TenSanPham,
                SoLuong    = ct.SoLuong
            }).ToList()
        }).ToList();
    }
}
