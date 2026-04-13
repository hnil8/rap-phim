using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface ICustomerService
{
    Task<KhachHangDto?> GetByIdAsync(int khachHangId);
    Task<KhachHangDto?> GetBySoDienThoaiAsync(string soDienThoai);
    Task<ServiceResult<KhachHangDto>> CreateAsync(CreateCustomerDto dto);
    Task<ServiceResult> UpdateAsync(UpdateCustomerDto dto);
    Task<ServiceResult> DeleteAsync(int khachHangId);
    Task<List<KhachHangDto>> SearchAsync(string keyword);
}
