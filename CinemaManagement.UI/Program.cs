using CinemaManagement.DAL.Context;
using CinemaManagement.UI.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows.Forms;
using CinemaManagement.BLL.Services;

namespace CinemaManagement.UI
{
    internal static class Program
    {
        // Khai báo DI Container toàn cục
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // 1. Cấu hình xử lý lỗi toàn cục (RULE-ERR-01)
            Application.ThreadException += GlobalExceptionHandler;
            AppDomain.CurrentDomain.UnhandledException += GlobalUnhandledExceptionHandler;

            // 2. Nạp cấu hình từ appsettings.json (RULE-SEC-01)
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            // 3. Khởi tạo Service Collection và Đăng ký các dịch vụ (RULE-ARCH-02)
            var services = new ServiceCollection();

            // Đăng ký DbContext với chuỗi kết nối
            services.AddDbContext<CinemaDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CinemaConnection")));

            // Đăng ký toàn bộ BLL Services
            services.AddScoped<IAuthService,      AuthService>();
            services.AddScoped<IMovieService,     MovieService>();
            services.AddScoped<IShowtimeService,  ShowtimeService>();
            services.AddScoped<ITicketService,    TicketService>();
            services.AddScoped<IPosService,       PosService>();
            services.AddScoped<ICustomerService,  CustomerService>();
            services.AddScoped<IProductService,   ProductService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<IUserService,      UserService>();
            services.AddScoped<IReportService,    ReportService>();

            // Đăng ký Form chính (frmLogin)
            services.AddTransient<frmLogin>();

            // 4. Build DI Container
            ServiceProvider = services.BuildServiceProvider();

            // 5. Khởi chạy Form chính thông qua DI thay vì dùng từ khóa 'new'
            var mainForm = ServiceProvider.GetRequiredService<frmLogin>();
            Application.Run(mainForm);

        }

        // --- Các hàm bắt lỗi toàn cục ---
        private static void GlobalExceptionHandler(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            // Ở đây sau này sẽ gọi thư viện Log (Serilog/NLog) để ghi ra file
            MessageBox.Show($"Đã xảy ra lỗi hệ thống: {e.Exception.Message}\nVui lòng liên hệ Admin.", "Lỗi Ứng Dụng", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void GlobalUnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;
            MessageBox.Show($"Lỗi nghiêm trọng (Background Thread): {ex.Message}", "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }
    }
}