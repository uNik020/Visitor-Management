using System.ComponentModel.DataAnnotations;

namespace VisitorManagement.DTO
{
    public class VisitorCreateDto
    {
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public string Comment { get; set; }
        public string IdProofType { get; set; }
        public string IdProofNumber { get; set; }
        public string LicensePlateNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string? PassCode { get; set; }          
        public string? QrCodeData { get; set; }
        public bool? IsPreRegistered { get; set; }
        public DateTime? ExpectedVisitDateTime { get; set; }
        public int HostId { get; set; }

        // Accept a list of companion objects
        public List<CompanionCreateDto>? Companions { get; set; }
        public List<VisitCreateDto>? Visits { get; set; }
    }
}
