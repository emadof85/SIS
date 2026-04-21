using System.Collections.Generic;

namespace SIS.Application.DTOs
{
    public class StudentCreateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; } = true;
        public string Password { get; set; }
        public string StudentNumber { get; set; }

        // Optional initial enrollments
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}