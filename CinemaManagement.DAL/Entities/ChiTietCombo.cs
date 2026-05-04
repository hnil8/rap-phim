using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class ChiTietCombo
{
    public int ChiTietComboId { get; set; }

    public int ComboId { get; set; }

    public int SanPhamId { get; set; }

    public int SoLuong { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual SanPhamFnB SanPham { get; set; } = null!;
}
