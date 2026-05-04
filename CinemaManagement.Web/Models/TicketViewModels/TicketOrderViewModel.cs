using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.TicketViewModels;

public class TicketOrderViewModel
{
    public int HoaDonId { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public DateTime? ThoiGianDat { get; set; }
    public List<VePhimDto> Tickets { get; set; } = new();

    public bool CanCancel => TrangThai == "DaBan";

    public string StatusClass => TrangThai switch
    {
        "DaBan" => "status-daban",
        "DaHuy" => "status-dahuy",
        "DaKiemSoat" => "status-dakiemsoat",
        _ => "status-daban"
    };

    public string StatusText => TrangThai switch
    {
        "DaBan" => "Da dat",
        "DaHuy" => "Da huy",
        "DaKiemSoat" => "Da dung",
        _ => TrangThai
    };
}
