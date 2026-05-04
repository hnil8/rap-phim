using System.Collections.Generic;

namespace CinemaManagement.BLL.DTOs
{
    // DTO để hiển thị danh sách Combo
    public class ComboDto
    {
        public int ComboId { get; set; }
        public string TenCombo { get; set; } = string.Empty;
        public decimal GiaCombo { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public string HinhAnhUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Danh sách các món ăn bên trong Combo này
        public List<ChiTietComboDto> ChiTietCombos { get; set; } = new List<ChiTietComboDto>();
    }

    // DTO để hiển thị từng món lẻ bên trong Combo
    public class ChiTietComboDto
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
    }

    // DTO dùng khi Thêm/Sửa Combo
    public class CreateComboDto
    {
        public string TenCombo { get; set; } = string.Empty;
        public decimal GiaCombo { get; set; }
        public string MoTa { get; set; } = string.Empty;
        public string HinhAnhUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        // Danh sách các ID sản phẩm và số lượng tương ứng
        public List<ChiTietComboDto> ChiTietCombos { get; set; } = new List<ChiTietComboDto>();
    }
}