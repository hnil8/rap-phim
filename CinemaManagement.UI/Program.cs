using CinemaManagement.BLL;
using CinemaManagement.DAL;
// Thêm các using này để gọi được DAL, BLL, Context và các Form
using CinemaManagement.DAL.Context;
using CinemaManagement.DAL.db;
using CinemaManagement.UI.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace CinemaManagement.UI
{
    internal static class Program
    {
        // Khai báo ServiceProvider toàn cục (giúp các chỗ khác nếu cần có thể gọi DI)
        public static IServiceProvider Services { get; private set; }

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 1. Tạo thùng chứa dịch vụ (Service Collection)
            var services = new ServiceCollection();

            // 2. Tiến hành đăng ký các Dependencies vào thùng chứa
            ConfigureServices(services);

            // 3. Đóng gói thùng chứa thành ServiceProvider
            Services = services.BuildServiceProvider();

            // 4. Lấy frmLogin ra khỏi thùng chứa (Lúc này hệ thống sẽ tự động BƠM BLL, DAL, DbContext vào frmLogin)
            var formDangNhap = Services.GetRequiredService<frmLogin>();

            // 5. Chạy phần mềm với Form đã được tiêm đầy đủ sức mạnh
            Application.Run(formDangNhap);
        }

        // Hàm chuyên dùng để khai báo các Dependencies
        private static void ConfigureServices(ServiceCollection services)
        {
            // ── 1. ĐĂNG KÝ DATABASE CONTEXT ──
            // (Giả sử DatabaseConfig là một class static chứa chuỗi kết nối của bạn)
            services.AddDbContext<CinemaDbContext>(options =>
                options.UseSqlServer(DatabaseConfig.ConnectionString));

            // ── 2. ĐĂNG KÝ TẦNG DAL (Data Access Layer) ──
            services.AddScoped<TaiKhoanDAL>();

            // ── 3. ĐĂNG KÝ TẦNG BLL (Business Logic Layer) ──
            services.AddScoped<TaiKhoanBLL>();

            // ── 4. ĐĂNG KÝ CÁC FORM (UI Layer) ──
            // AddTransient có nghĩa là mỗi lần gọi GetRequiredService, hệ thống sẽ tạo ra 1 Form mới tinh
            services.AddTransient<frmLogin>();

            // Lời khuyên: Sau này nếu bạn áp dụng DI cho các form khác, bạn cũng đăng ký chúng ở đây.
            // Ví dụ: services.AddTransient<frmMain>();
        }
    }
}