using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Domain.Interfaces.IRepositoryInterfaces
{
    public interface IAuthRepository
    {
        Task<User?> AuthAsync(
            User model, 
            CancellationToken cancellationToken);
    }
}
