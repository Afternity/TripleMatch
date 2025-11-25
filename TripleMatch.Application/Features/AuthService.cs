using AutoMapper;
using FluentValidation;
using TripleMatch.Application.Common.Exceptions;
using TripleMatch.Application.Common.Validations;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Application.Features
{
    public class AuthService
        : IAuthService
    {
        private readonly IAuthRepository _repository;
        private readonly IMapper _mapper;
        private readonly AuthDtoValidator _validator;

        public AuthService(
            IAuthRepository repository,
            IMapper mapper,
            AuthDtoValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task<UserProfileVm?> AuthAcync(
            AuthDto authDto,
            CancellationToken cancellationToken)
        {
            var validation = await _validator.ValidateAsync(authDto, cancellationToken);

            if (validation.IsValid == false)
                throw new ValidationException(validation.Errors);

            var entity = await _repository.AuthAsync(
                _mapper.Map<User>(authDto),
                cancellationToken);

            if (entity == null)
                throw new NotFoundException(nameof(entity), $"{authDto.Email} + {authDto.Password}");

            return _mapper.Map<UserProfileVm>(entity);
        }
    }
}
