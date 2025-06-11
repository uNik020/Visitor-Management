namespace VisitorManagement.DTO
{
    public class VisitCreateDto
    {
        public int VisitId { get; set; }
        public int VisitorId { get; set; }
        public int HostId { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? VisitStatus { get; set; }
    }
}
