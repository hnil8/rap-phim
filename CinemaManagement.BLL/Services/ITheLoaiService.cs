using CinemaManagement.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    // Bắt buộc phải có từ khóa "public" ở đây để hệ thống DI trong Program.cs có thể đăng ký được
    public interface ITheLoaiService
    {
        // Lấy danh sách thể loại
        Task<List<TheLoaiDto>> GetAllAsync();

        // Thêm thể loại mới
        Task<ServiceResult> CreateAsync(CreateTheLoaiDto dto);

        // Cập nhật thể loại
        Task<ServiceResult> UpdateAsync(int id, CreateTheLoaiDto dto);

        // Xóa thể loại
        Task<ServiceResult> DeleteAsync(int id);
    }
}