using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myHosts = VisitorManagement.Models.Hosts;
using VisitorManagement.Models;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly VisitorManagementContext _context;

        public HostController(VisitorManagementContext context)
        {
            _context = context;
        }

        // GET: api/Host
        [HttpGet]
        public async Task<ActionResult<IEnumerable<myHosts>>> GetHosts()
        {
            return await _context.Hosts.ToListAsync();
        }

        // GET: api/Host/5
        [HttpGet("{id}")]
        public async Task<ActionResult<myHosts>> GetHost(int id)
        {
            var host = await _context.Hosts.FindAsync(id);

            if (host == null)
            {
                return NotFound();
            }

            return host;
        }

        // PUT: api/Host/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHost(int id, myHosts host)
        {
            if (id != host.HostId)
            {
                return BadRequest();
            }

            _context.Entry(host).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Host
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<myHosts>> PostHost(myHosts host)
        {
            _context.Hosts.Add(host);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHost", new { id = host.HostId }, host);
        }

        // DELETE: api/Host/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHost(int id)
        {
            var host = await _context.Hosts.FindAsync(id);
            if (host == null)
            {
                return NotFound();
            }

            _context.Hosts.Remove(host);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HostExists(int id)
        {
            return _context.Hosts.Any(e => e.HostId == id);
        }
    }
}
