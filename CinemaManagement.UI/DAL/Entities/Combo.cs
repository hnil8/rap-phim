using System;
using System.Collections.Generic;

namespace CinemaManagement.UI.DAL.Entities;

public partial class Combo
{
    public int ComboId { get; set; }

    public string TenCombo { get; set; } = null!;

    public string? MoTa { get; set; }

    public decimal GiaCombo { get; set; }

    public string? HinhAnhUrl { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

    public virtual ICollection<HoaDonFnB> HoaDonFnBs { get; set; } = new List<HoaDonFnB>();
}
