// FILE: TaiKhoanDAL.cs
// Namespace: CinemaManagement.DAL.db  (thống nhất với DatabaseConfig)
// Dùng EF Core qua CinemaDbContext — KHÔNG dùng SqlConnection/ADO.NET

using System;
using System.Linq;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;

namespace CinemaManagement.DAL.db
{
    public class TaiKhoanDAL
    {
        private readonly CinemaDbContext _ctx;

        // CinemaDbContext được inject từ ngoài (Program.cs → frmLogin)
        public TaiKhoanDAL(CinemaDbContext ctx)
        {
            _ctx = ctx;
        }

        /// <summary>
        /// Xác thực tài khoản. Trả về TaiKhoan entity kèm NhanVien + VaiTro,
        /// hoặc null nếu sai thông tin / bị khóa.
        /// </summary>
        public TaiKhoan DangNhap(string tenDangNhap, string matKhau)
        {
            try
            {
                return _ctx.TaiKhoans
                    .Where(tk =>
                        tk.TenDangNhap == tenDangNhap &&
                        tk.MatKhauHash == matKhau &&
                        tk.IsActive == true &&
                        tk.IsDeleted == false)
                    .Select(tk => new TaiKhoan
                    {
                        TaiKhoanId = tk.TaiKhoanId,
                        TenDangNhap = tk.TenDangNhap,
                        IsActive = tk.IsActive,
                        NhanVien = new NhanVien
                        {
                            HoTen = tk.NhanVien.HoTen,
                            Email = tk.NhanVien.Email,
                            SoDienThoai = tk.NhanVien.SoDienThoai
                        },
                        VaiTro = new VaiTro
                        {
                            TenVaiTro = tk.VaiTro.TenVaiTro
                        }
                    })
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi truy vấn database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật LanDangNhapCuoi sau khi đăng nhập thành công.
        /// </summary>
        public void CapNhatLanDangNhapCuoi(int taiKhoanId)
        {
            try
            {
                var tk = _ctx.TaiKhoans.Find(taiKhoanId);
                if (tk != null)
                {
                    tk.LanDangNhapCuoi = DateTime.Now;
                    _ctx.SaveChanges();
                }
            }
            catch { /* thao tác phụ — bỏ qua lỗi */ }
        }

        /// <summary>
        /// Kiểm tra tài khoản bị khóa (IsActive = false) để hiện thông báo rõ hơn.
        /// </summary>
        public bool KiemTraTaiKhoanBiKhoa(string tenDangNhap)
        {
            try
            {
                return _ctx.TaiKhoans.Any(tk =>
                    tk.TenDangNhap == tenDangNhap &&
                    tk.IsActive == false &&
                    tk.IsDeleted == false);
            }
            catch { return false; }
        }
    }
}   