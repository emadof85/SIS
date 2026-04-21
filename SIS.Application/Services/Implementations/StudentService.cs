using AutoMapper;
using SIS.Application.DTOs;
using SIS.Application.Services.Interfaces;
using SIS.Domain.Common.Interfaces;
using SIS.Domain;

namespace SIS.Application.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public StudentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentListDto>> GetAllAsync()
        {
            var students = await _uow.Students.GetAllAsync();
            /*return students.Select(s => new StudentListDto
            {
                Id = s.Id,
                FullName = $"{s.FirstName} {s.LastName}",
                StudentNumber = s.StudentNumber,
                CourseNames = s.StudentCourses?.Select(sc => sc.Course.Name).ToList() ?? new List<string>()
            });*/
            return _mapper.Map<IEnumerable<StudentListDto>>(students);
        }

        public async Task<StudentDto?> GetByIdAsync(Guid id)
        {
            var student = await _uow.Students.GetStudentWithCoursesAsync(id);
            if (student == null) return null;

            /*return new StudentDto
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                PhoneNumber = student.PhoneNumber,
                Age = student.Age,
                IsActive = student.IsActive,
                StudentNumber = student.StudentNumber,
                CourseIds = student.StudentCourses?.Select(sc => sc.CourseId).ToList() ?? new List<int>(),
                CourseNames = student.StudentCourses?.Select(sc => sc.Course.Name).ToList() ?? new List<string>()
            };*/
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<StudentDto> CreateAsync(StudentCreateDto dto)
        {
            /*var student = new Student
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber,
                Age = dto.Age,
                IsActive = dto.IsActive,
                Password = dto.Password,
                StudentNumber = dto.StudentNumber
            };*/
            var student = _mapper.Map<Student>(dto);

            await _uow.Students.AddAsync(student);

            // handle enrollments
            foreach(var courseId in dto.CourseIds)
            {
                var sc = new StudentCourse { Student = student, CourseId = courseId };
                await _uow.StudentCourses.AddAsync(sc);
            }

            await _uow.CompleteAsync();

            // reload or map tracked student (StudentCourses should be tracked)
            /*return new StudentDto
             {
                 Id = student.Id,
                 FirstName = student.FirstName,
                 LastName = student.LastName,
                 PhoneNumber = student.PhoneNumber,
                 Age = student.Age,
                 IsActive = student.IsActive,
                 StudentNumber = student.StudentNumber,
                 CourseIds = dto.CourseIds
             };*/
            var result = _mapper.Map<StudentDto>(student);
            return result;
        }

        public async Task UpdateAsync(StudentUpdateDto dto)
        {
            var student = await _uow.Students.GetStudentWithCoursesAsync(dto.Id);
            if (student == null) return;

            /*student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.PhoneNumber = dto.PhoneNumber;
            student.Age = dto.Age;
            student.IsActive = dto.IsActive;
            student.Password = dto.Password;*/
            // map updated fields onto existing entity
            _mapper.Map(dto, student);

            _uow.Students.Update(student);

            // Replace enrollments: remove existing and add new
            var existing = student.StudentCourses ?? new List<StudentCourse>();
            foreach(var sc in existing.ToList())
            {
                _uow.StudentCourses.Delete(sc);
            }

            foreach(var cid in dto.CourseIds)
            {
                var sc = new StudentCourse { StudentId = dto.Id, CourseId = cid };
                await _uow.StudentCourses.AddAsync(sc);
            }

            await _uow.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var student = await _uow.Students.GetStudentWithCoursesAsync(id);
            if (student == null) return;

            // remove related studentcourses first
            var existing = student.StudentCourses ?? new List<StudentCourse>();
            foreach(var sc in existing.ToList())
            {
                _uow.StudentCourses.Delete(sc);
            }

            _uow.Students.Delete(student);
            await _uow.CompleteAsync();
        }

        public async Task<StudentDto?> GetByStudentNumberAsync(string studentNumber)
        {
            var student = await _uow.Students.GetByStudentNumberAsync(studentNumber);
            if (student == null) return null;

            /*return new StudentDto
                        {
                            Id = student.Id,
                            FirstName = student.FirstName,
                            LastName = student.LastName,
                            PhoneNumber = student.PhoneNumber,
                            Age = student.Age,
                            IsActive = student.IsActive,
                            StudentNumber = student.StudentNumber
                        };*/
            return _mapper.Map<StudentDto>(student);
        }

        public async Task<int> GetActiveStudentCountAsync()
        {
            var students = await _uow.Students.GetAllAsync();
            return students.Count(s => s.IsActive);
        }

        public async Task<IEnumerable<StudentListDto>> GetActiveStudentsAsync()
        {
            // 1. Get Data
            var students = await _uow.Students.GetAllAsync();

            // 2. Apply Business Logic
            var activeStudents = students.Where(s => s.IsActive);

            // 3. AutoMapper handles the manual mapping in one single line!
            return _mapper.Map<IEnumerable<StudentListDto>>(activeStudents);
        }
    }
}