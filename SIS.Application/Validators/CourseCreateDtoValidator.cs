using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators
{
    public class CourseCreateDtoValidator : AbstractValidator<CourseCreateDto>
    {
        public CourseCreateDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}