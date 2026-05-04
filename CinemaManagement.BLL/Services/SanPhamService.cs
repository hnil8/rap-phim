using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities; // Thay b?ng namespace ch?a Entity c?a b?n
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public class SanPhamService : ISanPhamService
    {
        private readonly CinemaDbContext _db;

        public SanPhamService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<List<SanPhamFnBDto>> GetAllAsync(string keyword = "")
        {
            // L?y danh sách s?n ph?m ch?a b? xóa và JOIN (Include) v?i b?ng Nhóm ?? l?y tên Nhóm
            var query = _db.SanPhamFnBs
                .Include(s => s.Nhom)
                .Where(s => !s.IsDeleted)
                .AsQueryable();

            // N?u ng??i dùng có gõ tìm ki?m
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(s => s.TenSanPham.Contains(keyword));
            }

            return await query
                .OrderByDescending(s => s.SanPhamId)
                .Select(s => new SanPhamFnBDto
                {
                    SanPhamId = s.SanPhamId,
                    TenSanPham = s.TenSanPham,
                    NhomId = s.NhomId,
                    TenNhom = s.Nhom.TenNhom,
                    GiaBan = s.GiaBan,
                    TonKho = s.TonKho,
                    MoTa = s.MoTa ?? "",
                    HinhAnhUrl = s.HinhAnhUrl ?? ""
                })
                .ToListAsync();
        }

        public async Task<ServiceResult> CreateAsync(CreateSanPhamDto dto)
        {
            // Ki?m tra trùng tên
            bool exists = await _db.SanPhamFnBs.AnyAsync(s => s.TenSanPham.ToLower() == dto.TenSanPham.ToLower() && !s.IsDeleted);
            if (exists) return ServiceResult.Fail("Tên s?n ph?m này ?ã t?n t?i!");

            var sanPham = new SanPhamFnB
            {
                TenSanPham = dto.TenSanPham,
                NhomId = dto.NhomId,
                GiaBan = dto.GiaBan,
                TonKho = dto.TonKho,
                MoTa = dto.MoTa,
                HinhAnhUrl = dto.HinhAnhUrl
            };

            _db.SanPhamFnBs.Add(sanPham);
            await _db.SaveChangesAsync();
            return ServiceResult.Success("Thêm s?n ph?m thành công!");
        }

        public async Task<ServiceResult> UpdateAsync(int id, CreateSanPhamDto dto)
        {
            var sanPham = await _db.SanPhamFnBs.FindAsync(id);
            if (sanPham == null || sanPham.IsDeleted) return ServiceResult.Fail("Không tìm th?y s?n ph?m!");

            // Ki?m tra trùng tên v?i s?n ph?m KHÁC
            bool exists = await _db.SanPhamFnBs.AnyAsync(s =>
                s.TenSanPham.ToLower() == dto.TenSanPham.ToLower() &&
                s.SanPhamId != id &&
                !s.IsDeleted);
            if (exists) return ServiceResult.Fail("Tên s?n ph?m ?ã b? trùng v?i m?t s?n ph?m khác!");

            sanPham.TenSanPham = dto.TenSanPham;
            sanPham.NhomId = dto.NhomId;
            sanPham.GiaBan = dto.GiaBan;
            sanPham.TonKho = dto.TonKho;
            sanPham.MoTa = dto.MoTa;
            sanPham.HinhAnhUrl = dto.HinhAnhUrl;

            await _db.SaveChangesAsync();
            return ServiceResult.Success("C?p nh?t thành công!");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var sanPham = await _db.SanPhamFnBs.FindAsync(id);
            if (sanPham == null || sanPham.IsDeleted) return ServiceResult.Fail("Không tìm th?y s?n ph?m!");

            // Xóa m?m (Soft Delete): Không xóa h?n kh?i Database ?? gi? l?i l?ch s? Hóa ??n c?
            sanPham.IsDeleted = true;
            await _db.SaveChangesAsync();

            return ServiceResult.Success("Xóa s?n ph?m thành công!");
        }
    }
}