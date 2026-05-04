using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities; // Thay namespace này bằng namespace chứa các class Entity của bạn
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using CinemaManagement.DAL.Entities;

namespace CinemaManagement.BLL.Services
{
    public class PhieuNhapService : IPhieuNhapService
    {
        private readonly CinemaDbContext _db;

        public PhieuNhapService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResult> CreatePhieuNhapAsync(CreatePhieuNhapDto dto)
        {
            // Bắt đầu Transaction bảo vệ tính toàn vẹn dữ liệu (Chống thất thoát)
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                if (dto.ChiTietPhieu == null || !dto.ChiTietPhieu.Any())
                    return ServiceResult.Fail("Phiếu nhập không có sản phẩm nào!");

                // 1. TẠO PHIẾU NHẬP (MASTER)
                var phieuNhap = new PhieuNhap
                {
                    NguoiNhap = dto.NguoiNhap,
                    NgayNhap = dto.NgayNhap,
                    GhiChu = dto.GhiChu,
                    TongTien = dto.ChiTietPhieu.Sum(x => x.ThanhTien)
                };

                _db.PhieuNhaps.Add(phieuNhap);
                await _db.SaveChangesAsync(); // Cần Save ở đây để Database cấp cho cái PhieuNhapId

                // 2. LƯU CHI TIẾT & CẬP NHẬT TỒN KHO (DETAIL)
                foreach (var item in dto.ChiTietPhieu)
                {
                    // 2.1 Thêm món hàng vào chi tiết phiếu
                    _db.ChiTietPhieuNhaps.Add(new ChiTietPhieuNhap
                    {
                        PhieuNhapId = phieuNhap.PhieuNhapId,
                        SanPhamId = item.SanPhamId,
                        SoLuong = item.SoLuong,
                        GiaNhap = item.GiaNhap,
                        ThanhTien = item.ThanhTien
                    });

                    // 2.2 CỘNG DỒN VÀO TỒN KHO THỰC TẾ
                    // (Đây là bước quyết định sự chính xác của kho hàng)
                    var sanPham = await _db.SanPhamFnBs.FindAsync(item.SanPhamId);
                    if (sanPham != null)
                    {
                        sanPham.TonKho += item.SoLuong;
                        // Lưu ý: Tùy nghiệp vụ, bạn có thể viết thêm code tính lại Giá Vốn trung bình ở đây nếu cần
                    }
                }

                // Lưu toàn bộ Chi tiết và Tồn kho vào Database
                await _db.SaveChangesAsync();

                // 3. MỌI THỨ HOÀN HẢO -> CHỐT TRANSACTION
                await transaction.CommitAsync();

                return ServiceResult.Success("Chốt phiếu nhập và cập nhật tồn kho thành công!");
            }
            catch (Exception ex)
            {
                // NẾU CÓ LỖI (Đứt cáp, rớt mạng, lỗi logic) -> HOÀN TÁC TOÀN BỘ!
                await transaction.RollbackAsync();
                return ServiceResult.Fail("Lỗi hệ thống khi lưu phiếu: " + ex.Message);
            }
        }
    }
}