using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Application.Features;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;

namespace TripleMatch.Application.DependencyInjections
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(DependencyInjection).Assembly);
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IReadHistoryService, ReadHistoryService>();
            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IWreateHistoryService, WriteHistoryService>();

            return services;
        }
    }
}
