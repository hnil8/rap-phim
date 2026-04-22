using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.MovieViewModels;

public class HomeIndexViewModel
{
    public List<PhimDto> DangChieu { get; set; } = new();
    public List<PhimDto> SapChieu { get; set; } = new();
}
