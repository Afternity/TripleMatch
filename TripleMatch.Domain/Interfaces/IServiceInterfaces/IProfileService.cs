using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IProfileService
    {
        Task UpdateAsync(
           UserProfileVm model,
           CancellationToken cancellationToken);
    }
}
