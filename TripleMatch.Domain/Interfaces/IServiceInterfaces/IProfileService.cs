using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IProfileService
    {
        Task UpdateAsync(
           UpdateProfileDto model,
           CancellationToken cancellationToken);
    }
}
