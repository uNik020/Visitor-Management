﻿namespace VisitorManagement.DTO
{
    public class VisitorReadDto
    {
        public int VisitorId { get; set; }
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

        public string? PassCode { get; set; }
        public string? QrCodeData { get; set; }

        public string? PhotoUrl { get; set; }

        public bool IsPreRegistered { get; set; }
        public DateTime? ExpectedVisitDateTime { get; set; }
        public List<CompanionReadDto>? Companions { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
