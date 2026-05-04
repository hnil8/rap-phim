using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

/// <summary>PosService: Bán vé tại quầy — ủy quyền sang TicketService.</summary>
public class PosService : IPosService
{
    private readonly ITicketService _ticketService;

    public PosService(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }

    public Task<ServiceResult<DatVeResultDto>> BanVeAsync(DatVeRequestDto request)
        => _ticketService.DatVeAsync(request);

    public Task<ServiceResult> XacNhanVeAsync(string maVach)
        => _ticketService.ValidateQrAsync(maVach);
}
