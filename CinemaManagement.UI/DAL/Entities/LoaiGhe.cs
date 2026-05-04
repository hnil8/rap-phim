using System;
using System.Collections.Generic;

namespace CinemaManagement.UI.DAL.Entities;

public partial class LoaiGhe
{
    public int LoaiGheId { get; set; }

    public string TenLoai { get; set; } = null!;

    public decimal HeSoGia { get; set; }

    public string? MoTa { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<GheNgoi> GheNgois { get; set; } = new List<GheNgoi>();
}
