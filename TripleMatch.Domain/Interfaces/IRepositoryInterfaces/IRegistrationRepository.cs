using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Domain.Interfaces.IRepositoryInterfaces
{
    public interface IRegistrationRepository
    {
        Task RegistrationAsync(
            User model,
            CancellationToken cancellationToken);
    }
}
