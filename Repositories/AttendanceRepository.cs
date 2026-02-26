using EmployeeApi.Models;
using EmployeeApi.Dtos;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EmployeeApi.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly string _connectionString;

        public AttendanceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<int> LogAttendanceAsync(AttendanceInputDto attendance)
        {
            using var connection = CreateConnection();
            var sql = @"INSERT INTO Attendances (EmployeeId, RecordDate, IsPresent, Remarks) 
                        VALUES (@EmployeeId, @RecordDate, @IsPresent, @Remarks);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            return await connection.ExecuteScalarAsync<int>(sql, attendance);
        }

        public async Task<IEnumerable<Attendance>> GetEmployeeAttendanceAsync(int employeeId, DateTime startDate, DateTime endDate)
        {
            using var connection = CreateConnection();
            var sql = @"SELECT * FROM Attendances 
                        WHERE EmployeeId = @EmployeeId 
                        AND RecordDate >= @StartDate 
                        AND RecordDate <= @EndDate
                        ORDER BY RecordDate DESC";

            return await connection.QueryAsync<Attendance>(sql, new { EmployeeId = employeeId, StartDate = startDate, EndDate = endDate });
        }
    }
}