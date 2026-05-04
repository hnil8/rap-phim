using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class KhuyenMai
{
    public int KhuyenMaiId { get; set; }

    public string TenChuongTrinh { get; set; } = null!;

    public string MaCode { get; set; } = null!;

    public string LoaiGiam { get; set; } = null!;

    public decimal GiaTriGiam { get; set; }

    public decimal? GiaTriGiamToiDa { get; set; }

    public int? SoLuongPhatHanh { get; set; }

    public int SoLuongDaDung { get; set; }

    public DateTime NgayBatDau { get; set; }

    public DateTime NgayHetHan { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();
}
