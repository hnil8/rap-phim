namespace CinemaManagement.BLL.DTOs;

// DTO đọc danh sách phim (hiển thị trên trang chủ, danh sách)
public class PhimDto
{
    public int PhimId { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? TenGoc { get; set; }
    public string? DaoDien { get; set; }
    public string? DienVienChinh { get; set; }
    public int ThoiLuongPhut { get; set; }
    public string? NuocSanXuat { get; set; }
    public int? NamPhatHanh { get; set; }
    public string GioiHanDoTuoi { get; set; } = "P";
    public string? NgonNgu { get; set; }
    public string? MoTa { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public DateOnly? NgayKhoiChieu { get; set; }
    public List<string> TheLoais { get; set; } = new();

    // Computed
    public string ThoiLuongText => $"{ThoiLuongPhut / 60}g{ThoiLuongPhut % 60:00}p";
    public bool DangChieu => TrangThai == "DangChieu";
    public bool SapChieu => TrangThai == "SapChieu";
}

// DTO tạo/sửa phim (dùng nội bộ BLL → DAL)
public class CreatePhimDto
{
    public string TenPhim { get; set; } = string.Empty;
    public string? TenGoc { get; set; }
    public string? DaoDien { get; set; }
    public string? DienVienChinh { get; set; }
    public int ThoiLuongPhut { get; set; }
    public string? NuocSanXuat { get; set; }
    public int? NamPhatHanh { get; set; }
    public string GioiHanDoTuoi { get; set; } = "P";
    public string NgonNgu { get; set; } = "VietSub";
    public string? MoTa { get; set; }
    public string? PosterUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public string TrangThai { get; set; } = "SapChieu";
    public DateOnly? NgayKhoiChieu { get; set; }
    public List<int> TheLoaiIds { get; set; } = new();
}
