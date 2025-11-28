using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Application.Common.Interfaces.InterfaceViewModels;
using TripleMatch.Application.DependencyInjections;
using TripleMatch.Application.Features;
using TripleMatch.ContractClient.ViewModels;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
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

            services.AddTransient<IAuthViewModel, AuthViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddScoped<IGameService, GameService>();
            services.AddTransient<GameViewModel>();
            services.AddTransient<ProfileViewModel>();

            return services;
        }
    }
}
