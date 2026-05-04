// ============================================================
// FILE: NguoiDungDTO.cs
// Tầng: DAL (Data Transfer Object)
// Mục đích: Đối tượng truyền dữ liệu người dùng giữa các tầng
//           DAL trả về → BLL xử lý → UI hiển thị
// Namespace: CinemaManagement.DAL
// ============================================================

namespace CinemaManagement.DAL.db
{
    public class NguoiDungDTO
    {
        public int TaiKhoanId { get; set; }
        public string TenDangNhap { get; set; }
        public string HoTen { get; set; }   // Lấy từ bảng NhanVien
        public string TenVaiTro { get; set; }   // Lấy từ bảng VaiTro: 'Admin', 'QuanLy', 'NhanVien'
        public string Email { get; set; }
        public string SoDienThoai { get; set; }
    }
}