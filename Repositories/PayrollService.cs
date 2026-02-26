using EmployeeApi.Models;
using EmployeeApi.Repositories;

namespace EmployeeApi.Repositories
{
    public class PayrollService : IPayrollService
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IAttendanceRepository _attendanceRepo;

        public PayrollService(IEmployeeRepository employeeRepo, IAttendanceRepository attendanceRepo)
        {
            _employeeRepo = employeeRepo;
            _attendanceRepo = attendanceRepo;
        }

        public async Task<Payslip> CalculatePayslipAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            var employee = await _employeeRepo.GetByIdAsync(employeeId);
            if (employee == null) throw new Exception("Employee not found");

            var attendanceRecords = await _attendanceRepo.GetEmployeeAttendanceAsync(employeeId, startDate, endDate);
            int daysWorked = attendanceRecords.Count(a => a.IsPresent);

            if (daysWorked == 0)
            {
                throw new Exception("Cannot generate payslip: No attendance records found for this period.");
            }

            decimal grossPay = employee.DailyRate * daysWorked;

            decimal sss = CalculateSSS(grossPay);
            decimal philHealth = grossPay * 0.05m / 2;
            decimal pagIbig = 100.00m;

            decimal taxableIncome = grossPay - (sss + philHealth + pagIbig);
            decimal tax = CalculateWithholdingTax(taxableIncome);

            decimal totalDeductions = sss + philHealth + pagIbig + tax;
            decimal netPay = grossPay > totalDeductions ? grossPay - totalDeductions : 0;

            return new Payslip
            {
                EmployeeId = employeeId,
                PayPeriodStart = startDate,
                PayPeriodEnd = endDate,
                GrossPay = grossPay,
                SSS_Deduction = sss,
                PhilHealth_Deduction = philHealth,
                PagIBIG_Deduction = pagIbig,
                Tax_Withheld = tax,

                NetPay = grossPay > totalDeductions ? grossPay - totalDeductions : 0
            };
        }

        private decimal CalculateSSS(decimal gross)
        {
            return gross * 0.045m;
        }

        private decimal CalculateWithholdingTax(decimal taxable)
        {
            if (taxable <= 20833) return 0;
            return (taxable - 20833) * 0.15m;
        }
    }
}