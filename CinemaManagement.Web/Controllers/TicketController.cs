using CinemaManagement.BLL.Common;
using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using CinemaManagement.Web.Models.TicketViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaManagement.Web.Controllers;

public class TicketController : Controller
{
    private readonly IShowtimeService _showtimeService;
    private readonly ITicketService _ticketService;

    public TicketController(IShowtimeService showtimeService, ITicketService ticketService)
    {
        _showtimeService = showtimeService;
        _ticketService = ticketService;
    }

    // GET /Ticket/Booking/5 — trang chọn ghế
    [AllowAnonymous]
    public async Task<IActionResult> Booking(int id)
    {
        var lichChieu = await _showtimeService.GetByIdAsync(id);
        if (lichChieu == null) return NotFound();

        var seatMap = await _showtimeService.GetSeatMapAsync(id);

        var model = new BookingViewModel
        {
            Showtime = lichChieu,

        // Nhóm ghế theo dãy để render CSS Grid
            SeatsByRow = seatMap
                .GroupBy(seat => seat.DayGhe)
                .OrderBy(group => group.Key)
                .ToDictionary(group => group.Key, group => group.OrderBy(seat => seat.CotGhe).ToList()),
            MaxCol = seatMap.Any() ? seatMap.Max(seat => seat.CotGhe) : 10,
            Form = new BookingSubmitViewModel
            {
                LichChieuId = lichChieu.LichChieuId,
                DoiTuong = BusinessRules.CustomerTypeAdult
            }
        };

        return View(model);
    }

    // POST /Ticket/Booking — xác nhận đặt vé
    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Booking(BookingSubmitViewModel model)
    {
        var seatIds = model.ParseSelectedSeatIds();
        if (!seatIds.Any())
        {
            TempData["Error"] = "Vui lòng chọn ít nhất 1 ghế.";
            return RedirectToAction(nameof(Booking), new { id = model.LichChieuId });
        }

        var khachHangId = User.FindFirstValue("KhachHangId") is string idStr
            ? int.Parse(idStr) : (int?)null;

        var request = new DatVeRequestDto
        {
            LichChieuId         = model.LichChieuId,
            LichChieuGheIds     = seatIds,
            KhachHangId         = khachHangId,
            MaKhuyenMai         = model.MaKhuyenMai,
            PhuongThucThanhToan = BusinessRules.PaymentCash,
            DoiTuongKhach       = model.DoiTuong
        };

        var result = await _ticketService.DatVeAsync(request);

        if (!result.IsSuccess)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Booking), new { id = model.LichChieuId });
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Confirmation), new { hoaDonId = result.Data!.HoaDonId });
    }

    // GET /Ticket/Confirmation/123 — trang xác nhận vé thành công
    [AllowAnonymous]
    public IActionResult Confirmation(int hoaDonId)
    {
        // Hiển thị kết quả từ TempData để tránh refresh duplicate booking
        // Trong thực tế production nên load lại từ DB bằng HoaDonId
        return View(new BookingConfirmationViewModel
        {
            HoaDonId = hoaDonId,
            IsAuthenticated = User.Identity?.IsAuthenticated == true
        });
    }

    // GET /Ticket/History — lịch sử vé của tôi
    [Authorize]
    public async Task<IActionResult> History()
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue) return RedirectToAction("Login", "Account");

        var vePhims = await _ticketService.GetLichSuVeAsync(customerId.Value);
        return View(TicketHistoryViewModel.FromTickets(vePhims));
    }

    // POST /Ticket/Cancel — hủy vé
    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int hoaDonId)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue) return RedirectToAction("Login", "Account");

        var result = await _ticketService.HuyVeAsync(hoaDonId, customerId.Value);
        TempData[result.IsSuccess ? "Success" : "Error"] = result.Message;
        return RedirectToAction(nameof(History));
    }

    private int? GetCurrentCustomerId()
    {
        return int.TryParse(User.FindFirstValue("KhachHangId"), out var customerId)
            ? customerId
            : null;
    }
}
