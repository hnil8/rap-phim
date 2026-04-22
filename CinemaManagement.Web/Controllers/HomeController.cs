using CinemaManagement.BLL.Services;
using CinemaManagement.Web.Models.MovieViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Web.Controllers;

public class HomeController : Controller
{
    private readonly IMovieService _movieService;

    public HomeController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeIndexViewModel
        {
            DangChieu = await _movieService.GetDangChieuAsync(),
            SapChieu = await _movieService.GetSapChieuAsync()
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
