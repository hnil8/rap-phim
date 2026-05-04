using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public class NhomFnBService : INhomFnBService
    {
        private readonly CinemaDbContext _db;

        // Tiêm CinemaDbContext thông qua Constructor (Dependency Injection)
        public NhomFnBService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<List<NhomFnBDto>> GetAllAsync()
        {
            // Chỉ lấy các nhóm chưa bị xóa và dùng AsNoTracking để tăng tốc độ đọc dữ liệu
            return await _db.NhomFnBs
                .AsNoTracking()
                .Where(n => !n.IsDeleted)
                .Select(n => new NhomFnBDto
                {
                    NhomId = n.NhomId,
                    TenNhom = n.TenNhom,
                    BieuTuong = n.BieuTuong // Đã cập nhật thêm thuộc tính này khớp với file DTO
                })
                .ToListAsync();
        }
    }
}