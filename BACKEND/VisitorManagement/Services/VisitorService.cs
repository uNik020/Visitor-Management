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

        public VisitorService(VisitorManagementContext context)
        {
            _context = context;
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

        public async Task<Visitor> GetVisitor(int id)
        {
            var visitor = await _context.Visitors.FindAsync(id);

            if (visitor == null)
            {
                throw new CustomException(404,"visitor not found");
            }

            return visitor;
        }

        public async Task<IEnumerable<Visitor>> GetVisitors()
        {
            return await _context.Visitors.Include(v => v.Visits).ToListAsync();
        }

        public async Task<string> PutVisitor(int id, VisitorCreateDto visitorDto)
        {
            var visitor = await _context.Visitors.FindAsync(id);
            if (visitor == null)
            {
                throw new CustomException(404, "Visitor not found");
            }

            visitor.FullName = visitorDto.FullName;
            visitor.PhoneNumber = visitorDto.PhoneNumber;
            visitor.Email = visitorDto.Email;
            visitor.Purpose = visitorDto.Purpose;

            _context.Visitors.Update(visitor);
            await _context.SaveChangesAsync();

            return "Visitor modified successfully";
        }


        public async Task<VisitorCreateDto> PostVisitor(VisitorCreateDto visitorDto)
        {
            var isVisitorPresent = _context.Visitors.Any(v => v.Email == visitorDto.Email);
            if (isVisitorPresent)
            {
                throw new Exception("Visitor already there in system");
            }
            var visitor = new Visitor
            {
                FullName = visitorDto.FullName,
                PhoneNumber = visitorDto.PhoneNumber,
                Email = visitorDto.Email,
                //PhotoPath = visitorDto.PhotoPath,
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
            return visitorDto;
        }

    }
}
