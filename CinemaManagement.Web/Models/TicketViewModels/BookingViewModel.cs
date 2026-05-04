using CinemaManagement.BLL.Common;
using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.TicketViewModels;

public class BookingViewModel
{
    public LichChieuDto Showtime { get; set; } = new();
    public Dictionary<string, List<SeatDto>> SeatsByRow { get; set; } = new();
    public int MaxCol { get; set; }
    public BookingSubmitViewModel Form { get; set; } = new()
    {
        DoiTuong = BusinessRules.CustomerTypeAdult
    };
}
