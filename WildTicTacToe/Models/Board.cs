using System.Linq;

namespace WildTicTacToe.Models
{
    public class Board
    {
        public const int Size = 3;
        private readonly char[,] _grid = new char[Size, Size];


        public bool IsValidMove(Move move)
        {
            if(move.Position.X < 0 || move.Position.X >= Size || move.Position.Y < 0 || move.Position.Y >= Size)
            {
                Console.WriteLine("Invalid move: Out of bounds. Try again.");
                return false;
            }
            return _grid[move.Position.X, move.Position.Y] == '\0';
        }

        public void Restore(char[,] grid)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _grid[i, j] = grid[i, j];
                }
            }
        }
        public char[,] GetGrid()
        {
            var gridCopy = new char[Size, Size];
            Array.Copy(_grid, gridCopy, _grid.Length);
            return gridCopy;
        }


        public void ApplyMove(Move move)
        {
            _grid[move.Position.X, move.Position.Y] = move.Player.Symbol;
        }

        public void RemoveMove(Move move)
        {
            _grid[move.Position.X, move.Position.Y] = '\0';
        }


        public void Clear()
        {
            Array.Clear(_grid, 0, _grid.Length);
        }

        public bool IsFull()
        {
            foreach (var cell in _grid)
            {
                if (cell == '\0') return false;
            }
            return true;
        }


        public char[] GetRow(int row)
        {
            return Enumerable.Range(0, Size)
                .Select(col => _grid[row, col])
                .ToArray();
        }

    }
}