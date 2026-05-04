---
trigger: always_on
---

- CẬP NHẬT ENTITIES :   dotnet ef dbcontext scaffold "Server=.;Database=RapphimDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --project CinemaManagement.DAL --startup-project CinemaManagement.UI --output-dir Entities --context-dir Context --context CinemaDbContext --no-onconfiguring --force
- Dùng trong DAL — tất cả các file DAL khác chỉ cần gọi 1 dòng:
csharpusing (var conn = new SqlConnection(DatabaseConfig.ConnectionString))
- 
appsettings.json
      ↓
DatabaseConfig.ConnectionString
      ↓
Program.cs → services.AddDbContext<CinemaDbContext>(
                 options => options.UseSqlServer(DatabaseConfig.ConnectionString))
      ↓
CinemaDbContext (inject vào TaiKhoanDAL)
      ↓
TaiKhoanDAL (inject vào TaiKhoanBLL)
      ↓
TaiKhoanBLL (inject vào frmLogin)