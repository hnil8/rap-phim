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
    public class TheLoaiService : ITheLoaiService
    {
        private readonly CinemaDbContext _db;

        public TheLoaiService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<List<TheLoaiDto>> GetAllAsync()
        {
            return await _db.TheLoaiPhims
                .AsNoTracking()
                .Select(t => new TheLoaiDto
                {
                    TheLoaiId = t.TheLoaiId,
                    TenTheLoai = t.TenTheLoai
                    // Đã bỏ MoTa ở đây
                })
                .ToListAsync();
        }

        public async Task<ServiceResult> CreateAsync(CreateTheLoaiDto dto)
        {
            // Kiểm tra trùng tên thể loại
            bool exists = await _db.TheLoaiPhims.AnyAsync(t => t.TenTheLoai.ToLower() == dto.TenTheLoai.ToLower());
            if (exists) return ServiceResult.Fail("Tên thể loại này đã tồn tại!");

            var theLoai = new TheLoaiPhim
            {
                TenTheLoai = dto.TenTheLoai
                // Đã bỏ MoTa ở đây
            };

            _db.TheLoaiPhims.Add(theLoai);
            await _db.SaveChangesAsync();
            return ServiceResult.Success("Thêm thể loại thành công!");
        }

        public async Task<ServiceResult> UpdateAsync(int id, CreateTheLoaiDto dto)
        {
            var theLoai = await _db.TheLoaiPhims.FindAsync(id);
            if (theLoai == null) return ServiceResult.Fail("Không tìm thấy thể loại!");

            // Kiểm tra trùng tên (nhưng bỏ qua chính nó)
            bool exists = await _db.TheLoaiPhims.AnyAsync(t => t.TenTheLoai.ToLower() == dto.TenTheLoai.ToLower() && t.TheLoaiId != id);
            if (exists) return ServiceResult.Fail("Tên thể loại đã bị trùng với một thể loại khác!");

            theLoai.TenTheLoai = dto.TenTheLoai;
            // Đã bỏ MoTa ở đây

            await _db.SaveChangesAsync();
            return ServiceResult.Success("Cập nhật thành công!");
        }

        public async Task<ServiceResult> DeleteAsync(int id)
        {
            var theLoai = await _db.TheLoaiPhims.FindAsync(id);
            if (theLoai == null) return ServiceResult.Fail("Không tìm thấy thể loại!");

            try
            {
                _db.TheLoaiPhims.Remove(theLoai);
                await _db.SaveChangesAsync();
                return ServiceResult.Success("Xóa thành công!");
            }
            catch (DbUpdateException)
            {
                // Bắt lỗi khóa ngoại (Foreign Key) nếu thể loại này đang được gắn cho phim nào đó
                return ServiceResult.Fail("Không thể xóa! Thể loại này đang được sử dụng bởi các bộ phim trong hệ thống.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail("Lỗi hệ thống: " + ex.Message);
            }
        }
    }
}