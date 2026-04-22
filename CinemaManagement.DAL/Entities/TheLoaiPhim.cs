using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class TheLoaiPhim
{
    public int TheLoaiId { get; set; }

    public string TenTheLoai { get; set; } = null!;

    public bool IsDeleted { get; set; }

    public virtual ICollection<Phim> Phims { get; set; } = new List<Phim>();
}
