using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class NhanVien
{
    public int NhanVienId { get; set; }

    public string HoTen { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public DateOnly NgayVaoLam { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? NgayXoa { get; set; }

    public virtual ICollection<CaLamViec> CaLamViecs { get; set; } = new List<CaLamViec>();

    public virtual TaiKhoan? TaiKhoan { get; set; }
}
