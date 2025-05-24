using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Interfaces
{
    public interface IVisitorService
    {
        public Task<IEnumerable<Visitor>> GetVisitors();
        public Task<Visitor> GetVisitor(int id);
        public Task<string> PutVisitor(int id, Visitor visitor);
        public Task<string> DeleteVisitor(int id);

    }
}
