using EmployeeApi.Models;
using EmployeeApi.Dtos;

namespace EmployeeApi.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync();
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(EmployeeInputDto employee);
        Task<bool> UpdateAsync(int id, EmployeeInputDto employee);
        Task<bool> DeleteAsync(int id);

        Task<DashboardDto> GetDashboardStatsAsync();
    }
}