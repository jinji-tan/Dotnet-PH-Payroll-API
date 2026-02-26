using EmployeeApi.Dtos;
using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceRepository _repo;

        public AttendanceController(IAttendanceRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> LogAttendance([FromBody] AttendanceInputDto input)
        {
            try
            {
                var id = await _repo.LogAttendanceAsync(input);
                return Ok(new { Message = "Attendance logged successfully", Id = id });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = "Could not log attendance. Does a record already exist for this date?",
                    Error = ex.Message
                });
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetAttendance(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate > endDate)
            {
                return BadRequest("Start date cannot be after end date.");
            }

            var records = await _repo.GetEmployeeAttendanceAsync(employeeId, startDate, endDate);
            return Ok(records);
        }
    }
}