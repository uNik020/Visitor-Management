namespace VisitorManagement.DTO.Appointments
{
    public class AppointmentReadDto
    {
        public int VisitId { get; set; }
        public string VisitorName { get; set; }
        public string HostName { get; set; }
        public DateTime? ScheduledVisitDateTime { get; set; }
        public string? VisitPurpose { get; set; }
        public string? VisitStatus { get; set; }
        public bool IsApproved { get; set; }
        public string? ApprovalComment { get; set; }
    }
}
