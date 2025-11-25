using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.Profilies;
using TripleMatch.ContractClient.Common.MessageVMs;
using TripleMatch.ContractClient.Common.Constants;
using TripleMatch.ContractClient.Common.ContractClientLogging;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class AuthViewModel
        : ObservableObject
    {
        [ObservableProperty]
        private AuthDto _authDto = new AuthDto();

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly IAuthService _service;

        public AuthViewModel(
            IAuthService service)
        {
            _service = service;
        }

        [RelayCommand]
        private async Task AuthAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConst.BaseLimit);

                MessageVm.Message = DecoratorLogging.Start;

                var entity = await _service.AuthAcync(
                    AuthDto,
                    tokenSource.Token);

                if (entity == null)
                    return;

                UserProfile.Profile = entity;

                MessageVm.Message = DecoratorLogging.EntityGetSuccess;

                await Task.Delay(2000);


            }
            catch (OperationCanceledException)
            {
                MessageVm.Message = DecoratorLogging.TimeLimit;
            }
            catch (Exception ex)
            {
                MessageVm.Message = ex.Message;
            }
        }
    }
}
