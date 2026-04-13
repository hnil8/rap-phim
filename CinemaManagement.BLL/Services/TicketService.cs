using CinemaManagement.BLL.Common;
using CinemaManagement.BLL.DTOs;
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaManagement.BLL.Services;

public class TicketService : ITicketService
{
    private readonly CinemaDbContext _db;

    public TicketService(CinemaDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Đặt vé với cơ chế khóa nguyên tử (RULE-SEAT-01).
    /// Gộp toàn bộ: Lock ghế → Tạo HoaDon → Tạo VePhim trong 1 Transaction (RULE-DB-03).
    /// </summary>
    public async Task<ServiceResult<DatVeResultDto>> DatVeAsync(DatVeRequestDto request)
    {
        if (!request.LichChieuGheIds.Any())
            return ServiceResult<DatVeResultDto>.Fail("Vui lòng chọn ít nhất 1 ghế");

        if (request.LichChieuGheIds.Count > BusinessRules.MaxSeatsPerBooking)
            return ServiceResult<DatVeResultDto>.Fail($"Chỉ được đặt tối đa {BusinessRules.MaxSeatsPerBooking} ghế");

        // Kiểm tra mã khuyến mãi trước transaction
        KhuyenMai? khuyenMai = null;
        if (!string.IsNullOrWhiteSpace(request.MaKhuyenMai))
        {
            khuyenMai = await _db.KhuyenMais
                .FirstOrDefaultAsync(km =>
                    km.MaCode == request.MaKhuyenMai &&
                    km.IsActive &&
                    !km.IsDeleted &&
                    km.NgayBatDau <= DateTime.Now &&
                    km.NgayHetHan >= DateTime.Now &&
                    (km.SoLuongPhatHanh == null || km.SoLuongDaDung < km.SoLuongPhatHanh));

            if (khuyenMai == null)
                return ServiceResult<DatVeResultDto>.Fail("Mã khuyến mãi không hợp lệ hoặc đã hết lượt sử dụng");
        }

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            // --- BƯỚC 1: Atomic Lock ghế (RULE-SEAT-01) ---
            // Sử dụng raw SQL để đảm bảo tính nguyên tử — không kiểm tra rồi cập nhật
            var lockedCount = 0;
            foreach (var lcgId in request.LichChieuGheIds)
            {
                var rowsAffected = await _db.Database.ExecuteSqlRawAsync(
                    @"UPDATE LichChieu_Ghe 
                      SET TrangThaiGhe = 'DangGiu', ThoiGianGiu = GETDATE()
                      WHERE LichChieuGheId = {0} AND TrangThaiGhe = 'Trong'",
                    lcgId);

                if (rowsAffected == 0)
                {
                    // Ghế bị người khác chọn trước — rollback ngay (RULE-SEAT-01)
                    await transaction.RollbackAsync();

                    // Lấy tên ghế để thông báo rõ
                    var seatName = await _db.LichChieuGhes
                        .AsNoTracking()
                        .Include(lcg => lcg.Ghe)
                        .Where(lcg => lcg.LichChieuGheId == lcgId)
                        .Select(lcg => lcg.Ghe.TenGhe)
                        .FirstOrDefaultAsync();

                    return ServiceResult<DatVeResultDto>.Fail($"Ghế {seatName ?? lcgId.ToString()} vừa được người khác chọn. Vui lòng chọn ghế khác.");
                }
                lockedCount++;
            }

            // --- BƯỚC 2: Lấy thông tin ghế đã lock để tính giá ---
            var gheDaLock = await _db.LichChieuGhes
                .Include(lcg => lcg.Ghe).ThenInclude(g => g.LoaiGhe)
                .Include(lcg => lcg.LichChieu)
                .Where(lcg => request.LichChieuGheIds.Contains(lcg.LichChieuGheId))
                .ToListAsync();

            var giaVeCoBan = gheDaLock.First().LichChieu.GiaVeCoBan;
            var tongTienVe = gheDaLock.Sum(lcg =>
                Math.Round(giaVeCoBan * lcg.Ghe.LoaiGhe.HeSoGia, 0));

            // --- BƯỚC 3: Tính giảm giá ---
            decimal tienGiam = 0;
            if (khuyenMai != null)
            {
                tienGiam = khuyenMai.LoaiGiam == "PhanTram"
                    ? Math.Round(tongTienVe * khuyenMai.GiaTriGiam / 100, 0)
                    : khuyenMai.GiaTriGiam;

                // Áp giới hạn giảm tối đa
                if (khuyenMai.GiaTriGiamToiDa.HasValue && tienGiam > khuyenMai.GiaTriGiamToiDa.Value)
                    tienGiam = khuyenMai.GiaTriGiamToiDa.Value;

                tienGiam = Math.Min(tienGiam, tongTienVe); // Không giảm quá 100%

                khuyenMai.SoLuongDaDung++;
            }

            var thanhTien = tongTienVe - tienGiam;

            // --- BƯỚC 4: Tạo HoaDon ---
            // Cần một CaLamViec — lấy ca đang mở (nullable — Web booking không cần ca)
            var caId = await _db.CaLamViecs
                .Where(c => c.TrangThai == "DangMo")
                .OrderByDescending(c => c.ThoiGianMoCa)
                .Select(c => c.CaId)
                .FirstOrDefaultAsync();

            if (caId == 0) return ServiceResult<DatVeResultDto>.Fail("Hệ thống chưa mở ca bán vé. Vui lòng liên hệ quầy.");

            var hoaDon = new HoaDon
            {
                KhachHangId       = request.KhachHangId,
                CaId              = caId,
                KhuyenMaiId       = khuyenMai?.KhuyenMaiId,
                TongTienVe        = tongTienVe,
                TongTienFnB       = 0,
                TienGiamKm        = khuyenMai != null ? tienGiam : 0,
                TienGiamDiem      = 0,
                TienGiamThanhVien = 0,
                ThanhTien         = thanhTien,
                PhuongThucTt      = request.PhuongThucThanhToan,
                TrangThai         = "HoanThanh",
                ThoiGianTao       = DateTime.Now
            };
            _db.HoaDons.Add(hoaDon);
            await _db.SaveChangesAsync();

            // --- BƯỚC 5: Tạo VePhim cho từng ghế ---
            var danhSachVe = new List<VePhim>();
            foreach (var lcg in gheDaLock)
            {
                var giaBan = Math.Round(giaVeCoBan * lcg.Ghe.LoaiGhe.HeSoGia, 0);
                var ve = new VePhim
                {
                    HoaDonId       = hoaDon.HoaDonId,
                    LichChieuGheId = lcg.LichChieuGheId,
                    GiaGoc         = giaVeCoBan,
                    GiaBan         = giaBan,
                    DoiTuongKhach  = request.DoiTuongKhach,
                    TrangThai      = "DaBan",
                    MaVach         = GenerateMaVach(hoaDon.HoaDonId, lcg.LichChieuGheId),
                    ThoiGianIn     = DateTime.Now
                };
                danhSachVe.Add(ve);

                // Cập nhật trạng thái ghế thành DaBan
                lcg.TrangThaiGhe = "DaBan";
            }

            _db.VePhims.AddRange(danhSachVe);
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();

            // --- BƯỚC 6: Tích điểm khách hàng ---
            if (request.KhachHangId.HasValue)
            {
                var diemCong = (int)(thanhTien / 1000) * BusinessRules.PointsPerThousandVnd;
                if (diemCong > 0)
                {
                    await _db.KhachHangs
                        .Where(kh => kh.KhachHangId == request.KhachHangId.Value)
                        .ExecuteUpdateAsync(kh => kh.SetProperty(x => x.DiemTichLuy,
                            x => x.DiemTichLuy + diemCong));
                }
            }

            var result = new DatVeResultDto
            {
                HoaDonId    = hoaDon.HoaDonId,
                TongTienVe  = tongTienVe,
                TongTienFnB = 0,
                TienGiam    = tienGiam,
                ThanhTien   = thanhTien,
                DanhSachVe  = danhSachVe.Zip(gheDaLock, (ve, lcg) => new VePhimDto
                {
                    VeId          = ve.VeId,
                    HoaDonId      = ve.HoaDonId,
                    TenGhe        = lcg.Ghe.TenGhe ?? $"{lcg.Ghe.DayGhe}{lcg.Ghe.CotGhe}",
                    LoaiGhe       = lcg.Ghe.LoaiGhe.TenLoai,
                    GiaBan        = ve.GiaBan,
                    TrangThai     = ve.TrangThai,
                    MaVach        = ve.MaVach,
                    DoiTuongKhach = ve.DoiTuongKhach,
                    ThoiGianIn    = ve.ThoiGianIn
                }).ToList()
            };

            return ServiceResult<DatVeResultDto>.Success(result, "Đặt vé thành công!");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return ServiceResult<DatVeResultDto>.Fail($"Lỗi đặt vé: {ex.Message}");
        }
    }

    public async Task<List<VePhimDto>> GetLichSuVeAsync(int khachHangId)
    {
        return await _db.VePhims
            .AsNoTracking()
            .Include(v => v.HoaDon)
            .Include(v => v.LichChieuGhe).ThenInclude(lcg => lcg.LichChieu).ThenInclude(lc => lc.Phim)
            .Include(v => v.LichChieuGhe).ThenInclude(lcg => lcg.LichChieu).ThenInclude(lc => lc.Phong)
            .Include(v => v.LichChieuGhe).ThenInclude(lcg => lcg.Ghe).ThenInclude(g => g.LoaiGhe)
            .Where(v => v.HoaDon.KhachHangId == khachHangId)
            .OrderByDescending(v => v.ThoiGianIn)
            .Select(v => new VePhimDto
            {
                VeId          = v.VeId,
                HoaDonId      = v.HoaDonId,
                TenPhim       = v.LichChieuGhe.LichChieu.Phim.TenPhim,
                TenGhe        = v.LichChieuGhe.Ghe.TenGhe ?? string.Empty,
                LoaiGhe       = v.LichChieuGhe.Ghe.LoaiGhe.TenLoai,
                TenPhong      = v.LichChieuGhe.LichChieu.Phong.TenPhong,
                GioChieu      = v.LichChieuGhe.LichChieu.GioBatDau,
                GiaBan        = v.GiaBan,
                TrangThai     = v.TrangThai,
                MaVach        = v.MaVach,
                DoiTuongKhach = v.DoiTuongKhach,
                ThoiGianIn    = v.ThoiGianIn
            })
            .ToListAsync();
    }

    public async Task<ServiceResult> HuyVeAsync(int hoaDonId, int khachHangId)
    {
        var hoaDon = await _db.HoaDons
            .Include(hd => hd.VePhims)
                .ThenInclude(v => v.LichChieuGhe)
            .FirstOrDefaultAsync(hd => hd.HoaDonId == hoaDonId && hd.KhachHangId == khachHangId);

        if (hoaDon == null) return ServiceResult.Fail("Không tìm thấy hóa đơn");
        if (hoaDon.TrangThai == "DaHuy") return ServiceResult.Fail("Hóa đơn đã được hủy trước đó");

        // Chỉ hủy được nếu còn > 2 giờ trước giờ chiếu
        var gioChieu = hoaDon.VePhims.FirstOrDefault()?.LichChieuGhe?.LichChieu?.GioBatDau;
        if (gioChieu.HasValue && gioChieu.Value.AddHours(-2) < DateTime.Now)
            return ServiceResult.Fail("Không thể hủy vé trong vòng 2 giờ trước giờ chiếu");

        await using var transaction = await _db.Database.BeginTransactionAsync();
        try
        {
            hoaDon.TrangThai  = "DaHuy";
            hoaDon.LyDoHuy    = "Khách hàng hủy";

            // Trả ghế về Trong
            foreach (var ve in hoaDon.VePhims)
            {
                ve.TrangThai = "DaHuy";
                if (ve.LichChieuGhe != null)
                    ve.LichChieuGhe.TrangThaiGhe = "Trong";
            }

            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            return ServiceResult.Success("Hủy vé thành công. Hoàn tiền sẽ được xử lý trong 3-5 ngày làm việc.");
        }
        catch
        {
            await transaction.RollbackAsync();
            return ServiceResult.Fail("Lỗi khi hủy vé. Vui lòng thử lại hoặc liên hệ quầy.");
        }
    }

    public async Task<ServiceResult> ValidateQrAsync(string maVach)
    {
        var ve = await _db.VePhims
            .Include(v => v.LichChieuGhe).ThenInclude(lcg => lcg.LichChieu)
            .FirstOrDefaultAsync(v => v.MaVach == maVach);

        if (ve == null) return ServiceResult.Fail("Mã vé không hợp lệ");
        if (ve.TrangThai == "DaKiemSoat") return ServiceResult.Fail("Vé này đã được sử dụng");
        if (ve.TrangThai == "DaHuy") return ServiceResult.Fail("Vé đã bị hủy");

        var gioChieu = ve.LichChieuGhe.LichChieu.GioBatDau;
        if (DateTime.Now > gioChieu.AddMinutes(30))
            return ServiceResult.Fail("Vé đã quá giờ chiếu 30 phút");

        ve.TrangThai = "DaKiemSoat";
        await _db.SaveChangesAsync();
        return ServiceResult.Success("Vé hợp lệ — Chào mừng quý khách!");
    }

    private static string GenerateMaVach(int hoaDonId, int lichChieuGheId)
        => $"NF-{hoaDonId:D6}-{lichChieuGheId:D6}-{DateTime.Now:yyyyMMddHHmmss}";
}