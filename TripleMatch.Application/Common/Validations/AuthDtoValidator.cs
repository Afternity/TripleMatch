using FluentValidation;
using TripleMatch.Application.Common.ApplicationLogging;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Common.Validations
{
    public class AuthDtoValidator
        : AbstractValidator<AuthDto>
    {
        public AuthDtoValidator()
        {
            RuleFor(dto => dto.Email)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
            RuleFor(dto => dto.Password)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
        }
    }
}
