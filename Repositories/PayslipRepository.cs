using System.Data;
using Dapper;
using EmployeeApi.Dtos;
using EmployeeApi.Models;
using Microsoft.Data.SqlClient;

namespace EmployeeApi.Repositories
{
    public class PayslipRepository : IPayslipRepository
    {
        private readonly string _connectionString;

        public PayslipRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<PayslipDto>> GetPayslipsByEmployeeIdAsync(int employeeId)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT p.*, 
                               CONCAT_WS(' ', e.FirstName, e.MiddleName, e.LastName) AS FullName
                        FROM Payslips p
                        JOIN Employees e ON p.EmployeeId = e.Id
                        WHERE p.EmployeeId = @EmployeeId
                        ORDER BY p.PayPeriodEnd DESC";

            return await connection.QueryAsync<PayslipDto>(sql, new { EmployeeId = employeeId });
        }

        public async Task<PayslipDto?> GetPayslipByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT p.*, 
                               CONCAT_WS(' ', e.FirstName, e.MiddleName, e.LastName) AS FullName
                        FROM Payslips p
                        JOIN Employees e ON p.EmployeeId = e.Id
                        WHERE p.Id = @Id";

            return await connection.QueryFirstOrDefaultAsync<PayslipDto>(sql, new { Id = id });
        }

        public async Task<int> CreatePayslipAsync(Payslip payslip)
        {
            using var connection = CreateConnection();

            var sql = @"INSERT INTO Payslips 
                (EmployeeId, PayPeriodStart, PayPeriodEnd, GrossPay, SSS_Deduction, 
                 PhilHealth_Deduction, PagIBIG_Deduction, Tax_Withheld, NetPay) 
                VALUES 
                (@EmployeeId, @PayPeriodStart, @PayPeriodEnd, @GrossPay, @SSS_Deduction, 
                 @PhilHealth_Deduction, @PagIBIG_Deduction, @Tax_Withheld, @NetPay);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            return await connection.ExecuteScalarAsync<int>(sql, payslip);
        }

        public async Task<bool> DeletePayslipAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Payslips WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
}