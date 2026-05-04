namespace CinemaManagement.BLL.DTOs
{
    // DTO cho Nhóm Sản Phẩm
    public class NhomFnBDto
    {
        public int NhomId { get; set; }
        public string TenNhom { get; set; } = string.Empty;
        public string? BieuTuong { get; set; }

        // Cập nhật lại dòng này để Web Project hiển thị được Menu Sản phẩm
        public List<SanPhamFnBDto> SanPhams { get; set; } = new List<SanPhamFnBDto>();
    }

    // DTO hiển thị Sản Phẩm lên DataGridView
    public class SanPhamFnBDto
    {
        public int SanPhamId { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public string TenNhom { get; set; } = string.Empty;
        public int NhomId { get; set; }
        public decimal GiaBan { get; set; }
        public int TonKho { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
    }

    // DTO dùng khi Thêm/Sửa Sản Phẩm
    public class CreateSanPhamDto
    {
        public string TenSanPham { get; set; } = string.Empty;
        public int NhomId { get; set; }
        public decimal GiaBan { get; set; }
        public int TonKho { get; set; }
        public string? MoTa { get; set; }
        public string? HinhAnhUrl { get; set; }
    }

}