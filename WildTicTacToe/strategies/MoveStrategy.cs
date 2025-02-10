using WildTicTacToe.Models;

namespace WildTicTacToe.Strategies
{
    public abstract class MoveStrategy
    {
        public abstract Move GenerateMove(Board board, Player player);

        internal (int, int) GenerateMove(Board board)
        {
            throw new NotImplementedException();
        }
    }
}