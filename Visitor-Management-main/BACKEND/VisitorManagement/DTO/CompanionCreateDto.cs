namespace VisitorManagement.DTO
{
    public class CompanionCreateDto
    {
        public int CompanionId { get; set; }
        public string FullName { get; set; } = null!;
        public string ContactNumber { get; set; }
        public string Email { get; set; }
    }
}
