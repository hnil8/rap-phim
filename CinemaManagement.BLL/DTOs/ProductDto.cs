namespace CinemaManagement.BLL.DTOs;

// DTO sản phẩm FnB
public class SanPhamDto
{
    public int SanPhamId { get; set; }
    public string TenSanPham { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public decimal GiaBan { get; set; }
    public int TonKho { get; set; }
    public string? HinhAnhUrl { get; set; }
    public string TenNhom { get; set; } = string.Empty;
    public int NhomId { get; set; }
}

// DTO combo FnB
public class ComboDto
{
    public int ComboId { get; set; }
    public string TenCombo { get; set; } = string.Empty;
    public string? MoTa { get; set; }
    public decimal GiaCombo { get; set; }
    public string? HinhAnhUrl { get; set; }
    public List<ComboItemDto> Items { get; set; } = new();
}

public class ComboItemDto
{
    public string TenSanPham { get; set; } = string.Empty;
    public int SoLuong { get; set; }
}

// DTO nhóm FnB
public class NhomFnBDto
{
    public int NhomId { get; set; }
    public string TenNhom { get; set; } = string.Empty;
    public string? BieuTuong { get; set; }
    public List<SanPhamDto> SanPhams { get; set; } = new();
}
