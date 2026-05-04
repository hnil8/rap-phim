namespace CinemaManagement.BLL.DTOs;

// DTO một chiếc ghế (dùng để render sơ đồ ghế)
public class SeatDto
{
    public int LichChieuGheId { get; set; }
    public int GheId { get; set; }
    public string TenGhe { get; set; } = string.Empty; // "A1", "B5"
    public string DayGhe { get; set; } = string.Empty;  // "A", "B"
    public int CotGhe { get; set; }
    public string LoaiGhe { get; set; } = string.Empty; // "Thuong", "VIP", "Sweetbox"
    public decimal HeSoGia { get; set; } = 1.0m;
    public string TrangThai { get; set; } = string.Empty; // "Trong", "DangGiu", "DaBan"
    public decimal GiaGoc { get; set; }      // Giá cơ bản lịch chiếu
    public decimal GiaBan { get; set; }      // Giá cuối = GiaGoc × HeSoGia

    // Computed cho CSS rendering
    public bool CoTheChon => TrangThai == "Trong";
    public string CssClass => TrangThai switch
    {
        "Trong"   => "seat-available",
        "DangGiu" => "seat-locked",
        "DaBan"   => "seat-sold",
        _         => "seat-blocked"
    };
}
