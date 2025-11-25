using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TripleMatch.Persistence.Data.DbContexts;

namespace TripleMatch.Persistence.Data.DbContextFactories
{
    public class TripleMatchDbContextFactory
        : IDesignTimeDbContextFactory<TripleMatchDbContext>
    {
        public TripleMatchDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Host=localhost;Port=5432;Database=TripleMatchDb;Username=postgres;Password=superuser123;";

            var optionsBuilder = new DbContextOptionsBuilder<TripleMatchDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new TripleMatchDbContext(optionsBuilder.Options);
        }
    }
}
