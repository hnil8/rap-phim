using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IShowtimeService
{
    Task<List<LichChieuDto>> GetByPhimIdAsync(int phimId, DateTime? ngay = null);
    Task<List<LichChieuDto>> GetByNgayAsync(DateTime ngay);
    Task<LichChieuDto?> GetByIdAsync(int lichChieuId);
    Task<List<SeatDto>> GetSeatMapAsync(int lichChieuId);
    Task<ServiceResult> CreateAsync(CreateLichChieuDto dto);
    Task<ServiceResult> UpdateTrangThaiAsync(int lichChieuId, string trangThai);
}
