using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Domain.Interfaces.IRepositoryInterfaces;
using TripleMatch.Persistence.Data.DbContexts;
using TripleMatch.Persistence.Data.Repositories;

namespace TripleMatch.Persistence.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(
            this IServiceCollection services,
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException(
                    "Connection string cannot be null or empty.", nameof(connectionString));

            services.AddDbContext<TripleMatchDbContext>(options =>
            {
                options.UseNpgsql(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorCodesToAdd: null);

                    sqlOptions.CommandTimeout(30);
                });

                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IReadHistoryRepository, ReadHistoryRepository>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<IWreateHistoryRepository, WreateHistoryRepository>();

            return services;
        }
    }
}
