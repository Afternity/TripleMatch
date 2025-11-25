using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TripleMatch.Domain.Models.DataBaseModels;

namespace TripleMatch.Persistence.Data.Configurations
{
    public class UserConfiguration
        : IEntityTypeConfiguration<User>
    {
        public void Configure(
            EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.HasIndex(user => user.Email)
                .IsUnique();

            builder.HasMany(user => user.Histories)
                .WithOne(history => history.User)
                .HasForeignKey(history => history.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
