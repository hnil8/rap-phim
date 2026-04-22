using System;
using System.ComponentModel.DataAnnotations;
using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.AccountViewModels;

public class ProfileViewModel
{
    public int KhachHangId { get; set; }

    [Required(ErrorMessage = "Vui long nhap ho va ten.")]
    [StringLength(100, ErrorMessage = "Ho va ten toi da 100 ky tu.")]
    [Display(Name = "Ho va ten")]
    public string HoTen { get; set; } = string.Empty;

    public string SoDienThoai { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email khong hop le.")]
    [StringLength(100, ErrorMessage = "Email toi da 100 ky tu.")]
    public string? Email { get; set; }

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngay sinh")]
    public DateOnly? NgaySinh { get; set; }

    public int DiemTichLuy { get; set; }
    public string? TenHangThanhVien { get; set; }
    public string? MauHang { get; set; }
    public DateTime NgayDangKy { get; set; }

    public string AvatarText =>
        string.IsNullOrWhiteSpace(HoTen) ? "?" : HoTen.Trim()[0].ToString().ToUpperInvariant();

    public static ProfileViewModel FromDto(KhachHangDto dto) => new()
    {
        KhachHangId = dto.KhachHangId,
        HoTen = dto.HoTen,
        SoDienThoai = dto.SoDienThoai,
        Email = dto.Email,
        GioiTinh = dto.GioiTinh,
        NgaySinh = dto.NgaySinh,
        DiemTichLuy = dto.DiemTichLuy,
        TenHangThanhVien = dto.TenHangThanhVien,
        MauHang = dto.MauHang,
        NgayDangKy = dto.NgayDangKy
    };

    public UpdateCustomerDto ToDto() => new()
    {
        KhachHangId = KhachHangId,
        HoTen = HoTen.Trim(),
        Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
        GioiTinh = string.IsNullOrWhiteSpace(GioiTinh) ? null : GioiTinh.Trim(),
        NgaySinh = NgaySinh
    };
}
