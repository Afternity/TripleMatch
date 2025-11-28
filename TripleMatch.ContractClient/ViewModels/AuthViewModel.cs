using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Application.Common.Interfaces.InterfaceViewModels;
using TripleMatch.ContractClient.Common.Constants;
using TripleMatch.ContractClient.Common.ContractClientLogging;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.MessageVm;
using TripleMatch.Shered.Contracts.MessageVMs;
using TripleMatch.Shered.Contracts.Profilies;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class AuthViewModel
        : ObservableObject,
        IAuthViewModel
    {
        [ObservableProperty]
        private AuthDto _authDto = new AuthDto
        {
            Email = "alex.vasiliev@example.com",
            Password = "hashed_password_1"
        };

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWindowManager _windowManager;

        public AuthViewModel(
            IServiceScopeFactory scopeFactory,
            IWindowManager windowManager)
        {
            _scopeFactory = scopeFactory;
            _windowManager = windowManager;
        }

        [RelayCommand]
        public async Task AuthAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var entity = await service.AuthAcync(AuthDto, tokenSource.Token);

                if (entity == null)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntityNotFound);

                    return;
                }

                UserProfile.Profile = entity;

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntityGetSuccess);

                await Task.Delay(2000);

                ShowMainWindow();
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

        [RelayCommand]
        public void ShowRegistrationWindow()
        {
            _windowManager.ShowRegistrationWindow();
        }

        public void ShowMainWindow()
        {
            _windowManager.ShowMainWindow();
        }

    }
}
