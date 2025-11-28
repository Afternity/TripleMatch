using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.Application.Features;
using TripleMatch.ContractClient.Common.Constants;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.MessageVm;
using TripleMatch.Shered.Contracts.MessageVMs;
using TripleMatch.Shered.Contracts.Profilies;
using TripleMatch.Shered.Contracts.VMs.GameModels;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class GameViewModel
        : ObservableObject
    {
        [ObservableProperty]
        private GameBoardVm _gameBoardVm = new GameBoardVm();

        [ObservableProperty]
        private GameCellVm? _selectedCell;

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly IGameService _gameService;
        private readonly IServiceScopeFactory _scopeFactory;
        private CancellationTokenSource? _gameTimerToken;

        public GameViewModel(
            IGameService gameService,
            IServiceScopeFactory scopeFactory)
        {
            _gameService = gameService;
            _scopeFactory = scopeFactory;
            InitializeGame();
        }

        private void InitializeGame()
        {
            _gameService.InitializeBoard(GameBoardVm);
            StartGameTimer();
        }

        private void StartGameTimer()
        {
            _gameTimerToken = new CancellationTokenSource();
            _ = GameTimerAsync(_gameTimerToken.Token);
        }

        private async Task GameTimerAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && !GameBoardVm.IsGameOver)
            {
                await Task.Delay(1000, cancellationToken);
                GameBoardVm.TimeRemaining--;

                if (GameBoardVm.TimeRemaining <= 0)
                {
                    GameBoardVm.IsGameOver = true;
                    await SaveGameHistoryAsync();
                }
            }
        }

        [RelayCommand]
        public void CellClicked(GameCellVm cell)
        {
            if (GameBoardVm.IsGameOver)
                return;

            if (SelectedCell == null)
            {
                SelectedCell = cell;
                cell.IsSelected = true;
            }
            else if (SelectedCell == cell)
            {
                SelectedCell.IsSelected = false;
                SelectedCell = null;
            }
            else
            {
                bool swapped = _gameService.TrySwapAndMatch(
                    GameBoardVm,
                    SelectedCell.Row,
                    SelectedCell.Column,
                    cell.Row,
                    cell.Column);

                SelectedCell.IsSelected = false;
                SelectedCell = null;

                if (!swapped)
                {
                    MessageVm.SetMassage(
                        MessageState.Warning,
                        "Нельзя сделать этот ход!");
                }
            }
        }

        [RelayCommand]
        private async Task EndGameAsync()
        {
            _gameTimerToken?.Cancel();
            GameBoardVm.IsGameOver = true;
            await SaveGameHistoryAsync();
        }

        private async Task SaveGameHistoryAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource(TimeLimitConstants.BaseLimit);
                using var scope = _scopeFactory.CreateScope();

                var writeHistoryService = scope.ServiceProvider.GetRequiredService<WriteHistoryService>();

                var writeHistoryDto = new WriteHistoryDto
                {
                    Score = GameBoardVm.Score,
                    UserId = UserProfile.Profile.Id
                };

                await writeHistoryService.CreateAsync(writeHistoryDto, tokenSource.Token);

                MessageVm.SetMassage(
                    MessageState.Success,
                    $"Игра окончена! Ваш счёт: {GameBoardVm.Score}");
            }
            catch (Exception ex)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    ex.Message);
            }
        }

        [RelayCommand]
        public void RestartGame()
        {
            _gameTimerToken?.Cancel();
            GameBoardVm = new GameBoardVm();
            SelectedCell = null;
            InitializeGame();
        }
    }
}
