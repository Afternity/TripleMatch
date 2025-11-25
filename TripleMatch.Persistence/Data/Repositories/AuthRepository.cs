using Microsoft.EntityFrameworkCore;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.Repositories
{
    public class AuthRepository
        : IAuthRepository
    {
        private readonly TripleMatchDbContext _context;

        public AuthRepository(
            TripleMatchDbContext context)
        {
            _context = context;
        }

        public async Task<User?> AuthAsync(
            User model,
            CancellationToken cancellationToken)
        {
            return await _context.Users
                .FirstOrDefaultAsync(user =>
                    user.Email == model.Email &&
                    user.Password == model.Password,
                    cancellationToken);
        }
    }
}
