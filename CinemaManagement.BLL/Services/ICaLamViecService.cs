using CinemaManagement.BLL.DTOs;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public interface ICaLamViecService
    {
        Task<int?> KiemTraCaDangMoAsync(int nhanVienId);
        Task<ServiceResult> MoCaAsync(int nhanVienId, decimal tienDauCa);
        Task<ServiceResult> ChotCaAsync(int caId, decimal tienMatThucTeTrongKet, string ghiChu);
    }
}