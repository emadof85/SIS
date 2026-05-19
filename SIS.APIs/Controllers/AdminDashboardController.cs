using Microsoft.AspNetCore.Mvc;
using SIS.Application.Services;
using SIS.Application.Services.Interfaces;

namespace SIS.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public AdminDashboardController(IStudentService studentService) => _studentService = studentService;

        [HttpGet("system-summary")]
        public async Task<IActionResult> GetSystemSummary()
        {
            // Reusing the exact same logic!
            int activeStudents = await _studentService.GetActiveStudentCountAsync();

            return Ok(new
            {
                ActiveStudents = activeStudents,
                SystemStatus = "Healthy",
                ReportGenerated = DateTime.UtcNow
            });
        }

    }
}
