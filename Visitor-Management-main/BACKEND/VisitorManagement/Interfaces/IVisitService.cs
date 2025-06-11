using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitService
    {
        Task<List<VisitDto>> GetAllVisitsAsync();
        Task<VisitReadDto> GetVisitByIdAsync(int id);
        Task<bool> CreateVisitAsync(VisitCreateDto visitDto); // updated
        Task<bool> UpdateVisitAsync(VisitUpdateDto visitDto, int id);
        Task<bool> DeleteVisitAsync(int id);
    }
}
