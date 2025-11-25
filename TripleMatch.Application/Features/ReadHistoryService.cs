using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Application.Features
{
    public class ReadHistoryService
        : IReadHistoryService
    {
        public Task<FiveBestHistoriesScoreListVm> GetFiveBestHistoriesScore(
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserHistoriesListVm> GetUserHistories(
            UserProfileVm model,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<UserLastHistoryVm?> UserLastHistory(
            UserProfileVm model,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
