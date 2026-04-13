using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class CaLamViec
{
    public int CaId { get; set; }

    public int NhanVienId { get; set; }

    public DateTime ThoiGianMoCa { get; set; }

    public DateTime? ThoiGianChotCa { get; set; }

    public decimal TienDauCa { get; set; }

    public decimal TongThuTienMat { get; set; }

    public decimal TongThuChuyenKhoan { get; set; }

    public decimal TongThuThe { get; set; }

    public string? GhiChuChotCa { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual NhanVien NhanVien { get; set; } = null!;
}
