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

        public async Task<HostReadDto> GetHost(int id)
        {
            var host = await _context.Hosts
                .Include(h => h.Department)
                .Include(h => h.Designation)
                .FirstOrDefaultAsync(h => h.HostId == id);

            if (host == null)
                throw new CustomException(404, "Host not found");

            return new HostReadDto
            {
                HostId = host.HostId,
                FullName = host.FullName,
                Email = host.Email,
                PhoneNumber = host.PhoneNumber,
                DepartmentId = host.DepartmentId,
                DepartmentName = host.Department?.DepartmentName,
                DesignationId = host.DesignationId,
                DesignationName = host.Designation?.DesignationName,
                ProfilePictureUrl = host.ProfilePictureUrl,
                About = host.About
            };
        }


        public async Task<IEnumerable<HostReadDto>> GetHosts()
        {
            return await _context.Hosts
                .Include(h => h.Department)
                .Include(h => h.Designation)
                .Select(h => new HostReadDto
                {
                    HostId = h.HostId,
                    FullName = h.FullName,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    DepartmentId = h.DepartmentId,
                    DepartmentName = h.Department != null ? h.Department.DepartmentName: null,
                    DesignationId = h.DesignationId,
                    DesignationName = h.Designation != null ? h.Designation.DesignationName : null,
                    ProfilePictureUrl = h.ProfilePictureUrl,
                    About = h.About,
                })
                .ToListAsync();
        }


        public async Task<string> PutHost(int id, HostUpdateDto hostDto)
        {
            var host = await _context.Hosts.FindAsync(id);
            if (host == null)
                throw new CustomException(404, "Host not found");

            // Only update if not null
            if (hostDto.FullName != null) host.FullName = hostDto.FullName;
            if (hostDto.Email != null) host.Email = hostDto.Email;
            if (hostDto.PhoneNumber != null) host.PhoneNumber = hostDto.PhoneNumber;
            if (hostDto.DepartmentId.HasValue) host.DepartmentId = hostDto.DepartmentId;
            if (hostDto.DesignationId.HasValue) host.DesignationId = hostDto.DesignationId;
            if (hostDto.ProfilePictureUrl != null) host.ProfilePictureUrl = hostDto.ProfilePictureUrl;
            if (hostDto.About != null) host.About = hostDto.About;

            _context.Hosts.Update(host);
            await _context.SaveChangesAsync();

            return "Host modified successfully";
        }

        public async Task<HostReadDto> PostHost(HostCreateDto hostDto)
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

            // Retrieve Department/Designation names
            var department = await _context.Departments.FindAsync(host.DepartmentId);
            var designation = await _context.Designation.FindAsync(host.DesignationId);

            return new HostReadDto
            {
                HostId = host.HostId,
                FullName = host.FullName,
                Email = host.Email,
                PhoneNumber = host.PhoneNumber,
                DepartmentId = host.DepartmentId,
                DepartmentName = department?.DepartmentName,
                DesignationId = host.DesignationId,
                DesignationName = designation?.DesignationName,
                ProfilePictureUrl = host.ProfilePictureUrl,
                About = host.About
            };
        }

    }

}
