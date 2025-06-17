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

        public async Task<List<VisitReadDto>> GetAllVisitsAsync()
        {
            return await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .ThenInclude(v => v.Department)
                .Select(v => new VisitReadDto
                {
                    VisitId = v.VisitId,
                    VisitorName = v.Visitor.FullName,
                    HostName = v.Host.FullName,
                    VisitStatus = v.VisitStatus,
                    Department = v.Host.Department.DepartmentName,
                    CheckInTime = v.CheckInTime ?? DateTime.MinValue,
                    CheckOutTime = v.CheckOutTime
                }).ToListAsync();
        }

        public async Task<VisitReadDto> GetVisitByIdAsync(int id)
        {
            var visit = await _context.Visits
                .Include(v => v.Visitor)
                .Include(v => v.Host)
                .ThenInclude(v => v.Department)
                .FirstOrDefaultAsync(v => v.VisitId == id);

            if (visit == null) return null;

            return new VisitReadDto
            {
                VisitId = visit.VisitId,
                VisitorName = visit.Visitor.FullName,
                HostName = visit.Host.FullName,
                VisitStatus = visit.VisitStatus,
                QrCodeData = visit.QrCodeData,
                Department = visit.Host.Department.DepartmentName,
                CheckInTime = visit.CheckInTime ?? DateTime.MinValue,
                CheckOutTime = visit.CheckOutTime
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
                VisitStatus = visitDto.VisitStatus,
                IsApproved = visitDto.IsApproved,
                ApprovalComment = visitDto.ApprovalComment,
                GatePassNumber = visitDto.GatePassNumber,
                QrCodeData = visitDto.QrCodeData
            };

            _context.Visits.Add(visit);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> UpdateVisitAsync(int visitId, VisitUpdateDto dto)
        {
            var visit = await _context.Visits.FindAsync(visitId);
            if (visit == null)
            {
                return false;
            }

            // Update fields
            Console.WriteLine($"Updating VisitStatus to: {dto.VisitStatus}");
            visit.VisitStatus = dto.VisitStatus;
            visit.CheckInTime = dto.CheckInTime;
            visit.CheckOutTime = dto.CheckOutTime;
            //visit.IsApproved = dto.IsApproved;
            visit.ApprovalComment = dto.ApprovalComment;
            visit.GatePassNumber = dto.GatePassNumber;
            visit.QrCodeData = dto.QrCodeData;

            return await _context.SaveChangesAsync() > 0;
            
        }


        public async Task<bool> DeleteVisitAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);
            if (visit == null) return false;
            if (visit.VisitStatus == "Inside")
                throw new Exception("Cannot delete a visit while visitor is still inside.");

            _context.Visits.Remove(visit);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
