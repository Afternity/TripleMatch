using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Domain.Interfaces.IRepositoryInterfaces
{
    public interface IProfileRepository
    {
        Task UpdateAsync(
            User model,
            CancellationToken cancellationToken);
    }
}
