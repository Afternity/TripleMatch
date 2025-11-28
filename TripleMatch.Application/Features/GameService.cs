using TripleMatch.Shered.Contracts.VMs.GameModels;
using TripleMatch.Domain.Interfaces.IServiceInterfaces;
using TripleMatch.Domain.Enums;

namespace TripleMatch.Application.Features
{
    public class GameService
        : IGameService
    {
        private Random _random = new Random();

        public void InitializeBoard(GameBoardVm board)
        {
            foreach (var cell in board.Cells)
            {
                cell.PieceType = (GamePieceType)_random.Next(1, 7);
            }

            RemoveInitialMatches(board);
        }

        public void RemoveInitialMatches(GameBoardVm board)
        {
            for (int row = 0; row < GameBoardVm.GridSize; row++)
            {
                for (int col = 0; col < GameBoardVm.GridSize; col++)
                {
                    while (HasMatchAt(board, row, col))
                    {
                        var cell = board.GetCell(row, col);
                        if (cell != null)
                        {
                            cell.PieceType = (GamePieceType)_random.Next(1, 7);
                        }
                    }
                }
            }
        }

        public bool TrySwapAndMatch(GameBoardVm board, int row1, int col1, int row2, int col2)
        {
            var cell1 = board.GetCell(row1, col1);
            var cell2 = board.GetCell(row2, col2);

            if (cell1 == null || cell2 == null)
                return false;

            // Проверка соседства
            if (!AreAdjacent(row1, col1, row2, col2))
                return false;

            // Обмен
            (cell1.PieceType, cell2.PieceType) = (cell2.PieceType, cell1.PieceType);

            // Проверка совпадений
            bool hasMatch = HasMatchAt(board, row1, col1) || HasMatchAt(board, row2, col2);

            if (hasMatch)
            {
                RemoveMatches(board);
                ApplyGravity(board);
                FillEmpty(board);
                return true;
            }
            else
            {
                // Отмена обмена
                (cell1.PieceType, cell2.PieceType) = (cell2.PieceType, cell1.PieceType);
                return false;
            }
        }

        private bool AreAdjacent(int row1, int col1, int row2, int col2)
        {
            int rowDiff = Math.Abs(row1 - row2);
            int colDiff = Math.Abs(col1 - col2);
            return (rowDiff == 1 && colDiff == 0) || (rowDiff == 0 && colDiff == 1);
        }

        private bool HasMatchAt(GameBoardVm board, int row, int col)
        {
            var cell = board.GetCell(row, col);
            if (cell == null || cell.PieceType == GamePieceType.Empty)
                return false;

            // Горизонтально
            int count = 1;
            for (int c = col - 1; c >= 0; c--)
            {
                var leftCell = board.GetCell(row, c);
                if (leftCell?.PieceType == cell.PieceType)
                    count++;
                else
                    break;
            }
            for (int c = col + 1; c < GameBoardVm.GridSize; c++)
            {
                var rightCell = board.GetCell(row, c);
                if (rightCell?.PieceType == cell.PieceType)
                    count++;
                else
                    break;
            }
            if (count >= 3)
                return true;

            // Вертикально
            count = 1;
            for (int r = row - 1; r >= 0; r--)
            {
                var topCell = board.GetCell(r, col);
                if (topCell?.PieceType == cell.PieceType)
                    count++;
                else
                    break;
            }
            for (int r = row + 1; r < GameBoardVm.GridSize; r++)
            {
                var bottomCell = board.GetCell(r, col);
                if (bottomCell?.PieceType == cell.PieceType)
                    count++;
                else
                    break;
            }
            return count >= 3;
        }

        private void RemoveMatches(GameBoardVm board)
        {
            var cellsToRemove = new HashSet<(int, int)>();

            for (int row = 0; row < GameBoardVm.GridSize; row++)
            {
                for (int col = 0; col < GameBoardVm.GridSize; col++)
                {
                    var cell = board.GetCell(row, col);
                    if (cell == null || cell.PieceType == GamePieceType.Empty)
                        continue;

                    // Горизонтальные
                    if (col + 2 < GameBoardVm.GridSize)
                    {
                        var cell1 = board.GetCell(row, col);
                        var cell2 = board.GetCell(row, col + 1);
                        var cell3 = board.GetCell(row, col + 2);

                        if (cell1?.PieceType == cell2?.PieceType && 
                            cell2?.PieceType == cell3?.PieceType && 
                            cell1?.PieceType != GamePieceType.Empty)
                        {
                            cellsToRemove.Add((row, col));
                            cellsToRemove.Add((row, col + 1));
                            cellsToRemove.Add((row, col + 2));
                        }
                    }

                    // Вертикальные
                    if (row + 2 < GameBoardVm.GridSize)
                    {
                        var cell1 = board.GetCell(row, col);
                        var cell2 = board.GetCell(row + 1, col);
                        var cell3 = board.GetCell(row + 2, col);

                        if (cell1?.PieceType == cell2?.PieceType && 
                            cell2?.PieceType == cell3?.PieceType && 
                            cell1?.PieceType != GamePieceType.Empty)
                        {
                            cellsToRemove.Add((row, col));
                            cellsToRemove.Add((row + 1, col));
                            cellsToRemove.Add((row + 2, col));
                        }
                    }
                }
            }

            foreach (var (row, col) in cellsToRemove)
            {
                var cell = board.GetCell(row, col);
                if (cell != null)
                {
                    cell.PieceType = GamePieceType.Empty;
                    board.Score += 10;
                }
            }
        }

        private void ApplyGravity(GameBoardVm board)
        {
            for (int col = 0; col < GameBoardVm.GridSize; col++)
            {
                for (int row = GameBoardVm.GridSize - 1; row > 0; row--)
                {
                    var cell = board.GetCell(row, col);
                    if (cell?.PieceType == GamePieceType.Empty)
                    {
                        for (int r = row - 1; r >= 0; r--)
                        {
                            var topCell = board.GetCell(r, col);
                            if (topCell?.PieceType != GamePieceType.Empty)
                            {
                                cell.PieceType = topCell!.PieceType;
                                topCell.PieceType = GamePieceType.Empty;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void FillEmpty(GameBoardVm board)
        {
            for (int col = 0; col < GameBoardVm.GridSize; col++)
            {
                for (int row = 0; row < GameBoardVm.GridSize; row++)
                {
                    var cell = board.GetCell(row, col);
                    if (cell?.PieceType == GamePieceType.Empty)
                    {
                        cell.PieceType = (GamePieceType)_random.Next(1, 7);
                    }
                }
            }
        }
    }
}