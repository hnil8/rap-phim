using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CinemaManagement.BLL.Common;

namespace CinemaManagement.Web.Models.TicketViewModels;

public class BookingSubmitViewModel
{
    public int LichChieuId { get; set; }

    [Required(ErrorMessage = "Vui long chon it nhat 1 ghe.")]
    public string SelectedSeats { get; set; } = "[]";

    [StringLength(50)]
    public string? MaKhuyenMai { get; set; }

    [Required]
    public string DoiTuong { get; set; } = BusinessRules.CustomerTypeAdult;

    public List<int> ParseSelectedSeatIds()
    {
        if (string.IsNullOrWhiteSpace(SelectedSeats))
        {
            return new List<int>();
        }

        try
        {
            return JsonSerializer.Deserialize<List<int>>(SelectedSeats) ?? new List<int>();
        }
        catch (JsonException)
        {
            return new List<int>();
        }
    }
}
