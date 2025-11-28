using Microsoft.Extensions.DependencyInjection;
using TripleMatch.ContractClient.Common.IViewManagers.IPageManagers;
using TripleMatch.ContractClient.ViewModels;
using TripleMatch.WPF.Common.ViewManagers.IFrameManagers;
using TripleMatch.WPF.Views.Pages;

namespace TripleMatch.WPF.Common.ViewManagers.PageManagers
{
    public class PageManager
        : IPageManager
    {
        private readonly IServiceProvider _provider;
        private readonly IFrameContainer _frameContainer;

        public PageManager(
            IServiceProvider provider,
            IFrameContainer frameContainer)
        {
            _provider = provider;
            _frameContainer = frameContainer;
        }

        public void ShowGamePage()
        {
            var frame = _frameContainer.GetNavigationFrame();

            // Очищаем навигационную историю
            frame.NavigationService.RemoveBackEntry();

            var page = _provider.GetRequiredService<GamePage>();
            page.DataContext = _provider.GetRequiredService<GameViewModel>();

            frame.Navigate(page);
        }
    }
}
