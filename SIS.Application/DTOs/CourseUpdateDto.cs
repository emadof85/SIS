namespace SIS.Application.DTOs
{
    public class CourseUpdateDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}