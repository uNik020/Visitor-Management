using VisitorManagement.DTO.Appointments;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace VisitorManagement.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly VisitorManagementContext _context;

        public AppointmentService(VisitorManagementContext context)
        {
            _context = context;
        }

        public async Task<Visit> CreateAppointment(AppointmentCreateDto dto)
        {
            var visit = new Visit
            {
                VisitorId = dto.VisitorId,
                HostId = dto.HostId,
                ScheduledVisitDateTime = dto.ScheduledVisitDateTime,
                VisitPurpose = dto.VisitPurpose,
                VisitStatus = "Pending",
                IsApproved = false
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            return visit;
        }

        public async Task<IEnumerable<AppointmentReadDto>> GetAllAppointments()
        {
            return await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .Select(v => new AppointmentReadDto
                {
                    VisitId = v.VisitId,
                    VisitorName = v.Visitor.FullName,
                    HostName = v.Host.FullName,
                    ScheduledVisitDateTime = v.ScheduledVisitDateTime,
                    VisitPurpose = v.VisitPurpose,
                    VisitStatus = v.VisitStatus,
                    IsApproved = v.IsApproved,
                    ApprovalComment = v.ApprovalComment
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentReadDto>> GetAppointmentsByHostId(int hostId)
        {
            return await _context.Visits
                .Where(v => v.HostId == hostId)
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .Select(v => new AppointmentReadDto
                {
                    VisitId = v.VisitId,
                    VisitorName = v.Visitor.FullName,
                    HostName = v.Host.FullName,
                    ScheduledVisitDateTime = v.ScheduledVisitDateTime,
                    VisitPurpose = v.VisitPurpose,
                    VisitStatus = v.VisitStatus,
                    IsApproved = v.IsApproved,
                    ApprovalComment = v.ApprovalComment
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateAppointment(int visitId, AppointmentUpdateDto dto)
        {
            var visit = await _context.Visits.FindAsync(visitId);
            if (visit == null) return false;

            visit.IsApproved = dto.IsApproved;
            visit.ApprovalComment = dto.ApprovalComment;
            visit.VisitStatus = dto.VisitStatus;

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
