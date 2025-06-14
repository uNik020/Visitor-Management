using VisitorManagement.DTO;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public interface IHostService
    {
        Task<IEnumerable<HostReadDto>> GetHosts();
        Task<HostReadDto> GetHost(int id);
        Task<string> PutHost(int id, HostUpdateDto hostDto);
        Task<string> DeleteHost(int id);
        Task<HostReadDto> PostHost(HostCreateDto hostDto);
    }
}
