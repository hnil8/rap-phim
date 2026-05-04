using CinemaManagement.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public interface IComboService
    {
        // Lấy danh sách Combo để hiển thị
        Task<List<ComboDto>> GetAllAsync(string keyword = "");

        // Lấy thông tin 1 Combo cụ thể (bao gồm cả danh sách các món bên trong)
        Task<ComboDto> GetByIdAsync(int id);

        Task<ServiceResult> CreateAsync(CreateComboDto dto);
        Task<ServiceResult> UpdateAsync(int id, CreateComboDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}