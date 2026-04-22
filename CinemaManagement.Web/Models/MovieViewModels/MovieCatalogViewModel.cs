using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.MovieViewModels;

public class MovieCatalogViewModel
{
    public string? SearchTerm { get; set; }
    public string? TrangThai { get; set; }
    public List<PhimDto> Movies { get; set; } = new();

    public string TitleText => TrangThai switch
    {
        "DangChieu" => "Dang Chieu",
        "SapChieu" => "Sap Chieu",
        _ => "Tat Ca Phim"
    };
}
