namespace VisitorManagement.DTO
{
    public class HostCreateDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? About { get; set; }
    }
}
