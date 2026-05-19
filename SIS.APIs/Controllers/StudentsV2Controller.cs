using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIS.Application.Features.Students.Queries;
using SIS.Application.Services;
using SIS.Application.Services.Interfaces;

namespace SIS.APIs.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsV2Controller : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly IMediator _mediator;

        public StudentsV2Controller(IStudentService studentService, IMediator mediator)
        {
            _studentService = studentService;
            _mediator = mediator;
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            int count = await _studentService.GetActiveStudentCountAsync();
            return Ok(new { TotalActive = count });
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            // The controller delegates all the hard work to the service
            var students = await _studentService.GetActiveStudentsAsync();

            return Ok(students);
        }

        [HttpGet("Cqrs")]
        public async Task<IActionResult> GetStudentsCqrs()
        {
            // Create the query
            var query = new GetActiveStudentsQuery();

            // Send it through MediatR. It will automatically find GetActiveStudentsQueryHandler!
            var students = await _mediator.Send(query);

            return Ok(students);
        }
    }
}
