using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public interface IHostService
    {
        public Task<IEnumerable<Hosts>> GetHosts();
        public Task<Hosts> GetHost(int id);
        public Task<string> PutHost(int id, Hosts host);
        public Task<string> DeleteHost(int id);
        Task<Hosts> PostHost(HostCreateDto hostDto);

    }
}
