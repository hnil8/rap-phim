using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaManagement.BLL.Common
{
    public static class PasswordHelper
    {
        // Hàm dùng khi Đăng ký (Tạo Hash)
        public static string HashPassword(string plainTextPassword)
        {
            // WorkFactor 11 là mức độ cân bằng hoàn hảo giữa bảo mật và hiệu suất
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword, workFactor: 11);
        }

        // Hàm dùng khi Đăng nhập (Kiểm tra đối chiếu)
        public static bool VerifyPassword(string plainTextPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);
        }
    }
}