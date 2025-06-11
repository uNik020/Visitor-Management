using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public class DesignationService : IDesignationService
    {
        private readonly VisitorManagementContext _context;

        public DesignationService(VisitorManagementContext context)
        {
            _context = context;
        }

        public async Task<List<DesignationDto>> GetAllDesignationsAsync()
        {
            return await _context.Designation
                .Select(d => new DesignationDto
                {
                    DesignationId = d.DesignationId,
                    DesignationName = d.DesignationName
                }).ToListAsync();
        }

        public async Task<DesignationDto?> GetDesignationByIdAsync(int id)
        {
            var desig = await _context.Designation.FindAsync(id);
            if (desig == null) return null;

            return new DesignationDto
            {
                DesignationId = desig.DesignationId,
                DesignationName = desig.DesignationName
            };
        }

        public async Task<string> CreateDesignationAsync(DesignationCreateDto DesignationDto)
        {
            var desig = await _context.Designation
     .SingleOrDefaultAsync(d => d.DesignationName.ToLower() == DesignationDto.DesignationName.ToLower());

            if(desig is not null)
            {
                throw new CustomException(400, "Designation already exists");
            }

            var Designation = new Designation
            {
                DesignationName = DesignationDto.DesignationName
            };

            _context.Designation.Add(Designation);
            await _context.SaveChangesAsync();
            return "Designation created successfully";
        }

        public async Task<string> UpdateDesignationAsync(int id, DesignationCreateDto DesignationDto)
        {
            var Designation = await _context.Designation.FindAsync(id);
            if (Designation == null)
                throw new CustomException(404, "Designation not found");

            Designation.DesignationName = DesignationDto.DesignationName;
            _context.Designation.Update(Designation);
            await _context.SaveChangesAsync();
            return "Designation updated successfully";
        }

        public async Task<bool> DeleteDesignationAsync(int id)
        {
            var hosts = _context.Hosts.Any(h => h.DesignationId == id);
            if (hosts)
            {
                throw new CustomException(400,"DesignationId is in use by Host !");
            }
            var Designation = await _context.Designation.FindAsync(id);
            if (Designation == null) return false;

            _context.Designation.Remove(Designation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}