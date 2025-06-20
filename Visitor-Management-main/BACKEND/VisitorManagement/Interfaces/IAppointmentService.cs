using VisitorManagement.Models;
using VisitorManagement.DTO.Appointments;

namespace VisitorManagement.Interfaces
{
    public interface IAppointmentService
    {
        Task<Visit> CreateAppointment(AppointmentCreateDto dto);
        Task<IEnumerable<AppointmentReadDto>> GetAllAppointments();
        Task<IEnumerable<AppointmentReadDto>> GetAppointmentsByHostId(int hostId);
        Task<bool> UpdateAppointment(int visitId, AppointmentUpdateDto dto);
    }
}
