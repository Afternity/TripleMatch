using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IAuthService
    {
        Task<UserProfileVm?> AuthAcync(
           AuthDto authDto,
           CancellationToken cancellationToken);
    }
}
