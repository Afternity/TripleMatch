using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IWreateHistoryService
    {
        Task CreateAsync(
            WriteHistoryDto model,
            CancellationToken cancellationToken);
    }
}
