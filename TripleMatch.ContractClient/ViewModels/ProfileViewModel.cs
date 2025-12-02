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
using TripleMatch.Shered.Contracts.Profilies;
using TripleMatch.Shered.Contracts.VMs;

namespace TripleMatch.ContractClient.ViewModels
{
    /// <summary>
    /// В этом классе несколько сервисов реализовано.
    /// ProfileService и ReadHistoryService.
    /// Что-то около CQRS.
    /// Реализация через ScopeFactory.
    /// </summary>
    public partial class ProfileViewModel
        : ObservableObject
    {
        [ObservableProperty]
        private UpdateProfileDto _updateProfileDto = new UpdateProfileDto
        {
            Id = UserProfile.Profile.Id,
            FullName = UserProfile.Profile.FullName,
            Email = UserProfile.Profile.Email,
            Password = UserProfile.Profile.Password,
        };

        [ObservableProperty]
        private BestUserHistoryVm _bestUserHistoryVm = new BestUserHistoryVm();

        [ObservableProperty]
        private FiveBestHistoriesScoreListVm _fiveBestHistoriesScoreListVm = new FiveBestHistoriesScoreListVm();

        [ObservableProperty]
        private UserHistoriesListVm _userHistoriesListVm = new UserHistoriesListVm();

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private AuthDto _authDto = new AuthDto();

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IWindowManager _windowManager;

        public ProfileViewModel(
            IServiceScopeFactory scopeFactory,
            IWindowManager windowManager)
        {
            _scopeFactory = scopeFactory;
            _windowManager = windowManager;
            _ = LoadedAsync();
        }

        private async Task LoadedAsync()
        {
            await BestUserHistory();
            await GetUserHistories();
            await GetFiveBestHistoriesScore();
        }

        [RelayCommand]
        private async Task UpdateProfileAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IProfileService>();
                await service.UpdateAsync(UpdateProfileDto, tokenSource.Token);

                var auth = scope.ServiceProvider.GetRequiredService<IAuthService>();

                var entity = await auth.AuthAcync(
                    new AuthDto
                    {
                        Email = UpdateProfileDto.Email,
                        Password = UpdateProfileDto.Password
                    },
                    tokenSource.Token);

                if (entity == null)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntityNotFound);

                    ShowAuthWindow();

                    return;
                }

                UserProfile.Profile = entity;

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntityUpdateSuccess);
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
        private async Task UserLastHistory()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IReadHistoryService>();

                var entity = await service.UserLastHistory(
                    UserProfile.Profile,
                    tokenSource.Token);

                if (entity == null)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntityNotFound);
                    return;
                }

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntityGetSuccess);
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
        private async Task BestUserHistory()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IReadHistoryService>();

                var entity = await service.BestUserHistory(
                    UserProfile.Profile,
                    tokenSource.Token);

                if (entity == null)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntityNotFound);
                    return;
                }

                BestUserHistoryVm = entity;

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntityGetSuccess);
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
        private async Task GetUserHistories()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConstants.BaseLimit);  
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IReadHistoryService>();

                var entities = await service.GetUserHistories(
                    UserProfile.Profile,
                    tokenSource.Token);

                if (entities.Histories?.Count == 0)  
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntitisCountZero);
                    return;
                }

                UserHistoriesListVm = entities;

                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntitiesGetSuccess);
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
        private async Task GetFiveBestHistoriesScore()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(
                    TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                MessageVm.SetMassage(
                    MessageState.Info,
                    DecoratorLogging.Start);

                var service = scope.ServiceProvider.GetRequiredService<IReadHistoryService>();

                var entities = await service.GetFiveBestHistoriesScore(
                    tokenSource.Token);

                if (entities.Histories.Count == 0)
                {
                    MessageVm.SetMassage(
                        MessageState.Fail,
                        DecoratorLogging.EntitisCountZero);
                    return;
                }

                FiveBestHistoriesScoreListVm = entities;
                MessageVm.SetMassage(
                    MessageState.Success,
                    DecoratorLogging.EntitiesGetSuccess);
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
        private void ShowAuthWindow()
        {
            _windowManager.ShowAuthWindow();    
        }
    }
}

