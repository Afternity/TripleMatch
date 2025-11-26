using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.ContractClient.Common.Constants;
using TripleMatch.ContractClient.Common.ContractClientLogging;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.MessageVm;
using TripleMatch.Shered.Contracts.MessageVMs;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class RegistrationViewModel
        : ObservableObject
    {
        [ObservableProperty]
        private RegistrationDto _registrationDto = new RegistrationDto();

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWindowManager _windowManager;

        public RegistrationViewModel(
            IServiceScopeFactory scopeFactory,
            IWindowManager windowManager)
        {
            _scopeFactory = scopeFactory;
            _windowManager = windowManager;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IRegistrationService>();
                await service.RegistrationAsync(RegistrationDto, tokenSource.Token);

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntityCreateSuccess);

                await Task.Delay(2000);

                ShowAuthWindow();
            }
            catch (OperationCanceledException)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    DecoratorLogging.TimeLimit);
            }
            catch (ValidationException ex)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    ex.Message);
            }
            catch (Exception ex)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    ex.Message);
            }
        }

        private void ShowAuthWindow()
        {
            _windowManager.ShowAuthWindow();
        }
    }
}
