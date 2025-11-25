using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IWreateHistoryService
    {
        Task CreateAsync(
            CreateHistoryDto model,
            CancellationToken cancellationToken);
    }
}
