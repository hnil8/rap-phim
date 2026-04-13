using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IProductService
{
    Task<List<NhomFnBDto>> GetAllNhomWithSanPhamAsync();
    Task<List<ComboDto>> GetAllCombosAsync();
}
