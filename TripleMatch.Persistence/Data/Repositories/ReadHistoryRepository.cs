using Microsoft.EntityFrameworkCore;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.Repositories
{
    public class ReadHistoryRepository
        : IReadHistoryRepository
    {
        private readonly TripleMatchDbContext _context;

        public ReadHistoryRepository(
            TripleMatchDbContext context)
        {
            _context = context;
        }

        public async Task<History?> BestUserHistory(
            User model,
            CancellationToken cancellationToken)
        {
            return await _context.Histories
                .AsNoTracking()
                .Where(history => history.UserId == model.Id)
                .OrderByDescending(history => history.Score)
                .Take(1)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<History>> GetFiveBestHistoriesScore(
            CancellationToken cancellationToken)
        {
            return await _context.Histories
                .AsNoTracking()
                .Include(h => h.User)
                .OrderByDescending(h => h.Score)
                .Take(5)
                .ToListAsync(cancellationToken);
        }

        public async Task<IList<History>> GetUserHistories(
            User model,
            CancellationToken cancellationToken)
        {
            return await _context.Histories
                .AsNoTracking()
                .Where(history => history.UserId == model.Id)
                .OrderByDescending(h => h.DateTime)  
                .ToListAsync(cancellationToken);
        }

        public async Task<History?> UserLastHistory(
            User model,
            CancellationToken cancellationToken)
        {
            return await _context.Histories
                .Where(history =>
                    history.UserId == model.Id)
                .OrderByDescending(history => history.DateTime)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
