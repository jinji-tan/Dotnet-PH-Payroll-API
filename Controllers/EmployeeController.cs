using EmployeeApi.Models;
using EmployeeApi.Dtos;
using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeApi.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeController(IEmployeeRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employeeDto = await _repo.GetByIdAsync(id);
            return employeeDto == null ? NotFound() : Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeInputDto input)
        {
            var id = await _repo.CreateAsync(input);

            var newEmployee = await _repo.GetByIdAsync(id);

            return CreatedAtAction(nameof(Get), new { id = id }, newEmployee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeInputDto input)
        {
            var updated = await _repo.UpdateAsync(id, input);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _repo.DeleteAsync(id) ? NoContent() : NotFound();
        }
    }
}