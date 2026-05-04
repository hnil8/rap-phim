namespace CinemaManagement.BLL.DTOs;

// DTO hiển thị suất chiếu
public class LichChieuDto
{
    public int LichChieuId { get; set; }
    public int PhimId { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string? PosterUrl { get; set; }
    public int PhongId { get; set; }
    public string TenPhong { get; set; } = string.Empty;
    public string LoaiPhong { get; set; } = "2D";
    public DateTime GioBatDau { get; set; }
    public DateTime GioKetThuc { get; set; }
    public decimal GiaVeCoBan { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public int SoGheTrong { get; set; }
    public int SoGheTong { get; set; }

    // Computed
    public string GioBatDauText => GioBatDau.ToString("HH:mm");
    public string GioKetThucText => GioKetThuc.ToString("HH:mm");
    public string NgayChieuText => GioBatDau.ToString("dd/MM/yyyy");
    public bool ConGhe => SoGheTrong > 0;
}

// DTO tạo lịch chiếu
public class CreateLichChieuDto
{
    public int PhimId { get; set; }
    public int PhongId { get; set; }
    public DateTime GioBatDau { get; set; }
    public decimal GiaVeCoBan { get; set; }
}
