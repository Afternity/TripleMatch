using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TripleMatch.ContractClient.Common.ContractClientLogging;
using TripleMatch.ContractClient.Common.IViewManagers.IWindowManagers;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Shered.Contracts.DTOs;
using TripleMatch.Shered.Contracts.MessageVm;
using TripleMatch.Shered.Contracts.MessageVMs;
using TripleMatch.Shered.Contracts.Profilies;
using TripleMatch.Shered.Contracts.VMs.GameModels;

namespace TripleMatch.ContractClient.ViewModels
{
    public partial class MainViewModel
        : ObservableObject
    {

        [ObservableProperty]
        private GameBoardVm _gameBoardVm = new GameBoardVm();

        [ObservableProperty]
        private GameCellVm? _selectedCell;

        [ObservableProperty]
        private MessageVm _messageVm = new MessageVm();

        private readonly IWindowManager _windowManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private CancellationTokenSource? _gameTimerToken;

        public MainViewModel(
            IWindowManager windowManager,
            IServiceScopeFactory scopeFactory)
        {
            _windowManager = windowManager;
            _scopeFactory = scopeFactory;
            InitializeGame();
        }

        private void InitializeGame()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IGameService>();

                service.InitializeBoard(GameBoardVm);
            }

            StartGameTimer();

            MessageVm.SetMassage(
                MessageState.Info,
                GameDecoratorLogging.Start);
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

                if (GameBoardVm.TimeRemaining <= 10)
                {
                    MessageVm.SetMassage(MessageState.Warning, $"⚠️ Осталось {GameBoardVm.TimeRemaining} секунд!");
                }

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
            {
                MessageVm.SetMassage(MessageState.Fail, "❌ Игра окончена!");
                return;
            }

            if (SelectedCell == null)
            {
                SelectedCell = cell;
                cell.IsSelected = true;
                MessageVm.SetMassage(MessageState.Info, "✓ Первая фишка выбрана. Нажми на соседнюю.");
            }
            else if (SelectedCell == cell)
            {
                SelectedCell.IsSelected = false;
                SelectedCell = null;
                MessageVm.SetMassage(MessageState.Info, "Выделение снято.");
            }
            else
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IGameService>();

                    bool swapped = service.TrySwapAndMatch(
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
                            "⛔ Соседние фишки не совпадают. Попробуй ещё!");
                    }
                    else
                    {
                        MessageVm.SetMassage(
                            MessageState.Success,
                            $"🎉 Отлично! +10 очков");
                    }
                }
            }
        }

        private async Task SaveGameHistoryAsync()
        {
            try
            {
                using var tokenSource = new CancellationTokenSource();
                using var scope = _scopeFactory.CreateScope();

                var writeHistoryService = scope.ServiceProvider.GetRequiredService<IWreateHistoryService>();

                var writeHistoryDto = new WriteHistoryDto
                {
                    Score = GameBoardVm.Score,
                    UserId = UserProfile.Profile.Id
                };

                await writeHistoryService.CreateAsync(writeHistoryDto, tokenSource.Token);

                MessageVm.SetMassage(
                    MessageState.Success,
                    $"✅ Результат сохранён! Ваш счёт: {GameBoardVm.Score}");
            }
            catch (Exception ex)
            {
                MessageVm.SetMassage(
                    MessageState.Error,
                    $"❌ Ошибка: {ex.Message}");
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

        [RelayCommand]
        private void ShowProfileWindow()
        {
            _windowManager.ShowProfileWindow();
        }
    }
}
