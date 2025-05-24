namespace VisitorManagement.DTO
{
    public class VisitorCreateDto
    {
        public string FullName { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        //public string? PhotoPath { get; set; }
        public string? Purpose { get; set; }
    }
}
