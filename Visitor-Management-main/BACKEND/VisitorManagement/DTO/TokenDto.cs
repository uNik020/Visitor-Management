namespace VisitorManagement.DTO
{
    public class TokenDto
    {
        public int AdminId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string JwtToken { get; set; } = null!;
    }
}
