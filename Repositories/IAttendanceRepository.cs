using EmployeeApi.Dtos;
using EmployeeApi.Models;

namespace EmployeeApi.Repositories
{
    public interface IAttendanceRepository
    {
        Task<int> LogAttendanceAsync(AttendanceInputDto attendance);
        Task<IEnumerable<Attendance>> GetEmployeeAttendanceAsync(int employeeId, DateTime startDate, DateTime endDate);
    }

}