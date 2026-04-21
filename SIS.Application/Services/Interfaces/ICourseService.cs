using SIS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIS.Application.Services.Interfaces
{
    public interface ICourseService
    {
        Task<IEnumerable<CourseListDto>> GetAllAsync();
        Task<CourseDto?> GetByIdAsync(int id);
        Task<CourseDto> CreateAsync(CourseCreateDto dto);
        Task UpdateAsync(CourseUpdateDto dto);
        Task DeleteAsync(int id);
    }
}