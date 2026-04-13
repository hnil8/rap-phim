using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class VePhim
{
    public int VeId { get; set; }

    public int HoaDonId { get; set; }

    public int LichChieuGheId { get; set; }

    public int? QuyTacId { get; set; }

    public decimal GiaGoc { get; set; }

    public decimal GiaBan { get; set; }

    public string DoiTuongKhach { get; set; } = null!;

    public string TrangThai { get; set; } = null!;

    public DateTime? ThoiGianIn { get; set; }

    public string? MaVach { get; set; }

    public virtual HoaDon HoaDon { get; set; } = null!;

    public virtual LichChieuGhe LichChieuGhe { get; set; } = null!;

    public virtual ICollection<LichChieuGhe> LichChieuGhes { get; set; } = new List<LichChieuGhe>();

    public virtual QuyTacGium? QuyTac { get; set; }
}
