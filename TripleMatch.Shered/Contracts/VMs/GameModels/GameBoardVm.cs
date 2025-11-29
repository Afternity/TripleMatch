using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace TripleMatch.Shered.Contracts.VMs.GameModels
{
    public partial class GameBoardVm
        : ObservableObject
    {
        public const int GridSize = 9;

        [ObservableProperty]
        private ObservableCollection<GameCellVm> _cells = [];

        [ObservableProperty]
        private int _score = 0;

        [ObservableProperty]
        private int _timeRemaining = 120;

        [ObservableProperty]
        private bool _isGameOver = false;

        public GameBoardVm()
        {
            InitializeCells();
        }

        private void InitializeCells()
        {
            Cells.Clear();
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Cells.Add(new GameCellVm(row, col));
                }
            }
        }

        public GameCellVm? GetCell(
            int row,
            int col)
        {
            if (row < 0 ||
                row >= GridSize ||
                col < 0 ||
                col >= GridSize)
                return null;

            return Cells[row * GridSize + col];
        }
    }
}