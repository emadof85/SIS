using SIS.Domain.Common;

namespace SIS.Domain
{
    public class StudentCourse : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student? Student { get; set; }

        public Guid CourseId { get; set; }
        public Course? Course { get; set; }
    }
}
