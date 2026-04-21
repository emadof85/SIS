using Microsoft.AspNetCore.Mvc;
using SIS.Application.DTOs;
using SIS.Application.Services.Interfaces;

namespace SIS.APIs.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet(Name = "GetStudents")]
        public async Task<IEnumerable<StudentListDto>> GetAsync()
        {
            return await _studentService.GetAllAsync();
        }

        [HttpGet("{id}", Name = "GetStudentById")]
        public async Task<ActionResult<StudentDto>> GetByIdAsync(Guid id)
        {
            var student = await _studentService.GetByIdAsync(id);
            if (student == null) return NotFound();
            return Ok(student);
        }

        [HttpPost(Name = "CreateStudent")]
        public async Task<ActionResult<StudentDto>> CreateAsync(StudentCreateDto dto)
        {
            var createdStudent = await _studentService.CreateAsync(dto);
            return CreatedAtRoute("GetStudentById", new { id = createdStudent.Id }, createdStudent);

        }

        [HttpPut(Name = "UpdateStudent")]
        public void UpdateAsync(StudentUpdateDto dto)
        {
            _studentService.UpdateAsync(dto);
        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        public void DeleteAsync(Guid id)
        {
            _studentService.DeleteAsync(id);
        }

    }
}
