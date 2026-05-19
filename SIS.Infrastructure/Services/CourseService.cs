using SIS.Application.DTOs;
using SIS.Application.Services.Interfaces;
using SIS.Domain.Common.Interfaces;
using SIS.Domain;

namespace SIS.Infrastructure.Services
{
    public class CourseService : ICourseService
    {
        private readonly IUnitOfWork _uow;

        public CourseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<CourseListDto>> GetAllAsync()
        {
            var courses = await _uow.Courses.GetAllAsync();
            return courses.Select(c => new CourseListDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public async Task<CourseDto?> GetByIdAsync(Guid id)
        {
            var course = await _uow.Courses.GetByIdAsync(id);
            if (course == null) return null;

            // try to include students - repository is generic so we'll fetch via context if needed later
            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description
            };
        }

        public async Task<CourseDto> CreateAsync(CourseCreateDto dto)
        {
            var course = new Course
            {
                Name = dto.Name,
                Description = dto.Description
            };

            await _uow.Courses.AddAsync(course);
            await _uow.CompleteAsync();

            return new CourseDto
            {
                Id = course.Id,
                Name = course.Name,
                Description = course.Description
            };
        }

        public async Task UpdateAsync(CourseUpdateDto dto)
        {
            var course = await _uow.Courses.GetByIdAsync(dto.Id);
            if (course == null) return;

            course.Name = dto.Name;
            course.Description = dto.Description;

            _uow.Courses.Update(course);
            await _uow.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var course = await _uow.Courses.GetByIdAsync(id);
            if (course == null) return;

            _uow.Courses.Delete(course);
            await _uow.CompleteAsync();
        }
    }
}