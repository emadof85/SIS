using System.Collections.Generic;

namespace SIS.Application.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public string StudentNumber { get; set; }

        // Related data
        public List<int> CourseIds { get; set; } = new List<int>();
        public List<string> CourseNames { get; set; } = new List<string>();
    }
}