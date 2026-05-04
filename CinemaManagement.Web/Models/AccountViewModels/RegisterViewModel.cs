using System;
using System.ComponentModel.DataAnnotations;
using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.Web.Models.AccountViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui long nhap ho va ten.")]
    [StringLength(100, ErrorMessage = "Ho va ten toi da 100 ky tu.")]
    [Display(Name = "Ho va ten")]
    public string HoTen { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui long nhap so dien thoai.")]
    [Display(Name = "So dien thoai")]
    [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "So dien thoai phai gom 10 den 11 chu so.")]
    public string SoDienThoai { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Email khong hop le.")]
    [StringLength(100, ErrorMessage = "Email toi da 100 ky tu.")]
    public string? Email { get; set; }

    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Ngay sinh")]
    public DateOnly? NgaySinh { get; set; }

    public CreateCustomerDto ToDto() => new()
    {
        HoTen = HoTen.Trim(),
        SoDienThoai = SoDienThoai.Trim(),
        Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
        GioiTinh = string.IsNullOrWhiteSpace(GioiTinh) ? null : GioiTinh.Trim(),
        NgaySinh = NgaySinh
    };
}
