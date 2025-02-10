using WildTicTacToe.Models;

namespace WildTicTacToe.GameLogic
{
    public static class TicTacToeRules
    {
        public static bool CheckWin(Board board, Player player)
        {
            // Check rows and columns
            for (var i = 0; i < Board.Size; i++)
            {
                if (CheckRow(board, i, player) || CheckColumn(board, i, player))
                    return true;
            }

            // Check diagonals
            return CheckDiagonalLeft(board, player) || CheckDiagonalRight(board, player);
        }

        private static bool CheckRow(Board board, int row, Player player)
        {
            return board.GetRow(row).All(c => c == player.Symbol);
        }

        private static bool CheckColumn(Board board, int col, Player player)
        {
            for (var row = 0; row < Board.Size; row++)
            {
                if (board.GetRow(row)[col] != player.Symbol)
                    return false;
            }
            return true;
        }

        private static bool CheckDiagonalLeft(Board board, Player player)
        {
            for (var i = 0; i < Board.Size; i++)
            {
                if (board.GetRow(i)[i] != player.Symbol) return false;
            }
            return true;
        }

        private static bool CheckDiagonalRight(Board board, Player player)
        {
            for (var i = 0; i < Board.Size; i++)
            {
                if (board.GetRow(i)[Board.Size - 1 - i] != player.Symbol) return false;
            }
            return true;
        }
    }
}