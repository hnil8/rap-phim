using CinemaManagement.BLL;
using CinemaManagement.BLL.Services;
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
        public static IServiceProvider Services { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var services = new ServiceCollection();
            ConfigureServices(services);

            Services = services.BuildServiceProvider();
            var formDangNhap = Services.GetRequiredService<frmLogin>();
            Application.Run(formDangNhap);
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            services.AddDbContext<CinemaDbContext>(options =>
                options.UseSqlServer(DatabaseConfig.ConnectionString));

            services.AddScoped<TaiKhoanDAL>();
            services.AddScoped<TaiKhoanBLL>();

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<ITheLoaiService, TheLoaiService>();
            services.AddScoped<IShowtimeService, ShowtimeService>();
            services.AddScoped<IPromotionService, PromotionService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<INhomFnBService, NhomFnBService>();
            services.AddScoped<ISanPhamService, SanPhamService>();
            services.AddScoped<IComboService, ComboService>();
            services.AddScoped<IPhieuNhapService, PhieuNhapService>();

            services.AddTransient<frmLogin>();
            services.AddTransient<frmMain>();
            services.AddTransient<frmAdmin_Phim>();
            services.AddTransient<frmAdmin_TheLoai>();
            services.AddTransient<frmAdmin_SanPham>();
            services.AddTransient<frmAdmin_WebPreview>();
            services.AddTransient<frmAdmin_LichChieu>();
            services.AddTransient<frmAdmin_UuDai>();
            services.AddTransient<frmStaff_POS>();
            services.AddTransient<frmStaff_QuanLyDon>();
            services.AddTransient<frmAdmin_NhanVien>();
            services.AddTransient<frmAdmin_DoanhThu>();
            services.AddTransient<frmQuanLyKhachHang>();
            services.AddTransient<frmAdmin_Combo>();
            services.AddTransient<frmAdmin_NhapKhoFnB>();
            services.AddTransient<frmAdmin_CaLamViec>();
            services.AddTransient<frmAdmin_ThietKePhong>();
            services.AddTransient<frmAdmin_QuanLyPhong>();
        }
    }
}
