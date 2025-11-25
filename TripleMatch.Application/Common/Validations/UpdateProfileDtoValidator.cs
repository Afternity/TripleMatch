using FluentValidation;
using TripleMatch.Application.Common.ApplicationLogging;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Common.Validations
{
    public class UpdateProfileDtoValidator
        : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(dto => dto.FullName)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError)
                .EmailAddress()
                .WithMessage(ValidationLogging.ValidationError);
            RuleFor(dto => dto.Password)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
        }
    }
}
