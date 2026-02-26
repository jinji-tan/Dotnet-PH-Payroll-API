using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using EmployeeApi.Models;
using EmployeeApi.Dtos;

namespace EmployeeApi.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = @"SELECT Id, EmployeeNumber, 
                               CONCAT_WS(' ', FirstName, MiddleName, LastName) AS FullName, 
                               DateOfBirth, DailyRate, WorkingDays 
                        FROM Employees";
            return await connection.QueryAsync<EmployeeDto>(sql);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT Id, EmployeeNumber, 
                               CONCAT_WS(' ', FirstName, MiddleName, LastName) AS FullName, 
                               DateOfBirth, DailyRate, WorkingDays 
                        FROM Employees 
                        WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<EmployeeDto>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(EmployeeInputDto input)
        {
            using var connection = CreateConnection();

            var sql = @"INSERT INTO Employees (EmployeeNumber, FirstName, LastName, MiddleName, DateOfBirth, DailyRate, WorkingDays) 
                VALUES (@EmployeeNumber, @FirstName, @LastName, @MiddleName, @DateOfBirth, @DailyRate, @WorkingDays);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            string generatedNumber = $"EMP-{DateTime.Now:yyMMdd}-{new Random().Next(1000, 9999)}";

            var parameters = new
            {
                EmployeeNumber = generatedNumber,
                input.FirstName,
                input.LastName,
                input.MiddleName,
                input.DateOfBirth,
                input.DailyRate,
                input.WorkingDays
            };

            var newId = await connection.ExecuteScalarAsync<int>(sql, parameters);
            return newId;
        }

        public async Task<bool> UpdateAsync(int id, EmployeeInputDto input)
        {
            using var connection = CreateConnection();
            var sql = @"UPDATE Employees 
                        SET FirstName = @FirstName, LastName = @LastName, 
                            MiddleName = @MiddleName, DateOfBirth = @DateOfBirth, 
                            DailyRate = @DailyRate, WorkingDays = @WorkingDays 
                        WHERE Id = @Id";
            var parameters = new
            {
                Id = id,
                input.FirstName,
                input.LastName,
                input.MiddleName,
                input.DateOfBirth,
                input.DailyRate,
                input.WorkingDays
            };
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            var sql = "DELETE FROM Employees WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }

        public async Task<DashboardDto> GetDashboardStatsAsync()
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT 
                    (SELECT COUNT(*) FROM Employees) as TotalEmployees,
                    (SELECT ISNULL(SUM(NetPay), 0) FROM Payslips 
                     WHERE MONTH(PayPeriodEnd) = MONTH(GETDATE()) 
                     AND YEAR(PayPeriodEnd) = YEAR(GETDATE())) as TotalPayrollMonthly,
                    (SELECT COUNT(*) FROM Attendances 
                     WHERE RecordDate = CAST(GETDATE() AS DATE) AND IsPresent = 1) as PresentToday";
            var stats = await connection.QueryFirstAsync<dynamic>(sql);
            return new DashboardDto
            {
                TotalEmployees = stats.TotalEmployees,
                TotalPayrollMonthly = stats.TotalPayrollMonthly,
                PresentToday = stats.PresentToday,
                AttendanceRate = stats.TotalEmployees > 0 ? (double)stats.PresentToday / stats.TotalEmployees * 100 : 0
            };
        }
    }
}