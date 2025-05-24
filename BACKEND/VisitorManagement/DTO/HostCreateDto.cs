namespace VisitorManagement.DTO
{
    public class HostCreateDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }
    }
}
