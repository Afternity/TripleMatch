using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Application.DependencyInjections;
using TripleMatch.ContractClient.ViewModels;
using TripleMatch.Persistence.DependencyInjections;

namespace TripleMatch.ContractClient.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddContractClient(
            this IServiceCollection services)
        {
            services.AddPersistence(
                "Host=localhost;Port=5432;Database=TripleMatchDb;Username=postgres;Password=superuser123;");

            services.AddApplication();

            services.AddTransient<AuthViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<ProfileViewModel>();

            return services;
        }
    }
}
