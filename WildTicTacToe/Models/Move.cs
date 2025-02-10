using System.Text.Json.Serialization;
using WildTicTacToe.Models;

namespace WildTicTacToe.Models
{
    public class Move
    {
        [JsonIgnore]
        public Player Player { get; set; }

        public Position Position { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonInclude]
        public char PlayerSymbol { get; set; }


        public void Execute(Board board)
        {
            board.ApplyMove(this);
        }

        public void Undo(Board board)
        {
            board.RemoveMove(this);
        }
    }
}