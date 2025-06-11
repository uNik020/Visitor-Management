using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitorService
    {
        Task<IEnumerable<VisitorReadDto>> GetVisitors();
        Task<VisitorReadDto> GetVisitor(int id);
        Task<string> PutVisitor(int id, VisitorUpdateDto visitorDto);
        Task<string> DeleteVisitor(int id);
        Task<VisitorReadDto> PostVisitor(VisitorCreateDto visitorDto);
    }
}
