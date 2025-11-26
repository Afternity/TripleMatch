using AutoMapper;
using FluentValidation;
using TripleMatch.Application.Common.Validations;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Application.Features
{
    public class RegistrationService
        : IRegistrationService
    {
        private readonly IRegistrationRepository _repository;
        private readonly IMapper _mapper;
        private readonly RegistrationDtoValidator _validator;

        public RegistrationService(
            IRegistrationRepository repository,
            IMapper mapper,
            RegistrationDtoValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task RegistrationAsync(
            RegistrationDto model,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(model, cancellationToken);

            if (validation.IsValid == false)
                throw new ValidationException(validation.Errors);

            await _repository.RegistrationAsync(
                _mapper.Map<User>(model),
                cancellationToken);
        }
    }
}
