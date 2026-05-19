using SIS.Domain.Common;

namespace SIS.Domain
{
    public class Course : BaseEntity
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        // Navigation Property
        public ICollection<StudentCourse>? StudentCourses { get; set; }
    }
}
