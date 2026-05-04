using CinemaManagement.BLL.Services;
using CinemaManagement.Web.Models.MovieViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaManagement.Web.Controllers;

public class MovieController : Controller
{
    private readonly IMovieService _movieService;
    private readonly IShowtimeService _showtimeService;

    public MovieController(IMovieService movieService, IShowtimeService showtimeService)
    {
        _movieService = movieService;
        _showtimeService = showtimeService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(string? q, string? trangThai)
    {
        var model = new MovieCatalogViewModel
        {
            SearchTerm = q,
            TrangThai = trangThai,
            Movies = await _movieService.GetAllAsync(q, trangThai)
        };

        return View(model);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(int id, string? ngay)
    {
        var phim = await _movieService.GetByIdAsync(id);
        if (phim == null) return NotFound();

        var tatCaLichChieu = await _showtimeService.GetByPhimIdAsync(id);

        // Lấy 7 ngày tới có lịch chiếu
        var ngayCoLich = tatCaLichChieu
            .Select(lc => lc.GioBatDau.Date)
            .Distinct()
            .OrderBy(date => date)
            .ToList();

        var selectedDate = ngay != null && DateTime.TryParse(ngay, out var parsedDate)
            ? parsedDate.Date
            : ngayCoLich.FirstOrDefault();

        if (selectedDate == default)
        {
            selectedDate = DateTime.Today;
            ngayCoLich = Enumerable.Range(0, 7)
                .Select(offset => DateTime.Today.AddDays(offset))
                .ToList();
        }

        var model = new MovieDetailsViewModel
        {
            Movie = phim,
            Showtimes = tatCaLichChieu
                .Where(lc => lc.GioBatDau.Date == selectedDate.Date)
                .ToList(),
            AvailableDates = ngayCoLich,
            SelectedDate = selectedDate
        };

        return View(model);
    }
}
