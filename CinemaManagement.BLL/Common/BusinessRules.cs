namespace CinemaManagement.BLL.Common;

/// <summary>
/// Các hằng số nghiệp vụ dùng chung toàn hệ thống.
/// Thay thế Magic Strings — tuân thủ RULE-DEV-02.
/// </summary>
public static class BusinessRules
{
    // --- Khóa ghế ---
    public const int SeatLockTimeoutMinutes = 10;
    public const int MaxSeatsPerBooking = 8;
    public const string SeatStatusAvailable = "Trong";
    public const string SeatStatusLocked    = "DangGiu";
    public const string SeatStatusSold      = "DaBan";
    public const string SeatStatusBlocked   = "Blocked";

    // --- Trạng thái phim ---
    public const string MovieStatusComing  = "SapChieu";
    public const string MovieStatusShowing = "DangChieu";
    public const string MovieStatusStopped = "NgungChieu";

    // --- Trạng thái hóa đơn ---
    public const string InvoiceStatusProcessing = "DangXuLy";
    public const string InvoiceStatusDone       = "HoanThanh";
    public const string InvoiceStatusCancelled  = "DaHuy";
    public const string InvoiceStatusRefunded   = "HoanTien";

    // --- Phòng chiếu ---
    public const int SeatButtonThreshold = 200; // Dưới mức này dùng Button, trên dùng GDI+

    // --- Điểm tích lũy ---
    public const int PointsPerThousandVnd = 1; // Cứ 1000đ → 1 điểm

    // --- Thanh toán ---
    public const string PaymentCash          = "TienMat";
    public const string PaymentTransfer      = "ChuyenKhoan";
    public const string PaymentVNPAY         = "VNPAY";
    public const string PaymentMoMo          = "MoMo";

    // --- Loại giảm giá ---
    public const string DiscountTypePercent  = "PhanTram";
    public const string DiscountTypeFixed    = "TienMat";

    // --- Đối tượng khách hàng ---
    public const string CustomerTypeAdult   = "NguoiLon";
    public const string CustomerTypeStudent = "SinhVien";
    public const string CustomerTypeChild   = "TreEm";
    public const string CustomerTypeSenior  = "NguoiCaoTuoi";
}
