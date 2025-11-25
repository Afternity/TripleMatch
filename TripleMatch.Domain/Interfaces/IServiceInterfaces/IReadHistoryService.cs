using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IReadHistoryService
    {
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
