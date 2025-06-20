using Microsoft.AspNetCore.Mvc;
using VisitorManagement.DTO.Appointments;
using VisitorManagement.Interfaces;

namespace VisitorManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDto dto)
        {
            var result = await _appointmentService.CreateAppointment(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var results = await _appointmentService.GetAllAppointments();
            return Ok(results);
        }

        [HttpGet("host/{hostId}")]
        public async Task<IActionResult> GetAppointmentsByHostId(int hostId)
        {
            var results = await _appointmentService.GetAppointmentsByHostId(hostId);
            return Ok(results);
        }

        [HttpPut("{visitId}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(int visitId, [FromBody] AppointmentUpdateDto dto)
        {
            var success = await _appointmentService.UpdateAppointment(visitId, dto);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
