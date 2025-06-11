using Microsoft.AspNetCore.Mvc;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService DesignationService)
        {
            _designationService = DesignationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _designationService.GetAllDesignationsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Designation = await _designationService.GetDesignationByIdAsync(id);
            if (Designation == null) return NotFound("Designation not found");
            return Ok(Designation);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DesignationCreateDto DesignationDto)
        {
            try { 
            var result = await _designationService.CreateDesignationAsync(DesignationDto);
            return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DesignationCreateDto DesignationDto)
        {
            try
            {
                var result = await _designationService.UpdateDesignationAsync(id, DesignationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
            var success = await _designationService.DeleteDesignationAsync(id);
            if (!success) return NotFound("Designation not found");
            return Ok("Designation deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
