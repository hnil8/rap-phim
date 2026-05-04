using System;
using System.Collections.Generic;

namespace CinemaManagement.UI.DAL.Entities;

public partial class QuyTacGium
{
    public int QuyTacId { get; set; }

    public string TenQuyTac { get; set; } = null!;

    public string LoaiNgay { get; set; } = null!;

    public string KhungGio { get; set; } = null!;

    public TimeOnly? GioTu { get; set; }

    public TimeOnly? GioDen { get; set; }

    public string DoiTuong { get; set; } = null!;

    public decimal GiaCoBan { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<VePhim> VePhims { get; set; } = new List<VePhim>();
}
