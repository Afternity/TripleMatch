using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.Repositories
{
    public class WreateHistoryRepository
        : IWreateHistoryRepository
    {
        private readonly TripleMatchDbContext _context;

        public WreateHistoryRepository(
            TripleMatchDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(
            History model,
            CancellationToken cancellationToken)
        {
            await _context.Histories.AddAsync(
                model,
                cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
