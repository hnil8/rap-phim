using CinemaManagement.BLL.DTOs;
using CinemaManagement.BLL.Services;
using CinemaManagement.Web.Models.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CinemaManagement.Web.Controllers;

public class AccountController : Controller
{
    private readonly ICustomerService _customerService;

    public AccountController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [AllowAnonymous]
    public IActionResult Register() => View(new RegisterViewModel());

    // POST /Account/Register
    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _customerService.CreateAsync(model.ToDto());
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        // Tự động đăng nhập sau khi đăng ký
        var customer = result.Data!;
        await SignInCustomerAsync(customer);

        TempData["Success"] = "Đăng ký thành công! Chào mừng bạn đến với NEON-FLORA Cinema.";
        return RedirectToAction("Index", "Home");
    }

    [AllowAnonymous]
    public IActionResult Login(string? returnUrl)
    {
        return View(new LoginViewModel
        {
            ReturnUrl = NormalizeReturnUrl(returnUrl)
        });
    }

    // POST /Account/Login
    [AllowAnonymous]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        model.ReturnUrl = NormalizeReturnUrl(model.ReturnUrl);
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var customer = await _customerService.GetBySoDienThoaiAsync(model.SoDienThoai.Trim());
        if (customer == null)
        {
            ModelState.AddModelError(string.Empty, "Số điện thoại chưa được đăng ký");
            return View(model);
        }

        await SignInCustomerAsync(customer);
        return LocalRedirect(model.ReturnUrl);
    }

    // POST /Account/Logout
    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // GET /Account/Profile
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var customer = await GetCurrentCustomerAsync();
        if (customer == null) return RedirectToAction(nameof(Login));

        return View(ProfileViewModel.FromDto(customer));
    }

    // POST /Account/Profile
    [Authorize]
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        var customer = await GetCurrentCustomerAsync();
        if (customer == null) return RedirectToAction(nameof(Login));

        ApplyCustomerMetadata(model, customer);
        if (!ModelState.IsValid) return View(model);

        var result = await _customerService.UpdateAsync(model.ToDto());
        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        TempData["Success"] = result.Message;
        return RedirectToAction(nameof(Profile));
    }

    // ------- Helper -------
    private async Task SignInCustomerAsync(KhachHangDto customer)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name,     customer.HoTen),
            new("KhachHangId",       customer.KhachHangId.ToString()),
            new("SoDienThoai",       customer.SoDienThoai),
            new(ClaimTypes.Role,     "KhachHang")
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true, ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8) });
    }

    private async Task<KhachHangDto?> GetCurrentCustomerAsync()
    {
        return int.TryParse(User.FindFirstValue("KhachHangId"), out var customerId)
            ? await _customerService.GetByIdAsync(customerId)
            : null;
    }

    private static void ApplyCustomerMetadata(ProfileViewModel model, KhachHangDto customer)
    {
        model.KhachHangId = customer.KhachHangId;
        model.SoDienThoai = customer.SoDienThoai;
        model.DiemTichLuy = customer.DiemTichLuy;
        model.TenHangThanhVien = customer.TenHangThanhVien;
        model.MauHang = customer.MauHang;
        model.NgayDangKy = customer.NgayDangKy;
    }

    private string NormalizeReturnUrl(string? returnUrl)
    {
        return !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
            ? returnUrl
            : "/";
    }
}
