using CinemaManagement.BLL.DTOs;
using System.Threading.Tasks;

namespace CinemaManagement.BLL.Services
{
    public interface IPhieuNhapService
    {
        Task<ServiceResult> CreatePhieuNhapAsync(CreatePhieuNhapDto dto);
    }
}