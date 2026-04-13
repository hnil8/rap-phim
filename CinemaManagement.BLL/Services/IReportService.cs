using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IReportService
{
    Task<DoanhThuDto> GetDoanhThuTheoNgayAsync(DateTime tuNgay, DateTime denNgay);
    Task<List<TopPhimDto>> GetTopPhimAsync(DateTime tuNgay, DateTime denNgay, int top = 10);
}
