using System;
using System.Collections.Generic;

namespace CinemaManagement.BLL.DTOs
{
    public class CreatePhieuNhapDto
    {
        public string NguoiNhap { get; set; } = string.Empty;
        public DateTime NgayNhap { get; set; }
        public string? GhiChu { get; set; }
        public List<ChiTietPhieuNhapDto> ChiTietPhieu { get; set; } = new();
    }

    public class ChiTietPhieuNhapDto
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public int SoLuong { get; set; }
        public decimal GiaNhap { get; set; }

        // Thuộc tính này tự động tính toán, không cần set
        public decimal ThanhTien => SoLuong * GiaNhap;
    }
}