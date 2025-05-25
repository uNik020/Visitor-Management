using Microsoft.EntityFrameworkCore;
using VisitorManagement.DTO;
using VisitorManagement.HelperClasses;
using VisitorManagement.Interfaces;
using VisitorManagement.Models;

namespace VisitorManagement.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly VisitorManagementContext _context;

        public DepartmentService(VisitorManagementContext context)
        {
            _context = context;
        }

        public async Task<List<DepartmentDto>> GetAllDepartmentsAsync()
        {
            return await _context.Departments
                .Select(d => new DepartmentDto
                {
                    DepartmentId = d.DepartmentId,
                    DepartmentName = d.DepartmentName
                }).ToListAsync();
        }

        public async Task<DepartmentDto?> GetDepartmentByIdAsync(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return null;

            return new DepartmentDto
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName
            };
        }

        public async Task<string> CreateDepartmentAsync(DepartmentCreateDto departmentDto)
        {
            var department = new Department
            {
                DepartmentName = departmentDto.DepartmentName
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return "Department created successfully";
        }

        public async Task<string> UpdateDepartmentAsync(int id, DepartmentCreateDto departmentDto)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                throw new CustomException(404, "Department not found");

            department.DepartmentName = departmentDto.DepartmentName;
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
            return "Department updated successfully";
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return false;

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}