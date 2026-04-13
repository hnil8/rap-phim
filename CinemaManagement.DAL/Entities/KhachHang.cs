using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class KhachHang
{
    public int KhachHangId { get; set; }

    public string HoTen { get; set; } = null!;

    public string SoDienThoai { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public int DiemTichLuy { get; set; }

    public int? HangId { get; set; }

    public DateTime NgayDangKy { get; set; }

    public bool IsDeleted { get; set; }

    public string? MatKhauHash { get; set; }

    public string? AvatarUrl { get; set; }

    public virtual HangThanhVien? Hang { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
