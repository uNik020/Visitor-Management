namespace VisitorManagement.DTO
{
    public class VisitorReadDto
    {
        public int VisitorId { get; set; }
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Purpose { get; set; }
        public string? Status { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }

        // Resolved name of the host from HostId
        public string? HostName { get; set; }
    }
}
