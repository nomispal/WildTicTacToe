using WildTicTacToe.Strategies;

namespace WildTicTacToe.Models
{
    public class AIPlayer : Player
    {
        public AIPlayer() => MoveStrategy = new AIStrategy();
    }
}