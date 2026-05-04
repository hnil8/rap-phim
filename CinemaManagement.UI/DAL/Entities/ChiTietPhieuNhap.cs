using System;
using System.Collections.Generic;

namespace CinemaManagement.UI.DAL.Entities;

public partial class ChiTietPhieuNhap
{
    public int PhieuNhapId { get; set; }

    public int SanPhamId { get; set; }

    public int SoLuong { get; set; }

    public decimal GiaNhap { get; set; }

    public decimal ThanhTien { get; set; }

    public virtual PhieuNhap PhieuNhap { get; set; } = null!;

    public virtual SanPhamFnB SanPham { get; set; } = null!;
}
