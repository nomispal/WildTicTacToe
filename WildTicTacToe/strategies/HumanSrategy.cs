using WildTicTacToe.Models;

namespace WildTicTacToe.Strategies
{
    public class HumanStrategy : MoveStrategy
    {
        public override Move GenerateMove(Board board, Player player)
        {
            Console.Write("Enter position (row,col): ");
            var input = Console.ReadLine().Split(',');
            var position = new Position(int.Parse(input[0]), int.Parse(input[1]));
            return new Move
            {
                Player = player,
                Position = position,
                Timestamp = DateTime.Now
            };
        }
    }
}