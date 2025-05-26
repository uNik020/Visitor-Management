using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitorService
    {
        public Task<IEnumerable<VisitorReadDto>> GetVisitors();
        public Task<VisitorReadDto> GetVisitor(int id);
        public Task<VisitorReadDto> PutVisitor(int id, VisitorCreateDto visitorDto);
        public Task<string> DeleteVisitor(int id);

        Task<Visitor> PostVisitor(VisitorCreateDto visitorDto);

    }
}
