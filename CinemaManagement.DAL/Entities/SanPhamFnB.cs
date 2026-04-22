using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class SanPhamFnB
{
    public int SanPhamId { get; set; }

    public int NhomId { get; set; }

    public string TenSanPham { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal GiaBan { get; set; }

    public int TonKho { get; set; }

    public string? HinhAnhUrl { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

    public virtual ICollection<HoaDonFnB> HoaDonFnBs { get; set; } = new List<HoaDonFnB>();

    public virtual NhomFnB Nhom { get; set; } = null!;
}
