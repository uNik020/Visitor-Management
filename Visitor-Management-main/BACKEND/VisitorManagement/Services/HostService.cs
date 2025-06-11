using Microsoft.EntityFrameworkCore;
using VisitorManagement.Interfaces;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public class HostService : IHostService
    {
        private readonly VisitorManagementContext _context;

        public HostService(VisitorManagementContext context)
        {
            _context = context;
        }

        public async Task<string> DeleteHost(int id)
        {
            var host = await _context.Hosts.FindAsync(id);
            if (host == null)
                throw new CustomException(404, "Host not found");

            try
            {
                _context.Hosts.Remove(host);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new CustomException(400, "Cannot delete host because it is referenced in other records.");
            }

            return "Host deleted successfully";
        }

        public async Task<Hosts> GetHost(int id)
        {
            var host = await _context.Hosts
                .Include(h => h.Department)
                .Include(h => h.Designation)
                .FirstOrDefaultAsync(h => h.HostId == id);

            if (host == null)
                throw new CustomException(404, "Host not found");

            return host;
        }

        public async Task<IEnumerable<Hosts>> GetHosts()
        {
            return await _context.Hosts
                .Include(h => h.Department)
                .Include(h => h.Designation)
                .ToListAsync();
        }

        public async Task<string> PutHost(int id, HostCreateDto hostDto)
        {
            var host = await _context.Hosts.FindAsync(id);
            if (host == null)
                throw new CustomException(404, "Host not found");

            host.FullName = hostDto.FullName;
            host.Email = hostDto.Email;
            host.PhoneNumber = hostDto.PhoneNumber;
            host.DepartmentId = hostDto.DepartmentId;
            host.DesignationId = hostDto.DesignationId;
            host.ProfilePictureUrl = hostDto.ProfilePictureUrl;
            host.About = hostDto.About;

            _context.Hosts.Update(host);
            await _context.SaveChangesAsync();

            return "Host modified successfully";
        }

        public async Task<Hosts> PostHost(HostCreateDto hostDto)
        {
            var host = new Hosts
            {
                FullName = hostDto.FullName,
                Email = hostDto.Email,
                PhoneNumber = hostDto.PhoneNumber,
                DepartmentId = hostDto.DepartmentId,
                DesignationId = hostDto.DesignationId,
                ProfilePictureUrl = hostDto.ProfilePictureUrl,
                About = hostDto.About
            };

            _context.Hosts.Add(host);
            await _context.SaveChangesAsync();

            return host;
        }
    }

}
