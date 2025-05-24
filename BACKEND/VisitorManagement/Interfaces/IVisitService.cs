using VisitorManagement.Models;
using VisitorManagement.DTO;

namespace VisitorManagement.Services
{
    public interface IVisitService
    {
        Task<List<VisitDto>> GetAllVisitsAsync();
        Task<VisitDto> GetVisitByIdAsync(int id);
        Task<bool> CreateVisitAsync(Visit visit);
        Task<bool> UpdateVisitAsync(Visit visit);
        Task<bool> DeleteVisitAsync(int id);
    }
}
