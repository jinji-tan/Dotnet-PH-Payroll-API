using EmployeeApi.Models;

namespace EmployeeApi.Repositories
{
    public interface IPayrollService
    {
        Task<Payslip> CalculatePayslipAsync(int employeeId, DateTime startDate, DateTime endDate);
    }
}