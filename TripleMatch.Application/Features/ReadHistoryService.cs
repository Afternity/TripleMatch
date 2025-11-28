using AutoMapper;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.VMs;
using TripleMatch.Shered.Contracts.VMs.LookupDTOs;

namespace TripleMatch.Application.Features
{
    public class ReadHistoryService
        : IReadHistoryService
    {
        private readonly IReadHistoryRepository _repository;
        private readonly IMapper _mapper;

        public ReadHistoryService(
            IReadHistoryRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BestUserHistoryVm?> BestUserHistory(
            UserProfileVm model,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(model);

            var history = await _repository.BestUserHistory(
                entity,
                cancellationToken);

            return _mapper.Map<BestUserHistoryVm>(history);
        }

        public async Task<FiveBestHistoriesScoreListVm> GetFiveBestHistoriesScore(
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetFiveBestHistoriesScore(
                cancellationToken);


            return new FiveBestHistoriesScoreListVm()
            {
                Histories = _mapper.Map<IList<FiveBestHistoriesScoreLookupDto>>(entities)
            };
        }

        public async Task<UserHistoriesListVm> GetUserHistories(
            UserProfileVm model,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(model);

            var entities = await _repository.GetUserHistories(
                entity,
                cancellationToken);

            return new UserHistoriesListVm 
            { 
                Histories = _mapper.Map<IList<UserHistoriesLookupDto>>(entities)
            };
        }

        public async Task<UserLastHistoryVm?> UserLastHistory(
            UserProfileVm model,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<User>(model);

            var entities = await _repository.UserLastHistory(
                entity,
                cancellationToken);

            return _mapper.Map<UserLastHistoryVm>(entities);
        }
    }
}
