using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TripleMatch.ContractClient.Common.IViewManagers.IPageManagers;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.ContractClient.DependencyInjections;
using TripleMatch.ContractClient.ViewModels;
using TripleMatch.WPF.Common.ViewManagers.WindowManagers;
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
        private IPageManager? _pageManager;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            _windowManager = new WindowManager(_serviceProvider);

            var window = _serviceProvider.GetRequiredService<AuthWindow>();
            window.DataContext = _serviceProvider.GetRequiredService<AuthViewModel>();
            window.Show();

            base.OnStartup(e);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddContractClient();

            services.AddTransient<AuthWindow>();
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
