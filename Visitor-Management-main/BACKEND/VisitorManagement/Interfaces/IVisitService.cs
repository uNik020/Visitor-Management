using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitService
    {
        Task<List<VisitReadDto>> GetAllVisitsAsync();
        Task<VisitReadDto> GetVisitByIdAsync(int id);
        Task<bool> CreateVisitAsync(VisitCreateDto visitDto);
        Task<bool> UpdateVisitAsync(int visitId, VisitUpdateDto dto);
        Task<bool> DeleteVisitAsync(int id);
    }
}
