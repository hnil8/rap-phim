using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class LichChieu
{
    public int LichChieuId { get; set; }

    public int PhimId { get; set; }

    public int PhongId { get; set; }

    public DateTime GioBatDau { get; set; }

    public DateTime GioKetThuc { get; set; }

    public decimal GiaVeCoBan { get; set; }

    public string TrangThai { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<LichChieuGhe> LichChieuGhes { get; set; } = new List<LichChieuGhe>();

    public virtual Phim Phim { get; set; } = null!;

    public virtual PhongChieu Phong { get; set; } = null!;
}
