using Microsoft.AspNetCore.Mvc;
using SIS.Application.DTOs;
using SIS.Domain.Common.Interfaces;

namespace SIS.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsV1Controller : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public StudentsV1Controller(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudents()
        {
            // 1. Data Access
            var students = await _unitOfWork.Students.GetAllAsync();

            // 2. Manual Mapping (The Pain Point)
            var studentDtos = new List<StudentListDto>();
            foreach (var student in students)
            {
                studentDtos.Add(new StudentListDto
                {
                    Id = student.Id,
                    // TEACHING POINT: Imagine doing this for 30 properties every time!
                    // What if a property name changes? You have to fix it everywhere.
                    FullName = $"{student.FirstName} {student.LastName}",
                    StudentNumber = student.StudentNumber,
                    IsActive = student.IsActive
                });
            }

            // 3. Business Logic (Filtering)
            var activeStudents = studentDtos.Where(s => s.IsActive).ToList();

            // 4. HTTP Response
            return Ok(activeStudents);
        }



    }
}
