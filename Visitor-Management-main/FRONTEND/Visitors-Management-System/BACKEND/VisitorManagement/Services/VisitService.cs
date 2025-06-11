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
                .ThenInclude(v => v.Department)
                .Select(v => new VisitDto
                {
                    VisitId = v.VisitId,
                    VisitorName = v.Visitor.FullName,
                    HostName = v.Host.FullName,
                    VisitStatus = v.VisitStatus,
                    Department = v.Host.Department.DepartmentName,
                    CheckInTime = v.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = v.CheckOutTime ?? DateTime.MinValue
                }).ToListAsync();
        }

        public async Task<VisitDto> GetVisitByIdAsync(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .ThenInclude(v => v.Department)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null) return null;

            return new VisitDto
            {
                VisitId = visit.VisitId,
                VisitorName = visit.Visitor.FullName,
                HostName = visit.Host.FullName,
                VisitStatus = visit.VisitStatus,
                Department = visit.Host.Department.DepartmentName,
                CheckInTime = visit.CheckInTime ?? DateTime.MinValue,
                CheckOutTime = visit.CheckOutTime ?? DateTime.MinValue
            };
        }

        public async Task<bool> CreateVisitAsync(VisitCreateDto visitDto)
        {
            var visit = new Visit
            {
                VisitorId = visitDto.VisitorId,
                HostId = visitDto.HostId,
                CheckInTime = visitDto.CheckInTime ?? DateTime.UtcNow,
                CheckOutTime = visitDto.CheckOutTime,
                VisitStatus = visitDto.VisitStatus
            };

            _context.Visits.Add(visit);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateVisitAsync(VisitCreateDto visitDto)
        {
            var visit = await _context.Visits.SingleOrDefaultAsync(v => v.VisitId == visitDto.VisitId);

            visit.VisitId = visitDto.VisitId;
            visit.VisitorId = visitDto.VisitorId;
            visit.HostId = visitDto.HostId;
            visit.VisitStatus = visitDto.VisitStatus;
                
           
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
