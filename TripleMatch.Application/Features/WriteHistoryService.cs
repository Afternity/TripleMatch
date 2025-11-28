using AutoMapper;
using FluentValidation;
using TripleMatch.Application.Common.Validations;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Features
{
    public class WriteHistoryService
        : IWreateHistoryService
    {
        private readonly IWreateHistoryRepository _repository;
        private readonly IMapper _mapper;
        private readonly WriteHistoryDtoValidator _validator;

        public WriteHistoryService(
            IWreateHistoryRepository repository,
            IMapper mapper,
            WriteHistoryDtoValidator validator)
        {
            _repository = repository;
            _mapper = mapper;
            _validator = validator;
        }

        public async Task CreateAsync(
            WriteHistoryDto model,
            CancellationToken cancellationToken)
        {
              var validation = await _validator.ValidateAsync(
                  model,
                  cancellationToken); 

            if (validation.IsValid == false)
                throw new ValidationException(validation.Errors);

            var entity = _mapper.Map<History>(model);

            await _repository.CreateAsync(
                entity,
                cancellationToken);
        }
    }
}
