using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class Phim
{
    public int PhimId { get; set; }

    public string TenPhim { get; set; } = null!;

    public string? TenGoc { get; set; }

    public string? DaoDien { get; set; }

    public string? DienVienChinh { get; set; }

    public int ThoiLuongPhut { get; set; }

    public string? NuocSanXuat { get; set; }

    public int? NamPhatHanh { get; set; }

    public string GioiHanDoTuoi { get; set; } = null!;

    public string? NgonNgu { get; set; }

    public string? MoTa { get; set; }

    public string? PosterUrl { get; set; }

    public string? TrailerUrl { get; set; }

    public string TrangThai { get; set; } = null!;

    public DateOnly? NgayKhoiChieu { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<LichChieu> LichChieus { get; set; } = new List<LichChieu>();

    public virtual ICollection<TheLoaiPhim> TheLoais { get; set; } = new List<TheLoaiPhim>();
}
