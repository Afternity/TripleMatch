using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TripleMatch.ContractClient.Common.IViewManagers.IPageManagers;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class MainViewModel
        : ObservableObject
    {
        private readonly IPageManager _pageManager;
        private readonly IWindowManager _windowManager;

        public MainViewModel(
            IPageManager pageManager,
            IWindowManager windowManager)
        {
            _pageManager = pageManager;
            _windowManager = windowManager;

            Loaded();
        }

        private void Loaded()
        {
            _pageManager.ShowGamePage();
        }

        [RelayCommand]
        private void ShowProfileWindow()
        {
            _windowManager.ShowProfileWindow();
        }
    }
}
