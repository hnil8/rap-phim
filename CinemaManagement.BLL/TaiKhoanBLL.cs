// FILE: TaiKhoanBLL.cs
// Namespace: CinemaManagement.BLL

using System;
using CinemaManagement.DAL.db;          // TaiKhoanDAL
using CinemaManagement.DAL.Entities;    // TaiKhoan entity

namespace CinemaManagement.BLL
{
    public class TaiKhoanBLL
    {
        private readonly TaiKhoanDAL _dal;

        public TaiKhoanBLL(TaiKhoanDAL dal)
        {
            _dal = dal;
        }

        /// <summary>
        /// Validate → xác thực DB → cập nhật lần đăng nhập.
        /// Trả về (TaiKhoan entity, thongBaoLoi).
        /// taiKhoan != null  → thành công
        /// taiKhoan == null  → thất bại, đọc thongBaoLoi
        /// </summary>
        public (TaiKhoan taiKhoan, string thongBaoLoi) DangNhap(
            string tenDangNhap, string matKhau)
        {
            // Validate đầu vào
            if (string.IsNullOrWhiteSpace(tenDangNhap))
                return (null, "Vui lòng nhập tên đăng nhập!");

            if (string.IsNullOrWhiteSpace(matKhau))
                return (null, "Vui lòng nhập mật khẩu!");

            if (matKhau.Length < 6)
                return (null, "Mật khẩu phải có ít nhất 6 ký tự!");

            try
            {
                var tk = _dal.DangNhap(tenDangNhap.Trim(), matKhau);

                if (tk != null)
                {
                    _dal.CapNhatLanDangNhapCuoi(tk.TaiKhoanId);
                    return (tk, null);
                }

                // Phân biệt sai mật khẩu vs tài khoản bị khóa
                if (_dal.KiemTraTaiKhoanBiKhoa(tenDangNhap.Trim()))
                    return (null, "Tài khoản đã bị khóa!\nVui lòng liên hệ quản trị viên.");

                return (null, "Tên đăng nhập hoặc mật khẩu không đúng!");
            }
            catch (Exception ex)
            {
                return (null, $"Không thể kết nối hệ thống!\n{ex.Message}");
            }
        }
    }
}