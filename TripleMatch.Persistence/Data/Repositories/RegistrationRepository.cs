using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.Repositories
{
    public class RegistrationRepository
        : IRegistrationRepository
    {
        private readonly TripleMatchDbContext _context;
        
        public RegistrationRepository(
            TripleMatchDbContext context)
        {
            _context = context;
        }

        public async Task RegistrationAsync(
            User model, 
            CancellationToken cancellationToken)
        {
            await _context.Users.AddAsync(
                model,
                cancellationToken);
            await _context.SaveChangesAsync(
                cancellationToken);
        }
    }
}
