using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.Application.Features
{
    public class ProfileService
        : IProfileService
    {
        public Task UpdateAsync(
            UserProfileVm model, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
