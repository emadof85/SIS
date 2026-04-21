using AutoMapper;
using SIS.Domain;
using SIS.Application.DTOs;
using System.Linq;

namespace SIS.Application.Mapping
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            // CreateMap<Source, Destination>()
            CreateMap<Student, StudentListDto>()
                // AutoMapper is smart, but we can teach it custom rules:
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.CourseNames, opt => opt.MapFrom(src => src.StudentCourses != null ? src.StudentCourses.Select(sc => sc.Course.Name).ToList() : new System.Collections.Generic.List<string>()));

            CreateMap<Student, StudentDto>()
                .ForMember(dest => dest.CourseIds, opt => opt.MapFrom(src => src.StudentCourses != null ? src.StudentCourses.Select(sc => sc.CourseId).ToList() : new System.Collections.Generic.List<Guid>()))
                .ForMember(dest => dest.CourseNames, opt => opt.MapFrom(src => src.StudentCourses != null ? src.StudentCourses.Select(sc => sc.Course.Name).ToList() : new System.Collections.Generic.List<string>()));

            CreateMap<StudentCreateDto, Student>();
            CreateMap<StudentUpdateDto, Student>();
        }
    }
}