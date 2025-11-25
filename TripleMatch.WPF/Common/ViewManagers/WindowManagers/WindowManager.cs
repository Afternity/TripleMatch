using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;

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
            throw new NotImplementedException();
        }

        public void ShowMainWindow()
        {
            throw new NotImplementedException();
        }

        public void ShowProfileWindow()
        {
            throw new NotImplementedException();
        }
    }
}
