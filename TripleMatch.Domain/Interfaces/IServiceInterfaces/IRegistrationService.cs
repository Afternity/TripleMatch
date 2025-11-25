using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IRegistrationService
    {
        Task RegistrationAsync(
            RegistrationDto model,
            CancellationToken cancellationToken);
    }
}
