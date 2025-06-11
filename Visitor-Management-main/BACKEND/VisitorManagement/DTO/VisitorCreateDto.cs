namespace VisitorManagement.DTO
{
    public class VisitorCreateDto
    {
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        public string? Purpose { get; set; }
        public string? Comment { get; set; }
        public string? IdProofType { get; set; }
        public string? IdProofNumber { get; set; }
        public string? LicensePlateNumber { get; set; }
        public string? PhotoUrl { get; set; }
        public bool IsPreRegistered { get; set; } = false;
        public DateTime? ExpectedVisitDateTime { get; set; }
        public int HostId { get; set; }

        // Adding companions here
        public List<string>? Companions { get; set; } // List of companion names
    }

}
