using Microsoft.AspNetCore.Mvc;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllDepartmentsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var department = await _departmentService.GetDepartmentByIdAsync(id);
            if (department == null) return NotFound("Department not found");
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateDto departmentDto)
        {
            var result = await _departmentService.CreateDepartmentAsync(departmentDto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, DepartmentCreateDto departmentDto)
        {
            try
            {
                var result = await _departmentService.UpdateDepartmentAsync(id, departmentDto);
                return Ok(result);
            }
            catch (CustomException ex)
            {
                return StatusCode(ex.StatusCode, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _departmentService.DeleteDepartmentAsync(id);
            if (!success) return NotFound("Department not found");
            return Ok("Department deleted successfully");
        }
    }
}
