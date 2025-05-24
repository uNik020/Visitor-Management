using Microsoft.EntityFrameworkCore;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;
using VisitorManagement.DTO;

namespace VisitorManagement.Services
{
    public class VisitService : IVisitService
    {
        private readonly VisitorManagementContext _context;

        public VisitService(VisitorManagementContext context)
        {
            _context = context;
        }

        public async Task<List<VisitDto>> GetAllVisitsAsync()
        {
            return await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .Select(v => new VisitDto
                {
                    VisitId = v.VisitId,
                    VisitorName = v.Visitor.FullName,
                    HostName = v.Host.FullName,
                    Department = v.Host.Department.DepartmentName ?? "N/A",
                    CheckInTime = v.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = v.CheckOutTime ?? DateTime.MinValue
                }).ToListAsync();
        }

        public async Task<VisitDto> GetVisitByIdAsync(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null) return null;

            return new VisitDto
            {
                VisitId = visit.VisitId,
                VisitorName = visit.Visitor.FullName,
                HostName = visit.Host.FullName,
                Department = visit.Host.Department?.DepartmentName ?? "N/A",
                CheckInTime = visit.CheckInTime ?? DateTime.MinValue,
                CheckOutTime = visit.CheckOutTime ?? DateTime.MinValue
            };
        }

        public async Task<bool> CreateVisitAsync(Visit visit)
        {
            _context.Visits.Add(visit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateVisitAsync(Visit visit)
        {
            _context.Visits.Update(visit);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return false;

            _context.Visits.Remove(visit);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
