using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.Repositories
{
    public class ProfileRepository
        : IProfileRepository
    {
        private readonly TripleMatchDbContext _context;

        public ProfileRepository(
            TripleMatchDbContext context)
        {
            _context = context; 
        }

        public Task<User?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(
            User model,
            CancellationToken cancellationToken)
        {
            _context.Users.Update(model);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
