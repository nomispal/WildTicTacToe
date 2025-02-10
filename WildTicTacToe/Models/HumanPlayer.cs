using WildTicTacToe.Strategies;

namespace WildTicTacToe.Models
{
    public class HumanPlayer : Player
    {
        public HumanPlayer() => MoveStrategy = new HumanStrategy();
    }
}