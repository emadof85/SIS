using System;
using System.Collections.Generic;
using System.Text;
using SIS.Domain;

namespace SIS.Domain.Common.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Students { get; }
        IGenericRepository<Course> Courses { get; }
        IGenericRepository<StudentCourse> StudentCourses { get; }
        Task<int> CompleteAsync(); // Saves changes to the DB
    }
}
