using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface IPosService
{
    Task<ServiceResult<DatVeResultDto>> BanVeAsync(DatVeRequestDto request);
    Task<ServiceResult> XacNhanVeAsync(string maVach);
}
