using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.ContractClient.ViewModels;
using TripleMatch.WPF.Views.Windows;

namespace TripleMatch.WPF.Common.ViewManagers.WindowManagers
{
    public class WindowManager
        : IWindowManager
    {
        private readonly IServiceProvider _provider;

        public WindowManager(
            IServiceProvider provider)
        {
            _provider = provider;
        }

        public void ShowAuthWindow()
        {
            var window = _provider.GetRequiredService<AuthWindow>();
            window.DataContext = _provider.GetRequiredService<AuthViewModel>();
            window.Show();

            var views = System.Windows.Application.Current.Windows.OfType<Window>().ToList();

            foreach (var view in views)
                if (view is not AuthWindow)
                    view.Close();
        }

        public void ShowMainWindow()
        {
            var window = System.Windows.Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault(view => view.IsVisible);

            if (window is not null)
            {
                window.Activate();
                return;
            }

            window = _provider.GetRequiredService<MainWindow>();
            window.DataContext = _provider.GetRequiredService<MainViewModel>();
            window.Show();

            var auth = System.Windows.Application.Current.Windows
               .OfType<AuthWindow>()
               .FirstOrDefault(view => view.IsVisible);

            auth?.Close();
        }

        public void ShowProfileWindow()
        {
            var window = System.Windows.Application.Current.Windows
               .OfType<ProfileWindow>()
               .FirstOrDefault(view => view.IsVisible);

            if (window is not null)
            {
                window.Activate();
                return;
            }

            window = _provider.GetRequiredService<ProfileWindow>();
            window.DataContext = _provider.GetRequiredService<ProfileViewModel>();
            window.Show();
        }

        public void ShowRegistrationWindow()
        {
            var window = System.Windows.Application.Current.Windows
                 .OfType<RegistrationWindow>()
                 .FirstOrDefault(view => view.IsVisible);

            if (window is not null)
            {
                window.Activate();
                return;
            }

            window = _provider.GetRequiredService<RegistrationWindow>();
            window.DataContext = _provider.GetRequiredService<RegistrationViewModel>();
            window.Show();
        }
    }
}
