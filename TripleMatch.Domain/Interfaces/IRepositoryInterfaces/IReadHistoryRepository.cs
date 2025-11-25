using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Domain.Interfaces.IRepositoryInterfaces
{
    public interface IReadHistoryRepository
    {
        Task<History?> UserLastHistory(
            User model,
            CancellationToken cancellationToken);
        Task<IList<History>> GetUserHistories(
            User model,
            CancellationToken cancellationToken);
        Task<IList<History>> GetFiveBestHistoriesScore(
            CancellationToken cancellationToken);
    }
}
