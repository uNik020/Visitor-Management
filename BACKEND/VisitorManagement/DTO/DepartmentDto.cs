namespace VisitorManagement.DTO
{

    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
    }
    public class DepartmentCreateDto
    {
        public string DepartmentName { get; set; } = null!;
    }
}
