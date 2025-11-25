using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;

namespace TripleMatch.Application.Features
{
    public class RegistrationService
        : IRegistrationService
    {
        public Task RegistrationAsync(
            RegistrationDto model,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
