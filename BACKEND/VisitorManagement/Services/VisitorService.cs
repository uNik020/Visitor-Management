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
            return await _context.Visitors.ToListAsync();
        }

        public async Task<string> PutVisitor(int id, Visitor visitor)
        {
            if (id != visitor.VisitorId)
            {
                throw new CustomException(404, "Visitor not found");
            }

            _context.Entry(visitor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VisitorExists(id))
                {
                    throw new CustomException(404, "visitor not found");
                }
                else
                {
                    throw;
                }
            }

            return "Visitor modified successfully";
        }

        private bool VisitorExists(int id)
        {
            return _context.Visitors.Any(e => e.VisitorId == id);
        }

       
    }
}
