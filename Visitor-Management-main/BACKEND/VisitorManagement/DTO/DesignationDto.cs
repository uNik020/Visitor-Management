using System.ComponentModel.DataAnnotations;

namespace VisitorManagement.DTO
{

    public class DesignationDto
    {
        public int DesignationId { get; set; }
        public string DesignationName { get; set; } = null!;
    }
    public class DesignationCreateDto
    {
        [Required(ErrorMessage = "DesignationName field is required")]
        public string DesignationName { get; set; } = null!;
    }
}
