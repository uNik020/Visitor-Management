namespace VisitorManagement.DTO.Appointments
{
    public class AppointmentUpdateDto
    {
        public bool IsApproved { get; set; }
        public string? ApprovalComment { get; set; }
        public string? VisitStatus { get; set; }  // Accepted, Rejected, Scheduled, CheckedIn, CheckedOut
    }
}
