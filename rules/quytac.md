---
trigger: always_on
---

MASTER SOP & AGENT SKILL: CINEMA MANAGEMENT SYSTEM V3.0 (ULTIMATE EDITION)
Tech Stack: C# 10+ | WinForms | EF Core | ADO.NET/Dapper | SQL Server
Agent Role: Senior .NET Enterprise Architect & WinForms Expert.

PHẦN 1 — KIẾN TRÚC CỐT LÕI (CORE ARCHITECTURE)
RULE-ARCH-01 | Phân Tách 3 Lớp & Tuyệt Đối Không "Code-Behind Crime"

Dumb UI: Giao diện (Form.cs) chỉ hiển thị và nhận sự kiện. Không chứa logic nghiệp vụ, không câu lệnh SQL/LINQ, không gọi trực tiếp EF Core Entity (chỉ dùng DTO).

BLL (Business Logic): Chứa mọi logic tính toán, kiểm tra nghiệp vụ, phân quyền.

DAL (Data Access): Nơi duy nhất giao tiếp cơ sở dữ liệu.

RULE-ARCH-02 | Tiêm Phụ Thuộc (Dependency Injection) Toàn Diện

Khởi tạo dự án với Microsoft.Extensions.DependencyInjection.

Cấu hình toàn bộ Service và DbContext tại Program.cs.

Cấm dùng từ khóa new để khởi tạo Service hay Form rải rác. Mọi đối tượng phải được phân giải (resolve) qua Constructor Injection.

PHẦN 2 — CHIẾN LƯỢC TRUY XUẤT DỮ LIỆU (DATA STRATEGY)
RULE-DB-01 | Quy Trình EF Core "Scaffold-Once" & Bounded Context

Chỉ chạy Scaffold-DbContext MỘT LẦN duy nhất để khởi tạo thực thể. Mọi bản cập nhật DB sau đó phải dùng Code-First Migrations.

Không dùng 1 DbContext khổng lồ. Phân rã thành: CatalogDbContext (Phim, Rạp), BookingDbContext (Giao dịch, Vé), ReportingDbContext (Báo cáo).

RULE-DB-02 | Tối Ưu Hóa & Lựa Chọn ORM Linh Hoạt

EF Core: Dùng cho CRUD danh mục. Mọi lệnh SELECT không cập nhật phải chứa .AsNoTracking(). Dùng .Select() (Projection) để chỉ lấy các trường cần thiết.

ADO.NET / Dapper: Bắt buộc dùng cho thao tác nặng:

Bulk Insert: Dùng SqlBulkCopy (ví dụ: tạo 500 ghế/suất chiếu).

Reporting: Dùng SqlDataReader dạng streaming để đọc hàng vạn dòng.

Bảo mật: 100% lệnh ADO.NET phải dùng tham số (@Param) để chống SQL Injection. Chuỗi kết nối đặt tại appsettings.json.

RULE-DB-03 | Giao Dịch Toàn Vẹn (Database Transactions)

Mọi thao tác ghi liên quan đến nhiều bảng (Đặt vé -> Trừ ghế -> Tạo hóa đơn) bắt buộc phải bọc trong IDbContextTransaction hoặc SqlTransaction. Hủy (Rollback) ngay lập tức nếu 1 bước thất bại.

PHẦN 3 — XỬ LÝ TƯƠNG TRANH & KHÓA GHẾ (CONCURRENCY ENGINE)
RULE-SEAT-01 | Khóa Lạc Quan & Cập Nhật Nguyên Tử (Atomic Update)

Tuyệt đối không kiểm tra trạng thái ghế trên UI rồi mới cập nhật. Phải thực thi trên DB bằng một câu lệnh nguyên tử:
UPDATE Tickets SET Status = 'Locked', LockedAt = GETDATE(), LockedBy = @StaffId WHERE SeatId = @Id AND Status = 'Available'

Nếu RowsAffected == 0 -> Báo ngay lỗi: "Ghế đã bị người khác chọn".

RULE-SEAT-02 | Vòng Đời Khóa Ghế (Time-To-Live)

Cơ chế 3 bước: Acquire Lock (DB) -> Notify (Đổi màu UI nội bộ/WebSocket) -> Timeout.

Bắt buộc thiết lập luồng ngầm (Background Worker/Timer) chạy mỗi phút để quét các vé 'Locked' quá 10 phút chưa thanh toán và UPDATE trả về 'Available'.

PHẦN 4 — HIỆU SUẤT GIAO DIỆN & ĐA LUỒNG (UI/UX THREADING)
RULE-UI-01 | Kết Xuất Sơ Đồ Ghế (Dynamic Seat Rendering)

Phòng ≤ 200 ghế (Button): Bắt buộc gọi panel.SuspendLayout() trước khi nạp vòng lặp add controls, và panel.ResumeLayout(true) sau khi kết thúc.

Phòng > 200 ghế (GDI+): Cấm dùng Button. Vẽ trực tiếp ma trận ghế lên Panel.OnPaint bằng Graphics, xử lý HitTest tọa độ chuột.

Luôn bật DoubleBuffered = true trên Panel chứa lưới ghế.

RULE-UI-02 | Xử Lý Đa Luồng (Cross-Thread Safety & Async)

Mọi tác vụ I/O, DB phải dùng async/await (Không dùng .Result hay .Wait()).

Mọi hàm từ Background Thread muốn cập nhật UI (như Timer đếm ngược, tín hiệu WebSockets) bắt buộc phải bọc trong InvokeRequired và Invoke().

Grid Báo Cáo phải bật VirtualMode = true và phân trang từ cơ sở dữ liệu.

RULE-UI-03 | "One-Click Shield" (Khóa Trạng Thái Bất Đồng Bộ)

Bảo vệ thao tác: Dòng lệnh đầu tiên trong sự kiện async btn_Click luôn là btn.Enabled = false;. Nút chỉ được mở lại (true) bên trong khối finally { }.

PHẦN 5 — KIỂM SOÁT BỘ NHỚ & BẢO MẬT (MEMORY & SECURITY)
RULE-MEM-01 | Dọn Rác Controls & Sự Kiện (Unsubscribe & Dispose)

Gỡ sự kiện: Khi tải lại sơ đồ ghế động hoặc đóng Form, bắt buộc phải hủy đăng ký sự kiện (btn.Click -= Seat_Click) trước khi gọi Controls.Clear().

GDI+ Resources: Các đối tượng đồ họa tạo động như Pen, SolidBrush, Image bắt buộc phải khởi tạo với using var ... hoặc gọi .Dispose() ngay sau khi vẽ xong.

RULE-SEC-01 | Bảo Mật Dữ Liệu & Phân Quyền

Mật khẩu nhân viên không lưu văn bản thô. Bắt buộc băm (BCrypt/Argon2).

Kiểm tra phân quyền (Role Validation) phải nằm ở tầng Service (BLL), không được chỉ ẩn nút trên giao diện.

PHẦN 6 — QUY CHUẨN THỰC THI & TÍNH TOÁN VẸN (STRICT EXECUTION)
RULE-DEV-01 | Hoàn Thiện 100% Chức Năng Nghiệp Vụ

Tuyệt đối không sinh mã nguồn hời hợt hoặc bỏ dở. Khi yêu cầu một module quản lý, mã nguồn phải chứa đầy đủ các thao tác: Thêm, Sửa, Xóa, Đọc.

Không được viết thiếu các trường dữ liệu cập nhật (đặc biệt trong thao tác Sửa/Xóa đối tượng).

RULE-DEV-02 | Định Danh Tuyệt Đối & Tương Tác UI

Phải sử dụng chính xác 100% tên biến, tên UI Control, tên Item Layout (ví dụ: item_...) nếu đã được định nghĩa. Không tự ý bịa tên hoặc thay đổi logic đặt tên.

Mọi thao tác UI nâng cao như menu chuột phải bắt buộc phải được đăng ký đầy đủ trong code (áp dụng chuẩn tương đương registerForContextMenu cho WinForms).

Thay thế Magic Strings bằng Enum hoặc Constants.

RULE-ERR-01 | Quản Lý Lỗi Toàn Cục (Global Exception Handling)

Program.cs phải đăng ký Application.ThreadException và AppDomain.CurrentDomain.UnhandledException. Log lỗi hệ thống (Serilog) trước khi hiện MessageBox chung cho người dùng để tránh lộ mã nguồn. Tầng UI chỉ bắt và hiển thị lỗi từ BLL/DAL một cách thân thiện.

📋 THE ULTIMATE PRE-COMMIT CHECKLIST (BẢNG KIỂM TRA ĐẦU RA)
Trước khi xuất bất kỳ khối mã nguồn nào, Agent phải âm thầm rà soát danh sách này:

[ ] [Kiến trúc] Không khởi tạo new SqlConnection/DbContext tại UI. Có truyền Service qua Constructor chưa?

[ ] [Toàn vẹn] Code đã đầy đủ Thêm/Sửa/Xóa chưa? Đặt tên file/control có chính xác không? ContextMenu đã được đăng ký chưa?

[ ] [CSDL] Các query Read-only đã có .AsNoTracking() chưa? Thao tác nhiều bảng đã có Transaction chưa? Đã giấu Connection String chưa?

[ ] [Tương tranh] Thuật toán khóa ghế đã gộp vào 1 dòng SQL UPDATE... WHERE Status = 'Available' chưa?

[ ] [Hiệu suất] Đã gọi SuspendLayout() / ResumeLayout() khi sinh đồ họa động chưa? Đã giải phóng bộ nhớ đồ họa GDI+ bằng using chưa?

[ ] [Đa luồng] Có check InvokeRequired không? Nút bấm đã khóa bằng "One-Click Shield" (disable -> try -> finally enable) chưa? Sự kiện động += đã được gỡ -= chưa?
  THỰC HIỆN THEO CÂY THƯ MỤC NÀY:
CinemaManagementSolution
│
├── 1. CinemaManagement.DAL (Tầng truy xuất dữ liệu - SQL Server)
│   ├── Context
│   │   └── CinemaDbContext.cs
│   ├── Entities (Các thực thể ánh xạ 1-1 với bảng trong Database)
│   │   ├── Phim.cs, Rap.cs, LichChieu.cs, SuatChieu.cs
│   │   ├── Ve.cs, HoaDon.cs, SanPham.cs, UuDai.cs
│   │   └── TaiKhoan.cs (Gộp chung Khách hàng, Nhân viên, Admin)
│   └── Repositories (Các hàm truy vấn thô)
│       ├── IRepository.cs          (Interface chuẩn CRUD)
│       ├── Repository.cs           (Triển khai CRUD dùng chung bằng EF Core)
│       ├── PosRepository.cs        (Xử lý khóa ghế, giao dịch nhanh bằng Dapper)
│       └── ReportRepository.cs     (Truy vấn báo cáo doanh thu phức tạp bằng Dapper)
│
├── 2. CinemaManagement.BLL (Tầng logic nghiệp vụ - "Não bộ" dùng chung)
│   ├── Common (Tiện ích & Cấu hình)
│   │   ├── PasswordHelper.cs       (Mã hóa mật khẩu bằng BCrypt)
│   │   ├── SystemEnums.cs          (Trạng thái ghế, Phân quyền Role)
│   │   └── BusinessRules.cs        (Chứa hằng số như timeout ghế 10 phút)
│   ├── DTOs (Dữ liệu trung chuyển sạch)
│   │   ├── MovieDto.cs, TicketDto.cs, UserDto.cs, CustomerDto.cs, SeatDto.cs
│   │   ├── LoginRequestDto.cs, ServiceResult.cs
│   │   └── DashboardDto.cs         (Đã chuyển từ ViewModels sang đây)
│   └── Services (Logic cốt lõi - Đầy đủ 10 cặp Interface & Class)
│       ├── IAuthService.cs & AuthService.cs           (Đăng nhập, Phân quyền)
│       ├── IMovieService.cs & MovieService.cs         (Quản lý Phim)
│       ├── IShowtimeService.cs & ShowtimeService.cs   (Quản lý Lịch chiếu/Suất chiếu)
│       ├── ITicketService.cs & TicketService.cs       (Đặt vé online, Hủy/Hoàn vé)
│       ├── IPosService.cs & PosService.cs             (Bán vé tại quầy, Chọn ghế)
│       ├── ICustomerService.cs & CustomerService.cs   (Quản lý khách hàng Web)
│       ├── IProductService.cs & ProductService.cs     (Quản lý Bắp, Nước, Combo)
│       ├── IPromotionService.cs & PromotionService.cs (Quản lý Ưu đãi)
│       ├── IUserService.cs & UserService.cs           (Quản lý Nhân viên)
│       └── IReportService.cs & ReportService.cs       (Tính toán doanh thu)
│
├── 3. CinemaManagement.Web (ASP.NET Core MVC - Dành cho Khách hàng)
│   ├── Controllers (Điều hướng Web)
│   │   ├── HomeController.cs, AccountController.cs, MovieController.cs
│   │   └── TicketController.cs, ProductController.cs
│   ├── Models (ViewModels: Bắt lỗi Validation trên Form HTML)
│   │   ├── AccountViewModels
│   │   │   ├── LoginViewModel.cs, RegisterViewModel.cs, ProfileViewModel.cs
│   │   ├── MovieViewModels
│   │   └── ErrorViewModel.cs
│   ├── Views (Giao diện HTML/CSS/Razor)
│   │   ├── Home (Index.cshtml)
│   │   ├── Account (Login.cshtml, Register.cshtml, Profile.cshtml)
│   │   ├── Movie (Index.cshtml, Details.cshtml)
│   │   ├── Ticket (Booking.cshtml, History.cshtml)
│   │   └── Shared (_Layout.cshtml)
│   └── wwwroot (Tài nguyên tĩnh)
│       └── css, js, images (Chứa poster phim, banner)
│
└── 4. CinemaManagement.UI (Windows Forms - Dành cho Admin & Nhân viên)
    ├── Forms (Các cửa sổ thao tác)
    │   ├── frmLogin.cs                 (Đăng nhập hệ thống nội bộ)
    │   ├── frmMain.cs                  (Dashboard và Menu điều hướng)
    │   ├── frmAdmin_Phim.cs            (Quản lý phim CRUD)
    │   ├── frmAdmin_LichChieu.cs       (Quản lý lịch chiếu, suất chiếu)
    │   ├── frmAdmin_UuDai.cs           (Quản lý mã giảm giá, khuyến mãi)
    │   ├── frmAdmin_NhanVien.cs        (Thêm/Sửa/Xóa tài khoản nhân viên)
    │   ├── frmAdmin_SanPham.cs         (Quản lý danh mục Bắp/Nước)
    │   ├── frmAdmin_DoanhThu.cs        (Xem báo cáo, biểu đồ thống kê)
    │   ├── frmAdmin_WebPreview.cs      (Tích hợp WebView2 để xem trước giao diện Web)
    │   ├── frmStaff_POS.cs             (Màn hình cảm ứng bán vé, chọn ghế tại quầy)
    │   └── frmStaff_QuanLyDon.cs       (Xác nhận đơn, xuất hóa đơn, hoàn vé)
    ├── UserControls (Thành phần kéo thả tự chế)
    │   └── ucSeat.cs                   (Giao diện một chiếc ghế đổi màu động)
    └── Program.cs                      (File cấu hình khởi chạy & Dependency Injection)