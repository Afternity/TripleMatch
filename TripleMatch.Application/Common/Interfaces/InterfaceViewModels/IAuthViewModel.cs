namespace TripleMatch.Application.Common.Interfaces.InterfaceViewModels
{
    public interface IAuthViewModel
    {
        Task AuthAsync();
        void ShowRegistrationWindow();
        void ShowMainWindow();
    }
}
