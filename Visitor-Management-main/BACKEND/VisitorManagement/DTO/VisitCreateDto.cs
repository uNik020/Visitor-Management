namespace VisitorManagement.DTO
{
    public class VisitCreateDto
    {
        //added afterwards for removing errors
        public int VisitId { get; set; }
        public int VisitorId { get; set; }
        public int HostId { get; set; }

        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public string? VisitStatus { get; set; }

        public bool IsApproved { get; set; } = false;
        public string? ApprovalComment { get; set; }

        public string? GatePassNumber { get; set; }
        public string? QrCodeData { get; set; }
    }
}
