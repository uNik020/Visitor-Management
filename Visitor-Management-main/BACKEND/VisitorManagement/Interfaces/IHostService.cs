using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public interface IHostService
    {
        Task<IEnumerable<Hosts>> GetHosts();
        Task<Hosts> GetHost(int id);
        Task<string> PutHost(int id, HostCreateDto hostDto);
        Task<string> DeleteHost(int id);
        Task<Hosts> PostHost(HostCreateDto hostDto);
    }
}
