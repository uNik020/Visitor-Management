using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Models;
using VisitorManagement.Services;
using myHosts = VisitorManagement.Models.Hosts;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : ControllerBase
    {
        private readonly IHostService _hostService;

        public HostController(IHostService hostService)
        {
            _hostService = hostService;
        }

        // GET: api/Host
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HostReadDto>>> GetHosts()
        {
            try
            {
                var hosts = await _hostService.GetHosts();
                var hostDtos = hosts.Select(h => new HostReadDto
                {
                    HostId = h.HostId,
                    FullName = h.FullName,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    DepartmentId = h.DepartmentId,
                    DepartmentName = h.Department?.DepartmentName,
                    DesignationId = h.DesignationId,
                    DesignationName = h.Designation?.DesignationName,
                    ProfilePictureUrl = h.ProfilePictureUrl,
                    About = h.About
                });

                return Ok(hostDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HostReadDto>> GetHost(int id)
        {
            try
            {
                var h = await _hostService.GetHost(id);

                var dto = new HostReadDto
                {
                    HostId = h.HostId,
                    FullName = h.FullName,
                    Email = h.Email,
                    PhoneNumber = h.PhoneNumber,
                    DepartmentId = h.DepartmentId,
                    DepartmentName = h.Department?.DepartmentName,
                    DesignationId = h.DesignationId,
                    DesignationName = h.Designation?.DesignationName,
                    ProfilePictureUrl = h.ProfilePictureUrl,
                    About = h.About
                };

                return Ok(dto);
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


        // PUT: api/Host/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHost(int id, HostCreateDto hostDto)
        {
            try
            {
                var res = await _hostService.PutHost(id, hostDto);
                return Ok(res);
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


        // POST: api/Host
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<myHosts>> PostHost(HostCreateDto hostDto)
        {
            try
            {
                var createdUser = await _hostService.PostHost(hostDto);
                return Ok(createdUser);
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message;

                if (message?.Contains("UQ_Users_Email") == true)
                    return BadRequest("Email already exists.");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE: api/Host/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHost(int id)
        {
            try
            {
                var res = await _hostService.DeleteHost(id);
                return Ok(res);
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
