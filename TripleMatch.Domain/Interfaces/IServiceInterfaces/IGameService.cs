using TripleMatch.Shered.Contracts.VMs.GameModels;

namespace TripleMatch.Domain.Interfaces.IServiceInterfaces
{
    public interface IGameService
    {
        void InitializeBoard(GameBoardVm board);
        bool TrySwapAndMatch(GameBoardVm board, int row1, int col1, int row2, int col2);
    }
}