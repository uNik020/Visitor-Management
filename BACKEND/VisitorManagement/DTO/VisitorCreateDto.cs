namespace VisitorManagement.DTO
{
    public class VisitorCreateDto
    {
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Purpose { get; set; }
        
    }
}
