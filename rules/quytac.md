---
trigger: always_on
---

👑 MASTER SOP & AGENT SKILL: CINEMA MANAGEMENT SYSTEM V3.0 (ULTIMATE EDITION)
Tech Stack: C# 14.0 (.NET 10) | WinForms (Guna.UI2, LiveCharts) | ASP.NET Core Razor Pages | EF Core 10.0 | SQL Server
Agent Role: Senior .NET Enterprise Architect & WinForms Expert.

PHẦN 1 — KIẾN TRÚC CỐT LÕI (CORE ARCHITECTURE)
RULE-ARCH-01 | Phân Tách N-Tier & Tuyệt Đối Không "Code-Behind Crime"

Dumb UI (Tầng Presentation): Giao diện (Form.cs hoặc .cshtml.cs) chỉ hiển thị và nhận sự kiện. Không chứa logic nghiệp vụ, không viết trực tiếp câu lệnh SQL/LINQ, không gọi trực tiếp EF Core Entity.

BLL (Business Logic Layer): Chứa mọi logic tính toán, kiểm tra nghiệp vụ, phân quyền (VD: TaiKhoanBLL.cs, PhimBLL.cs). Là cầu nối duy nhất giữa UI và DAL.

DAL (Data Access Layer): Nơi duy nhất giao tiếp cơ sở dữ liệu qua CinemaDbContext hoặc các lớp ...DAL.cs.

RULE-ARCH-02 | Tiêm Phụ Thuộc (Dependency Injection) Toàn Diện

Cấu hình toàn bộ DAL, BLL và DbContext tại Program.cs (cho cả UI và Web).

Cấm dùng từ khóa new để khởi tạo các lớp BLL/DAL rải rác trong Form. Mọi đối tượng phải được phân giải (resolve) qua Constructor Injection.

PHẦN 2 — CHIẾN LƯỢC TRUY XUẤT DỮ LIỆU (DATA STRATEGY)
RULE-DB-01 | Quy Trình EF Core & Entity Mapping

Sử dụng chung một DbContext là CinemaDbContext.cs đặt tại CinemaManagement.DAL.

Mọi bảng trong DB (25+ bảng) phải được ánh xạ chính xác thành các class trong thư mục Entities/ (Phim, LichChieu, GheNgoi, TaiKhoan, VePhim, HoaDon,...).

RULE-DB-02 | Tối Ưu Hóa Truy Vấn

Truy vấn tĩnh: Mọi lệnh SELECT để hiển thị danh sách (như xem danh sách phim, lịch chiếu) phải chứa .AsNoTracking() để giảm tải bộ nhớ.

Sử dụng DTO: Dữ liệu trả về UI không dùng trực tiếp Entity mà phải map sang DTO (VD: NguoiDungDTO, PhimDto) để tránh lộ cấu trúc DB và lỗi vòng lặp JSON (Circular Reference).

RULE-DB-03 | Giao Dịch Toàn Vẹn (Database Transactions)

Mọi thao tác ghi liên quan đến nhiều bảng (VD: POS Bán vé -> Trừ ghế -> Cập nhật LichChieu_Ghe -> Tạo HoaDon -> Tạo VePhim) bắt buộc phải bọc trong IDbContextTransaction. Hủy (Rollback) ngay lập tức nếu 1 bước thất bại.

PHẦN 3 — XỬ LÝ TƯƠNG TRANH & KHÓA GHẾ (CONCURRENCY ENGINE)
RULE-SEAT-01 | Khóa Lạc Quan & Cập Nhật Nguyên Tử (Atomic Update)

Tuyệt đối không kiểm tra trạng thái ghế trên UI rồi mới cập nhật. Phải thực thi trên DB bằng một câu lệnh nguyên tử đối với bảng LichChieu_Ghe:
UPDATE LichChieu_Ghe SET TrangThaiGhe = 'ĐangGiu', ThoiGianGiu = GETDATE() WHERE LichChieuGheId = @Id AND TrangThaiGhe = 'Trong'

Nếu RowsAffected == 0 -> Ném Exception: "Ghế đã bị người khác chọn".

RULE-SEAT-02 | Vòng Đời Khóa Ghế (Time-To-Live)

Thời gian giữ ghế cấu hình trong appsettings.json (SeatLockTimeoutMinutes).

Bắt buộc thiết lập luồng ngầm quét các ghế có trạng thái 'ĐangGiu' quá hạn và tự động trả về 'Trong'.

PHẦN 4 — HIỆU SUẤT GIAO DIỆN & ĐA LUỒNG (UI/UX THREADING)
RULE-UI-01 | Kết Xuất Sơ Đồ Ghế (Dynamic Seat Rendering)

Dùng UserControl ucSeat.cs để render ghế.

Bắt buộc gọi panel.SuspendLayout() trước khi nạp vòng lặp add controls (ghế) vào FlowLayoutPanel, và panel.ResumeLayout(true) sau khi kết thúc để tránh giật lag màn hình POS.

RULE-UI-02 | Xử Lý Đa Luồng (Cross-Thread Safety & Async)

Mọi tác vụ I/O, gọi BLL/DAL từ UI phải dùng async/await.

Mọi hàm từ Background Thread muốn cập nhật UI bắt buộc phải dùng InvokeRequired và Invoke().

RULE-UI-03 | "One-Click Shield" (Khóa Trạng Thái Bất Đồng Bộ)

Bảo vệ thao tác CRUD: Dòng lệnh đầu tiên trong sự kiện async btn_Click luôn là btn.Enabled = false;. Nút chỉ được mở lại (true) bên trong khối finally { }.

PHẦN 5 — KIỂM SOÁT BỘ NHỚ & BẢO MẬT (MEMORY & SECURITY)
RULE-MEM-01 | Dọn Rác Controls & Sự Kiện

Khi tải lại sơ đồ ghế động hoặc đóng frmStaff_POS, bắt buộc phải hủy đăng ký sự kiện (ucSeat.Click -= ...) trước khi gọi Controls.Clear().

Giải phóng các đối tượng GDI+ (nếu có) bằng using.

RULE-SEC-01 | Bảo Mật Dữ Liệu & Phân Quyền

Mật khẩu bảng TaiKhoan bắt buộc phải băm (Hash) trước khi lưu. Logic kiểm tra nằm ở TaiKhoanBLL.DangNhap().

Sử dụng biến toàn cục hoặc Session để lưu quyền VaiTroId. Các Form quản trị (frmAdmin_...) phải chặn truy cập nếu Role không hợp lệ.

PHẦN 6 — QUY CHUẨN THỰC THI & TÍNH TOÀN VẸN (STRICT EXECUTION)
RULE-DEV-01 | Hoàn Thiện 100% Chức Năng Nghiệp Vụ

Không sinh code hời hợt. Khi làm module (ví dụ: frmAdmin_Phim), phải hoàn thiện đủ: Thêm, Sửa, Xóa, Tìm kiếm và Load lên Grid.

RULE-ERR-01 | Quản Lý Lỗi Toàn Cục (Global Exception Handling)

Lỗi nghiệp vụ từ BLL trả về message thân thiện (ví dụ qua tham số out string errorMsg).

Catch toàn cục tại Program.cs để ghi Log (tránh crash WinForms). Tầng UI dùng MessageBox.Show có icon rõ ràng (GunaUI Alerts).

📁 CÂY THƯ MỤC CHUẨN ĐỂ AGENT THỰC THI (Theo đúng Project thực tế)
Plaintext
CinemaManagementSolution/
│
├── 1. CinemaManagement.DAL (Tầng Truy xuất dữ liệu)
│   ├── Context/
│   │   └── CinemaDbContext.cs       (Mapping 25+ bảng)
│   ├── Entities/
│   │   ├── Phim.cs, LichChieu.cs, PhongChieu.cs, GheNgoi.cs
│   │   ├── TaiKhoan.cs, NhanVien.cs, VaiTro.cs, KhachHang.cs
│   │   ├── HoaDon.cs, VePhim.cs, LichChieuGhe.cs
│   │   └── SanPhamFnB.cs, Combo.cs, KhuyenMai.cs...
│   ├── db/
│   │   ├── DatabaseConfig.cs        (Đọc chuỗi kết nối)
│   │   ├── NguoiDungDTO.cs          (Và các DTO khác)
│   │   ├── TaiKhoanDAL.cs           (Thực thi Entity Framework/SQL)
│   │   ├── PhimDAL.cs, LichChieuDAL.cs...
│   │   └── sql_insert.sql           (Kịch bản seed data)
│
├── 2. CinemaManagement.BLL (Tầng Logic nghiệp vụ)
│   ├── TaiKhoanBLL.cs               (Xác thực, kiểm tra rule)
│   ├── PhimBLL.cs                   (Validate Phim trước khi Insert)
│   └── LichChieuBLL.cs, HoaDonBLL.cs...
│
├── 3. CinemaManagement.Web (Khách hàng - ASP.NET Core Razor Pages)
│   ├── appsettings.json
│   ├── Pages/
│   │   ├── Home/ (Index.cshtml)     (Phim đang chiếu/sắp chiếu)
│   │   ├── Movie/ (Details.cshtml)  (Chi tiết & Lịch chiếu)
│   │   └── Booking/                 (Đặt vé)
│   ├── Models/
│   │   └── HomeIndexViewModel.cs, MovieDetailsViewModel.cs...
│   └── Program.cs                   (Middleware & DI Core)
│
└── 4. CinemaManagement.UI (Admin/Staff - WinForms)
    ├── appsettings.json
    ├── Forms/
    │   ├── frmLogin.cs              (Đăng nhập)
    │   ├── frmMain.cs               (Dashboard)
    │   ├── frmAdmin_Phim.cs         (CRUD Phim)
    │   ├── frmAdmin_LichChieu.cs    (CRUD Lịch chiếu)
    │   ├── frmAdmin_DoanhThu.cs     (Báo cáo)
    │   ├── frmAdmin_NhanVien.cs, frmAdmin_SanPham.cs, frmAdmin_UuDai.cs
    │   ├── frmAdmin_WebPreview.cs   (WebView2)
    │   ├── frmStaff_POS.cs          (Bán vé, chọn sơ đồ ghế)
    │   ├── frmStaff_QuanLyDon.cs    (In hóa đơn)
    │   └── frmQuanLyKhachHang.cs
    ├── UserControls/
    │   └── ucSeat.cs                (Component vẽ ghế ngồi động)
    └── Program.cs                   (Entry Point WinForms & DI Container)
📋 THE ULTIMATE PRE-COMMIT CHECKLIST (BẢNG KIỂM TRA ĐẦU RA CỦA AGENT)
Trước khi xuất bất kỳ khối mã nguồn nào, tôi (Agent) sẽ tự rà soát:

[x] Không gọi Entity/DbContext trực tiếp trên Form (frmAdmin_...).

[x] Tuân thủ đúng tên file/biến (VD: LichChieuGhe, TaiKhoanBLL).

[x] Lệnh read-only có .AsNoTracking() chưa? Có bọc async/await không?

[x] Khóa "One-Click Shield" (try/finally) trên các nút btnLuu_Click đã có chưa?

[x] Sơ đồ ghế đã có SuspendLayout và ResumeLayout?
  KHÔNG ĐƯỢC TỰ Ý THAY ĐỔI MÃ NGUỒN KHI TÔI CHƯA CHO PHÉP
