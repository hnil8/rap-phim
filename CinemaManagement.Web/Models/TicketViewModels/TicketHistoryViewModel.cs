using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.TicketViewModels;

public class TicketHistoryViewModel
{
    public List<TicketOrderViewModel> Orders { get; set; } = new();

    public static TicketHistoryViewModel FromTickets(List<VePhimDto> tickets)
    {
        return new TicketHistoryViewModel
        {
            Orders = tickets
                .GroupBy(v => v.HoaDonId)
                .OrderByDescending(g => g.Max(v => v.ThoiGianIn))
                .Select(g => new TicketOrderViewModel
                {
                    HoaDonId = g.Key,
                    TongTien = g.Sum(v => v.GiaBan),
                    TrangThai = g.First().TrangThai,
                    ThoiGianDat = g.Max(v => v.ThoiGianIn),
                    Tickets = g.OrderBy(v => v.TenGhe).ToList()
                })
                .ToList()
        };
    }
}
