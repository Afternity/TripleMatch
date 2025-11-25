using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Domain.Interfaces.IRepositoryInterfaces
{
    public interface IWreateHistoryRepository
    {
        Task CreateAsync(
            History model,
            CancellationToken cancellationToken);
    }
}
