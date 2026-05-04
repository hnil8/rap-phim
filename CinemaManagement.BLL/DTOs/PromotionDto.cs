namespace CinemaManagement.BLL.DTOs;

// DTO khuyến mãi (đọc)
public class KhuyenMaiDto
{
    public int KhuyenMaiId { get; set; }
    public string TenKhuyenMai { get; set; } = string.Empty;
    public string MaCode { get; set; } = string.Empty;
    public string LoaiGiam { get; set; } = string.Empty;
    public decimal GiaTriGiam { get; set; }
    public decimal? GiaTriGiamToiDa { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayHetHan { get; set; }
    public int? SoLuongPhatHanh { get; set; }
    public int SoLuongDaDung { get; set; }
    public bool IsActive { get; set; }
}

// DTO tạo/sửa khuyến mãi
public class CreateKhuyenMaiDto
{
    public string TenKhuyenMai { get; set; } = string.Empty;
    public string MaCode { get; set; } = string.Empty;
    public string LoaiGiam { get; set; } = "PhanTram";
    public decimal GiaTriGiam { get; set; }
    public decimal? GiaTriGiamToiDa { get; set; }
    public DateTime NgayBatDau { get; set; }
    public DateTime NgayHetHan { get; set; }
    public int? SoLuongPhatHanh { get; set; }
    public bool IsActive { get; set; } = true;
}
