namespace VisitorManagement.DTO.Appointments
{
    public class AppointmentCreateDto
    {
        public int VisitorId { get; set; }
        public int HostId { get; set; }
        public DateTime ScheduledVisitDateTime { get; set; }
        public string? VisitPurpose { get; set; }
    }
}
