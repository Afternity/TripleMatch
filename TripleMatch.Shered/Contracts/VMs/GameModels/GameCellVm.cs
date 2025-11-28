using CommunityToolkit.Mvvm.ComponentModel;

namespace TripleMatch.Shered.Contracts.VMs.GameModels
{
    public partial class GameCellVm
        : ObservableObject
    {
        [ObservableProperty]
        private GamePieceType _pieceType = GamePieceType.Empty;

        [ObservableProperty]
        private int _row;

        [ObservableProperty]
        private int _column;

        [ObservableProperty]
        private bool _isSelected = false;

        public GameCellVm(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}