using Microsoft.AspNetCore.Mvc;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;

namespace VisitorManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VisitorController : ControllerBase
    {
        private readonly IVisitorService _visitorService;

        public VisitorController(IVisitorService visitorService)
        {
            _visitorService = visitorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitorReadDto>>> GetVisitors()
        {
            var visitors = await _visitorService.GetVisitors();
            return Ok(visitors);
        }

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
        }

        [HttpPost]
        public async Task<ActionResult<VisitorReadDto>> PostVisitor(VisitorCreateDto dto)
        {
            try
            {
                var result = await _visitorService.PostVisitor(dto);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutVisitor(int id, VisitorUpdateDto dto)
        {
            try
            {
                var result = await _visitorService.PutVisitor(id, dto);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisitor(int id)
        {
            try
            {
                var result = await _visitorService.DeleteVisitor(id);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }
    }

}
