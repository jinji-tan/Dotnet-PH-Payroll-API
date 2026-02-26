using EmployeeApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayslipsController : ControllerBase
    {
        private readonly IPayslipRepository _repo;
        private readonly IPayrollService _payrollService;

        public PayslipsController(IPayslipRepository repo, IPayrollService payrollService)
        {
            _repo = repo;
            _payrollService = payrollService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var payslip = await _repo.GetPayslipByIdAsync(id);
            return payslip == null ? NotFound() : Ok(payslip);
        }

        [HttpPost("generate/{employeeId}")]
        public async Task<IActionResult> Generate(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest("Start date cannot be after end date.");
                }

                var calculatedPayslip = await _payrollService.CalculatePayslipAsync(employeeId, startDate, endDate);

                var newId = await _repo.CreatePayslipAsync(calculatedPayslip);

                var savedPayslip = await _repo.GetPayslipByIdAsync(newId);
                return CreatedAtAction(nameof(Get), new { id = newId }, savedPayslip);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeletePayslipAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}