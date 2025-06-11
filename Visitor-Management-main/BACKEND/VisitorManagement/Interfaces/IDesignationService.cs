using VisitorManagement.DTO;

namespace VisitorManagement.Interfaces
{
    public interface IDesignationService
    {
        Task<List<DesignationDto>> GetAllDesignationsAsync();
        Task<DesignationDto?> GetDesignationByIdAsync(int id);
        Task<string> CreateDesignationAsync(DesignationCreateDto designationDto);
        Task<string> UpdateDesignationAsync(int id, DesignationCreateDto designationDto);
        Task<bool> DeleteDesignationAsync(int id);
    }
}
