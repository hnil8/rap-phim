using System;
using System.ComponentModel.DataAnnotations;

namespace CinemaManagement.Web.Models.AccountViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui long nhap so dien thoai.")]
    [Display(Name = "So dien thoai")]
    [RegularExpression(@"^[0-9]{10,11}$", ErrorMessage = "So dien thoai phai gom 10 den 11 chu so.")]
    public string SoDienThoai { get; set; } = string.Empty;

    public string ReturnUrl { get; set; } = "/";
}