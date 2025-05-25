using System.ComponentModel.DataAnnotations;

namespace VisitorManagement.DTO
{

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
    }
    public class DepartmentCreateDto
    {
        [Required(ErrorMessage = "DepartmentName field is required")]
        public string DepartmentName { get; set; } = null!;
    }
}
