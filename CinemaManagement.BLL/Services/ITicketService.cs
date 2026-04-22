using CinemaManagement.BLL.DTOs;

namespace CinemaManagement.BLL.Services;

public interface ITicketService
{
    /// <summary>Đặt vé — atomic lock ghế + tạo HoaDon+VePhim trong 1 transaction</summary>
    Task<ServiceResult<DatVeResultDto>> DatVeAsync(DatVeRequestDto request);

    /// <summary>Lấy lịch sử đặt vé của khách hàng</summary>
    Task<List<VePhimDto>> GetLichSuVeAsync(int khachHangId);

    /// <summary>Hủy vé (yêu cầu hoàn tiền)</summary>
    Task<ServiceResult> HuyVeAsync(int hoaDonId, int khachHangId);

    /// <summary>Kiểm tra mã QR vé — dùng cho WinForms soát vé</summary>
    Task<ServiceResult> ValidateQrAsync(string maVach);
}
