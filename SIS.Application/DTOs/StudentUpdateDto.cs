using System.Collections.Generic;

namespace SIS.Application.DTOs
{
    public class StudentUpdateDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }

        // Replace enrollments
        public List<Guid> CourseIds { get; set; } = new List<Guid>();
    }
}