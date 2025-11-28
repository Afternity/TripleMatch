using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IReadHistoryService
    {
        Task<BestUserHistoryVm?> BestUserHistory(
            UserProfileVm model,
            CancellationToken cancellationToken);
        Task<UserLastHistoryVm?> UserLastHistory(
            UserProfileVm model,
            CancellationToken cancellationToken);
        Task<UserHistoriesListVm> GetUserHistories(
            UserProfileVm model,
            CancellationToken cancellationToken);
        Task<FiveBestHistoriesScoreListVm> GetFiveBestHistoriesScore(
            CancellationToken cancellationToken);
    }
}
