using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class PhieuNhap
{
    public int PhieuNhapId { get; set; }

    public string NguoiNhap { get; set; } = null!;

    public DateTime? NgayNhap { get; set; }

    public string? GhiChu { get; set; }

    public decimal TongTien { get; set; }

    public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; } = new List<ChiTietPhieuNhap>();
}
