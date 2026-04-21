using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators
{
    public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentCreateDtoValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(20);
            RuleFor(x => x.Age).InclusiveBetween(16, 60);
            RuleFor(x => x.StudentNumber).NotEmpty().MaximumLength(50);
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
        }
    }
}