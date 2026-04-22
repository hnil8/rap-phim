using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class GheNgoi
{
    public int GheId { get; set; }

    public int PhongId { get; set; }

    public int LoaiGheId { get; set; }

    public string DayGhe { get; set; } = null!;

    public int CotGhe { get; set; }

    public string? TenGhe { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<LichChieuGhe> LichChieuGhes { get; set; } = new List<LichChieuGhe>();

    public virtual LoaiGhe LoaiGhe { get; set; } = null!;

    public virtual PhongChieu Phong { get; set; } = null!;
}
