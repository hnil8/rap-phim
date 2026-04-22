// FILE: DatabaseConfig.cs  —  namespace: CinemaManagement.DAL.db  (giữ nguyên)
// NuGet cần cài vào project CinemaManagement.DAL:
//   Install-Package Microsoft.Extensions.Configuration
//   Install-Package Microsoft.Extensions.Configuration.Json

using System;
using System.IO;
using Microsoft.Extensions.Configuration;     // ← cần NuGet bên trên

namespace CinemaManagement.DAL.db
{
    public static class DatabaseConfig
    {
        private static IConfiguration _config;
        private static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                    _connectionString = LoadConnectionString();
                return _connectionString;
            }
        }

        public static string CinemaName
            => GetAppSetting("CinemaName") ?? "NEON-FLORA Cinema";

        public static int SeatLockTimeoutMinutes
            => int.TryParse(GetAppSetting("SeatLockTimeoutMinutes"), out int val) ? val : 10;

        private static string LoadConnectionString()
        {
            try
            {
                string connStr = GetConfig().GetConnectionString("CinemaConnection");
                if (string.IsNullOrWhiteSpace(connStr))
                    throw new Exception(
                        "Không tìm thấy 'CinemaConnection' trong appsettings.json!\n" +
                        "Set 'Copy to Output Directory' = 'Copy always'.");
                return connStr;
            }
            catch (FileNotFoundException)
            {
                throw new Exception(
                    "Không tìm thấy file appsettings.json!\n" +
                    "Đảm bảo file nằm cùng thư mục với .exe.");
            }
        }

        private static IConfiguration GetConfig()
        {
            if (_config != null) return _config;
            _config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();
            return _config;
        }

        private static string GetAppSetting(string key)
        {
            try { return GetConfig()[$"AppSettings:{key}"]; }
            catch { return null; }
        }
    }
}