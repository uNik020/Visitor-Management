namespace VisitorManagement.DTO
{
    public class VisitUpdateDto
    {
        public int Id { get; set; }
        public string? VisitStatus { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        public bool? IsApproved { get; set; }
        public string? ApprovalComment { get; set; }

        public string? GatePassNumber { get; set; }
        public string? QrCodeData { get; set; }
    }
}
