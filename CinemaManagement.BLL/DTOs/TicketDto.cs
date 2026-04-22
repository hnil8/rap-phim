namespace CinemaManagement.BLL.DTOs;

// DTO hiển thị vé / lịch sử đặt vé
public class VePhimDto
{
    public int VeId { get; set; }
    public int HoaDonId { get; set; }
    public string TenPhim { get; set; } = string.Empty;
    public string TenGhe { get; set; } = string.Empty;
    public string LoaiGhe { get; set; } = string.Empty;
    public string TenPhong { get; set; } = string.Empty;
    public DateTime GioChieu { get; set; }
    public decimal GiaBan { get; set; }
    public string TrangThai { get; set; } = string.Empty;
    public string? MaVach { get; set; }
    public DateTime? ThoiGianIn { get; set; }
    public string DoiTuongKhach { get; set; } = string.Empty;
}

// DTO đặt vé (gửi từ Web/UI sang BLL)
public class DatVeRequestDto
{
    public int LichChieuId { get; set; }
    public List<int> LichChieuGheIds { get; set; } = new();
    public int? KhachHangId { get; set; }
    public string? MaKhuyenMai { get; set; }
    public string PhuongThucThanhToan { get; set; } = "TienMat";
    public string DoiTuongKhach { get; set; } = "NguoiLon";
}

// DTO kết quả sau khi đặt vé thành công
public class DatVeResultDto
{
    public int HoaDonId { get; set; }
    public List<VePhimDto> DanhSachVe { get; set; } = new();
    public decimal TongTienVe { get; set; }
    public decimal TongTienFnB { get; set; }
    public decimal TienGiam { get; set; }
    public decimal ThanhTien { get; set; }
}
