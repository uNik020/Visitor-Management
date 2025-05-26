using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using AutoMapper;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public class VisitorService : IVisitorService
    {
        private readonly VisitorManagementContext _context;
        private readonly IMapper _mapper;

        public VisitorService(VisitorManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> DeleteVisitor(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                throw new CustomException(404, "visitor not found");
            }

            _context.Visitors.Remove(visitor);
            await _context.SaveChangesAsync();

            return "visitor deleted successfully";
        }

        public async Task<IEnumerable<VisitorReadDto>> GetVisitors()
        {
            var visitors = await _context.Visitors
                .Include(v => v.Host) // Ensure Host navigation property is loaded
                .ToListAsync();

            var visitorDtos = visitors.Select(v => new VisitorReadDto
            {
                VisitorId = v.VisitorId,
                FullName = v.FullName,
                PhoneNumber = v.PhoneNumber,
                Email = v.Email,
                Address = v.Address,
                Purpose = v.Purpose,
                Status = v.Status,
                CheckInTime = v.CheckInTime,
                CheckOutTime = v.CheckOutTime,
                HostName = v.Host?.FullName // Assuming Host has FullName property
            });

            return visitorDtos;
        }

        public async Task<VisitorReadDto> GetVisitor(int id)
        {
            var visitor = await _context.Visitors
                .Include(v => v.Host)
                .FirstOrDefaultAsync(v => v.VisitorId == id);

            if (visitor == null)
                throw new CustomException(404, "Visitor not found.");

            return new VisitorReadDto
            {
                VisitorId = visitor.VisitorId,
                FullName = visitor.FullName,
                PhoneNumber = visitor.PhoneNumber,
                Email = visitor.Email,
                Address = visitor.Address,
                Purpose = visitor.Purpose,
                Status = visitor.Status,
                CheckInTime = visitor.CheckInTime,
                CheckOutTime = visitor.CheckOutTime,
                HostName = visitor.Host?.FullName
            };
        }
        public async Task<VisitorReadDto> PutVisitor(int id, VisitorCreateDto visitorDto)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                throw new CustomException(404, "Visitor not found");
            }

            // Update fields
            visitor.FullName = visitorDto.FullName;
            visitor.PhoneNumber = visitorDto.PhoneNumber;
            visitor.Email = visitorDto.Email;
            visitor.Address = visitorDto.Address;
            visitor.Purpose = visitorDto.Purpose;
            visitor.HostId = visitorDto.HostId;
            visitor.Status = visitorDto.Status;

            if (visitorDto.Status == "Checkout")
            {
                visitor.CheckOutTime = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            // Map to VisitorReadDto
            var visitorReadDto = _mapper.Map<VisitorReadDto>(visitor);
            return visitorReadDto;

        }


        public async Task<Visitor> PostVisitor(VisitorCreateDto visitorDto)
        {
            var visitor = new Visitor
            {
                FullName = visitorDto.FullName,
                PhoneNumber = visitorDto.PhoneNumber,
                Email = visitorDto.Email,
                Purpose = visitorDto.Purpose,
                CreatedAt = DateTime.UtcNow
            };

            _context.Visitors.Add(visitor);
            await _context.SaveChangesAsync();
            var createdVisitor = _context.Visitors.SingleOrDefault(v => v.Email == visitorDto.Email);
            var visit = new Visit
            {
                VisitorId = createdVisitor.VisitorId,
                HostId = visitorDto.HostId,
                VisitStatus = "Inside"
            };
            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();
            return visitor;
        }

    }
}
