using System.ComponentModel.DataAnnotations;

namespace VisitorManagement.DTO
{
    public class AdminLoginDto
    {
        [Required(ErrorMessage = "Email field is required")]
        [EmailAddress(ErrorMessage = "Enter a valid Email Id")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password field is required")]
        public string Password { get; set; } = null!;
    }
}
