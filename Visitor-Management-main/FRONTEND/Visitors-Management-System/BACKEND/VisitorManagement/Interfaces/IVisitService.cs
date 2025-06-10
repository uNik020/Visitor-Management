using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitService
    {
        Task<List<VisitDto>> GetAllVisitsAsync();
        Task<VisitDto> GetVisitByIdAsync(int id);
        Task<bool> CreateVisitAsync(VisitCreateDto visitDto); // updated
        Task<bool> UpdateVisitAsync(VisitCreateDto visitDto);
        Task<bool> DeleteVisitAsync(int id);
    }
}
