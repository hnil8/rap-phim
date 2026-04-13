using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class HoaDonFnB
{
    public int HoaDonFnBid { get; set; }

    public int HoaDonId { get; set; }

    public int? SanPhamId { get; set; }

    public int? ComboId { get; set; }

    public int SoLuong { get; set; }

    public decimal DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual SanPhamFnB? SanPham { get; set; }
}
