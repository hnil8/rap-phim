using System;
using System.Collections.Generic;

namespace CinemaManagement.UI.DAL.Entities;

public partial class LichChieuGhe
{
    public int LichChieuGheId { get; set; }

    public int LichChieuId { get; set; }

    public int GheId { get; set; }

    public string TrangThaiGhe { get; set; } = null!;

    public DateTime? ThoiGianGiu { get; set; }

    public int? VePhimId { get; set; }

    public virtual GheNgoi Ghe { get; set; } = null!;

    public virtual LichChieu LichChieu { get; set; } = null!;

    public virtual VePhim? VePhim { get; set; }

    public virtual VePhim? VePhimNavigation { get; set; }
}
