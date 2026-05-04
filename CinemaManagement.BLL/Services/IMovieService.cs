using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace CinemaManagement.BLL.Services;

public interface IMovieService
{
    Task<List<PhimDto>> GetDangChieuAsync();
    Task<List<PhimDto>> GetSapChieuAsync();
    Task<List<PhimDto>> GetAllAsync(string? searchTerm = null, string? trangThai = null);
    Task<PhimDto?> GetByIdAsync(int phimId);
    Task<ServiceResult> CreateAsync(CreatePhimDto dto);
    Task<ServiceResult> UpdateAsync(int phimId, CreatePhimDto dto);
    Task<ServiceResult> SoftDeleteAsync(int phimId);
    Task<List<TheLoaiPhim>> GetAllTheLoaisAsync();
}
