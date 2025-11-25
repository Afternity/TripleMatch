using Microsoft.EntityFrameworkCore;
using TripleMatch.Domain.Models.DataBaseModels;
using TripleMatch.Persistence.Data.Configurations;

namespace TripleMatch.Persistence.Data.DbContexts
{
    public class TripleMatchDbContext
        : DbContext
    {
        public TripleMatchDbContext(
            DbContextOptions<TripleMatchDbContext> options)
            : base(options)
        {
        }

        public TripleMatchDbContext()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<History> Histories { get; set; }

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(
                new UserConfiguration());
            modelBuilder.ApplyConfiguration(
                new HistoryConfiguration());
        }
    }
}
