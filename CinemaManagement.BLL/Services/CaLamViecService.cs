using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public class CaLamViecService : ICaLamViecService
    {
        private readonly CinemaDbContext _db;

        public CaLamViecService(CinemaDbContext db)
        {
            _db = db;
        }

        public async Task<int?> KiemTraCaDangMoAsync(int nhanVienId)
        {
            var ca = await _db.CaLamViecs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.NhanVienId == nhanVienId && c.TrangThai == "DangMo");

            return ca?.CaId;
        }

        public async Task<ServiceResult> MoCaAsync(int nhanVienId, decimal tienDauCa)
        {
            var caHienTai = await KiemTraCaDangMoAsync(nhanVienId);
            if (caHienTai != null)
            {
                return ServiceResult.Fail("Nhân viên này đang có một ca chưa chốt. Vui lòng chốt ca cũ trước khi mở ca mới!");
            }

            var caMoi = new CaLamViec
            {
                NhanVienId = nhanVienId,
                ThoiGianMoCa = DateTime.Now,
                TienDauCa = tienDauCa,
                TongThuTienMat = 0,
                TongThuChuyenKhoan = 0,
                TongThuThe = 0,
                TrangThai = "DangMo"
            };

            _db.CaLamViecs.Add(caMoi);
            await _db.SaveChangesAsync();

            return ServiceResult.Success("Mở ca thành công!", caMoi.CaId);
        }

        public async Task<ServiceResult> ChotCaAsync(int caId, decimal tienMatThucTeTrongKet, string ghiChu)
        {
            var ca = await _db.CaLamViecs
                .Include(c => c.HoaDons)
                .FirstOrDefaultAsync(c => c.CaId == caId && c.TrangThai == "DangMo");

            if (ca == null)
                return ServiceResult.Fail("Không tìm thấy ca làm việc hoặc ca này đã được chốt!");

            decimal tienMatTrenPhanMem = ca.TienDauCa + ca.TongThuTienMat;
            decimal doLech = tienMatThucTeTrongKet - tienMatTrenPhanMem;

            string thongBaoLech = "";
            if (doLech > 0)
                thongBaoLech = $" (Dư {doLech:N0} VNĐ)";
            else if (doLech < 0)
                thongBaoLech = $" (Thiếu âm {Math.Abs(doLech):N0} VNĐ)";
            else
                thongBaoLech = " (Khớp quỹ hoàn hảo)";

            ca.ThoiGianChotCa = DateTime.Now;
            ca.GhiChuChotCa = (ghiChu + "\n" + thongBaoLech).Trim();
            ca.TrangThai = "DaChot";

            await _db.SaveChangesAsync();

            return ServiceResult.Success($"Chốt ca thành công!{thongBaoLech}");
        }
    }
}