using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IPromotionService
{
    Task<List<KhuyenMaiDto>> GetAllAsync();
    Task<KhuyenMaiDto?> GetByMaCodeAsync(string maCode);
    Task<ServiceResult> CreateAsync(CreateKhuyenMaiDto dto);
    Task<ServiceResult> UpdateAsync(int khuyenMaiId, CreateKhuyenMaiDto dto);
    Task<ServiceResult> DeleteAsync(int khuyenMaiId);
}
