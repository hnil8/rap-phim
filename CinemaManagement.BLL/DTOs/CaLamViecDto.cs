using System;

namespace CinemaManagement.BLL.DTOs
{
    public class CaLamViecDto
    {
        public int CaId { get; set; }

        public int NhanVienId { get; set; }
        // Thuộc tính mở rộng để hiển thị Tên thay vì ID
        public string TenNhanVien { get; set; } = string.Empty;

        public DateTime ThoiGianMoCa { get; set; }
        public DateTime? ThoiGianChotCa { get; set; }

        public decimal TienDauCa { get; set; }
        public decimal TongThuTienMat { get; set; }
        public decimal TongThuChuyenKhoan { get; set; }
        public decimal TongThuThe { get; set; }

        public string? GhiChuChotCa { get; set; }
        public string TrangThai { get; set; } = string.Empty;

        // Tự động dịch trạng thái sang Tiếng Việt có dấu để hiển thị lên DataGridView cho đẹp
        public string TrangThaiText => TrangThai == "DangMo" ? "Đang mở ca" : "Đã chốt ca";

        // Cột tự tính tổng doanh thu trong ca
        public decimal TongDoanhThu => TongThuTienMat + TongThuChuyenKhoan + TongThuThe;
    }
}