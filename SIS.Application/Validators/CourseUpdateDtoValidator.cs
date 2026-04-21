using FluentValidation;
using SIS.Application.DTOs;

namespace SIS.Application.Validators
{
    public class CourseUpdateDtoValidator : AbstractValidator<CourseUpdateDto>
    {
        public CourseUpdateDtoValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}