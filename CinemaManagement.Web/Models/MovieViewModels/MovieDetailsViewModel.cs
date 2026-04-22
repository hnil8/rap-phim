using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.MovieViewModels;

public class MovieDetailsViewModel
{
    public PhimDto Movie { get; set; } = new();
    public List<LichChieuDto> Showtimes { get; set; } = new();
    public List<DateTime> AvailableDates { get; set; } = new();
    public DateTime SelectedDate { get; set; }
}
