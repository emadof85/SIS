using System.Collections.Generic;

namespace SIS.Application.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Related data
        public List<int> StudentIds { get; set; } = new List<int>();
        public List<string> StudentNames { get; set; } = new List<string>();
    }
}