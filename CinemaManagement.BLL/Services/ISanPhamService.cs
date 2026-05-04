using CinemaManagement.BLL.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public interface ISanPhamService
    {
        Task<List<SanPhamFnBDto>> GetAllAsync(string keyword = "");
        Task<ServiceResult> CreateAsync(CreateSanPhamDto dto);
        Task<ServiceResult> UpdateAsync(int id, CreateSanPhamDto dto);
        Task<ServiceResult> DeleteAsync(int id);
    }
}