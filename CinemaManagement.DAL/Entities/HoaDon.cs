using System;
using System.Collections.Generic;

namespace CinemaManagement.DAL.Entities;

public partial class HoaDon
{
    public int HoaDonId { get; set; }

    public int CaId { get; set; }

    public int? KhachHangId { get; set; }

    public int? KhuyenMaiId { get; set; }

    public decimal TongTienVe { get; set; }

    public decimal TongTienFnB { get; set; }

    public decimal TongTienGoc { get; set; }

    public decimal TienGiamKm { get; set; }

    public decimal TienGiamDiem { get; set; }

    public decimal TienGiamThanhVien { get; set; }

    public decimal? TongTienGiam { get; set; }

    public decimal ThanhTien { get; set; }

    public string PhuongThucTt { get; set; } = null!;

    public decimal? TienKhachDua { get; set; }

    public decimal? TienThoiLai { get; set; }

    public int DiemTichDuoc { get; set; }

    public int DiemSuDung { get; set; }

    public string TrangThai { get; set; } = null!;

    public string? LyDoHuy { get; set; }

    public DateTime ThoiGianTao { get; set; }

    public string? GhiChu { get; set; }

    public virtual CaLamViec Ca { get; set; } = null!;

    public virtual ICollection<HoaDonFnB> HoaDonFnBs { get; set; } = new List<HoaDonFnB>();

    public virtual KhachHang? KhachHang { get; set; }

    public virtual KhuyenMai? KhuyenMai { get; set; }

    public virtual ICollection<VePhim> VePhims { get; set; } = new List<VePhim>();
}
