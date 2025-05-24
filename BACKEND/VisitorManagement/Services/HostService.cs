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
            {
                throw new CustomException(404, "Host not found");
            }

            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync();

            return "Host deleted successfully";
        }

        public async Task<Hosts> GetHost(int id)
        {
            var host = await _context.Hosts.FindAsync(id);

            if (host == null)
            {
                throw new CustomException(404,"Host not found");
            }

            return host;
        }

        public async Task<IEnumerable<Hosts>> GetHosts()
        {
            return await _context.Hosts.ToListAsync();
        }

        public async Task<string> PutHost(int id, Hosts host)
        {
            if (id != host.HostId)
            {
                throw new CustomException(404, "Host not found");
            }

            _context.Entry(host).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostExists(id))
                {
                    throw new CustomException(404, "Host not found");
                }
                else
                {
                    throw;
                }
            }

            return "Host modified successfully";
        }

        private bool HostExists(int id)
        {
            return _context.Hosts.Any(e => e.HostId == id);
        }

       
    }
}
