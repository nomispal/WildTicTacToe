using System.Collections.Generic;
using WildTicTacToe.Models;

namespace WildTicTacToe.Strategies
{
    public class AIStrategy : MoveStrategy
    {
        private readonly Random _random = new();

        public override Move GenerateMove(Board board, Player player)
        {
            var emptyCells = new List<Position>();

            for (var i = 0; i < Board.Size; i++)
            {
                for (var j = 0; j < Board.Size; j++)
                {
                    var move = new Move { Player = player, Position = new Position(i, j) };
                    if (board.IsValidMove(move)) // Ensure we check with the player set
                    {
                        emptyCells.Add(move.Position);
                    }
                }
            }

            if (emptyCells.Count == 0)
                throw new InvalidOperationException("No valid moves available for AI.");

            return new Move
            {
                Player = player,
                Position = emptyCells[_random.Next(emptyCells.Count)],
                Timestamp = DateTime.Now
            };
        }
    }
}
