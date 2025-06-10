namespace VisitorManagement.DTO
{
    public class VisitReadDto
    {
        public int VisitId { get; set; }

        public string VisitorName { get; set; } = null!;
        public string HostName { get; set; } = null!;
        public string? Department { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public string VisitStatus { get; set; } = "Scheduled";

        public bool IsApproved { get; set; }
        public string? ApprovalComment { get; set; }

        public string? GatePassNumber { get; set; }
        public string? QrCodeData { get; set; }
    }
}
