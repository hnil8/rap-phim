using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class HangThanhVien
{
    public int HangId { get; set; }

    public string TenHang { get; set; } = null!;

    public int DiemToiThieu { get; set; }

    public decimal PhanTramUuDai { get; set; }

    public string? MoTa { get; set; }

    public string? MauSac { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<KhachHang> KhachHangs { get; set; } = new List<KhachHang>();
}
