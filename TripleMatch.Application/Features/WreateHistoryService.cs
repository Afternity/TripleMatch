using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Features
{
    public class WreateHistoryService
        : IWreateHistoryService
    {
        public Task CreateAsync(
            CreateHistoryDto model,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
