using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public class ComboService : IComboService
    {
        private readonly CinemaDbContext _db;

        public ComboService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<List<ComboDto>> GetAllAsync(string keyword = "")
        {
            var query = _db.Combos
                .Where(c => !c.IsDeleted)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(c => c.TenCombo.Contains(keyword));
            }

            return await query
                .OrderByDescending(c => c.ComboId)
                .Select(c => new ComboDto
                {
                    ComboId = c.ComboId,
                    TenCombo = c.TenCombo,
                    GiaCombo = c.GiaCombo,
                    MoTa = c.MoTa ?? "",
                    HinhAnhUrl = c.HinhAnhUrl ?? "",
                    IsActive = c.IsActive
                })
                .ToListAsync();
        }

        public async Task<ComboDto> GetByIdAsync(int id)
        {
            // Include để lấy luôn các món ăn con nằm trong bảng ChiTietCombo
            var combo = await _db.Combos
                .Include(c => c.ChiTietCombos)
                    .ThenInclude(ct => ct.SanPham)
                .FirstOrDefaultAsync(c => c.ComboId == id && !c.IsDeleted);

            if (combo == null) return null;

            return new ComboDto
            {
                ComboId = combo.ComboId,
                TenCombo = combo.TenCombo,
                GiaCombo = combo.GiaCombo,
                MoTa = combo.MoTa ?? "",
                HinhAnhUrl = combo.HinhAnhUrl ?? "",
                IsActive = combo.IsActive,
                ChiTietCombos = combo.ChiTietCombos.Select(ct => new ChiTietComboDto
                {
                    SanPhamId = ct.SanPhamId,
                    TenSanPham = ct.SanPham.TenSanPham,
                    SoLuong = ct.SoLuong
                }).ToList()
            };
        }

        public async Task<ServiceResult> CreateAsync(CreateComboDto dto)
        {
            // Bắt đầu Transaction bảo vệ dữ liệu
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                bool exists = await _db.Combos.AnyAsync(c => c.TenCombo.ToLower() == dto.TenCombo.ToLower() && !c.IsDeleted);
                if (exists) return ServiceResult.Fail("Tên gói Combo này đã tồn tại!");

                if (dto.ChiTietCombos == null || !dto.ChiTietCombos.Any())
                    return ServiceResult.Fail("Gói Combo phải có ít nhất 1 sản phẩm bên trong!");

                // 1. Lưu thông tin Master (Gói Combo)
                var combo = new Combo
                {
                    TenCombo = dto.TenCombo,
                    GiaCombo = dto.GiaCombo,
                    MoTa = dto.MoTa,
                    HinhAnhUrl = dto.HinhAnhUrl,
                    IsActive = dto.IsActive
                };

                _db.Combos.Add(combo);
                await _db.SaveChangesAsync(); // Cần Save trước để sinh ra ComboId

                // 2. Lưu thông tin Detail (Các món trong Combo)
                foreach (var item in dto.ChiTietCombos)
                {
                    _db.ChiTietCombos.Add(new ChiTietCombo
                    {
                        ComboId = combo.ComboId,
                        SanPhamId = item.SanPhamId,
                        SoLuong = item.SoLuong
                    });
                }

                await _db.SaveChangesAsync();

                // Xác nhận hoàn tất lưu vào Database thực
                await transaction.CommitAsync();

                return ServiceResult.Success("Thêm gói Combo thành công!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResult.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ServiceResult> UpdateAsync(int id, CreateComboDto dto)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                var combo = await _db.Combos
                    .Include(c => c.ChiTietCombos)
                    .FirstOrDefaultAsync(c => c.ComboId == id && !c.IsDeleted);

                if (combo == null) return ServiceResult.Fail("Không tìm thấy gói Combo!");

                bool exists = await _db.Combos.AnyAsync(c => c.TenCombo.ToLower() == dto.TenCombo.ToLower() && c.ComboId != id && !c.IsDeleted);
                if (exists) return ServiceResult.Fail("Tên gói Combo đã bị trùng!");

                if (dto.ChiTietCombos == null || !dto.ChiTietCombos.Any())
                    return ServiceResult.Fail("Gói Combo phải có ít nhất 1 sản phẩm bên trong!");

                // 1. Cập nhật thông tin Master
                combo.TenCombo = dto.TenCombo;
                combo.GiaCombo = dto.GiaCombo;
                combo.MoTa = dto.MoTa;
                combo.HinhAnhUrl = dto.HinhAnhUrl;
                combo.IsActive = dto.IsActive;

                // 2. Dọn dẹp Detail cũ: Xóa toàn bộ các món cũ khỏi Combo này
                _db.ChiTietCombos.RemoveRange(combo.ChiTietCombos);

                // 3. Thêm Detail mới từ giao diện
                foreach (var item in dto.ChiTietCombos)
                {
                    _db.ChiTietCombos.Add(new ChiTietCombo
                    {
                        ComboId = combo.ComboId,
                        SanPhamId = item.SanPhamId,
                        SoLuong = item.SoLuong
                    });
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return ServiceResult.Success("Cập nhật gói Combo thành công!");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return ServiceResult.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var combo = await _db.Combos.FindAsync(id);
            if (combo == null || combo.IsDeleted) return ServiceResult.Fail("Không tìm thấy gói Combo!");

            combo.IsDeleted = true; // Xóa mềm
            await _db.SaveChangesAsync();

            return ServiceResult.Success("Xóa gói Combo thành công!");
        }
    }
}