using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Models;
using VisitorManagement.Interfaces;

namespace VisitorManagement.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly VisitorManagementContext _context;

        private readonly IVisitService _visitService;

        public VisitorService(VisitorManagementContext context, IVisitService visitService)
        {
            _context = context;
            _visitService = visitService;
        }

        

        public async Task<IEnumerable<VisitorReadDto>> GetVisitors()
        {
            var visitors = await _context.Visitors
                .Include(v => v.Visits)
                    .ThenInclude(v => v.Host)
                        .ThenInclude(h => h.Department)
                .Include(v => v.Companions)
                .ToListAsync();

            return visitors.Select(v => new VisitorReadDto
            {
                VisitorId = v.VisitorId,
                FullName = v.FullName,
                PhoneNumber = v.PhoneNumber,
                Email = v.Email,
                Address = v.Address,
                CompanyName = v.CompanyName,
                Purpose = v.Purpose,
                Comment = v.Comment,
                IdProofType = v.IdProofType,
                IdProofNumber = v.IdProofNumber,
                LicensePlateNumber = v.LicensePlateNumber,
                PassCode = v.PassCode,
                QrCodeData = v.QrCodeData,
                PhotoUrl = v.PhotoUrl,
                IsPreRegistered = v.IsPreRegistered,
                ExpectedVisitDateTime = v.ExpectedVisitDateTime,
                CreatedAt = v.CreatedAt,
                Companions = v.Companions.Select(c => new CompanionReadDto
                {
                    CompanionId = c.CompanionId,
                    FullName = c.FullName,
                    ContactNumber = c.ContactNumber,
                    Email = c.Email
                }).ToList(),

                Visits = v.Visits.Select(d => new VisitReadDto
                {
                    VisitId = d.VisitId,
                    VisitorName = d.Visitor.FullName,
                    HostName = d.Host.FullName,
                    Department = d.Host.Department.DepartmentName,
                    VisitStatus=d.VisitStatus,
                    CheckInTime = d.CheckInTime,
                    ApprovalComment = d.ApprovalComment,
                    GatePassNumber = d.GatePassNumber,
                    IsApproved = d.IsApproved,
                    CheckOutTime = d.CheckOutTime
                }).ToList()

            });
        }

        public async Task<VisitorReadDto> GetVisitor(int id)
        {
            var visitor = await _context.Visitors
       .Include(v => v.Visits)
           .ThenInclude(v => v.Host)
               .ThenInclude(h => h.Department)
        .Include(v => v.Companions)
       .FirstOrDefaultAsync(v => v.VisitorId == id);

            if (visitor == null)
                return null;

            Console.WriteLine($"Visitor found: {visitor.FullName}, Visits count: {visitor.Visits?.Count}");

            return new VisitorReadDto
            {
                VisitorId = visitor.VisitorId,
                FullName = visitor.FullName,
                PhoneNumber = visitor.PhoneNumber,
                Email = visitor.Email,
                Address = visitor.Address,
                CompanyName = visitor.CompanyName,
                Purpose = visitor.Purpose,
                Comment = visitor.Comment,
                IdProofType = visitor.IdProofType,
                IdProofNumber = visitor.IdProofNumber,
                LicensePlateNumber = visitor.LicensePlateNumber,
                PassCode = visitor.PassCode,
                QrCodeData = visitor.QrCodeData,
                PhotoUrl = visitor.PhotoUrl,
                IsPreRegistered = visitor.IsPreRegistered,
                ExpectedVisitDateTime = visitor.ExpectedVisitDateTime,
                CreatedAt = visitor.CreatedAt,
                Companions = visitor.Companions.Select(c => new CompanionReadDto
                {
                    CompanionId = c.CompanionId,
                    FullName = c.FullName,
                    ContactNumber = c.ContactNumber,
                    Email = c.Email
                }).ToList(),

                Visits = visitor.Visits.Select(d => new VisitReadDto
                {
                    VisitId =d.VisitId,
                    VisitorName =d.Visitor.FullName,
                    HostName = d.Host.FullName,
                    Department = d.Host.Department.DepartmentName,
                    VisitStatus=d.VisitStatus,
                    CheckInTime = d.CheckInTime,
                    ApprovalComment =d.ApprovalComment,
                    GatePassNumber=d.GatePassNumber,
                    IsApproved=d.IsApproved,
                    CheckOutTime=d.CheckOutTime

                }).ToList()

            };
        }


        public async Task<VisitorReadDto> PostVisitor(VisitorCreateDto dto)
        {
            if (_context.Visitors.Any(v => v.Email == dto.Email))
                throw new Exception("Visitor already exists");

            var visitor = new Visitor
            {
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = dto.Email,
                Address = dto.Address,
                CompanyName = dto.CompanyName,
                Purpose = dto.Purpose,
                Comment = dto.Comment,
                IdProofType = dto.IdProofType,
                IdProofNumber = dto.IdProofNumber,
                LicensePlateNumber = dto.LicensePlateNumber,
                PhotoUrl = dto.PhotoUrl,
                QrCodeData = dto.QrCodeData,
                PassCode = dto.PassCode,
                IsPreRegistered = dto.IsPreRegistered,
                ExpectedVisitDateTime = dto.ExpectedVisitDateTime,
                CreatedAt = DateTime.UtcNow
            };

            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();

            // Create Companion records if provided
            if (dto.Companions != null && dto.Companions.Any())
            {
                foreach (var companionDto in dto.Companions)
                {
                    _context.Companions.Add(new Companion
                    {
                        FullName = companionDto.FullName,
                        ContactNumber = companionDto.ContactNumber,
                        Email = companionDto.Email,
                        VisitorId = visitor.VisitorId
                    });
                }
                await _context.SaveChangesAsync();
            }


            // Create Visit record
            var visit = new Visit
            {
                VisitorId = visitor.VisitorId,
                HostId = dto.HostId,
                VisitStatus = "Inside",
                IsApproved=true,
                ApprovalComment="Approved",
                CheckInTime = DateTime.Now
            };
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            return await GetVisitor(visitor.VisitorId);
        }

        public async Task<bool> PutVisitor(int id, VisitorUpdateDto dto)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Companions)
                .Include(v => v.Visits)
                .FirstOrDefaultAsync(v => v.VisitorId == id);

            if (visitor == null)
                throw new CustomException(404, "Visitor not found");

            // Update visitor fields
            visitor.FullName = dto.FullName ?? visitor.FullName;
            visitor.PhoneNumber = dto.PhoneNumber ?? visitor.PhoneNumber;
            visitor.Email = dto.Email ?? visitor.Email;
            visitor.Address = dto.Address ?? visitor.Address;
            visitor.CompanyName = dto.CompanyName ?? visitor.CompanyName;
            visitor.Purpose = dto.Purpose ?? visitor.Purpose;
            visitor.Comment = dto.Comment ?? visitor.Comment;
            visitor.IdProofType = dto.IdProofType ?? visitor.IdProofType;
            visitor.IdProofNumber = dto.IdProofNumber ?? visitor.IdProofNumber;
            visitor.LicensePlateNumber = dto.LicensePlateNumber ?? visitor.LicensePlateNumber;
            visitor.PhotoUrl = dto.PhotoUrl ?? visitor.PhotoUrl;
            visitor.QrCodeData = dto.QrCodeData ?? visitor.QrCodeData;
            visitor.PassCode = dto.PassCode ?? visitor.PassCode;
            visitor.IsPreRegistered = dto.IsPreRegistered ?? visitor.IsPreRegistered;
            visitor.ExpectedVisitDateTime = dto.ExpectedVisitDateTime ?? visitor.ExpectedVisitDateTime;

            // Update companions
            if (dto.Companions != null)
            {
                _context.Companions.RemoveRange(visitor.Companions);

                foreach (var companionDto in dto.Companions)
                {
                    _context.Companions.Add(new Companion
                    {
                        FullName = companionDto.FullName,
                        ContactNumber = companionDto.ContactNumber,
                        Email = companionDto.Email,
                        VisitorId = visitor.VisitorId
                    });
                }
            }

            // Update visits
            if (dto.Visits != null)
            {
                try
                {
                    foreach (var visitDto in dto.Visits)
                    {
                        var success = await _visitService.UpdateVisitAsync(visitDto.visitId, visitDto);
                        if (!success)
                            throw new CustomException(400, $"Visit update failed for Visit ID {visitDto.visitId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Visit update failed: " + ex.Message);
                    throw new Exception("Visit update failed" +ex.Message);
                }

            }

            // Save changes to Visitor and companions
            await _context.SaveChangesAsync();

            return true;
        }


        public async Task<string> DeleteVisitor(int id)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Companions)
                .FirstOrDefaultAsync(v => v.VisitorId == id);

            if (visitor == null)
                throw new CustomException(404, "Visitor not found");

            var visits = _context.Visits.Where(v => v.VisitorId == id).ToList();

            if (visits.Any(v => v.VisitStatus == "Inside"))
                throw new Exception("Visitor hasn't checked out yet.");

            // Remove companions
            _context.Companions.RemoveRange(visitor.Companions);

            // Remove visits and visitor
            _context.Visits.RemoveRange(visits);
            _context.Visitors.Remove(visitor);

            await _context.SaveChangesAsync();
            return "Visitor deleted successfully";
        }

    }

}
