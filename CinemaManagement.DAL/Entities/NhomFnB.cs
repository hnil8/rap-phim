using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class NhomFnB
{
    public int NhomId { get; set; }

    public string TenNhom { get; set; } = null!;

    public string? BieuTuong { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SanPhamFnB> SanPhamFnBs { get; set; } = new List<SanPhamFnB>();
}
