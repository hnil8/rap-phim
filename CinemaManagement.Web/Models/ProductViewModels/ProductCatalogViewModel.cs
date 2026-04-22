using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.ProductViewModels;

public class ProductCatalogViewModel
{
    public List<ComboDto> Combos { get; set; } = new();
    public List<NhomFnBDto> Groups { get; set; } = new();

    public int TotalProducts => Groups.Sum(group => group.SanPhams.Count) + Combos.Count;
}
