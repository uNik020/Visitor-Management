using VisitorManagement.DTO;

namespace VisitorManagement.Interfaces
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDto>> GetAllDepartmentsAsync();
        Task<DepartmentDto?> GetDepartmentByIdAsync(int id);
        Task<string> CreateDepartmentAsync(DepartmentCreateDto departmentDto);
        Task<string> UpdateDepartmentAsync(int id, DepartmentCreateDto departmentDto);
        Task<bool> DeleteDepartmentAsync(int id);
    }
}
