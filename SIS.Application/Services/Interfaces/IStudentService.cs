using SIS.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIS.Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentListDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(Guid id);
        Task<StudentDto> CreateAsync(StudentCreateDto dto);
        Task UpdateAsync(StudentUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<StudentDto?> GetByStudentNumberAsync(string studentNumber);
        Task<int> GetActiveStudentCountAsync();
        Task<IEnumerable<StudentListDto>> GetActiveStudentsAsync();
    }
}