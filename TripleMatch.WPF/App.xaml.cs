using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TripleMatch.ContractClient.Common.IViewManagers.IPageManagers;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.ContractClient.DependencyInjections;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.Profilies;
using TripleMatch.WPF.Common.ViewManagers.IFrameManagers;
using TripleMatch.WPF.Common.ViewManagers.PageManagers;
using TripleMatch.WPF.Common.ViewManagers.WindowManagers;
using TripleMatch.WPF.Views.Pages;
using TripleMatch.WPF.Views.Windows;

namespace TripleMatch.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private IServiceProvider? _serviceProvider;
        private IWindowManager? _windowManager;

        protected override async void OnStartup(StartupEventArgs e)
        {

                var services = new ServiceCollection();
                ConfigureServices(services);

                _serviceProvider = services.BuildServiceProvider();
                _windowManager = new WindowManager(_serviceProvider);


                var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();

                using (var scope = scopeFactory.CreateScope())
                {
                    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                    var authDto = new AuthDto
                    {
                        Email = "alex.vasiliev@example.com",
                        Password = "hashed_password_1"
                    };

                    try
                    {
                        var userProfile = await authService.AuthAcync(authDto, CancellationToken.None);

                        if (userProfile != null)
                        {
                            UserProfile.Profile = userProfile;
                            _windowManager.ShowProfileWindow();
                        }
                        else
                        {
                            _windowManager.ShowAuthWindow();
                        }
                    }
                    catch (Exception ex)
                    {
                        _windowManager.ShowAuthWindow();
                    }
                }

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddContractClient();

            services.AddTransient<AuthWindow>();
            services.AddTransient<RegistrationWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<GamePage>();
            services.AddTransient<ProfileWindow>();


            services.AddSingleton<IWindowManager, WindowManager>();
            services.AddSingleton<IPageManager, PageManager>();
            services.AddSingleton<IFrameContainer>(sp => sp.GetRequiredService<MainWindow>());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }

            base.OnExit(e);
        }
    }

}
