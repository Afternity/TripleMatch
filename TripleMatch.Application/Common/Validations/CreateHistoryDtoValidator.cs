using FluentValidation;
using TripleMatch.Application.Common.ApplicationLogging;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Common.Validations
{
    public class CreateHistoryDtoValidator
        : AbstractValidator<CreateHistoryDto>
    {
        public CreateHistoryDtoValidator()
        {
            RuleFor(dto => dto.Score)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
            RuleFor(dto => dto.UserId)
                .NotEmpty()
                .WithMessage(ValidationLogging.ValidationError);
        }
    }
}
