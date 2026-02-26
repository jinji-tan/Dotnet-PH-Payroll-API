using EmployeeApi.Dtos;
using EmployeeApi.Models;

namespace EmployeeApi.Repositories
{
    public interface IPayslipRepository
    {
        Task<PayslipDto?> GetPayslipByIdAsync(int id);
        
        Task<int> CreatePayslipAsync(Payslip payslip); 
        Task<IEnumerable<PayslipDto>> GetPayslipsByEmployeeIdAsync(int employeeId);
        Task<bool> DeletePayslipAsync(int id);
    }
}