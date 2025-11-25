using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Persistence.Data.Configurations
{
    public class HistoryConfiguration
        : IEntityTypeConfiguration<History>
    {
        public void Configure(
            EntityTypeBuilder<History> builder)
        {
            builder.HasKey(history => history.Id);

            builder.HasOne(history => history.User)
                .WithMany(user => user.Histories)
                .HasForeignKey(history => history.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
