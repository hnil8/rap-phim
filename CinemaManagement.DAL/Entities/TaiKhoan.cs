using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class TaiKhoan
{
    public int TaiKhoanId { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhauHash { get; set; } = null!;

    public int VaiTroId { get; set; }

    public int NhanVienId { get; set; }

    public bool IsActive { get; set; }

    public DateTime NgayTao { get; set; }

    public DateTime? LanDangNhapCuoi { get; set; }

    public bool IsDeleted { get; set; }

    public virtual NhanVien NhanVien { get; set; } = null!;

    public virtual VaiTro VaiTro { get; set; } = null!;
}
