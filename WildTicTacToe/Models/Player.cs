using System.Text.Json.Serialization;
using WildTicTacToe.Strategies;

namespace WildTicTacToe.Models
{
   
    public abstract class Player
    {
        public char Symbol { get; set; }

        [JsonIgnore]
        public MoveStrategy MoveStrategy { get; set; }

        public virtual Move MakeMove(Board board)
        {
            return MoveStrategy.GenerateMove(board, this);
        }
    }
}