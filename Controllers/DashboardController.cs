using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;

        public DashboardController(IEmployeeRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var stats = await _repo.GetDashboardStatsAsync();
            return Ok(stats);
        }
    }
}