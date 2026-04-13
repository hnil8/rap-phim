using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class PhongChieu
{
    public int PhongId { get; set; }

    public string TenPhong { get; set; } = null!;

    public int SucChua { get; set; }

    public string? LoaiPhong { get; set; }

    public string TrangThai { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<GheNgoi> GheNgois { get; set; } = new List<GheNgoi>();

    public virtual ICollection<LichChieu> LichChieus { get; set; } = new List<LichChieu>();
}
