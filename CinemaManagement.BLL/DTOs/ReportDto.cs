namespace CinemaManagement.BLL.DTOs;

// DTO tổng hợp doanh thu
public class DoanhThuDto
{
    public decimal TongDoanhThu { get; set; }
    public int TongVeBan { get; set; }
    public int TongHoaDon { get; set; }
    public decimal DoanhThuVe { get; set; }
    public decimal DoanhThuFnB { get; set; }
    public List<DoanhThuNgayDto> ChiTietNgay { get; set; } = new();
}

public class DoanhThuNgayDto
{
    public DateOnly Ngay { get; set; }
    public decimal DoanhThu { get; set; }
    public int SoVe { get; set; }
}

// DTO top phim doanh thu
public class TopPhimDto
{
    public int PhimId { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public int TongVe { get; set; }
    public decimal TongDoanhThu { get; set; }
}
