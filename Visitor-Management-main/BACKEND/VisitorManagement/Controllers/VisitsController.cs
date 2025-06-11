using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        // GET: api/Visits
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitDto>>> GetVisits()
        {
            var visits = await _visitService.GetAllVisitsAsync();
            return Ok(visits);
        }

        // GET: api/Visits/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitReadDto>> GetVisit(int id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null)
            {
                return NotFound();
            }

            return Ok(visit);
        }

        // POST: api/Visits
        [HttpPost]
        public async Task<ActionResult> PostVisit(VisitCreateDto visitDto)
        {
            var success = await _visitService.CreateVisitAsync(visitDto);
            if (!success)
            {
                return BadRequest("Failed to create visit");
            }

            return Ok("Visit created successfully");
        }

        // PUT: api/Visits/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisit(int id, VisitUpdateDto visitDto)
        {
            var success = await _visitService.UpdateVisitAsync(visitDto, id);
            if (!success)
            {
                return NotFound("Visit not found or update failed");
            }

            return Ok("Visit updated successfully");
        }


        // DELETE: api/Visits/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            var success = await _visitService.DeleteVisitAsync(id);
            if (!success)
            {
                return NotFound("Visit not found");
            }

            return Ok("Visit deleted successfully");
        }

    }
}
