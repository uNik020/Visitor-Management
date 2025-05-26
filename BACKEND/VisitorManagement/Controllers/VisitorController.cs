using Microsoft.AspNetCore.Mvc;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        // GET: api/Visitor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorReadDto>>> GetVisitors()
        {
            try
            {
                var visitors = await _visitorService.GetVisitors();
                return Ok(visitors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


        // GET: api/Visitor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VisitorReadDto>> GetVisitor(int id)
        {
            try
            {
                var visitor = await _visitorService.GetVisitor(id);
                return Ok(visitor);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


        // POST: api/Visitor
        [HttpPost]
        public async Task<ActionResult<VisitorReadDto>> PostVisitor(VisitorCreateDto visitorDto)
        {
            try
            {
                var createdVisitor = await _visitorService.PostVisitor(visitorDto);

                return CreatedAtAction(nameof(GetVisitor), new { id = createdVisitor.VisitorId }, createdVisitor);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


        // PUT: api/Visitor/5
        [HttpPut("{id}")]
        public async Task<ActionResult<VisitorReadDto>> PutVisitor(int id, VisitorCreateDto visitorDto)
        {
            try
            {
                var updatedVisitor = await _visitorService.PutVisitor(id, visitorDto);
                return Ok(updatedVisitor);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


        // DELETE: api/Visitor/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VisitorReadDto>> DeleteVisitor(int id)
        {
            try
            {
                var deletedVisitor = await _visitorService.DeleteVisitor(id);
                return Ok(deletedVisitor);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


    }
}
