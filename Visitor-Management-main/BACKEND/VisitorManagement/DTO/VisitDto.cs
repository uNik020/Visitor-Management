namespace VisitorManagement.DTO
{
    public class VisitDto
    {
        public int VisitId { get; set; }
        public string VisitorName { get; set; }
        public string HostName { get; set; }
        public string Department { get; set; }
        public string VisitStatus { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
